using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data.Odbc;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("3625F02A-3619-3FFE-8854-76EBDB654B9F")]
    public class Washer : DBObject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int State { get; set; }

        private static Object devLock = new Object();

        private static List<Washer> listDevices = new List<Washer>();

        public string GetFullName()
        {
            return Description;
        }

        public static string Refresh()
        {
            Logger.Info("Device.Refresh...");
            string sReturn = "";

            OdbcConnection odbcConn = new OdbcConnection(ODBCConnectionString);
            try
            {
                odbcConn.Open();

                var query = "SELECT * FROM ARMADI_LAVATRICI WHERE Dismesso = 0";
                OdbcCommand odbcComm = new OdbcCommand(query, odbcConn);
                Logger.Info(query);
                OdbcDataReader odbcRead = odbcComm.ExecuteReader();

                List<Washer> listDevTemp = new List<Washer>();
                while (odbcRead.Read())
                {
                    if (!odbcRead.IsDBNull(0))
                    {
                        Washer temp = new Washer();

                        temp.ID = DBUtilities.GetIntValue(odbcRead, "ID");
                        temp.Code = !odbcRead.IsDBNull(odbcRead.GetOrdinal("MATRICOLA")) ? odbcRead.GetString(odbcRead.GetOrdinal("MATRICOLA")) : "";
                        temp.Description = !odbcRead.IsDBNull(odbcRead.GetOrdinal("DESCRIZIONE")) ? odbcRead.GetString(odbcRead.GetOrdinal("DESCRIZIONE")) : "";
                        temp.Tag = !odbcRead.IsDBNull(odbcRead.GetOrdinal("SERIALE")) ? odbcRead.GetString(odbcRead.GetOrdinal("SERIALE")) : "";

                        listDevTemp.Add(temp);

                        Logger.Info("Added device: " + temp.Code);
                    }
                }

                odbcRead.Close();

                lock (devLock)
                {
                    listDevices.Clear();

                    foreach (Washer devTemp in listDevTemp)
                    {
                        listDevices.Add(devTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                sReturn = ex.ToString();
            }

            odbcConn.Close();
            Logger.Info("...Device.Refresh");
            return sReturn;
        }

        public static bool Exist(string sBadgeTag)
        {
            Logger.Info("Device.Exist...");
            int nReturn = -1;

            lock (devLock)
            {
                Logger.Info("Checking Existence:" + sBadgeTag);
                nReturn = listDevices.FindIndex(delegate (Washer devTemp) { return (devTemp.Tag.ToUpper() == sBadgeTag.ToUpper()); });
            }

            Logger.Info("...Device.Exist");
            return nReturn >= 0;
        }

        public static Washer Get(string sBadgeTag)
        {
            Washer devReturn = null;

            lock (devLock)
            {
                devReturn = listDevices.Find(delegate (Washer devTemp) { return (devTemp.Tag.ToUpper() == sBadgeTag.ToUpper()); });
            }

            return devReturn;
        }

        public static Washer Get(int id)
        {
            Washer devReturn = null;

            lock (devLock)
            {
                devReturn = listDevices.Find(delegate (Washer devTemp) { return devTemp.ID == id; });
            }

            return devReturn;
        }
    }
}
