using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MedivatorsCleantrackParser
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if ((args.Length == 1) && (args[0].ToUpper() == "-D" || args[0].ToUpper() == "--DEBUG"))
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					 new MedivatorsService()
				};
				ServiceBase.Run(ServicesToRun);

			}

		}
	}
}
