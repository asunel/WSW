namespace WSW.Email
{
    public class EmailData
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public string FromMailAddress { get; set; }
        public string FromDisplayName { get; set; }
        public string ToMailAddress { get; set; }
        public string ToDisplayName { get; set; }
        public string FromMailPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public int TimeOutInMilliseconds { get; set; }
    }
}
