using System;
using System.Globalization;
using System.IO;
using System.ServiceProcess;

namespace ISAWD_PARSER
{
    static class Program
    {
		public static DateTime FromFilenameToDate(string sFilename)
		{
			DateTime dtTemp = DateTime.MinValue;

			if (sFilename.Length >= 16)
			{
				try
				{
					string sTemp = sFilename.Substring(0, 16);
					dtTemp = DateTime.ParseExact(sTemp, "yyyy-MM-dd HH.mm", CultureInfo.InvariantCulture);
				}
				catch (Exception v)
				{
					WriteLog("FromFilenameToDate", "ERROR", v.ToString());
					dtTemp = DateTime.MinValue;
				}
			}

			return dtTemp;
		}

		public static void WriteLog(string sFunction, string sTitle, string sText)
		{
			try
			{
				DateTime dtNow = DateTime.Now;

				string sLogName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + dtNow.ToString("yyyyMMdd") + ".log";

				StreamWriter swTemp = new StreamWriter(sLogName, true);
				swTemp.WriteLine(dtNow.ToString("HH:mm:ss.fff") + " " + sFunction + " --> " + sTitle + ": " + sText);
				swTemp.Close();
			}
			catch
			{
			}
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

                // SetupLog();
                ServiceBase.Run(ServicesToRun);
            }
            else
            if (args[0] == "-d" || args[0] == "-D")
            {
                Service nt = null;
                try
                {
                    // SetupLog();
                    nt = new Service();
                    nt.Start(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);

                }
                Console.ReadKey();
            }
            else
                if (args[0] == "-x" || args[0] == "-X")
                {
                    Service nt = null;
                    try
                    {
                        // SetupLog();
                        nt = new Service();
                        nt.CleanHistoryDB();
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
