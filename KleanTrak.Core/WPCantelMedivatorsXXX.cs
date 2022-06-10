using System;
using System.Collections.Generic;
using System.IO;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	class WPCantelMedivatorsXXX : WPBase
    {
        public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
			List<WasherCycle> listCyclesToReturn = new List<WasherCycle>();

			try
			{
				String sMedivatorDb = washer.FolderOrFileName;
				if ((sMedivatorDb == null) || (sMedivatorDb.Length < 4))
				{
					sMedivatorDb = ".\\Medivators.Reporting.mdb";
				}
				else
				{
					if (!String.Equals(sMedivatorDb.Substring(sMedivatorDb.Length - 4), ".mdb", StringComparison.OrdinalIgnoreCase))
					{
						if (!String.Equals(sMedivatorDb.Substring(sMedivatorDb.Length - 1), "\\"))
							sMedivatorDb += "\\";

						sMedivatorDb += "Medivators.Reporting.mdb";
					}
				}

				if (File.Exists(sMedivatorDb))
				{
					MedivatorsDBAnalyzer medDb = new MedivatorsDBAnalyzer();
					medDb.UID = washer.User;
					medDb.DBQ = sMedivatorDb;
					medDb.PWD = washer.Password;

					// writeLog("Extracting cycles ...");

					List<WasherCycle> cyclesToParse = medDb.Extract(lastDate);

					foreach (WasherCycle serieToParse in cyclesToParse)
					{
						if (IsValid(serieToParse.FileDatetime) && IsValid(serieToParse.StartTimestamp))
						{
							WasherCycle wcTemp = new WasherCycle();

							// wcTemp.Completed = ;
							// wcTemp.Failed = ;

							wcTemp.WasherExternalID = serieToParse.WasherExternalID;
							wcTemp.WasherID = TranscodeWasher(wcTemp.WasherExternalID);

							wcTemp.DeviceExternalID = serieToParse.DeviceExternalID;
							wcTemp.DeviceID = TranscodeDevice(wcTemp.DeviceExternalID);
							if (wcTemp.DeviceID <= 0)
								wcTemp.DeviceID = TranscodeDevice("UNKNOWN");

							wcTemp.OperatorStartExternalID = serieToParse.OperatorStartExternalID;
							wcTemp.OperatorStartID = TranscodeOperator(wcTemp.OperatorStartExternalID);
							if (wcTemp.OperatorStartID <= 0)
								wcTemp.OperatorStartID = TranscodeOperator("UNKNOWN");

							wcTemp.OperatorEndExternalID = serieToParse.OperatorEndExternalID;
							wcTemp.OperatorEndID = TranscodeOperator(wcTemp.OperatorEndExternalID);
							if (wcTemp.OperatorEndID <= 0)
								wcTemp.OperatorEndID = TranscodeOperator("UNKNOWN");

							wcTemp.StartTimestamp = serieToParse.StartTimestamp;
							wcTemp.EndTimestamp = serieToParse.EndTimestamp;
							// wcTemp.StationName = ;
							wcTemp.CycleCount = serieToParse.CycleCount;
							// wcTemp.Type
							// wcTemp.Filename
							wcTemp.FileDatetime = serieToParse.FileDatetime;
							// wcTemp.FileContent

							wcTemp.AdditionalInfoList = serieToParse.AdditionalInfoList;

							listCyclesToReturn.Add(wcTemp);
						}
					}
				}
				else
				{
					Logger.Error("Path DB Medivators non raggiungibile ...");
				}
			}
			catch (Exception e)
			{
                Logger.Error(e);
			}

			return listCyclesToReturn;
		}

		private bool IsValid(DateTime dt)
		{
			return dt != null && dt != DateTime.MinValue && dt != DateTime.MaxValue && !(dt.Hour == 0 && dt.Minute == 0);
		}
	}
}
