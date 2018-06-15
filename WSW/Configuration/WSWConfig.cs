using System.Configuration;

namespace WSW.Configuration
{
    public sealed class WswConfig
    {
        private static readonly object LockConfig = new object();

        public EmailSection EmailSection { get; internal set; }
        public SecureSection SecureSection { get; internal set; }
        public ServiceSection ServiceSection { get; internal set; }
        public static WswConfig Instance => new WswConfig();

        private WswConfig()
        {
            lock (LockConfig)
            {
                EncryptConfigSection(Constants.SecureSection);
                EmailSection = ConfigurationManager.GetSection(Constants.EmailSection) as EmailSection;
                SecureSection = ConfigurationManager.GetSection(Constants.SecureSection) as SecureSection;
                ServiceSection = ConfigurationManager.GetSection(Constants.ServiceSection) as ServiceSection;
            }
        }
        public void EncryptConfigSection(string sectionName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection(sectionName);
            if (section == null || section.SectionInformation.IsProtected ||
                section.ElementInformation.IsLocked) return;
            section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Full);
        }
    }
}
