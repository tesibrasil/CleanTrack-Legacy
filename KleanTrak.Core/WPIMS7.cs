using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	class WPIMS7 : WPBase
    {
		private const string m_sSerialNumber = "S/N:";
		private const string m_sSerialNumberEx = "NUMERO SERIALE:";
		private const string m_sInizioCiclo = "INIZIO:";
		private const string m_sInizioCicloEx = "INIZIO CICLO:";
		private const string m_sFineCiclo = "FINE:";
		private const string m_sStazione = "STAZIONE:";

		private const string m_sStrumento = "STRUMENTO:";
		private const string m_sMatricola = "MATRICOLA STRUM.:";
		private const string m_sMatricolaEx = "MATRICOLA:";
		private const string m_sOperatore = "OPERATORE:";
		private const string m_sMedico = "MEDICO:";
		private const string m_sPaziente = "PAZIENTE:";
		private const string m_sTipoCiclo = "CICLO:";
		private const string m_sTipoCicloEx = "CICLO:";
		private const string m_sNumeroCiclo = "NUMERO CICLO:";
		private const string m_sDurataCiclo = "DURATA CICLO:";

		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
		{
			List<string> listFilesToParse = new List<string>();

			string[] sFiles = Directory.GetFiles(FolderOrFileName);
			foreach (string sFile in sFiles)
			{
				if (FromFilenameToDate(sFile) > lastDate)
				{
					Logger.Info("Add file: " + sFile);
					listFilesToParse.Add(sFile);
				}
			}

			// per ogni nuovo scontrino trovato... //
			List<WasherCycle> ret = new List<WasherCycle>();
			foreach (string sFile in listFilesToParse)
				ret.Add(ParseFile(sFile));

			return ret;
		}

		private DateTime FromFilenameToDate(string fName)
		{
			DateTime ret = DateTime.MinValue;

			try
			{
				if (fName.IndexOf("-") == 8)
				{
					string num = fName.Substring(0, 8);
					ret = DateTime.ParseExact(num, "yyyyMMdd", CultureInfo.InvariantCulture);
				}
			}
			catch
			{
				ret = DateTime.MinValue;
			}

			return ret;
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

		private static DateTime ParseStringData(string sInizioCiclo)
		{
			int year, month, day, hour, minute, second;

			try
			{
				var match = Regex.Match(sInizioCiclo, "[0-9]+");

				if (match.Success)
					day = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;

				match = match.NextMatch();

				if (match.Success)
					month = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;


				match = match.NextMatch();

				if (match.Success)
					year = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;


				match = match.NextMatch();

				if (match.Success)
					hour = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;


				match = match.NextMatch();

				if (match.Success)
					minute = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;


				match = match.NextMatch();

				if (match.Success)
					second = Int32.Parse(match.Value);
				else
					return DateTime.MinValue;

				return new DateTime(year, month, day, hour, minute, second);
			}
			catch (Exception ex)
			{
                Logger.Error(sInizioCiclo);
                Logger.Error(ex);
			}

			return DateTime.MinValue;
		}

		private static WasherCycleInfo GetAlarm(string sText, DateTime dtDate)
		{
			WasherCycleInfo info = null;

			try
			{
				if (sText.Length > 0)
				{
					int index = -1;
					index = sText.ToUpper().IndexOf("*ALLARME") + 1;


					if (index > 0)
					{

						int indexEnd = -1;

						indexEnd = sText.IndexOf("*", index + 1);

						if (indexEnd > 0)
						{
							string alarm = "";
							alarm = sText.Substring(index, indexEnd - index);

							if (alarm != null & alarm != "")
							{
								info = new WasherCycleInfo();
								info.isAlarm = true;
								info.Description = alarm;

								try
								{
									info.Date = DateTime.ParseExact(dtDate + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
								}
								catch
								{
									info.Date = DateTime.MinValue;
								}
							}
						}
					}
				}
			}
			catch
			{
				info = null; ;
			}

			return info;
		}

		private static WasherCycleInfo ExtractInfo(string sText, string sDate)
		{
			WasherCycleInfo info = null;

			if (sText.Length >= 8)
			{
				info = new WasherCycleInfo();
				try
				{
					info.Date = DateTime.ParseExact(sDate + (sText.Substring(0, 8).Replace(".", "")), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
				}
				catch
				{
					info.Date = DateTime.MinValue;
				}

				if (sText.Length > 8)
				{
					info.Description = sText.Substring(8).Trim();

					if (info.Description.ToUpper().Contains("ALLARME"))
					{
						info.isAlarm = true;
					}
				}
			}

			return info;
		}

		private WasherCycle ParseFile(string sFile)
		{
			// FILE PARSING
			StreamReader srTemp = new StreamReader(sFile);
			string sText = srTemp.ReadToEnd();

			// Sandro 03/02/2014 //

			string sSerial = "";
			string sInizioCiclo = "";
			string sStazione = "";
			string sStrumento = "";
			string sMatricola = "";
			string sOperatore = "";
			string sMedico = "";
			string sPaziente = "";
			string sTipoCiclo = "";
			string sNumeroCiclo = "";
			string sAdditionalInfo = "";
			string sFineCiclo = "";
			string sDurataCiclo = "";
			string sEsito = "";

			int iTemp;

			// 
			int serial_index = 0;
			iTemp = sText.IndexOf(m_sSerialNumber);
			if (iTemp > 0)
				serial_index = iTemp + m_sSerialNumber.Length;
			else
			{
				iTemp = sText.IndexOf(m_sSerialNumberEx);
				if (iTemp > 0)
					serial_index = iTemp + m_sSerialNumberEx.Length;
			}

			try
			{
				if (serial_index > 0)
					sSerial = sText.Substring(serial_index, GetNextReturn(sText, serial_index) - serial_index).Trim();
			}
			catch
			{
				sSerial = "";
			}

			// 
			int inizio_ciclo_index = 0;
			iTemp = sText.IndexOf(m_sInizioCiclo);
			if (iTemp > 0)
				inizio_ciclo_index = iTemp + m_sInizioCiclo.Length;
			else
			{
				iTemp = sText.IndexOf(m_sInizioCicloEx);
				if (iTemp > 0)
					inizio_ciclo_index = iTemp + m_sInizioCicloEx.Length;
			}

			try
			{
				// if (text.Contains(inizio_ciclo_str))
				if (inizio_ciclo_index > 0)
					sInizioCiclo = sText.Substring(inizio_ciclo_index, GetNextReturn(sText, inizio_ciclo_index) - inizio_ciclo_index).Trim();
			}
			catch
			{
				sInizioCiclo = "";
			}
			
			//
			int fine_ciclo_index = 0;
			iTemp = sText.IndexOf(m_sFineCiclo);
			if (iTemp > 0)
				fine_ciclo_index = iTemp + m_sFineCiclo.Length;

			try
			{
				// if (text.Contains(fine_ciclo_str))
				if (fine_ciclo_index > 0)
					sFineCiclo = sText.Substring(fine_ciclo_index, GetNextReturn(sText, fine_ciclo_index) - fine_ciclo_index).Trim();
			}
			catch
			{
				sFineCiclo = "";
			}
			
			//
			int stazione_index = 0;
			iTemp = sText.IndexOf(m_sStazione);
			if (iTemp > 0)
				stazione_index = iTemp + m_sStazione.Length;

			try
			{
				// if (text.Contains(stazione_str))
				if (stazione_index > 0)
					sStazione = sText.Substring(stazione_index, GetNextReturn(sText, stazione_index) - stazione_index).Trim();
			}
			catch
			{
				sStazione = "";
			}
			
			//
			int strumento_index = 0;
			iTemp = sText.IndexOf(m_sStrumento);
			if (iTemp > 0)
				strumento_index = iTemp + m_sStrumento.Length;

			try
			{
				// if (text.Contains(strumento_str))
				if (strumento_index > 0)
					sStrumento = sText.Substring(strumento_index, GetNextReturn(sText, strumento_index) - strumento_index).Trim();
			}
			catch
			{
				sStrumento = "";
			}

			//
			int matricola_index = 0;
			iTemp = sText.IndexOf(m_sMatricola);
			if (iTemp > 0)
				matricola_index = iTemp + m_sMatricola.Length;
			else
			{
				iTemp = sText.IndexOf(m_sMatricolaEx);
				if (iTemp > 0)
					matricola_index = iTemp + m_sMatricolaEx.Length;
			}

			try
			{
				//if (text.Contains(matricola_str))
				if (matricola_index > 0)
					sMatricola = sText.Substring(matricola_index, GetNextReturn(sText, matricola_index) - matricola_index).Trim();
			}
			catch
			{
				sMatricola = "";
			}
			
			//
			int operatore_index = 0;
			iTemp = sText.IndexOf(m_sOperatore);
			if (iTemp > 0)
				operatore_index = iTemp + m_sOperatore.Length;

			try
			{
				// if (text.Contains(operatore_str))
				if (operatore_index > 0)
					sOperatore = sText.Substring(operatore_index, GetNextReturn(sText, operatore_index) - operatore_index).Trim();
			}
			catch
			{
				sOperatore = "";
			}
			
			//
			int medico_index = 0;
			iTemp = sText.IndexOf(m_sMedico);
			if (iTemp > 0)
				medico_index = iTemp + m_sMedico.Length;

			try
			{
				// if (text.Contains(medico_str))
				if (medico_index > 0)
					sMedico = sText.Substring(medico_index, GetNextReturn(sText, medico_index) - medico_index).Trim();
			}
			catch
			{
				sMedico = "";
			}
			
			//
			int paziente_index = 0;
			iTemp = sText.IndexOf(m_sPaziente);
			if (iTemp > 0)
				paziente_index = iTemp + m_sPaziente.Length;

			try
			{
				// if (text.Contains(paziente_str))
				if (paziente_index > 0)
					sPaziente = sText.Substring(paziente_index, GetNextReturn(sText, paziente_index) - paziente_index).Trim();
			}
			catch
			{
				sPaziente = "";
			}

			//
			int tipo_ciclo_index = 0;
			iTemp = sText.IndexOf(m_sTipoCiclo);
			if (iTemp > 0)
				tipo_ciclo_index = iTemp + m_sTipoCiclo.Length;

			try
			{
				//if (text.Contains(tipo_ciclo_str))
				if (tipo_ciclo_index > 0)
					sTipoCiclo = sText.Substring(tipo_ciclo_index, GetNextReturn(sText, tipo_ciclo_index) - tipo_ciclo_index).Trim();
			}
			catch
			{
				sTipoCiclo = "";
			}
			
			//
			int numero_ciclo_index = 0;
			iTemp = sText.IndexOf(m_sNumeroCiclo);
			if (iTemp > 0)
				numero_ciclo_index = iTemp + m_sNumeroCiclo.Length;

			try
			{
				// if (text.Contains(numero_ciclo_str))
				if (numero_ciclo_index > 0)
					sNumeroCiclo = sText.Substring(numero_ciclo_index, GetNextReturn(sText, numero_ciclo_index) - numero_ciclo_index);
			}
			catch
			{
				sNumeroCiclo = "";
			}
			
			//
			int durata_ciclo_index = 0;
			iTemp = sText.IndexOf(m_sDurataCiclo);
			if (iTemp > 0)
				durata_ciclo_index = iTemp + m_sDurataCiclo.Length;

			try
			{
				// if (text.Contains(durata_ciclo_str))
				if (durata_ciclo_index > 0)
					sDurataCiclo = sText.Substring(durata_ciclo_index, GetNextReturn(sText, durata_ciclo_index) - durata_ciclo_index);
			}
			catch
			{
				sDurataCiclo = "";
			}
			
			//
			System.Text.RegularExpressions.Regex matcher = new System.Text.RegularExpressions.Regex(@"[0-9][0-9].[0-9][0-9].[0-9][0-9]");
			Match match = matcher.Match(sText, tipo_ciclo_index);
			int additionalInfoIndex = -1;
			if (match.Success)
				additionalInfoIndex = match.Index;

			try
			{

				if (additionalInfoIndex > 0)
					sAdditionalInfo = sText.Substring(additionalInfoIndex).Trim();
			}
			catch
			{
				sAdditionalInfo = "";
			}

			//
			List<string> additionalStringList = new List<string>();
			int start = 0;
			while (start < sAdditionalInfo.Length)
			{
				Match tempMatch = matcher.Match(sAdditionalInfo, start + 8);

				if (tempMatch.Success)
				{
					string tempAdditional = sAdditionalInfo.Substring(start, tempMatch.Index - start).Trim();
					additionalStringList.Add(tempAdditional);
					start = tempMatch.Index;
				}
				else
				{
					string tempAdditional = sAdditionalInfo.Substring(start);
					string[] splitChar = { Environment.NewLine };
					string[] splitted = tempAdditional.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

					if (splitted != null)
					{
						if (splitted.Length > 0)
						{
							sFineCiclo = splitted[0];

							if (sFineCiclo != null && sFineCiclo.Trim() != "")
							{
								additionalStringList.Add(sFineCiclo.Trim());
							}
						}

						if (splitted.Length > 1)
							sEsito = sEsito + splitted[1];
					}

					break;
				}
			}

			// CREAZIONE ADDITIONALINFO
			// mese a una cifra .... andrà bene?
			sInizioCiclo.Trim();
			if (sInizioCiclo.Length > 0)
			{
				string sDate = ParseStringData(sInizioCiclo).ToString("yyyyMMdd");

				WasherCycle myCycle = new WasherCycle();

				myCycle.Filename = sFile;
				myCycle.FileDatetime = FromFilenameToDate(sFile);
				myCycle.FileContent = sText;

				foreach (string s in additionalStringList)
				{
					WasherCycleInfo info = ExtractInfo(s, sDate);
					if (info != null)
						myCycle.AdditionalInfoList.Add(info);
				}

				myCycle.StartTimestamp = ParseStringData(sInizioCiclo);

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

				try
				{
					myCycle.WasherExternalID = sSerial;
				}
				catch
				{
					myCycle.WasherExternalID = "";
				}

				try
				{
					myCycle.StationName = sStazione;
				}
				catch
				{
					myCycle.StationName = "";
				}

				try
				{
					myCycle.OperatorStartExternalID = sOperatore;
				}
				catch
				{
					myCycle.OperatorStartExternalID = "";
				}

				try
				{
					myCycle.OperatorEndExternalID = sOperatore;
				}
				catch
				{
					myCycle.OperatorEndExternalID = "";
				}

				try
				{
					myCycle.DeviceExternalID = sMatricola;
				}
				catch
				{
					myCycle.DeviceExternalID = "";
				}

				try
				{
					myCycle.CycleType = sTipoCiclo;
				}
				catch
				{
					myCycle.CycleType = "";
				}

				bool failedBool = !sEsito.Contains("CICLO REGOLARE");
				myCycle.Failed = failedBool;

				// AGGIUNGO ESITO
				myCycle.AdditionalInfoList.Add(new WasherCycleInfo()
				{
					Date = myCycle.EndTimestamp,
					Description = sEsito,
					isAlarm = failedBool
				});

				WasherCycleInfo tempalarm = GetAlarm(sText, myCycle.EndTimestamp);
				if (tempalarm != null)
				{
					myCycle.AdditionalInfoList.Add(tempalarm);
				}

				myCycle.AdditionalInfoList.Reverse();
				if (myCycle.StartTimestamp == DateTime.MinValue)
				{
					Logger.Error("StartTimeStamp error...");
					return null;
				}

				if (myCycle.EndTimestamp == DateTime.MinValue)
				{
					Logger.Error("EndTimestamp error...");
					return null;
				}

				//

				// myCycle.Completed = ???

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

				//

				return myCycle;
			}

			return null;
		}

	}
}
