using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MedivatorsCleantrackParser
{
	public static class AmRfidManagerExtension
	{
		public static void insertNewCycleMedivators(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, DateTime date, Cycle cycle, string connectionString)
		{
			int id = amrfidmgrex.DBUtilities.insertnewCycleWithDateForReturn(deviceTag, userTag, cleaner, state, oldState, idExamToSave, date, connectionString);

			SDBUtil.update("CICLI", id, "IDVasca", "'" + cycle.Station + "'", "ID", connectionString);
			SDBUtil.update("CICLI", id, "MachineCycleId", "" + cycle.CycleCount, "ID", connectionString);
			SDBUtil.update("CICLI", id, "DisinfectantCycleId", "" + cycle.DisinfectantCycleCount, "ID", connectionString);

			foreach (AdditionalInfo info in cycle.AdditionalInfoList)
			{
				if (info.Description.Trim() == "Ratio=")
				{
					info.Value = FormatRatio(info.Value.Trim());
				}

				int newId = SDBUtil.InsertNewCyclePlus(id, connectionString);
				SDBUtil.update("CICLIEXT", newId, "Descrizione", "'" + info.Description + "'", "ID", connectionString);
				SDBUtil.update("CICLIEXT", newId, "Valore", "'" + info.Value + "'", "ID", connectionString);

				try
				{
					string stringDate = info.Date.ToString("yyyyMMddHHmmss");
					SDBUtil.update("CICLIEXT", newId, "Data", "'" + stringDate + "'", "ID", connectionString);
				}
				catch (Exception e)
				{
					writeLog("Error in AdditionalInfoDate: " + e.ToString());
				}

				SDBUtil.update("CICLIEXT", newId, "Error", "" + (info.isAlarm ? 1 : 0), "ID", connectionString);
			}
		}


		private static string FormatRatio(string p)
		{
			string newval = p;

			if (p.Length == 9)
			{
				try {
					newval = p.Substring(0, 3) + ":" + p.Substring(3, 3) + ":" + p.Substring(6, 2) + "." + p.Substring(8, 1);
				}
				catch {
					newval = p;
				}
			}

			return newval;
		}


		private static void writeLog(string text)
		{
			try
			{
				string logName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
				StreamWriter sw = new StreamWriter(logName, true);
				sw.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
				sw.Close();
			}
			catch
			{ }
		}


	}
}
