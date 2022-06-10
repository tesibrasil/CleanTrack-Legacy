using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleanTrak.Models;
using LibLog;

namespace KleanTrak.Managers
{
    public class StateTransactions
    {
        static public List<StateTransaction> Get()
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT StatoCambio.ID, StatoCambio.IDStatoOld, StatoOld.Descrizione AS StatoOld, StatoCambio.IDStatoNew, StatoNew.Descrizione AS StatoNew, StatoNew.InizioCiclo FROM StatoCambio INNER JOIN Stato StatoOld ON StatoCambio.IDStatoOld = StatoOld.ID INNER JOIN Stato StatoNew ON StatoCambio.IDStatoNew = StatoNew.ID WHERE StatoCambio.Eliminato = 0");

            List<StateTransaction> ret = new List<StateTransaction>();
            foreach (var record in dataset)
            {
                StateTransaction t = new StateTransaction()
                {
                    ID = record.GetInt("ID").Value,
                    IDStateOld = record.GetInt("IDStatoOld").Value,
                    StateOld = record.GetString("StatoOld"),
                    IDStateNew = record.GetInt("IDStatoNew").Value,
                    StateNew = record.GetString("StatoNew"),
                    InsertNewCycle = record.GetBoolean("InizioCiclo").Value
                };

                ret.Add(t);
            }

            return ret;
        }

        static public bool IsValid(int oldStateID, int newStateID)
        {
            return (from s in Get() where s.IDStateOld == oldStateID && s.IDStateNew == newStateID select s).Count() > 0;
        }

        static public IEnumerable<int> GetValidPreviousState(int newStateID)
        {
            return (from s in Get() where s.IDStateNew == newStateID select s.IDStateOld);
        }

        static public void Add(int deviceID, int userID, int stateOld, int stateNew)
        {
            if (!IsValid(stateOld, stateNew))
                return;

            StateTransaction st = (from s in Get() where s.IDStateOld == stateOld && s.IDStateNew == stateNew select s).First();

            DbConnection db = new DbConnection();

            DbRecordset dataset = db.ExecuteReader("SELECT COUNT(*) AS CONTEGGIO FROM CICLI WHERE IDDispositivo = " + deviceID.ToString());
            if ((dataset.Count == 1 && dataset[0].GetInt("CONTEGGIO") == 0) || st.InsertNewCycle)
                db.ExecuteNonQuery(string.Format("INSERT INTO Cicli (IDDispositivo) VALUES ({0:d})", deviceID));

            int lastCycleID = 0;
            dataset = db.ExecuteReader("SELECT MAX(ID) AS IDCICLO FROM CICLI WHERE IDDispositivo = " + deviceID.ToString());
            if (dataset.Count == 1)
                lastCycleID = dataset[0].GetInt("IDCICLO").Value;

            DateTime now = DateTime.Now;
            db.ExecuteNonQuery(string.Format("INSERT INTO CicliStatoLog (IDCICLO, IDSTATOOLD, IDSTATONEW, IDOPERATORE, DATAORA) VALUES ({0:d}, {1:d}, {2:d}, {3:d}, '{4:d4}{5:d2}{6:d2}{7:d2}{8:d2}{9:d2}')",
                                             lastCycleID,
                                             stateOld,
                                             stateNew,
                                             userID,
                                             now.Year,
                                             now.Month,
                                             now.Day,
                                             now.Hour,
                                             now.Minute,
                                             now.Second));

            db.ExecuteNonQuery(string.Format("UPDATE Dispositivi SET Stato = {0:d}, IDOperatoreStato = {1:d}, DataStato = '{2:d4}{3:d2}{4:d2}{5:d2}{6:d2}{7:d2}' WHERE ID = {8:d}",
                                             stateNew,
                                             userID,
                                             now.Year,
                                             now.Month,
                                             now.Day,
                                             now.Hour,
                                             now.Minute,
                                             now.Second,
                                             deviceID));
        }

