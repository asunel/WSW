namespace WSW.Email
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Mail;
    using System.Reflection;
    using WSW.Performance;

    class MailSender
    {
        public static bool SendMail(EmailData emailData, string source)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                try
                {
                    var fromMailAddress = new MailAddress(emailData.FromMailAddress, emailData.FromDisplayName);
                    var toMailAddress = new MailAddress(emailData.ToMailAddress, emailData.ToDisplayName);
                    using (var smtp = new SmtpClient
                    {
                        Host = emailData.Host,
                        Port = emailData.Port,
                        EnableSsl = emailData.EnableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromMailAddress.Address, emailData.FromMailPassword),
                        Timeout = emailData.TimeOutInMilliseconds,
                    })
                    {
                        using (var msg = new MailMessage(fromMailAddress, toMailAddress)
                        {
                            Subject = emailData.Subject,
                            Body = emailData.Body,
                            IsBodyHtml = emailData.IsBodyHtml
                        })
                        {
                            smtp.Send(msg);
                            EventLog.WriteEntry(source, emailData.Body, EventLogEntryType.Information, 0);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(source, ex.Message, EventLogEntryType.Error, 0);
                    return false;
                }
            }
        }
    }
}
