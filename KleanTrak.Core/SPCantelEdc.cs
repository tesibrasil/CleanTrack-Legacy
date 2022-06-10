using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	class SPCantelEdc : WPBase
	{
		private enum CantelColumns
		{
			Date,
			Week,
			Time,
			Machine,
			CycleNo,
			CycleType,
			CycleTime,
			CycleStatus,
			FailReason, // (FaultCode) 
			StatusMessage,
			HookNo,
			ScopeSerial,
			GS1,
			Manufacturer,
			Model,
			LoadOperatorID,
			LoadOperator,
			UnloadOperatorID,
			UnloadOperator,
			Ignore1,
			Ignore2,
			Ignore3,
			Ignore4,
			Ignore5,
			Ignore6,
			Channel1FlowCtrl,
			Channel2FlowCtrl,
			Channel3FlowCtrl,
			Channel4FlowCtrl,
			Channel5FlowCtrl,
			Channel6FlowCtrl,
			ChannelTempCtrl,
			ChannelRHCtrl,
			CabinetTempCtrl,
			CabinetRHCtrl,
			Ignore7,
			Ignore8,
			Channel1FlowIms,
			Channel2FlowIms,
			Channel3FlowIms,
			Channel4FlowIms,
			Channel5FlowIms,
			Channel6FlowIms,
			ChannelTempIms,
			ChannelRHIms,
			CabinetTempIms,
			CabinetRHIms,
			Ignore9
		}

		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
		{
			List<WasherCycle> retcycles = new List<WasherCycle>();
			if (!CheckWasherSerialNumber(washer))
				return retcycles;
			string[] sFiles = Directory.GetFiles(FolderOrFileName, washer.SerialNumber + "_ProcessLog_*.csv");
			Array.Sort(sFiles);
			foreach (string sFile in sFiles)
			{
				if (sFile.Contains(OldDir))
					continue;
				Logger.Info("Reading file: " + sFile);
				string[] sFileLines = File.ReadAllLines(sFile);

				if (sFileLines.Length == 1)
				{
					Logger.Info("ok 1");
					string[] sFileSplitted = sFileLines[0].Split(',');

					if (sFileSplitted.Length >= (int)CantelColumns.Ignore1) // Enum.GetValues(typeof(CantelColumns)).Length)
					{
						Logger.Info("ok 2");
						DateTime dtFile = FromStringsToDateTime(sFileSplitted[(int)CantelColumns.Date], sFileSplitted[(int)CantelColumns.Time]);
						if (dtFile >= lastDate)
						{
							Logger.Info("ok 3");
							int iFinalNum = FromFilenameToFinalNumber(sFile);
							if ((iFinalNum == 3) || (iFinalNum == 4))
							{
								Logger.Info(sFile + " --> parsing...");
								retcycles.Add(ParseFile(iFinalNum, sFile, sFileLines[0], dtFile, sFileSplitted));
								MoveToOldDir(sFile);
							}
							else
							{
								Logger.Warn(sFile + " --> Wrong final number (" + iFinalNum.ToString() + ", expected 3 or 4)");
								MoveToOldDir(sFile);
							}
						}
						else
						{
							Logger.Warn(sFile + " --> Wrong date time (" + dtFile.ToString() + ", expected min " + lastDate.ToString() + ")");
							MoveToOldDir(sFile);
						}
					}
					else
					{
						Logger.Warn(sFile + " --> Wrong columns number (" + sFileSplitted.Length.ToString() + ", expected " + Enum.GetValues(typeof(CantelColumns)).Length.ToString() + ")");
						MoveToOldDir(sFile);
					}
				}
				else
				{
					Logger.Warn(sFile + " --> Wrong lines number (" + sFileLines.Length.ToString() + ", expected 1)");
					MoveToOldDir(sFile);
				}
			}

			return retcycles;
		}

		private WasherCycle ParseFile(int iFinalNum, string sFilename, string sFileContent, DateTime dtFileDatetime, string[] sFileSplitted)
		{
			const int inizioCiclo = 3;
			const int fineCiclo = 4;

			WasherCycle wcReturn = new WasherCycle();
			wcReturn.Completed = true;
			wcReturn.IsStorage = true;
			wcReturn.Failed = iFinalNum != inizioCiclo && !String.Equals(sFileSplitted[(int)CantelColumns.CycleStatus], "PASS", StringComparison.OrdinalIgnoreCase);
			wcReturn.WasherExternalID = sFileSplitted[(int)CantelColumns.Machine];
			wcReturn.WasherID = TranscodeWasher(wcReturn.WasherExternalID);
			wcReturn.DeviceExternalID = sFileSplitted[(int)CantelColumns.ScopeSerial];
			wcReturn.DeviceID = TranscodeDevice(wcReturn.DeviceExternalID);
			wcReturn.OperatorStartExternalID = sFileSplitted[(int)CantelColumns.LoadOperator];
			wcReturn.OperatorStartID = TranscodeOperator(wcReturn.OperatorStartExternalID);
			wcReturn.OperatorEndExternalID = sFileSplitted[(int)CantelColumns.UnloadOperator];
			wcReturn.OperatorEndID = TranscodeOperator(wcReturn.OperatorEndExternalID);
			wcReturn.StartTimestamp = dtFileDatetime;
			wcReturn.EndTimestamp = dtFileDatetime; // TranscodeEndTimestamp(dtFileDatetime, sFileSplitted[(int)CantelColumns.CycleTime]);
			wcReturn.StationName = sFileSplitted[(int)CantelColumns.Machine];
			wcReturn.CycleCount = TranscodeCycleCount(sFileSplitted[(int)CantelColumns.CycleNo]).ToString();
			wcReturn.CycleType = TranscodeCycleType(sFileSplitted[(int)CantelColumns.CycleType]);
			wcReturn.Filename = sFilename;
			wcReturn.FileDatetime = dtFileDatetime;
			wcReturn.FileContent = sFileContent;
			for (int i = 0; i < Math.Min(sFileSplitted.Length, Enum.GetValues(typeof(CantelColumns)).Length); i++)
			{
				WasherCycleInfo wciTemp = new WasherCycleInfo();
				wciTemp.Description = Enum.GetName(typeof(CantelColumns), i);
				wciTemp.Value = sFileSplitted[i];
				wciTemp.Date = dtFileDatetime;
				wciTemp.isAlarm = false;
				wcReturn.AdditionalInfoList.Add(wciTemp);
			}
			switch (iFinalNum)
			{
				case inizioCiclo: // inizio ciclo //
					wcReturn.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_store);
					wcReturn.Completed = false;
					break;
				case fineCiclo: // fine ciclo //
					wcReturn.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_store);
					wcReturn.Completed = true;
					break;
			}
			return wcReturn;
		}
		private int FromFilenameToFinalNumber(string sFilename)
		{
			int iReturn = 0;

			int iMinus = sFilename.LastIndexOf('-');
			int iDot = sFilename.LastIndexOf('.');

			if ((iMinus > 0) && (iDot > iMinus))
			{
				string sParse = sFilename.Substring(iMinus + 1, iDot - (iMinus + 1));

				try
				{
					iReturn = Int32.Parse(sParse);
				}
				catch (Exception)
				{
				}
			}

			return iReturn;
		}

		private DateTime FromStringsToDateTime(string sDate, string sTime)
		{
			DateTime dtReturn = DateTime.MinValue;

			try
			{
				dtReturn = DateTime.ParseExact(sDate + " " + sTime, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
			}

			return dtReturn;
		}

		DateTime TranscodeEndTimestamp(DateTime dtStartTimestamp, string sCycleTime)
		{
			DateTime dtReturn = dtStartTimestamp;

			int iColon = sCycleTime.IndexOf(":");
			if (iColon > 0)
			{
				string sHours = sCycleTime.Substring(0, iColon);
				string sMinutes = sCycleTime.Substring(iColon + 1);

				int iHours = 0;
				try
				{
					iHours = Int32.Parse(sHours);
				}
				catch (Exception)
				{
				}

				int iMinutes = 0;
				try
				{
					iMinutes = Int32.Parse(sMinutes);
				}
				catch (Exception)
				{
				}

				dtReturn = dtStartTimestamp.AddHours(iHours).AddMinutes(iMinutes);
			}

			return dtReturn;
		}

		int TranscodeCycleCount(string sCycleCount)
		{
			int iReturn = 0;

			try
			{
				iReturn = Int32.Parse(sCycleCount);
			}
			catch
			{
			}

			return iReturn;
		}

		string TranscodeCycleType(string sCycleType)
		{
			string sReturn = sCycleType;

			//

			return sReturn;
		}
	}
}
