using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data.Odbc;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("108BF4BD-3D32-32FA-AC6D-BA9289780104")]
    public class Operator : DBObject
	{
		public int nID { get; set; }
		public string sMatricola { get; set; }
		public string sCognome { get; set; }
		public string sNome { get; set; }
		public string sTag { get; set; }

		private static Object opLock = new Object();

		private static List<Operator> listOperators = new List<Operator>();

        public string GetFullName()
        {
            return sCognome + " " + sNome;
        }

		public static string Refresh()
		{
			Logger.Info("Operator.Refresh...");
			string sReturn = "";

			OdbcConnection odbcConn = new OdbcConnection(ODBCConnectionString);
			try
			{
				odbcConn.Open();

				var query = "SELECT ID, Matricola, Cognome, Nome, Tag FROM Operatori WHERE Disattivato=0";

                OdbcCommand odbcComm = new OdbcCommand(query, odbcConn);
				Logger.Info(query);
				OdbcDataReader odbcRead = odbcComm.ExecuteReader();

				List<Operator> listOpTemp = new List<Operator>();
				while (odbcRead.Read())
				{
					if (!odbcRead.IsDBNull(0))
					{
						Operator opTemp = new Operator();

						opTemp.nID = DBUtilities.GetIntValue(odbcRead, "ID");
						opTemp.sMatricola = !odbcRead.IsDBNull(1) ? odbcRead.GetString(1) : "";
						opTemp.sCognome = !odbcRead.IsDBNull(2) ? odbcRead.GetString(2) : "";
						opTemp.sNome = !odbcRead.IsDBNull(3) ? odbcRead.GetString(3) : "";
						opTemp.sTag = !odbcRead.IsDBNull(4) ? odbcRead.GetString(4) : "";

						listOpTemp.Add(opTemp);
						Logger.Info("Added op: " + opTemp.nID.ToString() + " -> " + opTemp.sCognome + " " + opTemp.sNome);
					}
				}

				odbcRead.Close();

				lock (opLock)
				{
					listOperators.Clear();

					foreach (Operator opTemp in listOpTemp)
					{
						listOperators.Add(opTemp);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				sReturn = ex.ToString();
			}

			odbcConn.Close();
			Logger.Info("...Operator.Refresh");
			return sReturn;
		}

        public static bool ExistFromAnagraphicData(string sAnagraphicData)
        {
            Logger.Info("Operator.Exist...");
            int nReturn = -1;

            lock (opLock)
            {
                Logger.Info("Checking Existence:" + sAnagraphicData);
                nReturn = listOperators.FindIndex(delegate (Operator opTemp) {
                    return ((opTemp.sCognome.ToUpper() == sAnagraphicData) ||
                    (opTemp.sCognome.ToUpper() + " " + opTemp.sNome.ToUpper() == sAnagraphicData) ||
                    (opTemp.sNome.ToUpper() + " " + opTemp.sCognome.ToUpper() == sAnagraphicData));
                });
            }

            Logger.Info("...Operator.Exist");
            return nReturn >= 0;
        }


        public static bool Exist(string sBadgeTag)
		{
			Logger.Info("Operator.Exist...");
			int nReturn = -1;

            if (listOperators.Count <= 0)
                Refresh();

            lock (opLock)
			{
				Logger.Info("Checking Existence:" + sBadgeTag);
				nReturn = listOperators.FindIndex(delegate (Operator opTemp) { return (opTemp.sTag.ToUpper() == sBadgeTag.ToUpper()) || (opTemp.sMatricola.ToUpper() == sBadgeTag.ToUpper()); });
			}

			Logger.Info("...Operator.Exist");
			return nReturn >= 0;
		}

		public static Operator Get(string sBadgeTag)
		{
			Logger.Info("Operator.Get...");
			Operator opReturn = null;

			lock (opLock)
			{
				opReturn = listOperators.Find(delegate (Operator opTemp) { return (opTemp.sTag.ToUpper() == sBadgeTag.ToUpper()); });
			}

			Logger.Info("...Operator.Get");
			return opReturn;
		}

        public static Operator Get(int id)
        {
            Logger.Info("Operator.Get...");
            Operator opReturn = null;

            lock (opLock)
            {
                opReturn = listOperators.Find(delegate (Operator opTemp) { return (opTemp.nID == id); });
            }

            Logger.Info("...Operator.Get");
            return opReturn;
        }

    }
}
