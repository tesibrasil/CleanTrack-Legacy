using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ISAWD_PARSER
{
	public class ISAWDObject
    {
        public const string m_sNumeroSerialeFile = "NUMERO SERIALE:";
        private const string m_sInizioCicloFile = "INIZIO CICLO:";
        private const string m_sStrumentoFile = "STRUMENTO:";
        public const string m_sMatricolaFile = "MATRICOLA:";
        public const string m_sOperatoreFile = "OPERATORE:";
        private const string m_sMedicoFile = "MEDICO:";
        private const string m_sPazienteFile = "PAZIENTE:";
        private const string m_sTipoCicloFile = "TIPO CICLO:";
        private const string m_sNumeroCicloFile = "NUMERO CICLO:";
        private const string m_sStazioneFile = "STAZIONE:";

		public const string m_sAutosanificazione = "AUTOSANIFICAZIONE";

        public static Cycle GetCycleFromFile(string sFile)
        {
            return ParseFile(sFile);
        }

        private static int GetNextReturn(string sInput, int iStart)
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

        private static AdditionalInfo ExtractInfo(string sInput, string sData)
        {
            AdditionalInfo info = null;

            if (sInput.Length >= 8)
            {
                info = new AdditionalInfo();
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
                    {
                        info.isAlarm = true;
                    }
                }
            }

            return info;
        }

        private static Cycle ParseFile(string sFile)
        {
            StreamReader srTemp = new StreamReader(new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            string sText = srTemp.ReadToEnd();

            int iIndexNumeroCiclo = sText.IndexOf(m_sNumeroCicloFile) + m_sNumeroCicloFile.Length;

            System.Text.RegularExpressions.Regex myMatcher = new System.Text.RegularExpressions.Regex(@"[0-9][0-9]:[0-9][0-9]:[0-9][0-9]");
            Match myMatch = myMatcher.Match(sText, iIndexNumeroCiclo);

            int iIndexAdditionalInfo = -1;
            if (myMatch.Success)
                iIndexAdditionalInfo = myMatch.Index;

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

			if (sTipoCiclo == m_sAutosanificazione)
			{
				Cycle myCycleAutosan = new Cycle();
				myCycleAutosan.Type = sTipoCiclo;
				return myCycleAutosan;
			}

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

            string sOperatore = "";
            try
            {
                if (sText.Contains(m_sOperatoreFile))
                {
                    int iIndexOperatore = sText.IndexOf(m_sOperatoreFile) + m_sOperatoreFile.Length;
                    sOperatore = sText.Substring(iIndexOperatore, GetNextReturn(sText, iIndexOperatore) - iIndexOperatore).Trim();
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
                                listAdditionalString.Add(sFineCiclo.Trim());
                        }

                        if (sSplitted.Length > 1)
                        {
                            for (int i = 1; i < sSplitted.Length; i++)
                                sEsito = sEsito + sSplitted[i] + " ";
                        }
                    }

                    break;
                }
            }

            // CREAZIONE ADDITIONALINFO //

            string sDate = DateTime.ParseExact(sInizioCiclo, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            Cycle myCycle = new Cycle();

            foreach (string s in listAdditionalString)
            {
                AdditionalInfo info = ExtractInfo(s, sDate);
                if (info != null) { myCycle.AdditionalInfoList.Add(info); }
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
                myCycle.CycleCount = int.Parse(sNumeroCiclo);
            }
            catch
            {
                myCycle.CycleCount = 0;
            }

            try
            {
                AdditionalInfo tempInfo = ExtractInfo(sFineCiclo, sDate);
                myCycle.EndTimestamp = tempInfo.Date;
            }
            catch
            {
                myCycle.EndTimestamp = DateTime.MinValue;
            }

            try
            {
                myCycle.MachineID = sSeriale;
            }
            catch
            {
                myCycle.MachineID = "";
            }

            try
            {
                myCycle.Station = sStazione;
            }
            catch
            {
                myCycle.Station = "";
            }

            try
            {
                myCycle.OperatorID = sOperatore;
            }
            catch
            {
                myCycle.OperatorID = "";
            }

            try
            {
                myCycle.ScopeID = sMatricola;
            }
            catch
            {
                myCycle.ScopeID = "";
            }

            try
            {
                myCycle.Type = sTipoCiclo;
            }
            catch
            {
                myCycle.Type = "";
            }

            bool bFailed = !sAdditionalInfoComplete.Contains("CICLO REGOLARE");
            myCycle.Failed = bFailed;

            if (sAdditionalInfoComplete.Contains("CICLO REGOLARE") || sAdditionalInfoComplete.Contains("CICLO IRREGOLARE"))
            {
                myCycle.Completed = true;

				Program.WriteLog("ParseFile", "INFO", m_sNumeroSerialeFile + " " + sSeriale);
				Program.WriteLog("ParseFile", "INFO", m_sInizioCicloFile + " " + sInizioCiclo);
				Program.WriteLog("ParseFile", "INFO", m_sStrumentoFile + " " + sStrumento);
				Program.WriteLog("ParseFile", "INFO", m_sMatricolaFile + " " + sMatricola);
				Program.WriteLog("ParseFile", "INFO", m_sOperatoreFile + " " + sOperatore);
				Program.WriteLog("ParseFile", "INFO", m_sTipoCicloFile + " " + sTipoCiclo);
				Program.WriteLog("ParseFile", "INFO", m_sNumeroCicloFile + " " + sNumeroCiclo);

				if (sAdditionalInfoComplete.Contains("CICLO REGOLARE"))
					Program.WriteLog("ParseFile", "INFO", "FINISHED OK");
				else if (sAdditionalInfoComplete.Contains("CICLO IRREGOLARE"))
					Program.WriteLog("ParseFile", "INFO", "FINISHED KO");
				else
					Program.WriteLog("ParseFile", "INFO", "FINISHED ??");
			}

			// AGGIUNGO ESITO //
			myCycle.AdditionalInfoList.Add(new AdditionalInfo()
            {
                Date = myCycle.EndTimestamp,
                Description = sEsito,
                isAlarm = bFailed
            });

            myCycle.AdditionalInfoList.Reverse();

            return myCycle;
        }
    }
}
