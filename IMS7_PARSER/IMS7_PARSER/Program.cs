using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace IMS7_PARSER
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
            fileTarget.FileName = "${basedir}/log.txt";
            fileTarget.Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${threadid} ${message}";

            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            LogManager.Configuration = config;
            Log = LogManager.GetLogger("IMS7_PARSER");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Main...");
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun = new ServiceBase[]
                {
                    new Service()
                };

                SetupLog();
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                Service nt = null;
                try
                {
                    SetupLog();
                    nt = new Service();
                    nt.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);

                }
                Console.ReadKey();
            }

        }
    }
}
