using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using KleanTrak.Model;
using OdbcExtensions;
using Commons;

namespace KleanTrak.Core
{
    public class StateTransactions
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const int FAKE_WASHERID = -3;
        public delegate void DeviceChangedHandler(DeviceStateChangeData obj);
        static public event DeviceChangedHandler DeviceStateChanged;

        static public List<StateTransaction> Get()
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader($"SELECT STATOCAMBIO.ID, STATOCAMBIO.IDSTATOOLD, " +
                $"STATOOLD.DESCRIZIONE AS STATOOLD, STATOCAMBIO.IDSEDE, " +
                $"STATOCAMBIO.IDSTATONEW, STATONEW.DESCRIZIONE AS STATONEW, STATONEW.INIZIOCICLO " +
                $"FROM STATOCAMBIO INNER JOIN Stato STATOOLD ON STATOCAMBIO.IDSTATOOLD = STATOOLD.ID " +
                $"INNER JOIN Stato STATONEW ON STATOCAMBIO.IDSTATONEW = STATONEW.ID " +
                $"WHERE STATOCAMBIO.Eliminato = 0");
            List<StateTransaction> ret = new List<StateTransaction>();
            foreach (var record in dataset)
            {
                StateTransaction t = new StateTransaction()
                {
                    ID = record.GetInt("ID").Value,
                    IDStateOld = record.GetInt("IDSTATOOLD").Value,
                    StateOld = record.GetString("STATOOLD"),
                    IDStateNew = record.GetInt("IDSTATONEW").Value,
                    StateNew = record.GetString("STATONEW"),
                    InsertNewCycle = record.GetBoolean("INIZIOCICLO").Value,
                    Id_sede = record.GetInt("IDSEDE").Value
                };

                ret.Add(t);
            }
            return ret;
        }

        static public bool IsValid(int oldStateID, int newStateID, int id_sede)
        {
            return (from s in Get() where s.IDStateOld == oldStateID && s.IDStateNew == newStateID && s.Id_sede == id_sede select s).Count() > 0;
        }
        static private int UpdateDeviceAndCycleLog(WasherCycle cycle,
            int stateOld,
            int stateNew,
            bool force_new_cycle,
            int id_sede,
            int? examID = null,
            int? idsedeesame = null,
            int? iduoesame = null)
        {
            try
            {
                if (!force_new_cycle && !IsValid(stateOld, stateNew, id_sede))
                {
                    var descr_state_old = GetStateName(stateOld);
                    var descr_state_new = GetStateName(stateNew);
                    Logger.Error($"Device {cycle.DeviceID} " +
                        $"--> Error transaction from state {stateOld} ({descr_state_old}) " +
                        $"to state {stateNew} ({descr_state_new}) not allowed!");
                    return 0;
                }
                DateTime data_cambio_stato = cycle.StartTimestamp;
                if (cycle.Completed)
                    data_cambio_stato = cycle.EndTimestamp;
                int last_cycle_id = GetLastCycleID(cycle.DeviceID,
                    force_new_cycle,
                    stateNew,
                    examID,
                    idsedeesame,
                    iduoesame);
                string str_data_stato = data_cambio_stato.Year.ToString() +
                    data_cambio_stato.Month.ToString().PadLeft(2, '0') +
                    data_cambio_stato.Day.ToString().PadLeft(2, '0') +
                    data_cambio_stato.Hour.ToString().PadLeft(2, '0') +
                    data_cambio_stato.Minute.ToString().PadLeft(2, '0') +
                    data_cambio_stato.Second.ToString().PadLeft(2, '0');
                int id_operator = GetCycleOperator(cycle);
                DbConnection db = new DbConnection();
                var cyclelogquery = $"INSERT INTO CICLISTATOLOG " +
                    $"(IDCICLO, IDSTATOOLD, IDSTATONEW, IDOPERATORE, DATAORA) " +
                    $"VALUES " +
                    $"({last_cycle_id}, " +
                    $"(SELECT STATO FROM DISPOSITIVI WHERE ID = {cycle.DeviceID}), " +
                    $"{stateNew}, " +
                    $"{id_operator}, " +
                    $"'{str_data_stato}')";
                db.ExecuteNonQuery(cyclelogquery);
                Logger.Info("______________________ UPDATING CYCLE LOG _________________________");
                Logger.Info(cyclelogquery);
                Logger.Info("___________________________________________________________________");
                var devicestatusquery = $"UPDATE DISPOSITIVI SET STATO = {stateNew}, " +
                    $"IDOPERATORESTATO = {id_operator}, " +
                    $"DATASTATO = '{str_data_stato}' " +
                    $"WHERE ID = {cycle.DeviceID}";
                db.ExecuteNonQuery(devicestatusquery);
                Logger.Info("------------------ UPDATING DEVICE STATUS -------------------------");
                Logger.Info(devicestatusquery);
                Logger.Info("-------------------------------------------------------------------");

                OnDeviceStateChanged(cycle, stateOld, stateNew, data_cambio_stato, last_cycle_id);
                return last_cycle_id;
            }
            catch (Exception e)
            {
                Logger.Error($"cycle.DeviceID :{cycle.DeviceID} " +
                    Environment.NewLine +
                    $"stateOld :{stateOld} " +
                    Environment.NewLine +
                    $"stateNew :{stateNew} " +
                    Environment.NewLine +
                    $"force_new_cycle :{force_new_cycle} " +
                    Environment.NewLine +
                    $"id_sede :{id_sede} " +
                    Environment.NewLine,
                    e);
                return -1;
            }
        }

        private static int GetCycleOperator(WasherCycle cycle)
        {
            int id_operator = Operators.GetUnknownOperator().ID;
            // gestione separata per gli armadi
            if (cycle.IsStorage)
            {
                if (cycle.DesiredDestinationState == GetStateId(FixedStates.Start_store))
                    id_operator = cycle.OperatorStartID;
                else
                    id_operator = cycle.OperatorEndID;
                return id_operator;
            }
            if (!cycle.Completed && cycle.OperatorStartID > 0)
                id_operator = cycle.OperatorStartID;
            if (cycle.Completed && cycle.OperatorEndID > 0)
                id_operator = cycle.OperatorEndID;
            return id_operator;
        }
        public static int GetLastCycleID(int deviceID,
            bool bForceCreateNewCycle,
            int new_state_id,
            int? examID = null,
            int? idsedeesame = null,
            int? iduoesame = null)
        {
            DbConnection db = new DbConnection();
            string query_count = $"SELECT COUNT(*) AS CONTEGGIO FROM CICLI WHERE IDDISPOSITIVO = {deviceID}";
            if (examID != null)
            {
                query_count = $"SELECT COUNT(*) AS CONTEGGIO FROM CICLI " +
                    $"WHERE IDDISPOSITIVO = {deviceID} " +
                    $"AND IDESAME = {examID} " +
                    $"AND IDSEDEESAME = {idsedeesame} " +
                    $"AND IDUOESAME = {iduoesame}";
            }
            DbRecordset dataset = db.ExecuteReader(query_count);
            if (((dataset.Count == 1) && (dataset[0].GetInt("CONTEGGIO") == 0)) ||
                bForceCreateNewCycle ||
                new_state_id == GetStateId(FixedStates.Start_cycle))
            {
                string query = $"INSERT INTO CICLI (IDDISPOSITIVO) VALUES ({deviceID})";
                if (examID != null)
                    query = $"INSERT INTO CICLI (IDDISPOSITIVO, IDESAME, IDSEDEESAME, IDUOESAME) " +
                    $"VALUES ({deviceID}, {examID}, {idsedeesame}, {iduoesame})";
                db.ExecuteNonQuery(query);
            }
            int lastCycleID = 0;
            dataset = db.ExecuteReader($"SELECT MAX(ID) AS IDCICLO FROM CICLI WHERE IDDISPOSITIVO = {deviceID}");
            if (dataset.Count == 1)
                lastCycleID = dataset[0].GetInt("IDCICLO").Value;
            return lastCycleID;
        }
        static private bool CheckSatesId(out int id_state_dirty,
            out int id_state_wash_start,
            out int id_state_wash_end,
            out int id_state_store_start,
            out int id_state_store_end)
        {
            id_state_dirty = GetStateId(FixedStates.Start_cycle);
            id_state_wash_start = GetStateId(FixedStates.Start_wash);
            id_state_wash_end = GetStateId(FixedStates.End_wash);
            id_state_store_start = GetStateId(FixedStates.Start_store);
            id_state_store_end = GetStateId(FixedStates.End_store);
            if (id_state_dirty <= 0)
            {
                Logger.Error("configure state for dirty device!!!");
                return false;
            }
            if ((id_state_wash_start <= 0) || (id_state_wash_end <= 0))
            {
                Logger.Error("configure states for start/end wash device!!!");
                return false;
            }
            if ((id_state_store_start <= 0) || (id_state_store_end <= 0))
            {
                Logger.Error("configure states for start/end store device!!!");
                return false;
            }
            return true;
        }
        private static bool CheckTransactions(WasherCycle cycle,
            int id_sede,
            int id_state_dirty,
            int id_state_wash_start,
            int id_state_wash_end,
            int id_state_store_start,
            int id_state_store_end)
        {
            //if (cycle.bSteelcoFineLavaggioManuale && !IsValid(id_state_wash_start, id_state_dirty, id_sede))
            //{
            //	Logger.Error("configure valid transaction for start wash state to dirty!!!");
            //	return false;
            //}
            //unico controllo sporco pulito, tutto il resto dipende troppo dall'interfacciamento
            //if (!IsValid(id_state_dirty, id_state_wash_start, id_sede))
            //{
            //	Logger.Error("configure valid transaction for start/end wash state!!!");
            //	return false;
            //}
            if (cycle.IsStorage && !IsValid(id_state_store_start, id_state_store_end, id_sede))
            {
                Logger.Error("configure valid transaction for start/end store state!!!");
                return false;
            }
            return true;
        }
        public static bool Add(int device_id,
            int user_id,
            int state_new,
            DateTime timestamp,
            int id_sede,
            bool force_device_status = false,
            int? exam_id = null,
            int? idsedeesame = null,
            int? iduoesame = null)
        {
            return Add(GetFakeWasherCycle(device_id, user_id, state_new, timestamp),
                id_sede,
                exam_id,
                force_device_status,
                idsedeesame,
                iduoesame);
        }
        private static WasherCycle GetFakeWasherCycle(int device_id, int operator_id, int desired_state, DateTime timestamp)
        {
            return new WasherCycle
            {
                WasherID = FAKE_WASHERID, //fake washer id
                                          //completed and not failed chiude il ciclo
                Completed = desired_state == StateTransactions.GetStateId(FixedStates.End_wash),
                Failed = false,
                DeviceID = device_id,
                StartTimestamp = timestamp,
                EndTimestamp = timestamp,
                OperatorStartID = operator_id,
                OperatorEndID = operator_id,
                DesiredDestinationState = desired_state
            };
        }
        public static bool DeviceExpired(int device_id, int user_id, int state_new, DateTime date_time, int id_sede)
        {
            try
            {
                var fake_cycle = GetFakeWasherCycle(device_id, user_id, state_new, date_time);
                //controlli su stato e transazioni
                if (!CheckSatesId(out int id_state_dirty,
                        out int id_state_wash_start,
                        out int id_state_wash_end,
                        out int id_state_store_start,
                        out int id_state_store_end))
                    return false;
                //non ci sono i dati sulla washerid!
                var db = new DbConnection();
                var tuple = GetDeviceStatusId(fake_cycle.DeviceID);
                int current_device_state = tuple.Item1;
                string device_state_date = tuple.Item2;
                //nessun check sulla data, è scaduto il dispositivo si aggiorna e basta!
                var retval = UpdateDeviceAndCycleLog(fake_cycle,
                    current_device_state,
                    state_new,
                    state_new == id_state_dirty, //forza inserimento ciclo se lo stato è di inizio ciclo
                    id_sede,
                    null);
                return retval >= 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        public static (int,int) GetLastDeviceStateTransition(int device_id, int cycle_id, out string data, out int operator_id)
        {
            OdbcCommand cmd = null;
            OdbcConnection dbconn = null;
            OdbcDataReader rdr = null;
            operator_id = -1;
            data = "";
            try
            {
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                string query = $"SELECT IDSTATOOLD, IDSTATONEW, DATAORA, IDOPERATORE FROM CICLISTATOLOG " +
                    $"WHERE (IDCICLO = {cycle_id}) " +
                    $"ORDER BY ID DESC";
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                // visto order by per evitare clausola top che in oracle non va
                // si prende solo il risultato della prima iterazione del reader
                if (rdr.Read())
                {
                    data = rdr.GetString(rdr.GetOrdinal("DATAORA"));
                    operator_id = rdr.GetInt32(rdr.GetOrdinal("IDOPERATORE"));
                    return (rdr.GetInt32(rdr.GetOrdinal("IDSTATOOLD")), rdr.GetInt32(rdr.GetOrdinal("IDSTATONEW")));
                }
                throw new ApplicationException("cycle info not found");
            }
            catch (Exception e)
            {
                Logger.Error($"GetLastDeviceState Error device_id {device_id} cycle_id {cycle_id}", e);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (dbconn != null)
                    dbconn.Close();
            }
        }
        /// <summary>
        /// Riporta lo stato del dispositivo in quello passato come parametro, 
        /// non controlla lo stato precedente e l'esistenza di transazioni consentite.
        /// </summary>
        /// <param name="device_id">id del device</param>
        /// <
        /// <returns></returns>
        public static bool ResetDeviceState(int device_id, int state_id, int operator_id, string date)
        {
            OdbcCommand cmd = null;
            OdbcConnection dbconn = null;
            try
            {
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                string query = $"UPDATE DISPOSITIVI SET STATO = {state_id}, " +
                    $"IDOPERATORESTATO = {operator_id}, " +
                    $"DATASTATO = '{date}' " +
                    $"WHERE ID = {device_id}";
                cmd = new OdbcCommand(query, dbconn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"ResetDeviceState Error " +
                    $"device_id {device_id} " +
                    $"state_id {state_id} " +
                    $"operator_id {operator_id}",
                    e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (dbconn != null)
                    dbconn.Close();
            }
        }
        /// <summary>
        /// Forza lo stato del device, ogni controllo sulle transazioni viene bypassato,
        /// deve essere eseguito a priori.
        /// Utilizzata dalla integrazione con EndoxWeb.
        /// </summary>
        public static bool SetDeviceState(int device_id,
            int operator_id,
            int forced_state,
            DateTime date_time,
            int id_sede,
            int? exam_id = null,
            int? idsedeesame = null,
            int? iduoesame = null)
        {
            try
            {
                //controlli su stato e transazioni
                if (!CheckSatesId(out int id_state_dirty,
                        out int id_state_wash_start,
                        out int id_state_wash_end,
                        out int id_state_store_start,
                        out int id_state_store_end))
                    return false;
                //non ci sono i dati sulla washerid!
                var db = new DbConnection();
                var tuple = GetDeviceStatusId(device_id);
                int current_device_state = tuple.Item1;
                string device_state_date = tuple.Item2;
                //nessun check sulla data, è scaduto il dispositivo si aggiorna e basta!
                var retval = UpdateDeviceAndCycleLog(GetFakeWasherCycle(device_id, operator_id, forced_state, date_time),
                    current_device_state,
                    forced_state,
                    //forza inserimento ciclo se lo stato è di inizio ciclo o se è stato eliminato l'ultimo ciclo
                    forced_state == id_state_dirty,
                    id_sede,
                    exam_id,
                    idsedeesame,
                    iduoesame);
                return retval > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        public static bool Add(WasherCycle cycle,
            int id_sede,
            int? examID = null,
            bool force_device_state = false,
            int? idsedeesame = null,
            int? iduoesame = null)
        {
            try
            {
                Logger.Info("************************ ADDING CYCLE **************************** ");
                Logger.Info(cycle.ToString());
                Logger.Info("****************************************************************** ");

                //controlli su stato e transazioni
                if (!CheckSatesId(out int id_state_dirty,
                        out int id_state_wash_start,
                        out int id_state_wash_end,
                        out int id_state_store_start,
                        out int id_state_store_end)
                    ||
                    !CheckTransactions(cycle,
                        id_sede,
                        id_state_dirty,
                        id_state_wash_start,
                        id_state_wash_end,
                        id_state_store_start,
                        id_state_store_end))
                    return false;
                if (cycle.Failed)
                {
                    InsertParsingData(cycle, WasherCycle.ParsingErrors.MissingTranscode, 0, "");
                    Logger.Error($"Cycle has failed! " +
                        $"DeviceID ({cycle.DeviceExternalID} --> {cycle.DeviceID}), " +
                        $"OperatorStartID ({cycle.OperatorStartExternalID} --> {cycle.OperatorStartID}) " +
                        $"or StartTimestamp ({cycle.StartTimestamp})");
                    return false;
                }
                if (cycle.WasherID != FAKE_WASHERID && !cycle.ValidData())
                {
                    InsertParsingData(cycle, WasherCycle.ParsingErrors.MissingTranscode, 0, "");
                    Logger.Error($"Cycle missing data! " +
                        $"Can be DeviceID ({cycle.DeviceExternalID} --> {cycle.DeviceID}), " +
                        $"OperatorStartID ({cycle.OperatorStartExternalID} --> {cycle.OperatorStartID}) " +
                        $"or StartTimestamp ({cycle.StartTimestamp})");
                    return false;
                }
                var db = new DbConnection();
                var tuple = GetDeviceStatusId(cycle.DeviceID);
                int current_state = tuple.Item1;
                string device_state_date = tuple.Item2;
                if (!CheckDeviceDate(cycle, current_state, device_state_date))
                    return false;
                //in caso di desired dest state indefinito si setta start_wash
                int desired_state = (cycle.DesiredDestinationState == null || cycle.DesiredDestinationState <= 0) ?
                    id_state_wash_start : cycle.DesiredDestinationState.Value;
                int last_cycle_id = -1;
                //inizio lavaggio e transazione non consentita -> forzatura inserimento nuovo ciclo
                if (desired_state == id_state_wash_start && !IsValid(current_state, desired_state, id_sede))
                {
                    Logger.Error($"Device {cycle.DeviceID} --> Error " +
                        $"device state {current_state} " +
                        $"before washing state {id_state_wash_start}... " +
                        $"force create new cycle!");
                    force_device_state = true;
                }
                if (cycle.Completed && cycle.Failed && cycle.bSteelcoFineLavaggioManuale)
                    desired_state = id_state_dirty;
                last_cycle_id = UpdateDeviceAndCycleLog(cycle,
                    current_state,
                    desired_state,
                    force_device_state,
                    id_sede,
                    examID,
                    idsedeesame,
                    iduoesame);
                if (last_cycle_id <= 0)
                    return false;
                InsertAdditionalInfo(cycle, db, last_cycle_id);
                if (!cycle.IsStorage && cycle.WasherID != FAKE_WASHERID)
                    db.ExecuteNonQuery($"UPDATE CICLI SET IDSTERILIZZATRICE = {cycle.WasherID} WHERE ID = {last_cycle_id}");
                if (!cycle.IsStorage && !string.IsNullOrWhiteSpace(cycle.CycleCount))
                    db.ExecuteNonQuery($"UPDATE CICLI SET MACHINECYCLEID = {cycle.CycleCount} WHERE ID = {last_cycle_id}");
                InsertParsingData(cycle, WasherCycle.ParsingErrors.NoError, current_state, device_state_date);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }
        private static void InsertAdditionalInfo(WasherCycle cycle, DbConnection db, int last_cycle_id)
        {
            foreach (var info in cycle.AdditionalInfoList)
            {
                info.Value = (info.Value == null) ? "" : info.Value;
                string sNonQuery = $"INSERT INTO CICLIEXT " +
                    $"(IDCICLO, DESCRIZIONE, VALORE, DATA, ERROR) " +
                    $"VALUES " +
                    $"({last_cycle_id}, " +
                    $"'{info.Description.Replace("'", "''")}', " +
                    $"'{info.Value.Replace("'", "''")}', " +
                    $"'{info.Date.ToString("yyyyMMddHHmmss")}', " +
                    $"{(info.isAlarm ? 1 : 0)})";
                db.ExecuteNonQuery(sNonQuery);
            }
        }
        private static bool CheckDeviceDate(WasherCycle cycle, int currentDeviceState, string currentStateDate)
        {
            try
            {
                if (currentStateDate == null)
                {
                    Logger.Error("Device " + cycle.DeviceID.ToString() + " --> DataStato is NULL or bad!");
                    return false;
                }
                if (currentStateDate.Length != 14)
                {
                    Logger.Error("Device " + cycle.DeviceID.ToString() + " --> DataStato wrong format!");
                    return false;
                }
                DateTime currentDate = new DateTime(Convert.ToInt32(currentStateDate.Substring(0, 4)),
                                             Convert.ToInt32(currentStateDate.Substring(4, 2)),
                                             Convert.ToInt32(currentStateDate.Substring(6, 2)),
                                             Convert.ToInt32(currentStateDate.Substring(8, 2)),
                                             Convert.ToInt32(currentStateDate.Substring(10, 2)),
                                             Convert.ToInt32(currentStateDate.Substring(12, 2)));
                //come confronto per le date si prendono differenti orari del ciclo, se è inizio lavaggio
                //il device viene confrontato con la data di inizio lavaggio, altrimenti con quella di fine
                var timestamp = cycle.StartTimestamp.AddMinutes(5);// 5 minuti di tolleranza
                if (cycle.DesiredDestinationState != null &&
                    (cycle.DesiredDestinationState == GetStateId(FixedStates.End_wash) ||
                    (cycle.DesiredDestinationState == GetStateId(FixedStates.End_store))))
                    timestamp = cycle.EndTimestamp.AddMinutes(5);

                // Sandro 04/05/2017 ci vuole un minimo di tolleranza sugli orari tra server/pcs/lavaendoscopi/armadi //
                if (currentDate > timestamp)
                {
                    InsertParsingData(cycle, WasherCycle.ParsingErrors.DateWrong, currentDeviceState, currentStateDate);
                    Logger.Error($"Device {cycle.DeviceID} --> Last device state change ({currentDate}) " +
                        $"is after than washer cycle start timestamp ({timestamp})!!");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"cycle: {cycle}" + Environment.NewLine +
                    $"actual_device_state: {currentDeviceState}" + Environment.NewLine +
                    $"cycle: {cycle}", e);
                return false;
            }
        }
        public static (int, string) GetDeviceStatusId(int device_id)
        {
            try
            {
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader("SELECT Stato, DataStato FROM Dispositivi WHERE ID = " + device_id);
                int currentStatus = dataset[0].GetInt("Stato").Value;
                string statusDate = dataset[0].GetString("DataStato");
                Logger.Info($"device id {device_id} current db status {statusDate}");
                return (currentStatus, statusDate);
            }
            catch (Exception e)
            {
                Logger.Error($"device_id: {device_id}", e);
                throw e;
            }
        }
        private static void OnDeviceStateChanged(WasherCycle cycle, int iStateStart, int iStateEnd, DateTime date, int lastCycleID)
        {
            if (DeviceStateChanged == null)
                return;
            Device device = Devices.FromID(cycle.DeviceID);
            Operator user = Operators.FromID(cycle.OperatorStartID);
            Washer washer = Washers.FromMatr(cycle.WasherExternalID);
            Operation state1 = Operations.FromID(iStateStart.ToString());
            Operation state2 = Operations.FromID(iStateEnd.ToString());
            if (device == null || user == null || washer == null || /*state1 == null ||*/ state2 == null)
            {
                string sAdditionalError = "";
                if (device == null)
                {
                    if (sAdditionalError.Length > 0)
                        sAdditionalError += " - ";
                    sAdditionalError += "DEVICE";
                }
                if (user == null)
                {
                    if (sAdditionalError.Length > 0)
                        sAdditionalError += " - ";
                    sAdditionalError += "USER";
                }
                if (washer == null)
                {
                    if (sAdditionalError.Length > 0)
                        sAdditionalError += " - ";
                    sAdditionalError += "WASHER";
                }
                if (state1 == null)
                {
                    if (sAdditionalError.Length > 0)
                        sAdditionalError += " - ";
                    sAdditionalError += "STATE1 (id " + iStateStart.ToString() + ")";
                }
                if (state2 == null)
                {
                    if (sAdditionalError.Length > 0)
                        sAdditionalError += " - ";
                    sAdditionalError += "STATE2 (id " + iStateEnd.ToString() + ")";
                }
                sAdditionalError = "[" + sAdditionalError + "]";
                Logger.Error("Something goes wrong with trancodes! " + sAdditionalError);
                return;
            }
            DeviceStateChanged(new DeviceStateChangeData
            {
                DeviceCode = device.Code,
                DeviceSerial = device.Tag,
                DeviceDescription = device.Description,
                DeviceType = null,
                DeviceBrand = null,
                WasherCode = washer.Code,
                WasherSerial = washer.SerialNumber,
                WasherDescription = washer.Description,
                WasherType = washer.Type.ToString(),
                CycleNumber = cycle.CycleCount.ToString(),
                CycleType = cycle.CycleType,
                CycleStartDateTime = cycle.StartTimestamp.ToString(),
                CycleEndDateTime = cycle.EndTimestamp.ToString(),
                OperatorCode = user.Code,
                OperatorSurname = user.LastName,
                OperatorName = user.FirstName,
                StateOld = (state1 != null ? state1.Description : ""),
                StateNew = (state2 != null ? state2.Description : ""),
                StateChangedDateTime = date.ToString(),
            });
        }
        public static int GetStateId(FixedStates status)
        {
            try
            {
                string filter = "";
                switch (status)
                {
                    case FixedStates.Start_cycle:
                        filter = "INIZIOCICLO = 1";
                        break;
                    case FixedStates.Start_wash:
                        filter = "INIZIOSTERILIZZAZIONE = 1";
                        break;
                    case FixedStates.End_wash:
                        filter = "FINESTERILIZZAZIONE = 1";
                        break;
                    case FixedStates.Start_store:
                        filter = "INIZIOSTOCCAGGIO = 1";
                        break;
                    case FixedStates.End_store:
                        filter = "FINESTOCCAGGIO = 1";
                        break;
                    case FixedStates.Start_pre_wash:
                        filter = "INIZIOPRELAVAGGIO = 1";
                        break;
                    case FixedStates.End_pre_wash:
                        filter = "FINEPRELAVAGGIO = 1";
                        break;
                }
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader($"SELECT MAX(ID) AS ID FROM STATO WHERE Eliminato = 0 AND {filter}");
                if (dataset.Count == 1 && dataset[0].GetInt("ID").HasValue)
                    return dataset[0].GetInt("ID").Value;
                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return 0;
            }
        }

        public static string GetStateName(int state_id)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                string query = $"SELECT DESCRIZIONE FROM STATO WHERE ID = {state_id}";
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return rdr.GetStringEx("DESCRIZIONE");
                return "STATE NOT FOUND";
            }
            catch (Exception e)
            {
                Logger.Error($"state_id {state_id} exception {e}");
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (dbconn != null)
                    dbconn.Close();
            }
        }

        public static string GetStateName(FixedStates status) => GetStateName(GetStateId(status));

        public static bool CheckWardConfiguration(int id_sede, bool storage_present, bool pre_washer_present)
        {
            try
            {
                var errors = false;
                if (!IsValid(GetStateId(FixedStates.Start_wash), GetStateId(FixedStates.End_wash), id_sede))
                {
                    Logger.Error($"MISSING TRANSACTION => configure valid transaction for START WASH -> END WASH in site {id_sede}!!!");
                    errors = true;
                }
                if (storage_present && !IsValid(GetStateId(FixedStates.Start_store), GetStateId(FixedStates.End_store), id_sede))
                {
                    Logger.Error($"MISSING TRANSACTION => configure valid transaction for START STORE -> END STORE in site {id_sede}!!!");
                    errors = true;
                }
                if (pre_washer_present && !IsValid(GetStateId(FixedStates.Start_pre_wash), GetStateId(FixedStates.End_pre_wash), id_sede))
                {
                    Logger.Error($"MISSING TRANSACTION => configure valid transaction for START PRE WASH -> END PRE WASH in site {id_sede}!!!");
                    errors = true;
                }
                return !errors;
            }
            catch (Exception e)
            {
                Logger.Error($"id_sede: {id_sede}", e);
                return false;
            }
        }
        static private void InsertParsingData(WasherCycle cycle, WasherCycle.ParsingErrors errorCode, int stateOld, string dateStateOld)
        {
            OdbcCommand cmd = null;
            OdbcConnection conn = null;
            try
            {
                //verifica presenza armadio 
                conn = new OdbcConnection(DbConnection.ConnectionString);
                conn.Open();
                cmd = new OdbcCommand($"SELECT COUNT(*) FROM ARMADI_LAVATRICI WHERE ID = {cycle.WasherID}", conn);
                int count = cmd.ExecuteScalarInt(0);
                if (count == 0)
                    return;
                string query = $"INSERT INTO STERILIZZATRICIPARSING " +
                    $"(IDSTERILIZZATRICE, DATAPARSING, FILENAMEDATA, FILENAME, CONTENUTOFILE, CICLOCOMPLETO, ERROREIMPORT, IDDISPOSITIVO, DATAORASTART, IDOPERATORESTART, DATAORAEND, IDOPERATOREEND, DATASTATODISPOSITIVO, STATODISPOSITIVO) " +
                    $"VALUES " +
                    $"({cycle.WasherID}, " +
                    $"'{DateTime.Now.ToString("yyyyMMddHHmmss")}', " +
                    $"'{cycle.FileDatetime.ToString("yyyyMMddHHmmss")}', " +
                    $"'{cycle.Filename}', " +
                    "?, " + // file_content parameter
                            //$"'##START_STRING_PARAMETER{cycle.FileContent.PrepareForQuery()}END_STRING_PARAMETER##', " +
                    $"{(cycle.Completed ? 1 : 0)}, " +
                    $"{(int)errorCode}, " +
                    $"{cycle.DeviceID}, " +
                    $"'{cycle.StartTimestamp.ToString("yyyyMMddHHmmss")}', " +
                    $"{cycle.OperatorStartID}, " +
                    $"'{cycle.EndTimestamp.ToString("yyyyMMddHHmmss")}', " +
                    $"{cycle.OperatorEndID}, " +
                    $"'{dateStateOld}', " +
                    $"{stateOld})";
                cmd.Parameters.Add(new OdbcParameter("file_content",
                    $"##START_STRING_PARAMETER{cycle.FileContent}END_STRING_PARAMETER##"));
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
    }
}
