namespace WSW.Configuration
{
    using System.Configuration;

    public class SecureSection : ConfigurationSection
    {
        private static class SecureSectionProperties
        {
            internal const string FromMailPassword = "fromMailPassword";
        }

        [ConfigurationProperty(SecureSectionProperties.FromMailPassword, IsRequired = true)]
        public string FromMailPassword => (string)this[SecureSectionProperties.FromMailPassword];
    }
}
