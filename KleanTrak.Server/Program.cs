using System;
using System.IO;
using KleanTrak.Core;
using log4net;
using log4net.Config;

namespace KleanTrak.Server
{
    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static void Main(string[] args)
        {
            Dictionary.Init();

			string pathCfg = "Settings.cfg";
            if ((args.Length == 1) && (args[0].ToUpper() == "-I" || args[0].ToUpper() == "--INSTALL"))
            {
                KleanTrakService.Install(pathCfg, true);
            }
            else if ((args.Length == 1) && (args[0].ToUpper() == "-U" || args[0].ToUpper() == "--UNINSTALL"))
            {
                KleanTrakService.Uninstall(pathCfg, true);
            }
            else if ((args.Length == 1) && (args[0].ToUpper() == "-D" || args[0].ToUpper() == "--DEBUG"))
            {
                KleanTrakService.Debug(pathCfg);
            }
            else if ((args.Length == 1) && (args[0].ToUpper() == "-SI")) // silent install //
            {
                KleanTrakService.Install(pathCfg, false);
            }
            else if ((args.Length == 1) && (args[0].ToUpper() == "-SU")) // silent uninstall
            {
                KleanTrakService.Uninstall(pathCfg, false);
            }
            else if (args.Length == 0)
            {
                KleanTrakService.Run(pathCfg);
            }
            else
            {
                Console.WriteLine("Command line parameter accepted:");
                Console.WriteLine("       [-i] [--install] <dependencies> Install service");
                Console.WriteLine("       [-u] [--uninstall] Unistall service");
                Console.WriteLine("       [-d] [--debug] Launch debug session");
                Console.Read();
            }

			Logger.Info("program exiting 1....");
			Dictionary.Instance.Save();
            Logger.Info("program exiting 2....");
        }
    }
}
