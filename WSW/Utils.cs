namespace WSW
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using System.Threading;
    using System.Xml;
    using System.Xml.Linq;
    using Configuration;
    using Extensions;
    using Performance;
    using SC;

    public static class Utils
    {
        public static Dictionary<string, string> GetDictionary(string headerColumn1, string headerColumn2, ServiceController[] services)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dic = new Dictionary<string, string> { { headerColumn1.Bold(), headerColumn2.Bold() } };
                services.ToList().ForEach(svc => dic.Add(svc.DisplayName, svc.Status.ToString()));
                return dic;
            }
        }

        public static Dictionary<string, string> ToDictionary(string headerColumn1, string headerColumn2, object record)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var dic = new Dictionary<string, string> { { headerColumn1.Bold(), headerColumn2.Bold() } };

                if (record == null)
                {
                    dic.Add("Event record is null while reading Event viewer logs", "");
                }
                else
                {
                    record.GetType().GetProperties().ToList().ForEach(prop => dic[prop.Name] = (prop.GetValue(record, null) == null) ? "NULL" : ((prop.PropertyType == typeof(DateTime?)) ? prop.GetValue(record, null).ToString() + string.Format("({0} in {1} culture)", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, Thread.CurrentThread.CurrentCulture.Name) : prop.GetValue(record, null).ToString()));
                }

                return dic;
            }
        }

        public static Dictionary<string, string> GetServiceGeneralInfo(ServiceController sc)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var path = "Win32_Service.Name='" + sc.ServiceName + "'";
                var managementObj = new ManagementObject(path);

                var properties = new List<string>()
                {
                    "DisplayName",
                    "Description",
                    "StartMode",
                    "StartName",
                };

                var dic = new Dictionary<string, string>
                {
                    {"Property".Bold(), "Value".Bold()},
                    {"Machine", Environment.MachineName},
                    {"Username", System.Security.Principal.WindowsIdentity.GetCurrent().Name},
                };

                properties.ForEach(prop => dic.Add(prop, (managementObj[prop] == null) ? string.Empty : managementObj[prop].ToString()));
                return dic;
            }
        }

        public static string[] GetAllConfigFromSpecifiedDirectory(string customConfigDirectoryPath)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var allConfigPaths = new string[0];
                try
                {
                    allConfigPaths = Directory.GetFiles(customConfigDirectoryPath);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(Constants.WswService, ex.Message, EventLogEntryType.Error, 0);
                }

                return allConfigPaths;
            }
        }

        public static List<KeyVal<string, Dictionary<string, string>>> GetOtherThanExeConfigData(string[] configFilePaths)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var list = new List<KeyVal<string, Dictionary<string, string>>>();

                foreach (var configPath in configFilePaths)
                {
                    var dicConfigData = new Dictionary<string, string> { { "Node".Bold(), "Value".Bold() } };

                    if (!File.Exists(configPath)) continue;
                    var document = XDocument.Load(configPath);
                    var xml = document.ToString();

                    var rdr = XmlReader.Create(new StringReader(xml));
                    var key = string.Empty;

                    while (rdr.Read())
                    {
                        switch (rdr.NodeType)
                        {
                            case XmlNodeType.Element:
                                key = rdr.LocalName;
                                break;
                            case XmlNodeType.Text:
                                var value = rdr.Value;
                                dicConfigData.Add(key, value);
                                break;
                        }
                    }

                    var pair = new KeyVal<string, Dictionary<string, string>>
                    {
                        Id = configPath,
                        Value = dicConfigData
                    };

                    list.Add(pair);
                }

                return list;
            }
        }

        public static string[] GetAllOtherConfigsFullPaths(Service svc, ServiceController sc)
        {
            var configFilePaths = new string[0];
            foreach (var relativePath in svc.OtherConfigs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                var fullPath = Path.Combine(Directory.GetParent(sc.ImagePath).ToString(), relativePath);
                var pathInfo = GetPathInfo(fullPath);

                if (pathInfo == Constants.PathInfoEnum.Directory)
                {
                    configFilePaths = configFilePaths.Concat(GetAllConfigFromSpecifiedDirectory(fullPath)).ToArray();
                }
                else if (pathInfo == Constants.PathInfoEnum.File)
                {
                    configFilePaths = configFilePaths.Concat(new[] { Path.GetFullPath(fullPath) }).ToArray();
                }
            }

            return configFilePaths;
        }

        public static Constants.PathInfoEnum GetPathInfo(string relativePath)
        {
            try
            {
                var fullPath = Path.GetFullPath(relativePath);
                var attr = File.GetAttributes(fullPath);
                return attr.HasFlag(FileAttributes.Directory) ? Constants.PathInfoEnum.Directory : Constants.PathInfoEnum.File;
            }
            catch (Exception)
            {
                return Constants.PathInfoEnum.None;
            }
        }
    }
}