        static private void ForceAdd(int deviceID, int userID, int stateNew)
        {
            int lastCycleID = GetLastCycleID(deviceID);

            DbConnection db = new DbConnection();
            DateTime now = DateTime.Now;

            db.ExecuteNonQuery(string.Format("INSERT INTO CicliStatoLog (IDCICLO, IDSTATOOLD, IDSTATONEW, IDOPERATORE, DATAORA) VALUES ({0:d}, (SELECT STATO FROM DISPOSITIVI WHERE ID = {1:d}), {2:d}, {3:d}, '{4:d4}{5:d2}{6:d2}{7:d2}{8:d2}{9:d2}')",
                                             lastCycleID,
                                             deviceID,
                                             stateNew,
                                             userID,
                                             now.Year,
                                             now.Month,
                                             now.Day,
                                             now.Hour,
                                             now.Minute,
                                             now.Second));

            db.ExecuteNonQuery(string.Format("UPDATE Dispositivi SET Stato = {0:d}, IDOperatoreStato = {1:d}, DataStato = '{2:d4}{3:d2}{4:d2}{5:d2}{6:d2}{7:d2}' WHERE ID = {8:d}",
                                             stateNew,
                                             userID,
                                             now.Year,
                                             now.Month,
                                             now.Day,
                                             now.Hour,
                                             now.Minute,
                                             now.Second,
                                             deviceID));
        }

        static private int GetLastCycleID(int deviceID)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT COUNT(*) AS CONTEGGIO FROM CICLI WHERE IDDispositivo = " + deviceID.ToString());
            if ((dataset.Count == 1 && dataset[0].GetInt("CONTEGGIO") == 0))
                db.ExecuteNonQuery(string.Format("INSERT INTO Cicli (IDDispositivo) VALUES ({0:d})", deviceID));

            int lastCycleID = 0;
            dataset = db.ExecuteReader("SELECT MAX(ID) AS IDCICLO FROM CICLI WHERE IDDispositivo = " + deviceID.ToString());
            if (dataset.Count == 1)
                lastCycleID = dataset[0].GetInt("IDCICLO").Value;

