namespace WSW.Configuration
{
    using System.Configuration;

    public sealed class WswConfig
    {
        private static readonly object LockConfig = new object();

        public static WswConfig Instance => new WswConfig();

        public EmailSection EmailSection { get; internal set; }

        public SecureSection SecureSection { get; internal set; }

        public ServiceSection ServiceSection { get; internal set; }

        private WswConfig()
        {
            lock (LockConfig)
            {
                // If encrypted password rollback to plain password on service stop.
                // Then, run in release mode and Project + Properties, Debug tab, turn off "Enable the Visual Studio hosting process".
                EncryptConfigSection(Constants.SecureSection);
                EmailSection = ConfigurationManager.GetSection(Constants.EmailSection) as EmailSection;
                SecureSection = ConfigurationManager.GetSection(Constants.SecureSection) as SecureSection;
                ServiceSection = ConfigurationManager.GetSection(Constants.ServiceSection) as ServiceSection;

                if (EmailSection.FromMailAddress.EndsWith("@gmail.com", System.StringComparison.OrdinalIgnoreCase))
                {
                    EmailSection.Host = Constants.EmailType.Gmail.Host;
                    EmailSection.Port = Constants.EmailType.Gmail.Port;
                }
                else if (EmailSection.FromMailAddress.EndsWith(".com", System.StringComparison.OrdinalIgnoreCase))
                {
                    EmailSection.Host = Constants.EmailType.Outlook.Host;
                    EmailSection.Port = Constants.EmailType.Outlook.Port;
                }
            }
        }

        public void EncryptConfigSection(string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection(sectionName);

            if (section == null || section.SectionInformation.IsProtected || section.ElementInformation.IsLocked)
            {
                return;
            }

            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Full);
        }
    }
}
