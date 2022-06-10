using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StorageTimeService
{
	static class Program
	{
        static LoggingConfiguration config = new LoggingConfiguration();
        static FileTarget fileTarget = new FileTarget();
        public static Logger Log = null;

        static void SetupLog()
        {
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            config.AddTarget("file", fileTarget);

            consoleTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${threadid} ${message}";
            fileTarget.FileName = "${basedir}/" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${threadid} ${message}";

            var rule1 = new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
            Log = LogManager.GetLogger("WImageService");
        }


        static void Main(string[] args)
		{
            SetupLog();    
            if ((args.Length == 1) && (args[0].ToUpper() == "-D" || args[0].ToUpper() == "--DEBUG"))
                Debug();
            else
                RunService();
		}

        private static void RunService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                     new Service()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void Debug()
        {
            try
            {
                Service.GetSettings();
                Service.StartProc();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            return;
        }

    }
}
