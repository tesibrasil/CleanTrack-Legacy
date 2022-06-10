using System;
using System.Data.Odbc;

namespace ISAWD_PARSER
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

		public static string getOperatorTagFromNomeCognome(string nome,string cognome, string connectionString)
        {
            string tag = "";

            if ((nome != null) && (nome != "") && (cognome != null) && (cognome != ""))
            {
                OdbcConnection connection = new OdbcConnection(connectionString);
                OdbcDataReader dr = null;

                try
                {
                    connection.Open();
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = connection;
                    if (nome != "" && cognome != "" )
                    {
                        if(nome!=cognome)
                        {
                            cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE NOME= '" + nome + "' AND COGNOME='" + cognome + "' AND DISATTIVATO=0";
                        }
                        else
                        {                        
                            cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE NOME= '" + nome + "' OR COGNOME='"+cognome+"' AND DISATTIVATO=0";                               
                        }
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

		// Sandro 17/04/2019 //
		public static string getOperatorTagFromMatricola(string sMatricola, string connectionString)
		{
			string sReturn = "";

			if (sMatricola.Length > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE MATRICOLA LIKE '" + sMatricola + "' AND DISATTIVATO = 0";

					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						sReturn = dr.GetString(0);
					}
				}
				catch (Exception)
				{
					sReturn = "";
				}

				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}

				connection.Close();
			}

			return sReturn;
		}

        public static string getOperatorTagFromTag(string sTag, string connectionString)
        {
            string sReturn = "";

            if (sTag.Length > 0)
            {
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (OdbcCommand cmdFCod = new OdbcCommand("SELECT TAG FROM OPERATORI WHERE TAG LIKE '" + sTag + "' AND DISATTIVATO = 0", connection))
                        {
                            using (OdbcDataReader dr = cmdFCod.ExecuteReader())
                            {
                                dr.Read();

                                if (!dr.IsDBNull(0))
                                    sReturn = dr.GetString(0);

                                if (!dr.IsClosed)
                                    dr.Close();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        sReturn = "";
                    }

                    connection.Close();
                }
            }

            return sReturn;
        }

        public static void insertNewCycleMedivators(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, DateTime date, Cycle cycle, string connectionString, int idcycle)
        {
            //int id = amrfidmgrex.DBUtilities.insertnewCycleWithDateForReturn(deviceTag, userTag, cleaner, state, oldState, idExamToSave, date, connectionString);
            //SDBUtil.update("CICLI", id, "IDVasca", "'" + cycle.Station + "'", "ID", connectionString);
            //SDBUtil.update("CICLI", id, "MachineCycleId", "'" + cycle.CycleCount + "'", "ID", connectionString);

            foreach (AdditionalInfo info in cycle.AdditionalInfoList)
            {
                if (info.Description.Trim() == "Ratio=")
                {
                    info.Value = FormatRatio(info.Value.Trim());
                }

                try
                {
                    OdbcConnection conn = new OdbcConnection(connectionString);
                    conn.Open();
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = conn;
                    string stringDate = info.Date.ToString("yyyyMMddHHmmss");
                    cmdFCod.CommandText = "INSERT INTO CICLIEXT (IDCICLO, DESCRIZIONE, VALORE, DATA, ERROR) VALUES (" + 
                        idcycle + ", '" + info.Description + "', '" + info.Value + "', '" + stringDate + "', " + (info.isAlarm ? 1 : 0) + ")";
                    var rowAffected = cmdFCod.ExecuteNonQuery();
					Program.WriteLog("insertNewCycleMedivators", "INFO", rowAffected.ToString() + " rows added in CICLIEXT");

                    conn.Close();
                }
                catch (Exception ex)
                {
					Program.WriteLog("insertNewCycleMedivators", "ERROR", ex.Message);
					Program.WriteLog("insertNewCycleMedivators", "ERROR", ex.StackTrace);
                }
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
	}
}
