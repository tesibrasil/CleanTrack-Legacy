using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace IMS7_PARSER
{
    public class IMS7Object
    {
		private const string serial_number_str = "S/N:";
		private const string serial_number_strex = "NUMERO SERIALE:";
		private const string inizio_ciclo_str = "INIZIO:";
		private const string inizio_ciclo_strex = "INIZIO CICLO:";
		private const string fine_ciclo_str = "FINE:";
		private const string stazione_str = "STAZIONE:";

		private const string strumento_str = "STRUMENTO:";
		private const string matricola_str = "MATRICOLA STRUM.:";
		private const string matricola_strex = "MATRICOLA:";
		private const string operatore_str = "OPERATORE:";
		private const string medico_str = "MEDICO:";
		private const string paziente_str = "PAZIENTE:";
		private const string tipo_ciclo_str = "CICLO:";
		private const string tipo_ciclo_strex = "CICLO:";
		private const string numero_ciclo_str = "NUMERO CICLO:";
		private const string durata_ciclo_str = "DURATA CICLO:";

		public static Cycle GetCycleFromFile(string path)
        {
            return parseFile(path);
        }

        private static int getNextReturn(string val, int start)
        {
            if (start < val.Length)
            {
                return val.IndexOf(Environment.NewLine, start);
            }
            else
            {
                throw new Exception();
            }
        }

        private static Cycle parseFile(string path)
        {
            // FILE PARSING
            StreamReader sr = new StreamReader(path);
            string text = sr.ReadToEnd();

            // Sandro 03/02/2014 //

            string serial = "";
            string inizio_ciclo = "";
            string stazione = "";
            string strumento = "";
            string matricola = "";
            string operatore = "";
            string medico = "";
            string paziente = "";
            string tipo_ciclo = "";
            string numero_ciclo = "";
            string additionalInfoString = "";
            string fine_ciclo = "";
            string durata_ciclo = "";
            string esito = "";


            int iTemp;

            // 
            int serial_index = 0;
            iTemp = text.IndexOf(serial_number_str);
            if (iTemp > 0)
                serial_index = iTemp + serial_number_str.Length;
            else
            {
                iTemp = text.IndexOf(serial_number_strex);
                if (iTemp > 0)
                    serial_index = iTemp + serial_number_strex.Length;
            }

            try
            {
                //if (text.Contains(serial_number_str))
                if (serial_index > 0)
                    serial = text.Substring(serial_index, getNextReturn(text, serial_index) - serial_index).Trim();
            }
            catch
            {
                serial = "";
            }


            // 
            int inizio_ciclo_index = 0;
            iTemp = text.IndexOf(inizio_ciclo_str);
            if (iTemp > 0)
                inizio_ciclo_index = iTemp + inizio_ciclo_str.Length;
            else
            {
                iTemp = text.IndexOf(inizio_ciclo_strex);
                if (iTemp > 0)
                    inizio_ciclo_index = iTemp + inizio_ciclo_strex.Length;
            }

            try
            {
                // if (text.Contains(inizio_ciclo_str))
                if (inizio_ciclo_index > 0)
                    inizio_ciclo = text.Substring(inizio_ciclo_index, getNextReturn(text, inizio_ciclo_index) - inizio_ciclo_index).Trim();
            }
            catch
            {
                inizio_ciclo = "";
            }


            //
            int fine_ciclo_index = 0;
            iTemp = text.IndexOf(fine_ciclo_str);
            if (iTemp > 0)
                fine_ciclo_index = iTemp + fine_ciclo_str.Length;

            try
            {
                // if (text.Contains(fine_ciclo_str))
                if (fine_ciclo_index > 0)
                    fine_ciclo = text.Substring(fine_ciclo_index, getNextReturn(text, fine_ciclo_index) - fine_ciclo_index).Trim();
            }
            catch
            {
                fine_ciclo = "";
            }


            //
            int stazione_index = 0;
            iTemp = text.IndexOf(stazione_str);
            if (iTemp > 0)
                stazione_index = iTemp + stazione_str.Length;

            try
            {
                // if (text.Contains(stazione_str))
                if (stazione_index > 0)
                    stazione = text.Substring(stazione_index, getNextReturn(text, stazione_index) - stazione_index).Trim();
            }
            catch
            {
                stazione = "";
            }


            //
            int strumento_index = 0;
            iTemp = text.IndexOf(strumento_str);
            if (iTemp > 0)
                strumento_index = iTemp + strumento_str.Length;

            try
            {
                // if (text.Contains(strumento_str))
                if (strumento_index > 0)
                    strumento = text.Substring(strumento_index, getNextReturn(text, strumento_index) - strumento_index).Trim();
            }
            catch
            {
                strumento = "";
            }


            //
            int matricola_index = 0;
            iTemp = text.IndexOf(matricola_str);
            if (iTemp > 0)
                matricola_index = iTemp + matricola_str.Length;
            else
            {
                iTemp = text.IndexOf(matricola_strex);
                if (iTemp > 0)
                    matricola_index = iTemp + matricola_strex.Length;
            }

            try
            {
                //if (text.Contains(matricola_str))
                if (matricola_index > 0)
                    matricola = text.Substring(matricola_index, getNextReturn(text, matricola_index) - matricola_index).Trim();
            }
            catch
            {
                matricola = "";
            }


            //
            int operatore_index = 0;
            iTemp = text.IndexOf(operatore_str);
            if (iTemp > 0)
                operatore_index = iTemp + operatore_str.Length;

            try
            {
                // if (text.Contains(operatore_str))
                if (operatore_index > 0)
                    operatore = text.Substring(operatore_index, getNextReturn(text, operatore_index) - operatore_index).Trim();
            }
            catch
            {
                operatore = "";
            }


            //
            int medico_index = 0;
            iTemp = text.IndexOf(medico_str);
            if (iTemp > 0)
                medico_index = iTemp + medico_str.Length;

            try
            {
                // if (text.Contains(medico_str))
                if (medico_index > 0)
                    medico = text.Substring(medico_index, getNextReturn(text, medico_index) - medico_index).Trim();
            }
            catch
            {
                medico = "";
            }


            //
            int paziente_index = 0;
            iTemp = text.IndexOf(paziente_str);
            if (iTemp > 0)
                paziente_index = iTemp + paziente_str.Length;

            try
            {
                // if (text.Contains(paziente_str))
                if (paziente_index > 0)
                        paziente = text.Substring(paziente_index, getNextReturn(text, paziente_index) - paziente_index).Trim();
            }
            catch
            {
                paziente = "";
            }

            //
            int tipo_ciclo_index = 0;
            iTemp = text.IndexOf(tipo_ciclo_str);
            if (iTemp > 0)
                tipo_ciclo_index = iTemp + tipo_ciclo_str.Length;

            try
            {
                //if (text.Contains(tipo_ciclo_str))
                if (tipo_ciclo_index > 0)
                    tipo_ciclo = text.Substring(tipo_ciclo_index, getNextReturn(text, tipo_ciclo_index) - tipo_ciclo_index).Trim();
            }
            catch
            {
                tipo_ciclo = "";
            }


            //
            int numero_ciclo_index = 0;
            iTemp = text.IndexOf(numero_ciclo_str);
            if (iTemp > 0)
                numero_ciclo_index = iTemp + numero_ciclo_str.Length;

            try
            {
                // if (text.Contains(numero_ciclo_str))
                if (numero_ciclo_index > 0)
                    numero_ciclo = text.Substring(numero_ciclo_index, getNextReturn(text, numero_ciclo_index) - numero_ciclo_index);
            }
            catch
            {
                numero_ciclo = "";
            }


            //
            int durata_ciclo_index = 0;
            iTemp = text.IndexOf(durata_ciclo_str);
            if (iTemp > 0)
                durata_ciclo_index = iTemp + durata_ciclo_str.Length;

            try
            {
                // if (text.Contains(durata_ciclo_str))
                if (durata_ciclo_index > 0)
                    durata_ciclo = text.Substring(durata_ciclo_index, getNextReturn(text, durata_ciclo_index) - durata_ciclo_index);
            }
            catch
            {
                durata_ciclo = "";
            }


            //
            System.Text.RegularExpressions.Regex matcher = new System.Text.RegularExpressions.Regex(@"[0-9][0-9].[0-9][0-9].[0-9][0-9]");
            Match match = matcher.Match(text, tipo_ciclo_index);
            int additionalInfoIndex = -1;
            if (match.Success)
                additionalInfoIndex = match.Index;

            try
            {

                if (additionalInfoIndex > 0)
                    additionalInfoString = text.Substring(additionalInfoIndex).Trim();
            }
            catch
            {
                additionalInfoString = "";
            }

            //
            List<string> additionalStringList = new List<string>();
            int start = 0;
            while (start < additionalInfoString.Length)
            {
                Match tempMatch = matcher.Match(additionalInfoString, start + 8);

                if (tempMatch.Success)
                {
                    string tempAdditional = additionalInfoString.Substring(start, tempMatch.Index - start).Trim();
                    additionalStringList.Add(tempAdditional);
                    start = tempMatch.Index;
                }
                else
                {
                    string tempAdditional = additionalInfoString.Substring(start);
                    string[] splitChar = { Environment.NewLine };
                    string[] splitted = tempAdditional.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                    if (splitted != null)
                    {
                        if (splitted.Length > 0)
                        {
                            fine_ciclo = splitted[0];

                            if (fine_ciclo != null && fine_ciclo.Trim() != "") 
                            {
                                additionalStringList.Add(fine_ciclo.Trim());
                            }
                        }

                        if (splitted.Length > 1)
                            esito = esito + splitted[1];                            
                    }

                    break;
                }
            }

            // CREAZIONE ADDITIONALINFO
            // mese a una cifra .... andrà bene?
            inizio_ciclo.Trim();
            if (inizio_ciclo.Length > 0)
            {
                string date = ParseStringData(inizio_ciclo).ToString("yyyyMMdd");
                Cycle cycle = new Cycle();
                foreach (string s in additionalStringList)
                {
                    AdditionalInfo info = extractInfo(s, date);
                    if (info != null)
                        cycle.AdditionalInfoList.Add(info);
                }


                cycle.StartTimestamp = ParseStringData(inizio_ciclo);                


                try {
                    cycle.CycleCount = int.Parse(numero_ciclo);
                }
                catch {
                    cycle.CycleCount = 0;
                }

                try {
                    AdditionalInfo tempInfo = extractInfo(fine_ciclo, date);
                    cycle.EndTimestamp = tempInfo.Date;
                }
                catch {
                    cycle.EndTimestamp = DateTime.MinValue;
                }

                try {
                    cycle.MachineID = serial;
                }
                catch {
                    cycle.MachineID = "";
                }

                try {
                    cycle.Station = stazione;
                }
                catch
                {
                    cycle.Station = "";
                }

                try {
                    cycle.OperatorID = operatore;
                }
                catch
                {
                    cycle.OperatorID = "";
                }

                try {
                    cycle.ScopeID = matricola;
                }
                catch
                {
                    cycle.ScopeID = "";
                }

                try {
                    cycle.Type = tipo_ciclo;
                }
                catch
                {
                    cycle.Type = "";
                }

                bool failedBool = !esito.Contains("CICLO REGOLARE");
                cycle.Failed = failedBool;

                // AGGIUNGO ESITO
                cycle.AdditionalInfoList.Add(new AdditionalInfo()
                {
                    Date = cycle.EndTimestamp,
                    Description = esito,
                    isAlarm = failedBool
                });

                AdditionalInfo tempalarm = getAlarm(text, cycle.EndTimestamp);
                if (tempalarm != null)
                {
                    cycle.AdditionalInfoList.Add(tempalarm);
                }

                cycle.AdditionalInfoList.Reverse();
                if (cycle.StartTimestamp == DateTime.MinValue)
                {
                    Program.Log.Error("StartTimeStamp error...");
                    return null;
                }

                if (cycle.EndTimestamp == DateTime.MinValue)
                {
                    Program.Log.Error("EndTimestamp error...");
                    return null;
                }


                return cycle;
            }

            return null;
        }

        private static DateTime ParseStringData(string inizio_ciclo)
        {
            int year, month, day, hour, minute, second;

            try
            {
                var match = Regex.Match(inizio_ciclo, "[0-9]+");

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
            catch (Exception exc)
            {
                Program.Log.Error(exc.Message);
                Program.Log.Error(exc.StackTrace);
                Program.Log.Error(inizio_ciclo);
            }

            return DateTime.MinValue;
        }

        private static AdditionalInfo getAlarm(string text, DateTime date)
        {
            AdditionalInfo info = null;

            try
            {
                if (text.Length > 0)
                {
                    int index = -1;
                    index = text.ToUpper().IndexOf("*ALLARME")+1;


                    if (index > 0)
                    {

                        int indexEnd = -1;

                        indexEnd = text.IndexOf("*", index + 1);

                        if (indexEnd > 0)
                        {
                            string alarm = "";
                            alarm = text.Substring(index, indexEnd - index);

                            if (alarm != null & alarm != "")
                            {
                                info = new AdditionalInfo();
                                info.isAlarm = true;
                                info.Description = alarm;

                                try
                                {
                                    info.Date = DateTime.ParseExact(date + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
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

        private static AdditionalInfo extractInfo(string a, string date)
        {
            AdditionalInfo info = null;

            if (a.Length >= 8)
            {
                info = new AdditionalInfo();
                try
                {
                    info.Date = DateTime.ParseExact(date + (a.Substring(0, 8).Replace(".", "")), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    info.Date = DateTime.MinValue;
                }

                if (a.Length > 8)
                {
                    info.Description = a.Substring(8).Trim();

                    if (info.Description.ToUpper().Contains("ALLARME"))
                    {
                        info.isAlarm = true;
                    }
                }
            }

            return info;
        }



        public string pDate(string ini)
        {
            string dateTemp = "";
            string date = "";

            try
            {
                int indT = ini.LastIndexOf("/");
                if (indT > 0)
                {
                    indT = indT + 4;
                    if (ini.Length >= indT)
                    {
                        dateTemp = ini.Substring(0, indT);
                    }
                    else
                    {
                        dateTemp = "";
                    }
                }
            }
            catch
            {
                dateTemp = "";
            }

            if (dateTemp != "")
            {

                date = dateTemp;
            }

            return date;
        }
    }
}
