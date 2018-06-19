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

        [ConfigurationProperty(EmailSectionProperties.Host, IsRequired = false)]
        public string Host
        {
            get { return (string)this[EmailSectionProperties.Host]; }
            set {
                this[EmailSectionProperties.Host] = value; }
        }

        [ConfigurationProperty(EmailSectionProperties.Port, IsRequired = false)]
        public int Port
        {
            get { return (int)this[EmailSectionProperties.Port]; }
            set { this[EmailSectionProperties.Port] = value; }
        }

        [ConfigurationProperty(EmailSectionProperties.EnableSsl, IsRequired = true)]
        public bool EnableSsl => (bool)this[EmailSectionProperties.EnableSsl];

        [ConfigurationProperty(EmailSectionProperties.TimeoutInMilliseconds, IsRequired = true)]
        public int TimeoutInMilliseconds => (int)this[EmailSectionProperties.TimeoutInMilliseconds];

        // Overriding this method is necessary to avoid error "ConfigurationErrorsException - The configuration is read only"
        // while setting any properties like Host, Port etc.
        public override bool IsReadOnly() => false;
    }
}
