using System;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Odbc;
using System.IO;
using amrfidmgrex;

namespace WindowsService1
{
    class InfoCleaner
    {
        // !!!! da mettere in configurazione
        // private string DevicePrefix = "RC";
        private string DevicePrefix = "";

        private string cleaningOperator = "Operatore PLC: ";
        private string cleaningSuccess = "Esito: ";
        private string cleaningDevice1 = "Strumento 1: ";
        private string cleaningDevice2 = "Strumento 2: ";
        private string startDateMatch = "Data inizio ciclo: ";
        private string endDateMatch = "Data fine ciclo: ";
        private string startTimeMatch = "Ora inizio ciclo: ";
        private string endTimeMatch = "Ora fine ciclo: ";
        private string datePrefix = "";

        private string cycleDone = "-1";
        private string startDate = "-1";
        private string finishDate = "-1";

        private bool cycleSuccess = false;
        private string opName = "-1";
        private string device1 = "-1";
        private string device2 = "-1";

        public InfoCleaner(String cycleDone, String datePrefix)
        {
            this.cycleDone = cycleDone;
            this.datePrefix = datePrefix;
        }

        public bool loadInfos(String path, String idSterilizzatrice, String newState, String connectionString)
        {
            bool bReturn1 = false;
            bool bReturn2 = false;
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            String fileContent = sr.ReadToEnd();
            sr.Close();

            CleanerSynchro.WriteLog("- FILE --> " + path);

            this.cycleSuccess = getSuccess(fileContent);

            if (this.cycleSuccess)
            {
                CleanerSynchro.WriteLog("--- CICLO --> BENE");
                device1 = getDevice(fileContent, cleaningDevice1);
                CleanerSynchro.WriteLog("--- \t DEVICE1 ID:" + device1);
                device2 = getDevice(fileContent, cleaningDevice2);
                CleanerSynchro.WriteLog("--- \t DEVICE2 ID:" + device2);
                opName = getOperator(fileContent);
                CleanerSynchro.WriteLog("--- \t OPERATORE:" + opName);
                startDate = getDate(fileContent, startDateMatch, startTimeMatch);
                CleanerSynchro.WriteLog("--- \t StartDate:" + startDate.ToString());
                finishDate = getDate(fileContent, endDateMatch, endTimeMatch);
                CleanerSynchro.WriteLog("--- \t EndDate:" + finishDate.ToString());

                Device.ODBCConnectionString = connectionString;
                Device.Refresh();
                Operator.ODBCConnectionString = connectionString;
                Operator.Refresh();

                var CleanerID = amrfidmgrex.DBUtilities.getCleanerIdFromCode(idSterilizzatrice, connectionString);

                if (CleanerID >= 0 && (newState != "") && (Int32.Parse(newState) > 0))
                {
                    if ((device1 != "") && (device2 != ""))
                    {
                        if (device1 != "")
                            bReturn1 = amrfidmgrex.DBUtilities.insertCleanerCycle(device1, opName, CleanerID.ToString(), startDate, finishDate, connectionString);

                        if (device2 != "")
                            bReturn2 = amrfidmgrex.DBUtilities.insertCleanerCycle(device2, opName, CleanerID.ToString(), startDate, finishDate, connectionString);
                    }
                    else
                        bReturn1 = false;
                }
            }
            else
            {
                CleanerSynchro.WriteLog("--- CICLO --> MALE");
                bReturn1 = false;
            }

            bool result = bReturn1 || bReturn2;
            if (result)
                CleanerSynchro.WriteLog("--- RISULTATO --> OK");
            else
                CleanerSynchro.WriteLog("--- RISULTATO --> !!! ERRORE !!!");

            return result;
        }

