using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using WSW.Extensions;

namespace WSW.SC
{
    public class ServiceController : System.ServiceProcess.ServiceController
    {
        private string _imagePath;
        private ServiceController[] _dependentServices;
        private ServiceController[] _servicesDependedOn;

        public ServiceController()
        {
        }
        public ServiceController(string name) : base(name) { }
        public ServiceController(string name, string machineName) : base(name, machineName) { }
        public string ImagePath => _imagePath = _imagePath ?? GetImagePath();
        public new ServiceController[] DependentServices => _dependentServices = _dependentServices ?? GetServices(base.DependentServices);
        public new ServiceController[] ServicesDependedOn => _servicesDependedOn = _servicesDependedOn ?? GetServices(base.ServicesDependedOn);
        public new static ServiceController[] GetServices() => GetServices(".");
        public new static ServiceController[] GetServices(string machineName) => GetServices(System.ServiceProcess.ServiceController.GetServices(machineName));
        private static ServiceController[] GetServices(System.ServiceProcess.ServiceController[] systemServices) => systemServices.Select(s => new ServiceController(s.ServiceName, s.MachineName)).ToArray();
        private string GetImagePath()
        {
            //Reference: https://www.codeproject.com/Articles/26533/A-ServiceController-Class-that-Contains-the-Path-t
            string registryPath = string.Format(Constants.RegistryPathTemplate, ServiceName);
            var keyHklm = Registry.LocalMachine;
            var serviceKey = keyHklm.OpenSubKey(registryPath);
            if (serviceKey == null) throw new Exception($"Could not find service key for ImagePath: {ImagePath}");
            var imagePath = serviceKey.GetValue(Constants.ImagePath).ToString();
            serviceKey.Close();
            imagePath = Environment.ExpandEnvironmentVariables(imagePath);
            imagePath = imagePath.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
            return imagePath;

        }
        public Dictionary<string, string> GetExeConfigData()
        {
            var dic = new Dictionary<string, string> { { "Key".Bold(), "Value".Bold() } };
            var config = ConfigurationManager.OpenExeConfiguration(ImagePath);
            ConfigurationManager.OpenExeConfiguration(ImagePath).AppSettings.Settings.AllKeys.ToList().ForEach(key => dic.Add(key, config.AppSettings.Settings[key].Value));
            return dic;
        }
    }
}
