using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace TRfid
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if ((args.Length == 1) && (args[0].ToUpper() == "-D" || args[0].ToUpper() == "--DEBUG"))
				Debug();
			else
                if ((args.Length == 3) && (args[0].ToUpper() == "-X" || args[0].ToUpper() == "--EXECUTE"))
					UpdateDB(args[1], args[2]);
                else
                    RunService();
		}

        private static void UpdateDB(string devTag, string opTag)
        {
            var service = new Service();
            service.UpdateDB(devTag, opTag);
            service.Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
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
				(new Service()).DebugService();
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