        public bool insertCycle(string cleanerId, string dev, string op, string newState, string startDate, string finishDate, string connectionString)
        {
            int result = -100;

            try
            {
                OdbcConnection connection = new OdbcConnection(connectionString);
                connection.Open();
                OdbcCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "{call sp_steelcoSynchro (?,?,?,?,?,?,?,?)}";

                OdbcParameter paramRes = cmd.Parameters.Add("@nRES", OdbcType.Int);
                paramRes.Direction = ParameterDirection.Output;

                OdbcParameter paramCleaner = cmd.Parameters.Add("@nCLEANER", OdbcType.Int);
                paramCleaner.Direction = ParameterDirection.Input;
                paramCleaner.Value = Int32.Parse(cleanerId);

                OdbcParameter paramDev = cmd.Parameters.Add("@sDISPOSITIVO", OdbcType.VarChar);
                paramDev.Direction = ParameterDirection.Input;
                paramDev.Value = dev;

                string[] sn = op.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string name = "";
                string surname = "";

                if (sn.Length > 0) { surname = sn[0]; }
                if (sn.Length > 1) { name = sn[1]; }

                OdbcParameter paramOpSurname = cmd.Parameters.Add("@sOPSURNAME", OdbcType.VarChar);
                paramOpSurname.Direction = ParameterDirection.Input;
                paramOpSurname.Value = surname;

                OdbcParameter paramOpName = cmd.Parameters.Add("@sOPNAME", OdbcType.VarChar);
                paramOpName.Direction = ParameterDirection.Input;
                paramOpName.Value = name;

                OdbcParameter paramNewState = cmd.Parameters.Add("@nNEWSTATE", OdbcType.Int);
                paramNewState.Direction = ParameterDirection.Input;
                paramNewState.Value = Int32.Parse(newState);

                OdbcParameter paramDateClean = cmd.Parameters.Add("@sDATECLEAN", OdbcType.VarChar);
                paramDateClean.Direction = ParameterDirection.Input;
                paramDateClean.Value = startDate;

                OdbcParameter paramDateFinish = cmd.Parameters.Add("@sDATEFINISH", OdbcType.VarChar);
                paramDateFinish.Direction = ParameterDirection.Input;
                paramDateFinish.Value = finishDate;

                cmd.ExecuteNonQuery();

                result = (int)paramRes.Value;

                connection.Close();

                switch (result)
                {
                    case -2:
                        CleanerSynchro.WriteLog("----- SONDA --> " + dev + " --> (" + result + ") --> seriale della sonda non trovato nel db");
                        break;
                    case -1:
                        CleanerSynchro.WriteLog("----- SONDA --> " + dev + " --> (" + result + ") --> errore non identificato");
                        break;
                    default:
                        CleanerSynchro.WriteLog("----- SONDA --> " + dev + " --> (" + result + ")");
                        break;
                }
            }
            catch (Exception e)
            {
                CleanerSynchro.WriteLog("----- SONDA --> " + dev + " --> (" + result + ") --> " + e.Message);
            }

            return (result > 0);
        }

        public bool getSuccess(string text)
        {
            string name = "";

            Match match = Regex.Match(text, cleaningSuccess + ".*");
            if (match.Success)
            {
                char[] delim = { '\n', '\r', ' ', ':', '.' };

                name = match.Value.Trim(delim);

                name = name.Replace(cleaningSuccess, "");
                name = name.Replace("\n", "");
                name = name.Replace("\r", "");
                name = name.Replace(": ", "");
                name = name.Replace(".", "");
                name = name.Trim();
            }

            if (name.Length > 0)
            {
                string state = name.Substring(0, 1);
                return (state == cycleDone);
            }

            return false;
        }

        public string getDate(string text, string regDate, string regTime)
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

            if ((date.Length == 6) && (datePrefix != ""))
            {
                return datePrefix + date.Substring(4, 2) + date.Substring(2, 2) + date.Substring(0, 2) + time;
            }
            else
            {
                return "";
            }
        }

        public string getOperator(string text)
        {
            string name = "";

            Match match = Regex.Match(text, cleaningOperator + ".*");
            if (match.Success)
            {
                char[] delim = { '\n', '\r', ' ', ':', '.' };

                name = match.Value.Trim(delim);

                name = name.Replace(cleaningOperator, "");
                name = name.Replace("\n", "");
                name = name.Replace("\r", "");
                name = name.Replace(": ", "");
                name = name.Replace(".", "");
                name = name.Trim();
            }

            return name;
        }

        public string getDevice(string text, string cDev)
        {
            string name = "";

            Match match = Regex.Match(text, cDev + ".*");
            if (match.Success)
            {
                char[] delim = { '\n', '\r', ' ', ':', '.', '|' };

                name = match.Value.Trim(delim);

                name = name.Replace(cDev, "");
                name = name.Replace("\n", "");
                name = name.Replace("\r", "");
                name = name.Replace(": ", "");
                name = name.Replace(".", "");
                name = name.Replace("|", "");
                name = name.Trim();
            }

            return DevicePrefix + (name.Substring(0, name.IndexOf(' ')));
        }

        public static int GetIntValue(OdbcDataReader reader, string field)
        {
            int value = -1;

            try
            {
                value = reader.GetInt32(reader.GetOrdinal(field));
                return value;
            }
            catch (System.InvalidCastException /*exception*/)
            {
            }

            try
            {
                value = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal(field)));
                return value;
            }
            catch (System.InvalidCastException /*exception*/)
            {
            }

            return value;
        }
    }
}
