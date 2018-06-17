namespace WSW.Configuration
{
    using System;
    using System.Configuration;
    using WSW.Extensions;

    public class ServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ServiceCategoryCollection Categories => (ServiceCategoryCollection)base[""];
    }

    public class ServiceCategoryCollection : ConfigurationElementCollection
    {
        private static class ServiceCategoryCollectionElements
        {
            internal const string ServiceCategory = "serviceCategory";
        }

        public ServiceCategoryCollection()
        {
            var details = (ServiceCategory)CreateNewElement();
            if (details.Category != "")
            {
                Add(details);
            }
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected sealed override ConfigurationElement CreateNewElement() => new ServiceCategory();

        protected override Object GetElementKey(ConfigurationElement element) => ((ServiceCategory)element).Category;

        protected override string ElementName => ServiceCategoryCollectionElements.ServiceCategory;

        protected override void BaseAdd(ConfigurationElement element) => BaseAdd(element, false);

        public ServiceCategory this[int index]
        {
            get
            {
                return (ServiceCategory)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new ServiceCategory this[string key] => (ServiceCategory)BaseGet(key);

        public int IndexOf(ServiceCategory details) => BaseIndexOf(details);

        public void Add(ServiceCategory details) => BaseAdd(details);

        public void Remove(ServiceCategory details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Category);
        }

        public void RemoveAt(int index) => BaseRemoveAt(index);

        public void Remove(string name) => BaseRemove(name);

        public void Clear() => BaseClear();
    }

    public class ServiceCategory : ConfigurationElement
    {
        private static class ServiceCategoryProperties
        {
            public const string Category = "category";
            public const string Services = "services";
        }

        [ConfigurationProperty(ServiceCategoryProperties.Category, IsRequired = true)]
        public string Category => (string)this[ServiceCategoryProperties.Category];

        [ConfigurationProperty(ServiceCategoryProperties.Services, IsDefaultCollection = false, IsRequired = true)]
        public ServiceCollection Services => (ServiceCollection)base[ServiceCategoryProperties.Services];
    }

    public class ServiceCollection : ConfigurationElementCollection
    {
        private static class ServiceCollectionElements
        {
            internal const string Service = "service";
        }

        public new Service this[string key] => (IndexOf(key) < 0) ? null : (Service)BaseGet(key);

        public Service this[int index] => (Service)BaseGet(index);

        public int IndexOf(string key) => this.ToListCasted<Service>().FindIndex(a => a.Name.ToLower() == key.ToLower());

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override ConfigurationElement CreateNewElement() => new Service();

        protected override object GetElementKey(ConfigurationElement element) => ((Service)element).Name;

        protected override bool IsElementName(string elementName) => !string.IsNullOrEmpty(elementName) && elementName == ServiceCollectionElements.Service;

        protected override string ElementName => ServiceCollectionElements.Service;
    }

    public class Service : ConfigurationElement
    {
        public class ServiceProperties
        {
            internal const string Name = "name";
            internal const string Source = "source";
            internal const string LogName = "logName";
            internal const string EnableNotification = "enableNotification";
            internal const string EnableStart = "enableStart";
            internal const string IsMailSent = "isMailSent";
            internal const string OtherConfigs = "otherConfigs";
            internal const string WaitTimeoutForStartInSeconds = "waitTimeoutForStartInSeconds";
        }

        [ConfigurationProperty(ServiceProperties.Name, IsRequired = true, IsKey = true)]
        public string Name => (string)this[ServiceProperties.Name];

        [ConfigurationProperty(ServiceProperties.Source, IsRequired = true)]
        public string Source => (string)this[ServiceProperties.Source];

        [ConfigurationProperty(ServiceProperties.LogName, IsRequired = true)]
        public string LogName => (string)this[ServiceProperties.LogName];

        [ConfigurationProperty(ServiceProperties.EnableNotification, IsRequired = true)]
        public bool EnableNotification => (bool)this[ServiceProperties.EnableNotification];

        [ConfigurationProperty(ServiceProperties.EnableStart, IsRequired = true)]
        public bool EnableStart => (bool)this[ServiceProperties.EnableStart];

        [ConfigurationProperty(ServiceProperties.IsMailSent, IsRequired = true)]
        public bool IsMailSent
        {
            get { return (bool)this[ServiceProperties.IsMailSent]; }
            set { this[ServiceProperties.IsMailSent] = value; }
        }

        [ConfigurationProperty(ServiceProperties.OtherConfigs, IsRequired = true)]
        public string OtherConfigs => (string)this[ServiceProperties.OtherConfigs];

        [ConfigurationProperty(ServiceProperties.WaitTimeoutForStartInSeconds, IsRequired = true)]
        public int WaitTimeoutForStart => (int)base[ServiceProperties.WaitTimeoutForStartInSeconds];
    }
}
