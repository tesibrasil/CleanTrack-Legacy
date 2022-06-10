using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Text;


namespace IMS7_PARSER
{
    public static class AMRFIDMGREXExtension
    {
        public static string getDeviceTagFromSerial(string Serial, string connectionString)
        {
            string tag = "";

            if (Serial != null && Serial != "")
            {
                OdbcConnection connection = new OdbcConnection(connectionString);
                OdbcDataReader dr = null;

                try
                {
                    connection.Open();
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = connection;
                    cmdFCod.CommandText = "SELECT TAG FROM DISPOSITIVI WHERE SERIALE= '" + Serial + "' AND DISMESSO=0";
                    dr = cmdFCod.ExecuteReader();
                    dr.Read();
                    if (!dr.IsDBNull(0))
                    {
                        tag = dr.GetString(0);
                    }
                }
                catch (Exception)
                {
                    tag = "";
                }

                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                connection.Close();
            }

            return tag;

        }


        public static string getOperatorTagFromNomeCognome(string nome, string cognome, string connectionString)
        {
            string tag = "";

            if (nome != null && nome != "" && cognome != null && cognome != "")
            {
                OdbcConnection connection = new OdbcConnection(connectionString);
                OdbcDataReader dr = null;

                try
                {
                    connection.Open();
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = connection;
                    if (nome != "" && cognome != "")
                    {
                        if (nome != cognome & !nome.Contains("."))
                            cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE NOME= '" + nome + "' AND COGNOME='" + cognome + "' AND DISATTIVATO=0";
                        else
                            cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE NOME= '" + nome + "' OR COGNOME='" + cognome + "' AND DISATTIVATO=0";
                    }

                    dr = cmdFCod.ExecuteReader();
                    dr.Read();
                    if (!dr.IsDBNull(0))
                    {

                        tag = dr.GetString(0);

                    }
                }
                catch (Exception)
                {
                    tag = "";
                }

                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }

                connection.Close();
            }

            return tag;

        }

        public static void insertNewCycleMedivators(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, DateTime date, Cycle cycle, string connectionString)
        {
            int id = amrfidmgrex.DBUtilities.insertnewCycleWithDateForReturn(deviceTag, userTag, cleaner, state, oldState, idExamToSave, date, connectionString);
            SDBUtil.update("CICLI", id, "IDVasca", "'" + cycle.Station + "'", "ID", connectionString);
            SDBUtil.update("CICLI", id, "MachineCycleId", "'" + cycle.CycleCount + "'", "ID", connectionString);

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
                    Program.Log.Error("Error in AdditionalInfoDate: " + e.Message);
                    Program.Log.Error(e.StackTrace);
                }

                SDBUtil.update("CICLIEXT", newId, "Error", "" + (info.isAlarm ? 1 : 0), "ID", connectionString);

            }
        }


        private static string FormatRatio(string p)
        {
            string newval = p;

            if (p.Length == 9)
            {
                try
                {
                    newval = p.Substring(0, 3) + ":" + p.Substring(3, 3) + ":" + p.Substring(6, 2) + "." + p.Substring(8, 1);
                }
                catch
                {
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
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " - " + text);
                sw.Close();
            }
            catch
            {
            }
        }
    }
}
