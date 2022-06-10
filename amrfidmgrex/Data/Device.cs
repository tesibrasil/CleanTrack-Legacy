using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data.Odbc;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("24F1224C-EAC2-3B5A-A295-DFB74AD9544C")]
    public class Device : DBObject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int State { get; set; }

        private static Object devLock = new Object();

        private static List<Device> listDevices = new List<Device>();

        public static string Refresh()
        {
            Logger.Info("Device.Refresh...");
            string sReturn = "";

            OdbcConnection odbcConn = new OdbcConnection(ODBCConnectionString);
            try
            {
                odbcConn.Open();

                var query = "SELECT ID, Matricola, Descrizione, Tag, Stato FROM Dispositivi WHERE Dismesso=0";
                OdbcCommand odbcComm = new OdbcCommand(query, odbcConn);
                Logger.Info(query);
                OdbcDataReader odbcRead = odbcComm.ExecuteReader();

                List<Device> listDevTemp = new List<Device>();
                while (odbcRead.Read())
                {
                    if (!odbcRead.IsDBNull(0))
                    {
                        Device devTemp = new Device();

                        devTemp.ID = DBUtilities.GetIntValue(odbcRead, "ID");
                        devTemp.Code = !odbcRead.IsDBNull(1) ? odbcRead.GetString(1) : "";
                        devTemp.Description = !odbcRead.IsDBNull(2) ? odbcRead.GetString(2) : "";
                        devTemp.Tag = !odbcRead.IsDBNull(3) ? odbcRead.GetString(3) : "";
                        devTemp.State = !odbcRead.IsDBNull(4) ? DBUtilities.GetIntValue(odbcRead, "STATO") : -1;

                        listDevTemp.Add(devTemp);

                        Logger.Info("Added device: " + devTemp.Code);
                    }
                }

                odbcRead.Close();

                lock (devLock)
                {
                    listDevices.Clear();

                    foreach (Device devTemp in listDevTemp)
                    {
                        listDevices.Add(devTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
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

            if (listDevices.Count <= 0)
                Refresh();

            lock (devLock)
            {
                Logger.Info("Checking Existence:" + sBadgeTag);
                nReturn = listDevices.FindIndex(delegate (Device devTemp) { return (devTemp.Tag.ToUpper() == sBadgeTag.ToUpper()) || (devTemp.Code.ToUpper() == sBadgeTag.ToUpper()); });
            }

            Logger.Info("...Device.Exist");
            return nReturn >= 0;
        }

        public static Device Get(string sBadgeTag)
        {
            Device devReturn = null;

            lock (devLock)
            {
                devReturn = listDevices.Find(delegate (Device devTemp) { return (devTemp.Tag == sBadgeTag); });
            }

            return devReturn;
        }
    }
}
