using System.Configuration;

namespace WSW.Configuration
{
    public class EmailSection : ConfigurationSection
    {
        public class EmailSectionProperties
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

        [ConfigurationProperty(EmailSectionProperties.FromMailAddress)]
        public string FromMailAddress => (string)this[EmailSectionProperties.FromMailAddress];

        [ConfigurationProperty(EmailSectionProperties.FromDisplayName)]
        public string FromDisplayName => (string)this[EmailSectionProperties.FromDisplayName];

        [ConfigurationProperty(EmailSectionProperties.ToMailAddress)]
        public string ToMailAddress => (string)this[EmailSectionProperties.ToMailAddress];

        [ConfigurationProperty(EmailSectionProperties.ToDisplayName)]
        public string ToDisplayName => (string)this[EmailSectionProperties.ToDisplayName];

        [ConfigurationProperty(EmailSectionProperties.Host)]
        public string Host => (string)this[EmailSectionProperties.Host];

        [ConfigurationProperty(EmailSectionProperties.Port)]
        public int Port => (int)this[EmailSectionProperties.Port];

        [ConfigurationProperty(EmailSectionProperties.EnableSsl)]
        public bool EnableSsl => (bool)this[EmailSectionProperties.EnableSsl];

        [ConfigurationProperty(EmailSectionProperties.TimeoutInMilliseconds)]
        public int TimeoutInMilliseconds => (int)this[EmailSectionProperties.TimeoutInMilliseconds];
    }
}
