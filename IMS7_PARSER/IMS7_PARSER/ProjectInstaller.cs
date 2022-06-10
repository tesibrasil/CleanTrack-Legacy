using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Text;
using ImageSI.Configuration;

namespace IMS7_PARSER
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

            string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\ims7.cfg";

            var ConfigFile = new ConfigurationFile(configPath, true);
            ConfigFile.LoadSettings();

            string serviceName = "IMS7";

            string serviceNameTemp = ConfigFile.Get("ServiceName", "IMS7");

            serviceName = serviceNameTemp.Length > 0 ? serviceNameTemp : serviceName;

            ConfigFile.Set("ServiceName", serviceName, false);

            ConfigFile.SaveSettings();



            service.ServiceName = serviceName;

            Installers.Add(process);
            Installers.Add(service);
        }


    }
}
