using System.Configuration;

namespace WSW.Configuration
{
    public class SecureSection : ConfigurationSection
    {
        private static class SecureSectionProperties
        {
            internal const string FromMailPassword = "fromMailPassword";
        }

        [ConfigurationProperty(SecureSectionProperties.FromMailPassword)]
        public string FromMailPassword => (string)this[SecureSectionProperties.FromMailPassword];
    }
}