            return lastCycleID;
        }

        static public bool Add(WasherCycle cycle)
        {
            int stateWashing = StateTransactions.GetStateWashing();
            int stateWashed = StateTransactions.GetStateWashed();

            if (stateWashing <= 0 || stateWashed <= 0)
            {
                Logger.Get().Write("", "StateTransactions.Add", "Error...configure states for washing/washed device!!!", null, Logger.LogLevel.Error);
                return false;
            }

            if (!IsValid(stateWashing, stateWashed))
            {
                Logger.Get().Write("", "StateTransactions.Add", "Error...configure valid transaction for washing/washed state!!!", null, Logger.LogLevel.Error);
                return false;
            }

            if (!cycle.ValidData)
            {
                InsertParsingData(cycle, WasherCycle.ParsingErrors.MissingTranscode, 0, "");
                Logger.Get().Write("", "StateTransactions.Add", "Cycle missing data!! (can be DeviceID, OperatorStartID or StartTimestamp)", null, Logger.LogLevel.Error);
                return false;
            }

            try
            {
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader("SELECT Stato, DataStato FROM Dispositivi WHERE ID = " + cycle.DeviceID);

                int stateID = dataset[0].GetInt("Stato").Value;
                string stateDate = dataset[0].GetString("DataStato");
                if (stateDate == null)
                    return false;
				if (stateDate.Length != 14)
					return false;

				DateTime date = new DateTime(Convert.ToInt32(stateDate.Substring(0, 4)),
                                             Convert.ToInt32(stateDate.Substring(4, 2)),
                                             Convert.ToInt32(stateDate.Substring(6, 2)),
                                             Convert.ToInt32(stateDate.Substring(8, 2)),
                                             Convert.ToInt32(stateDate.Substring(10, 2)),
                                             Convert.ToInt32(stateDate.Substring(12, 2)));

#if (!DEBUG)
				if (date > cycle.StartTimestamp)
                {
                    InsertParsingData(cycle, WasherCycle.ParsingErrors.DateWrong, stateID, stateDate);
                    Logger.Get().Write("", "StateTransactions.Add", "Last device state change is after than washer cycle start timestamp!!", null, Logger.LogLevel.Error);
                    return false;
                }

                if (!GetValidPreviousState(stateWashing).Contains(stateID))
                    Logger.Get().Write("", "StateTransactions.Add", "Error device state " + stateID.ToString() + " before washing!!!" , null, Logger.LogLevel.Error);
#endif

                ForceAdd(cycle.DeviceID, cycle.OperatorStartID, stateWashing);

                int lastCycleID = GetLastCycleID(cycle.DeviceID);
                foreach (var info in cycle.AdditionalInfoList)
                {
					if (info.Value == null)
						info.Value = "";

					String sNonQuery = string.Format("INSERT INTO CicliExt (IDCICLI, DESCRIZIONE, VALORE, DATA, ERROR) VALUES ({0:d}, '{1:s}', '{2:s}', '{3:s}', {4:d})",
													 lastCycleID,
													 info.Description.Replace("'", "''"),
													 info.Value.Replace("'", "''"),
													 info.Date.ToString("yyyyMMddHHmmss"),
													 info.isAlarm ? 1 : 0);

					db.ExecuteNonQuery(sNonQuery);
                }

                db.ExecuteNonQuery(string.Format("UPDATE Cicli SET IDSterilizzatrice = {0:d} WHERE ID = {1:d}",
                                                 lastCycleID,
                                                 cycle.WasherID));

                if (cycle.Completed && !cycle.Failed)
                    Add(cycle.DeviceID, cycle.OperatorEndID, stateWashing, stateWashed);

                InsertParsingData(cycle, WasherCycle.ParsingErrors.NoError, stateID, stateDate);
            }
            catch (Exception ex)
            {
                Logger.Get().Write("", "StateTransactions.Add", "Exception " + ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
            }

            return true;
        }

        static public int GetStateWashing()
        {
            int ret = 0;

            try
            {
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader("SELECT MAX(ID) AS ID FROM STATO WHERE InizioSterilizzazione = 1");
                if (dataset.Count == 1 && dataset[0].GetInt("ID").HasValue)
                    ret = dataset[0].GetInt("ID").Value;
            }
            catch (Exception)
            {
            }

            return ret;
        }

        static public int GetStateWashed()
        {
            int ret = 0;

            try
            {
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader("SELECT MAX(ID) AS ID FROM STATO WHERE FineSterilizzazione = 1");
                if (dataset.Count == 1 && dataset[0].GetInt("ID").HasValue)
                    ret = dataset[0].GetInt("ID").Value;
            }
            catch (Exception)
            {
            }

            return ret;
        }

        static private void InsertParsingData(WasherCycle cycle, WasherCycle.ParsingErrors errorCode, int stateOld, string dateStateOld)
        {
            string query = string.Format("INSERT INTO STERILIZZATRICIPARSING (IDSTERILIZZATRICE, DATAPARSING, FILENAMEDATA, FILENAME, CONTENUTOFILE, CICLOCOMPLETO, ERROREIMPORT, IDDISPOSITIVO, DATAORASTART, IDOPERATORESTART, DATAORAEND, IDOPERATOREEND, DATASTATODISPOSITIVO, STATODISPOSITIVO) VALUES ({0:d}, '{1:s}', '{2:s}', '{3:s}', ##START_STRING_PARAMETER{4:s}END_STRING_PARAMETER##, {5:d}, {6:d}, {7:d}, '{8:s}', {9:d}, '{10:s}', {11:d}, '{12:s}', {13:d})",
                                         cycle.WasherID,
                                         DateTime.Now.ToString("yyyyMMddHHmmss"),
                                         cycle.FileDatetime.ToString("yyyyMMddHHmmss"),
                                         cycle.Filename,
                                         cycle.FileContent,
                                         cycle.Completed ? 1 : 0,
                                         (int)errorCode,
                                         cycle.DeviceID,
                                         cycle.StartTimestamp.ToString("yyyyMMddHHmmss"),
                                         cycle.OperatorStartID,
                                         cycle.EndTimestamp.ToString("yyyyMMddHHmmss"),
                                         cycle.OperatorEndID,
                                         dateStateOld,
                                         stateOld);

            DbConnection db = new DbConnection();
            db.ExecuteNonQuery(query);
        }
    }
}
