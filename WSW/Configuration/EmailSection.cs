namespace WSW.Configuration
{
    using System.Configuration;

    public class EmailSection : ConfigurationSection
    {
        private static class EmailSectionProperties
        {
            internal const string FromMailAddress = "fromMailAddress";
            internal const string FromDisplayName = "fromDisplayName";
            internal const string ToMailAddress = "toMailAddress";
            internal const string ToDisplayName = "toDisplayName";
            internal const string Host = "host";
            internal const string Port = "port";
            internal const string EnableSsl = "enableSsl";
            internal const string TimeoutInMilliseconds = "timeoutInMilliseconds";
        }

        [ConfigurationProperty(EmailSectionProperties.FromMailAddress, IsRequired = true)]
        public string FromMailAddress => (string)this[EmailSectionProperties.FromMailAddress];

        [ConfigurationProperty(EmailSectionProperties.FromDisplayName, IsRequired = true)]
        public string FromDisplayName => (string)this[EmailSectionProperties.FromDisplayName];

        [ConfigurationProperty(EmailSectionProperties.ToMailAddress, IsRequired = true)]
        public string ToMailAddress => (string)this[EmailSectionProperties.ToMailAddress];

        [ConfigurationProperty(EmailSectionProperties.ToDisplayName, IsRequired = true)]
        public string ToDisplayName => (string)this[EmailSectionProperties.ToDisplayName];

        [ConfigurationProperty(EmailSectionProperties.Host, IsRequired = true)]
        public string Host => (string)this[EmailSectionProperties.Host];

        [ConfigurationProperty(EmailSectionProperties.Port, IsRequired = true)]
        public int Port => (int)this[EmailSectionProperties.Port];

        [ConfigurationProperty(EmailSectionProperties.EnableSsl, IsRequired = true)]
        public bool EnableSsl => (bool)this[EmailSectionProperties.EnableSsl];

        [ConfigurationProperty(EmailSectionProperties.TimeoutInMilliseconds, IsRequired = true)]
        public int TimeoutInMilliseconds => (int)this[EmailSectionProperties.TimeoutInMilliseconds];
    }
}
