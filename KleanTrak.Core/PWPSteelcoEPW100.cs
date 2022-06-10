using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	public class PWPSteelcoEPW100 : WPBase
    {
		private string sWasherMatch = "Numero di fabbrica: ";
		private string sWasherAddress = "Indirizzo: ";
		private string sStartDateMatch = "Data inizio ciclo: ";
		private string sStartTimeMatch = "Ora inizio ciclo: ";
		private string sEndDateMatch = "Data fine ciclo: ";
		private string sEndTimeMatch = "Ora fine ciclo: ";
		private string sJobCodeMatch = "Codice Commessa: ";
		private string sCycleTypeMatch = "Numero Ciclo PLC: ";
		private string sCleaningOperatorMatch = "Operatore PLC: ";
		private string sCleaningSuccess = "Esito: ";
		private string sCycleNumberMatch = "Ciclo Macchina: ";
		private string[] endoscopes = { "Strumento 1: ", "Strumento 2: ", "Strumento 3: ", "Strumento 4: ", "Strumento 5: ", "Strumento 6: ", "Strumento 7: ", "Strumento 8: " };
		private string sUnloadOperatorMatch = "Operatore scarico: ";

		private string sCycleOK = "Ciclo OK";

		public PWPSteelcoEPW100()
		{
		}

		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
			List<WasherCycle> listReturn = new List<WasherCycle>();

            Logger.Info("start");

			List<String> listFileToParse = CheckDirectory(washer.Code, lastDate);

			if (listFileToParse.Count > 0)
			{
				listFileToParse.Sort();
				try
				{
					foreach (String sFile in listFileToParse)
					{
						List<WasherCycle> listWCTemp = LoadInfos(sFile);

						foreach (WasherCycle wcTemp in listWCTemp)
						{
							listReturn.Add(wcTemp);
						}
					}
				}
				catch (Exception ex)
				{
                    Logger.Error(ex);
				}
			}

            Logger.Info("end");

			return listReturn;
		}

		public List<String> CheckDirectory(string sWasherCode, DateTime lastDate)
		{
			List<String> listReturn = new List<string>();

			if (sWasherCode.Length > 0)
			{
				if (FolderOrFileName != null)
				{
					if (FolderOrFileName.Length > 0)
					{
						try
						{
							String[] sFileArray = Directory.GetFiles(FolderOrFileName, "MM" + sWasherCode + "*.txt");

							if (sFileArray.Length > 0)
							{
								Array.Sort(sFileArray);

								foreach (String sFile in sFileArray)
								{
									if (FromFileToDate(sFile) > lastDate)
									{
										Logger.Info("Add file: " + sFile);
										listReturn.Add(sFile);
									}
								}
							}
						}
						catch (Exception ex)
						{
                            Logger.Error(ex);
						}
					}
					else
					{
						Logger.Warn("- CheckDirectory --> Folder path is empty");
					}
				}
				else
				{
                    Logger.Warn("- CheckDirectory --> Folder path is null");
				}
			}
			else
			{
                Logger.Warn("- CheckDirectory --> Washer code is empty");
			}

			return listReturn;
		}

		private DateTime FromFileToDate(string sFileCompleteName)
		{
			StreamReader srTemp = new StreamReader(new FileStream(sFileCompleteName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			string sFileContent = srTemp.ReadToEnd();
			srTemp.Close();

			String sFinishDate = GetDate(sFileContent, sEndDateMatch, sEndTimeMatch);

			DateTime dtTemp = DateTime.MinValue;
			if (sFinishDate.Length >= 12)
			{
				try
				{
					string sTemp = sFinishDate.Substring(0, 12);
					dtTemp = DateTime.ParseExact(sTemp, "yyMMddHHmmss", CultureInfo.InvariantCulture);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
					dtTemp = DateTime.MinValue;
				}
			}

			return dtTemp;
		}

		public string GetDate(string text, string regDate, string regTime)
		{
			string date = "";
			string time = "";

			Match match0 = Regex.Match(text, regDate + ".*");
			if (match0.Success)
			{
				char[] delim = { '\n', '\r', ' ', ':', '.' };

				date = match0.Value.Trim(delim);

				date = date.Replace(regDate, "");
				date = date.Replace("\n", "");
				date = date.Replace("\r", "");
				date = date.Replace(": ", "");
				date = date.Replace(".", "");
				date = date.Trim();
			}

			Match match1 = Regex.Match(text, regTime + ".*");
			if (match1.Success)
			{
				char[] delim = { '\n', '\r', ' ', ':', '.' };

				time = match1.Value.Trim(delim);

				time = time.Replace(regTime, "");
				time = time.Replace("\n", "");
				time = time.Replace("\r", "");
				time = time.Replace(": ", "");
				time = time.Replace(".", "");
				time = time.Trim();
			}

			if (date.Length >= 6)
				return date.Substring(4, 2) + date.Substring(2, 2) + date.Substring(0, 2) + time;

			return "";
		}

		public List<WasherCycle> LoadInfos(String sPath)
		{
			List<WasherCycle> listWCReturn = new List<WasherCycle>();

			StreamReader sr = new StreamReader(sPath);
			String fileContent = sr.ReadToEnd();
			sr.Close();

			Logger.Info("- FILE --> " + sPath);

			//bool bFailed = !GetSuccess(fileContent);

			string sWasherExternalID = GetWasher(fileContent);
			int iWasherID = TranscodeWasher(sWasherExternalID);
			if (iWasherID <= 0)
				iWasherID = TranscodeWasher("UNKNOWN");
			Logger.Info("--- \t MACCHINA: " + sWasherExternalID + " (" + iWasherID.ToString() + ")");

			string sOperatorStartExternalID = GetOperator(fileContent, false);
			int iOperatorStartID = TranscodeOperator(sOperatorStartExternalID);
			if (iOperatorStartID <= 0)
				iOperatorStartID = TranscodeOperator("UNKNOWN");
            Logger.Info("--- \t OPERATORE START: " + sOperatorStartExternalID + " (" + iOperatorStartID.ToString() + ")");

			string sOperatorEndExternalID = GetOperator(fileContent, true);
			int iOperatorEndID = TranscodeOperator(sOperatorEndExternalID);
			if (iOperatorEndID <= 0)
				iOperatorEndID = TranscodeOperator("UNKNOWN");
            Logger.Info("--- \t OPERATORE END: " + sOperatorEndExternalID + " (" + iOperatorEndID.ToString() + ")");

			DateTime dtStartTimestamp = DateTime.ParseExact(GetDate(fileContent, sStartDateMatch, sStartTimeMatch), "yyMMddHHmmss", CultureInfo.InvariantCulture);
            Logger.Info("--- \t DATA INIZIO: " + dtStartTimestamp);

			DateTime dtEndTimestamp = DateTime.ParseExact(GetDate(fileContent, sEndDateMatch, sEndTimeMatch), "yyMMddHHmmss", CultureInfo.InvariantCulture);
            Logger.Info("--- \t DATA FINE: " + dtEndTimestamp);

			string sStationName = "";

			int iCycleCount = GetCycleNumber(fileContent);

			string sCycleType = GetCycleType(fileContent);

			string sFilename = sPath;

			DateTime dtFileDatetime = dtEndTimestamp;

			string sFileContent = fileContent;

			List<WasherCycleInfo> listAdditionalInfo = new List<WasherCycleInfo>();

			//

			int iEventiCicloIndex = fileContent.IndexOf("-Eventi Ciclo]");
			int iConsumiCicloIndex = fileContent.IndexOf("-Consumi Ciclo]");

			if ((iEventiCicloIndex > 0) && (iConsumiCicloIndex > 0))
			{
				iEventiCicloIndex += 16;
				iConsumiCicloIndex -= 8;

				string sAdditionalInfoComplete = fileContent.Substring(iEventiCicloIndex, iConsumiCicloIndex - iEventiCicloIndex);

				string[] sSplitChar = { Environment.NewLine };
				string[] sSplitted = sAdditionalInfoComplete.Split(sSplitChar, StringSplitOptions.RemoveEmptyEntries);
				if (sSplitted != null)
				{
					for (int i = 0; i < sSplitted.Length; i++)
					{
						int iLineIndex1 = sSplitted[i].IndexOf("-");
						int iLineIndex2 = sSplitted[i].IndexOf(":", iLineIndex1 + 1);
						int iLineIndex3 = sSplitted[i].IndexOf("\"", iLineIndex2 + 1);
						int iLineIndex4 = sSplitted[i].IndexOf("\"", iLineIndex3 + 1);

						if ((iLineIndex1 > 0) && (iLineIndex2 > 0) && (iLineIndex3 > 0) && (iLineIndex4 > 0))
						{
							string sDescrizione = sSplitted[i].Substring(iLineIndex1 + 1, iLineIndex2 - (iLineIndex1 + 1));
							string sOra = sSplitted[i].Substring(iLineIndex2 + 2, iLineIndex3 - (iLineIndex2 + 3));
							string sValore = sSplitted[i].Substring(iLineIndex3 + 1, iLineIndex4 - (iLineIndex3 + 1));

							DateTime dtCicliExt = DateTime.MinValue;
							try
							{
								string sTemp = GetDate(fileContent, sStartDateMatch, sStartTimeMatch).Substring(0, 6) + " " + sOra;
								dtCicliExt = DateTime.ParseExact(sTemp, "yyMMdd HH.mm.ss", CultureInfo.InvariantCulture);
							}
							catch (Exception)
							{
							}

							//

							WasherCycleInfo info = new WasherCycleInfo();

							info.Description = sDescrizione;
							info.Value = sValore;
							info.Date = dtCicliExt;
							info.isAlarm = false;

							listAdditionalInfo.Add(info);
						}
					}
				}
			}

			listAdditionalInfo.Reverse();

			//

			int iNum = 0;
			foreach (string sCD in endoscopes)
			{
				string sDeviceExternalID = GetDevice(fileContent, sCD);
				int iDeviceID = TranscodeDevice(sDeviceExternalID);

				iNum++;

				Logger.Info("--- \t DEVICE " + iNum.ToString() + ": " + sDeviceExternalID + " (" + iDeviceID.ToString() + ")");

				if (iDeviceID > 0)
				{
					// le steelco vanno dirette a chiusura ciclo
					// per ogni dispositivo aggiungo ciclo di inizio e fine lavaggio
					WasherCycle startcycle = new WasherCycle 
					{ 
						Completed = false,
						Failed = false,
						WasherExternalID = sWasherExternalID,
						WasherID = iWasherID,
						DeviceExternalID = sDeviceExternalID,
						DeviceID = iDeviceID,
						OperatorStartExternalID = sOperatorStartExternalID,
						OperatorStartID = iOperatorStartID,
						StartTimestamp = dtStartTimestamp,
						StationName = sStationName,
						CycleCount = iCycleCount.ToString(),
						CycleType = sCycleType,
						Filename = sFilename,
						FileDatetime = dtFileDatetime,
						FileContent = sFileContent,
						AdditionalInfoList = listAdditionalInfo,
						DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_pre_wash),
					};
					var endcycle = new WasherCycle 
					{
						Completed = true,
						Failed = !GetSuccess(fileContent),
						WasherExternalID = sWasherExternalID,
						WasherID = iWasherID,
						DeviceExternalID = sDeviceExternalID,
						DeviceID = iDeviceID,
						OperatorStartExternalID = sOperatorStartExternalID,
						OperatorStartID = iOperatorStartID,
						OperatorEndExternalID = sOperatorEndExternalID,
						OperatorEndID = iOperatorEndID,
						StartTimestamp = dtStartTimestamp,
						EndTimestamp = dtEndTimestamp,
						StationName = sStationName,
						CycleCount = iCycleCount.ToString(),
						CycleType = sCycleType,
						Filename = sFilename,
						FileDatetime = dtFileDatetime,
						FileContent = sFileContent,
						AdditionalInfoList = listAdditionalInfo,
						DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_pre_wash)
					};
					listWCReturn.Add(startcycle);
					listWCReturn.Add(endcycle);
				}
			}

			return listWCReturn;
		}

		private WasherCycleInfo ExtractInfo(string sInput, string sData)
		{
			if (sInput.Length < 8)
				return null;

			WasherCycleInfo info = new WasherCycleInfo();

			try
			{
				info.Date = DateTime.ParseExact(sData + (sInput.Substring(0, 8).Replace(":", "")), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
			}
			catch
			{
				info.Date = DateTime.MinValue;
			}

			if (sInput.Length > 8)
			{
				info.Description = sInput.Substring(8).Trim();
				if (info.Description.ToUpper().Contains("ALLARME"))
					info.isAlarm = true;
			}

			return info;
		}

		public string GetWasher(string text)
		{
			string sReturn = "";

			Match match = Regex.Match(text, sWasherMatch + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.', '|' };

				sReturn = match.Value.Trim(delim);

				sReturn = sReturn.Replace(sWasherMatch, "");
				sReturn = sReturn.Replace("\n", "");
				sReturn = sReturn.Replace("\r", "");
				sReturn = sReturn.Replace(": ", "");
				sReturn = sReturn.Replace(".", "");
				sReturn = sReturn.Replace("|", "");
				sReturn = sReturn.Trim();
			}

			return sReturn;
		}

		public string GetOperator(string text, bool unloadOperator)
		{
			string sReturn = "";

			Match match = Regex.Match(text, unloadOperator ? sUnloadOperatorMatch : sCleaningOperatorMatch + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.' };

				sReturn = match.Value.Trim(delim);

				sReturn = sReturn.Replace(unloadOperator ? sUnloadOperatorMatch : sCleaningOperatorMatch, "");
				sReturn = sReturn.Replace("\n", "");
				sReturn = sReturn.Replace("\r", "");
				sReturn = sReturn.Replace(": ", "");
				sReturn = sReturn.Replace(".", "");
				sReturn = sReturn.Trim();
			}

			return sReturn;
		}

		public int GetCycleNumber(string text)
		{
			string sCycleNum = "";

			Match match = Regex.Match(text, sCycleNumberMatch + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.', '|' };

				sCycleNum = match.Value.Trim(delim);

				sCycleNum = sCycleNum.Replace(sCycleNumberMatch, "");
				sCycleNum = sCycleNum.Replace("\n", "");
				sCycleNum = sCycleNum.Replace("\r", "");
				sCycleNum = sCycleNum.Replace(": ", "");
				sCycleNum = sCycleNum.Replace(".", "");
				sCycleNum = sCycleNum.Replace("|", "");
				sCycleNum = sCycleNum.Trim();
			}

			int iReturn = 0;
			Int32.TryParse(sCycleNum, out iReturn);
			return iReturn;
		}

		public string GetCycleType(string text)
		{
			string sReturn = "";

			Match match = Regex.Match(text, sCycleTypeMatch + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.', '|' };

				sReturn = match.Value.Trim(delim);

				sReturn = sReturn.Replace(sCycleTypeMatch, "");
				sReturn = sReturn.Replace("\n", "");
				sReturn = sReturn.Replace("\r", "");
				sReturn = sReturn.Replace(": ", "");
				sReturn = sReturn.Replace(".", "");
				sReturn = sReturn.Replace("|", "");
				sReturn = sReturn.Trim();
			}

			return sReturn.Substring(0, sReturn.IndexOf(' '));
		}

		public string GetDevice(string text, string cDev)
		{
			Match match = Regex.Match(text, cDev + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.', '|' };

				string sReturn = match.Value.Trim(delim);

				sReturn = sReturn.Replace(cDev, "");
				sReturn = sReturn.Replace("\n", "");
				sReturn = sReturn.Replace("\r", "");
				sReturn = sReturn.Replace(": ", "");
				sReturn = sReturn.Replace(".", "");
				sReturn = sReturn.Replace("|", "");
				sReturn = sReturn.Trim();

				if(sReturn.IndexOf(' ') > 0)
					sReturn = sReturn.Substring(0, sReturn.IndexOf(' '));

				if (sReturn == "-")
					sReturn = "";

				return sReturn;
			}

			return "";
		}

		public string GetEsito(string text)
		{
			string sReturn = "";

			Match match = Regex.Match(text, sCleaningSuccess + ".*");
			if (match.Success)
			{
				char[] delim = { '\n', '\r', /*' ', ':',*/ '.' };

				sReturn = match.Value.Trim(delim);

				sReturn = sReturn.Replace(sCleaningSuccess, "");
				sReturn = sReturn.Replace("\n", "");
				sReturn = sReturn.Replace("\r", "");
				sReturn = sReturn.Replace(": ", "");
				sReturn = sReturn.Replace(".", "");
				sReturn = sReturn.Trim();
			}

			return sReturn;
		}

		public bool GetSuccess(string text)
		{
			string sReturn = GetEsito(text);

			if (sReturn.Length > 0)
				return (sReturn.IndexOf(sCycleOK) >= 0);

			return false;
		}
	}
}
