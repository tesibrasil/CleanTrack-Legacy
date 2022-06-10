using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	class WPISAWD : WPBase
    {
        private const string m_sNumeroSerialeFile = "NUMERO SERIALE:";
        private const string m_sInizioCicloFile = "INIZIO CICLO:";
        private const string m_sStrumentoFile = "STRUMENTO:";
        private const string m_sMatricolaFile = "MATRICOLA:";
        private const string m_sOperatoreFile = "OPERATORE:";
        private const string m_sMedicoFile = "MEDICO:";
        private const string m_sPazienteFile = "PAZIENTE:";
        private const string m_sTipoCicloFile = "TIPO CICLO:";
        private const string m_sNumeroCicloFile = "NUMERO CICLO:";
        private const string m_sStazioneFile = "STAZIONE:";
        private const string m_sPrelevatoDaFile = "PRELEVATO DA:";

        public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
            List<string> listFilesToParse = new List<string>();
			List<string> fileToMoveInOld = new List<string>();
            string[] sFiles = Directory.GetFiles(FolderOrFileName);
            foreach (string sFile in sFiles)
            {
				if (FromFilenameToDate(sFile) > lastDate)
				{
					Logger.Info("Add file: " + sFile);
					listFilesToParse.Add(sFile);
				}
				else
				{
					fileToMoveInOld.Add(sFile);
				}
            }
			foreach (string file in fileToMoveInOld)
				MoveToOldDir(file);
            // per ogni nuovo scontrino trovato... //
            List<WasherCycle> ret = new List<WasherCycle>();
            foreach (string sFile in listFilesToParse)
            {
                WasherCycle cycle = ParseFile(sFile);
				if (cycle != null)
				{
					// add fake start cycle 
					// gli scontrini vengono emessi
					// solo in chiusura del ciclo
					ret.Add(GetStartFakeCycle(cycle));
                    ret.Add(cycle);
				}
            }
			foreach (string file in listFilesToParse)
				MoveToOldDir(file);
			Logger.Info($"retruning list of {ret.Count} cycles");
            return ret;
        }

        private DateTime FromFilenameToDate(string sFileCompleteName)
        {
            string sFilename = Path.GetFileName(sFileCompleteName);
            DateTime dtTemp = DateTime.MinValue;

            if (sFilename.Length >= 16)
            {
                try
                {
                    string sTemp = sFilename.Substring(0, 16);
                    dtTemp = DateTime.ParseExact(sTemp, "yyyy-MM-dd HH.mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    dtTemp = DateTime.MinValue;
                }
            }

            return dtTemp;
        }

        private int GetNextReturn(string sInput, int iStart)
        {
            if (iStart < sInput.Length)
            {
                return sInput.IndexOf(Environment.NewLine, iStart);
            }
            else
            {
                throw new Exception();
            }
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

        private WasherCycle ParseFile(string sFile)
        {
			string sText = "";

			try
			{
				bool bAlreadyProcessedFile = IsFileAlreadyProcessed(sFile);
				StreamReader srTemp = new StreamReader(new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
				sText = srTemp.ReadToEnd();
				srTemp.Close();

				int iIndexNumeroCiclo = sText.IndexOf(m_sNumeroCicloFile) + m_sNumeroCicloFile.Length;

				System.Text.RegularExpressions.Regex myMatcher = new System.Text.RegularExpressions.Regex(@"[0-9][0-9]:[0-9][0-9]:[0-9][0-9]");
				Match myMatch = myMatcher.Match(sText, iIndexNumeroCiclo);

				int iIndexAdditionalInfo = -1;
				if (myMatch.Success)
					iIndexAdditionalInfo = myMatch.Index;

				string sSeriale = "";
				try
				{
					if (sText.Contains(m_sNumeroSerialeFile))
					{
						int iIndexSeriale = sText.IndexOf(m_sNumeroSerialeFile) + m_sNumeroSerialeFile.Length;
						sSeriale = sText.Substring(iIndexSeriale, GetNextReturn(sText, iIndexSeriale) - iIndexSeriale).Trim();
					}
				}
				catch
				{
				}

				string sInizioCiclo = "";
				try
				{
					if (sText.Contains(m_sInizioCicloFile))
					{
						int iIndexInizioCiclo = sText.IndexOf(m_sInizioCicloFile) + m_sInizioCicloFile.Length;
						sInizioCiclo = sText.Substring(iIndexInizioCiclo, GetNextReturn(sText, iIndexInizioCiclo) - iIndexInizioCiclo).Trim();
					}
				}
				catch
				{
				}

				string sStrumento = "";
				try
				{
					if (sText.Contains(m_sStrumentoFile))
					{
						int iIndexStrumento = sText.IndexOf(m_sStrumentoFile) + m_sStrumentoFile.Length;
						sStrumento = sText.Substring(iIndexStrumento, GetNextReturn(sText, iIndexStrumento) - iIndexStrumento).Trim();
					}
				}
				catch
				{
				}

				string sMatricola = "";
				try
				{
					if (sText.Contains(m_sMatricolaFile))
					{
						int iIndexMatricola = sText.IndexOf(m_sMatricolaFile) + m_sMatricolaFile.Length;
						sMatricola = sText.Substring(iIndexMatricola, GetNextReturn(sText, iIndexMatricola) - iIndexMatricola).Trim();
					}
				}
				catch
				{
				}

				string sOperatoreStart = "";
				try
				{
					if (sText.Contains(m_sOperatoreFile))
					{
						int iIndexOperatoreStart = sText.IndexOf(m_sOperatoreFile) + m_sOperatoreFile.Length;
						sOperatoreStart = sText.Substring(iIndexOperatoreStart, GetNextReturn(sText, iIndexOperatoreStart) - iIndexOperatoreStart).Trim();
					}
				}
				catch
				{
				}

				string sOperatoreEnd = "";
				try
				{
					if (sText.Contains(m_sPrelevatoDaFile))
					{
						int iIndexOperatoreEnd = sText.IndexOf(m_sPrelevatoDaFile) + m_sPrelevatoDaFile.Length;
						sOperatoreEnd = sText.Substring(iIndexOperatoreEnd, GetNextReturn(sText, iIndexOperatoreEnd) - iIndexOperatoreEnd).Trim();
					}
				}
				catch
				{
				}

				/*
				string sMedico = "";
				try
				{
					if (sText.Contains(m_sMedicoFile))
					{
						int iIndexMedico = sText.IndexOf(m_sMedicoFile) + m_sMedicoFile.Length;
						sMedico = sText.Substring(iIndexMedico, GetNextReturn(sText, iIndexMedico) - iIndexMedico).Trim();
					}
				}
				catch
				{
				}
				*/

				/*
				string sPaziente = "";
				try
				{
					if (sText.Contains(m_sPazienteFile))
					{
						int iIndexPaziente = sText.IndexOf(m_sPazienteFile) + m_sPazienteFile.Length;
						sPaziente = sText.Substring(iIndexPaziente, GetNextReturn(sText, iIndexPaziente) - iIndexPaziente).Trim();
					}
				}
				catch
				{
				}
				*/

				string sTipoCiclo = "";
				try
				{
					if (sText.Contains(m_sTipoCicloFile))
					{
						int iIndexTipoCiclo = sText.IndexOf(m_sTipoCicloFile) + m_sTipoCicloFile.Length;
						sTipoCiclo = sText.Substring(iIndexTipoCiclo, GetNextReturn(sText, iIndexTipoCiclo) - iIndexTipoCiclo).Trim();
					}
				}
				catch
				{
				}

				string sNumeroCiclo = "";
				try
				{
					if (sText.Contains(m_sNumeroCicloFile))
					{
						sNumeroCiclo = sText.Substring(iIndexNumeroCiclo, iIndexAdditionalInfo - iIndexNumeroCiclo).Trim();
					}
				}
				catch
				{
				}

				string sStazione = "";
				try
				{
					if (sText.Contains(m_sStazioneFile))
					{
						int iIndexStazione = sText.IndexOf(m_sStazioneFile) + m_sStazioneFile.Length;
						sStazione = sText.Substring(iIndexStazione, GetNextReturn(sText, iIndexStazione) - iIndexStazione).Trim();
					}
				}
				catch
				{
				}

				string sAdditionalInfoComplete = "";
				try
				{
					if (iIndexAdditionalInfo > 0)
						sAdditionalInfoComplete = sText.Substring(iIndexAdditionalInfo).Trim();
				}
				catch
				{
				}

				List<string> listAdditionalString = new List<string>();

				int iStart = 0;
				string sEsito = "";
				string sFineCiclo = "";

				while (iStart < sAdditionalInfoComplete.Length)
				{
					Match tempMatch = myMatcher.Match(sAdditionalInfoComplete, iStart + 8);

					if (tempMatch.Success)
					{
						string tempAdditional = sAdditionalInfoComplete.Substring(iStart, tempMatch.Index - iStart).Trim();
						listAdditionalString.Add(tempAdditional);
						iStart = tempMatch.Index;
					}
					else
					{
						string tempAdditional = sAdditionalInfoComplete.Substring(iStart);

						string[] sSplitChar = { Environment.NewLine };

						string[] sSplitted = tempAdditional.Split(sSplitChar, StringSplitOptions.RemoveEmptyEntries);

						if (sSplitted != null)
						{
							if (sSplitted.Length > 0)
							{
								sFineCiclo = sSplitted[0];

								if (sFineCiclo != null && sFineCiclo.Trim() != "")
								{
									listAdditionalString.Add(sFineCiclo.Trim());
								}
							}

							if (sSplitted.Length > 1)
							{

								for (int i = 1; i < sSplitted.Length; i++)
								{
									sEsito = sEsito + sSplitted[i] + " ";

								}
							}
						}

						break;
					}
				}

				// CREAZIONE ADDITIONALINFO //

				if (sInizioCiclo.Length > 0)
				{
					string sDate = DateTime.ParseExact(sInizioCiclo, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

					WasherCycle myCycle = new WasherCycle();

					myCycle.Filename = sFile;
					myCycle.FileDatetime = FromFilenameToDate(sFile);
					myCycle.FileContent = sText;

					foreach (string s in listAdditionalString)
					{
						WasherCycleInfo info = ExtractInfo(s, sDate);
						if (info != null)
							myCycle.AdditionalInfoList.Add(info);
					}

					// CREAZIONE CICLO           

					try
					{
						myCycle.StartTimestamp = DateTime.ParseExact(sInizioCiclo, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
					}
					catch
					{
						myCycle.StartTimestamp = DateTime.MinValue;
					}

					try
					{
						myCycle.CycleCount = int.Parse(sNumeroCiclo).ToString();
					}
					catch
					{
						myCycle.CycleCount = "0";
					}

					try
					{
						WasherCycleInfo tempInfo = ExtractInfo(sFineCiclo, sDate);
						myCycle.EndTimestamp = tempInfo.Date;
					}
					catch
					{
						myCycle.EndTimestamp = DateTime.MinValue;
					}

					myCycle.WasherExternalID = sSeriale;
					myCycle.StationName = sStazione;
					myCycle.DeviceExternalID = sMatricola;
					myCycle.DeviceID = 0;
					myCycle.OperatorStartExternalID = sOperatoreStart;
					myCycle.OperatorStartID = 0;
					myCycle.OperatorEndExternalID = sOperatoreEnd;
					myCycle.OperatorEndID = 0;
					myCycle.CycleType = sTipoCiclo;
					myCycle.Failed = !sAdditionalInfoComplete.Contains("CICLO REGOLARE");

					if (sAdditionalInfoComplete.Contains("CICLO REGOLARE") || sAdditionalInfoComplete.Contains("CICLO IRREGOLARE"))
					{
						myCycle.Completed = true;

						Logger.Info("------- " + m_sNumeroSerialeFile + " " + sSeriale);
                        Logger.Info("------- " + m_sInizioCicloFile + " " + sInizioCiclo);
                        Logger.Info("------- " + m_sStrumentoFile + " " + sStrumento);
                        Logger.Info("------- " + m_sMatricolaFile + " " + sMatricola);
                        Logger.Info("------- " + m_sOperatoreFile + " " + sOperatoreStart);
                        Logger.Info("------- " + m_sPrelevatoDaFile + " " + sOperatoreEnd);
                        Logger.Info("------- " + m_sTipoCicloFile + " " + sTipoCiclo);
                        Logger.Info("------- " + m_sNumeroCicloFile + " " + sNumeroCiclo);
					}

					// AGGIUNGO ESITO //
					myCycle.AdditionalInfoList.Add(new WasherCycleInfo()
					{
						Date = myCycle.EndTimestamp,
						Description = sEsito,
						isAlarm = myCycle.Failed
					});

					myCycle.AdditionalInfoList.Reverse();

					myCycle.WasherID = TranscodeWasher(myCycle.WasherExternalID);

					myCycle.DeviceID = TranscodeDevice(myCycle.DeviceExternalID);
					if (myCycle.DeviceID <= 0)
						myCycle.DeviceID = TranscodeDevice("UNKNOWN");

					myCycle.OperatorStartID = TranscodeOperator(myCycle.OperatorStartExternalID);
					if (myCycle.OperatorStartID <= 0)
						myCycle.OperatorStartID = TranscodeOperator("UNKNOWN");

					myCycle.OperatorEndID = TranscodeOperator(myCycle.OperatorEndExternalID);
					if (myCycle.OperatorEndID <= 0)
						myCycle.OperatorEndID = TranscodeOperator("UNKNOWN");

					if (myCycle.Completed && !myCycle.Failed)
						myCycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_wash);
					else
						myCycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_wash);
					if(myCycle == null)
						Logger.Info("returning cycle and cycle is NULL");
					else
						Logger.Info("returning cycle and cycle is NOT NULL");
					return (myCycle.Completed || !bAlreadyProcessedFile) ? myCycle : null;
				}
				else
				{
					Logger.Warn("Inizio ciclo not found in " + sFile);
					Logger.Info("returning **NULL** cycle");
					MoveToBadDir(sFile);
					return null;
				}
			}
			catch (Exception e)
			{
				Logger.Error("Parse file: " + sFile + $" exception {e}");
				Logger.Error("exception during file parse, returning null cycle!!");
				MoveToBadDir(sFile);
				return null;
			}
        }
    }
}
