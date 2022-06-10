using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ImageSI.Configuration;

namespace TRfid
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();

            string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\trfid.cfg";

            var ConfigFile = new ConfigurationFile(configPath, true);           
            ConfigFile.LoadSettings();

            string serviceName = "Cleantrack";

            string serviceNameTemp = ConfigFile.Get("ServiceName", "Cleantrack");

            serviceName = serviceNameTemp.Length > 0 ? serviceNameTemp : serviceName;

            ConfigFile.Set("ServiceName", serviceName, false);

            ConfigFile.SaveSettings();
            
            service.ServiceName = serviceName;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
