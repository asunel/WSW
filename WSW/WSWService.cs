using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using WSW.Configuration;
using WSW.Email;
using WSW.Extensions;
using WSW.Performance;
using ServiceController = WSW.SC.ServiceController;
using EventRecord = WSW.EventViewer.EventRecord;

namespace WSW
{
    public partial class WswService : ServiceBase
    {
        private Thread _thread;
        private static readonly object LockConfig = new object();
        private static readonly WswConfig Config = WswConfig.Instance;

        public WswService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(RunEachServiceMonitorTaskInParallel);
            _thread.Start();
        }

        public void RunEachServiceMonitorTaskInParallel()
        {
            var tasks = new List<Task>();

            Config.ServiceSection.Categories.ToListCasted<ServiceCategory>().ForEach(cat => cat.Services.ToListCasted<Service>().ForEach(svc =>
            {
                tasks.Add(Task.Factory.StartNew(() => StartMonitoring(cat.Category, svc)));
            }));

            Task.WaitAll(tasks.ToArray());
        }

        public static void StartMonitoring(string category, Service svc)
        {
            try
            {
                using (var sc = new ServiceController(svc.Name))
                {
                    while (true)
                    {
                        // Don't move ahead till Enable Notification gets enabled
                        svc = LoopTillExpectationMeets(category, svc.Name, true, true);

                        // If mail already sent for Service XYZ and XYZ is still in STOP state, then don't send a mail again
                        var desiredStatus = IsMailAlreadySent(sc, svc) ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped;

                        // Wait infinitely till desired status is achieved
                        sc.WaitForStatus(desiredStatus);

                        // If the desired status is Running and it has been achieved
                        // then Mail Sent flag to false in service settings config file,
                        // so that in case WSW Service fails it will not assume that mail has been
                        // sent for service XYZ previously before failure.
                        if (desiredStatus == ServiceControllerStatus.Running)
                        {
                            UpdateIsMailSentFlag(category, svc.Name, false);
                            desiredStatus = ServiceControllerStatus.Stopped;
                            continue;
                        }

                        if (desiredStatus == ServiceControllerStatus.Stopped)
                        {
                            var eventLevel = StandardEventLevel.Error;

                            // Read the updated service settings.
                            // Check if the EnableStart = true
                            svc = LoopTillExpectationMeets(category, svc.Name, true, false);
                            if (svc.EnableStart)
                            {
                                // try to start the XYZ service, so in case of failure a new event entry will be added in
                                // "source (as configured in service settings xml)" Windows logs in Event Viewer. 
                                // This log will be sent as failure reason of the serevice XYZ in email.
                                sc.Start();

                                try
                                {
                                    // Must provide a timeout, otherwise it will wait for Running status infinitely
                                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 5));

                                    // Since we reached here, means timeout occured and hence service has been started successfully
                                    // It means an Information log entry will be created in Event Viewer
                                    eventLevel = StandardEventLevel.Informational;
                                }
                                catch (System.ServiceProcess.TimeoutException)
                                {
                                    // Since Timeout occured, it means XYZ service was unable to start above 
                                    // and hence an error entry got created in Event Viewer
                                    eventLevel = StandardEventLevel.Error;
                                }
                            }

                            // Get the log entry from the Event Viewer to be sent in mail notification
                            var eventRecord = (new EventViewer.EventViewer()).GetLog(svc.LogName, svc.Source, eventLevel);

                            // Get other than .exe.config files whose relative paths are configurable
                            // as comma-separated relative paths with mixture of both directories and files
                            var configFilePaths = Utils.GetAllOtherConfigsFullPaths(svc, sc);

                            // Prepare the body HTML
                            var body = PrepareMailBody(sc, eventRecord, configFilePaths);

                            var isMailSent = TriggerMail(category, Constants.WswService, svc.Name, eventLevel == StandardEventLevel.Informational, body);
                            UpdateIsMailSentFlag(category, svc.Name, isMailSent);
                            desiredStatus = ServiceControllerStatus.Running;
                        }
                    }
                }
            }
            catch (ArgumentException argEx)
            {
                EventLog.WriteEntry(Constants.WswService, argEx.Message, EventLogEntryType.Error, 0);
            }
            catch (Win32Exception win32Ex)
            {
                EventLog.WriteEntry(Constants.WswService, win32Ex.Message, EventLogEntryType.Error, 0);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Constants.WswService, ex.Message, EventLogEntryType.Error, 0);
            }
        }

        protected override void OnStop()
        {
            _thread.Abort();
        }

        public static string PrepareMailBody(ServiceController sc, EventRecord eventRecord, string[] configFilePaths)
        {
            return string.Join(Constants.HtmlBrTag,
                 Utils.ToDictionary("Property", "Value", eventRecord).ToHtml("Event Log"),
                 Utils.GetDictionary("Service Name", "Status", sc.DependentServices).ToHtml("Dependent Services"),
                 Utils.GetDictionary("Service Name", "Status", sc.ServicesDependedOn).ToHtml("Service Depends On"),
                 sc.GetExeConfigData().ToHtml(string.Format(Constants.FileHyperlinkFormat, sc.ImagePath + Constants.ConfigExtension, Path.GetFileName(sc.ImagePath) + Constants.ConfigExtension)),
                 Utils.GetServiceGeneralInfo(sc).ToHtml($"{sc.ServiceName}"),
                 string.Join(Constants.HtmlBrTag, Utils.GetOtherThanExeConfigData(configFilePaths).Select(dic => dic.Value.ToHtml(string.Format(Constants.FileHyperlinkFormat, dic.Id, Path.GetFileName(dic.Id)))).ToArray())
                 );
        }

        public static bool TriggerMail(string category, string source, string serviceName, bool isStartSuccess, string body)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var msgServiceStartTry = isStartSuccess ? Constants.MsgServiceRestartSuccess : Constants.MsgServiceRestartFail;
                var backgroundColor = isStartSuccess ? Constants.Green : Constants.Red;

                body = string.Format(Constants.BodyTemplate.Replace(Environment.NewLine, string.Empty).Replace("\t", string.Empty), Constants.Restart, backgroundColor, msgServiceStartTry, body);

                var emailData = new EmailData
                {
                    Subject = string.Format(Constants.SubjectTemplate, serviceName),
                    Body = body,
                    IsBodyHtml = true,
                    FromMailAddress = Config.EmailSection.FromMailAddress,
                    FromDisplayName = Config.EmailSection.FromDisplayName,
                    ToMailAddress = Config.EmailSection.ToMailAddress,
                    ToDisplayName = Config.EmailSection.ToDisplayName,
                    FromMailPassword = Config.SecureSection.FromMailPassword,
                    Host = Config.EmailSection.Host,
                    Port = Config.EmailSection.Port,
                    EnableSsl = Config.EmailSection.EnableSsl,
                    TimeOutInMilliseconds = Config.EmailSection.TimeoutInMilliseconds
                };

                return MailSender.SendMail(emailData, source);
            }
        }

        public static void UpdateIsMailSentFlag(string category, string serviceName, bool isMailSent)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                lock (LockConfig)
                {
                    var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    try
                    {
                        ((ServiceSection)conf.GetSection(Constants.ServiceSection)).Categories[category]
                            .Services[serviceName]
                            .IsMailSent = isMailSent;
                        conf.Save(); // Note: All comments in the saved xml will get cleaned up (Ref: https://stackoverflow.com/questions/1954358/can-configurationmanager-retain-xml-comments-on-save)
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(Constants.WswService, ex.Message, EventLogEntryType.Error, 0);
                    }
                }
            }
        }
        private static Service LoopTillExpectationMeets(string category, string serviceName, bool expectedState, bool loopInfinitely)
        {
            Service serviceObj = null;
            var i = loopInfinitely ? 0 : 1;

            while (true)
            {
                try
                {
                    var sec = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(Constants.ServiceSection) as ServiceSection;
                    serviceObj = sec?.Categories[category].Services[serviceName];
                    if (serviceObj != null && (serviceObj.EnableNotification == expectedState || i == 1)) break;
                }
                catch (Exception)
                {
                    if (i == 1) break;
                }
            }
            return serviceObj;
        }

        private static bool IsMailAlreadySent(ServiceController sc, Service svc) => sc.Status == ServiceControllerStatus.Stopped && svc.IsMailSent;
    }
}
