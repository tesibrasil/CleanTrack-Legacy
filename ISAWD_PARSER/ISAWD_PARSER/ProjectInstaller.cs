using ImageSI.Configuration;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;

namespace ISAWD_PARSER
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

            string configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\isawd.cfg";

            var ConfigFile = new ConfigurationFile(configPath, true);
            ConfigFile.LoadSettings();

            string serviceName = "ISAWD";

            string serviceNameTemp = ConfigFile.Get("ServiceName", "ISAWD");

            serviceName = serviceNameTemp.Length > 0 ? serviceNameTemp : serviceName;

            ConfigFile.Set("ServiceName", serviceName, false);

            ConfigFile.SaveSettings();

            service.ServiceName = serviceName;

            Installers.Add(process);
            Installers.Add(service);
        }


    }
}
