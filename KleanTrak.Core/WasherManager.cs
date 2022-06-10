using KleanTrack.License;
using KleanTrak.Model;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Threading;

namespace KleanTrak.Core
{
    public class WasherManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly WasherManager _instance = new WasherManager();
        public static WasherManager Instance => _instance;
        private List<Thread> _listThread = new List<Thread>();
        ManualResetEvent _terminateThreadEvent = new ManualResetEvent(false);
        public void Start(bool debug = false)
        {
            try
            {
                Logger.Info("Starting");
                CheckUnknownOperator();
                if (!CheckConfiguration())
                    throw new ApplicationException("application not properly set");
                List<Washer> washers = Washers.Get();
                foreach (var washer in washers)
                {
                    if (!debug && !IsDeviceLicensed(washer))
                    {
                        var msg = $"device {washer.Description} not licensed (UO {washer.IDUO}, Site {washer.IDSede})";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(msg);
                        Logger.Error(msg);
                        Console.ResetColor();
                        continue;
                    }
                    
                    Logger.Info("Add washer thread " + washer.Description);
                    Thread thread = new Thread(ThreadWasher);
                    _listThread.Add(thread);
                    thread.Start(washer);
                }
                Logger.Info("Started");
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
        private static bool IsDeviceLicensed(Washer washer)
        {
            try
            {
                var claims = CTLicenseManager.uo_claims
                    .Where(ctl => ctl.Iduo == washer.IDUO && ctl.Sedi.Keys.Contains(washer.IDSede))
                    .FirstOrDefault();
                if (claims == null)
                {
                    Logger.Info("No claims found");
                    return false;
                }
                if (!claims.Claims.TryGetValue("Armadi", out int armadi))
                    armadi = 0;
                if (!claims.Claims.TryGetValue("Lavatrici", out int lavatrici))
                    lavatrici = 0;
                if (!claims.Claims.TryGetValue("Pompe", out int pompe))
                    pompe = 0;
                switch (washer.Type)
                {
                    case WasherStorageTypes.Washer_IMS7:
                    case WasherStorageTypes.Washer_ISAWD:
                    case WasherStorageTypes.Washer_Cantel_Medivators_XXX:
                    case WasherStorageTypes.Washer_Steelco:
                    case WasherStorageTypes.Washer_Mirth:
                    case WasherStorageTypes.Washer_Cantel_RapidAir:
                    case WasherStorageTypes.Washer_Steelco_ManualEnd:
                    case WasherStorageTypes.Washer_Cantel_Medivators_Serial:
                    case WasherStorageTypes.Washer_ICT_DbConnect:
                    case WasherStorageTypes.Washer_Cantel_AdvantagePassThrough_PV_3_0_0_16:
                        if (lavatrici <= 0)
                            return false;
                        lavatrici--;
                        claims.Claims["Lavatrici"] = lavatrici;
                        return true;
                    case WasherStorageTypes.Storage_Cantel_EDC:
                    case WasherStorageTypes.Storage_Cantel_EndoDry:
                        if (armadi <= 0)
                            return false;
                        armadi--;
                        claims.Claims["Armadi"] = armadi;
                        return true;
                    case WasherStorageTypes.PreWasher_Cantel_MDG:
                    case WasherStorageTypes.PreWasher_Steelco_EPW100:
                        if (pompe <= 0)
                            return false;
                        pompe--;
                        claims.Claims["Pompe"] = pompe;
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                Logger.Error(washer, e);
                return false;
            }
        }
        private static bool CheckConfiguration()
        {
            OdbcConnection conn = null;
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                //verifica configurazione stati
                if (!CheckWasherStates(out string message))
                    throw new ApplicationException($"Check washer states: {Environment.NewLine}{message}");
                if (!CheckStorageStates(out bool storage_present, out string storage_message))
                    throw new ApplicationException($"Check storage states: {Environment.NewLine}{storage_message}");
                if (!CheckPreWasherStates(out bool pre_washer_present, out string pre_washer_message))
                    throw new ApplicationException($"Check pre washer states: {Environment.NewLine}{pre_washer_message}");
                string query = "SELECT ID FROM SEDIESAME WHERE ELIMINATO = 0";
                conn = new OdbcConnection(DbConnection.ConnectionString);
                conn.Open();
                cmd = new OdbcCommand(query, conn);
                rdr = cmd.ExecuteReader();
                //verifica transazioni per ciascuna sede
                while (rdr.Read())
                {
                    int id_sede = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    if (!StateTransactions.CheckWardConfiguration(id_sede, storage_present, pre_washer_present))
                    {
                        //non si blocca l'applicativo, potrebbero essere presenti integrazioni con
                        //software di terze parti (ict group) che non permettono di tracciare tutte
                        //le fasi di lavaggio.
                        Logger.Error($"configure transacions for seed {id_sede}");
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }
        private static bool CheckWasherStates(out string message)
        {
            message = "";
            try
            {
                int iStateDirty = StateTransactions.GetStateId(FixedStates.Start_cycle);
                int iStateWashStart = StateTransactions.GetStateId(FixedStates.Start_wash);
                int iStateWashEnd = StateTransactions.GetStateId(FixedStates.End_wash);
                bool errors = false;
                if (iStateDirty <= 0)
                {
                    message = "MISSING STATUS => configure DIRTY STATE!!!" + Environment.NewLine;
                    errors = true;
                }
                if (iStateWashStart <= 0)
                {
                    message += "MISSING STATUS => configure START WASH STATE!!!" + Environment.NewLine; ;
                    errors = true;
                }
                if (iStateWashEnd <= 0)
                {
                    message += "MISSING STATUS => configure END WASH STATE!!!" + Environment.NewLine; ;
                    errors = true;
                }
                return !errors;
            }
            catch (Exception e)
            {
                message = $"error: {e}";
                Logger.Error(e);
                return false;
            }
            throw new NotImplementedException();
        }
        private static bool CheckStorageStates(out bool storage_present, out string message)
        {
            storage_present = false;
            message = "";
            try
            {
                storage_present = Washers.Get()
                    .Where(w => w.Type == WasherStorageTypes.Storage_Cantel_EDC)
                    .Count() > 0;
                if (!storage_present)
                    return true;
                bool errors = false;
                if (StateTransactions.GetStateId(FixedStates.Start_store) <= 0)
                {
                    message = "MISSING STATUS => configure START STORE STATE!!!" + Environment.NewLine;
                    errors = true;
                }
                if (StateTransactions.GetStateId(FixedStates.End_store) <= 0)
                {
                    message += "MISSING STATUS => configure END STORE STATE!!!" + Environment.NewLine; ;
                    errors = true;
                }
                return !errors;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }
        private static bool CheckPreWasherStates(out bool pre_washer_present, out string message)
        {
            pre_washer_present = false;
            message = "";
            try
            {
                pre_washer_present = Washers.Get()
                    .Where(w => w.Type == WasherStorageTypes.PreWasher_Cantel_MDG
                    || w.Type == WasherStorageTypes.PreWasher_Steelco_EPW100)
                    .Count() > 0;
                if (!pre_washer_present)
                    return true;
                bool errors = false;
                if (StateTransactions.GetStateId(FixedStates.Start_pre_wash) <= 0)
                {
                    message = "MISSING STATUS => configure START PRE WASH STATE!!!" + Environment.NewLine;
                    errors = true;
                }
                if (StateTransactions.GetStateId(FixedStates.End_pre_wash) <= 0)
                {
                    message += "MISSING STATUS => configure END PRE WASH STATE!!!" + Environment.NewLine; ;
                    errors = true;
                }
                return !errors;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }
        public void Stop()
        {
            Logger.Info("Stopping");
            _terminateThreadEvent.Set();
            foreach (var thread in _listThread)
                thread.Join();
            _listThread.Clear();
            Logger.Info("Stopped");
        }
        public void AddCycles(List<WasherCycle> cycles)
        {
            foreach (var cycle in cycles)
            {
                try
                {
                    (new WPMirth()).Prepare(cycle);
                    var startstate = StateTransactions.GetStateId(FixedStates.Start_wash);
                    var endstate = StateTransactions.GetStateId(FixedStates.End_wash);
                    cycle.DesiredDestinationState = (cycle.Completed) ? endstate : startstate;
                    if (cycle.ValidData())
                        StateTransactions.Add(cycle, GetWasherIdSede(cycle.WasherID));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
        private int GetWasherIdSede(int washer_id)
        {
            OdbcCommand cmd = null;
            OdbcConnection conn = null;
            try
            {
                string query = $"SELECT IDSEDE FROM ARMADI_LAVATRICI WHERE ID = {washer_id}";
                conn = new OdbcConnection(DbConnection.ConnectionString);
                conn.Open();
                cmd = new OdbcCommand(query, conn);
                var result = cmd.ExecuteScalar();
                if (result == DBNull.Value)
                    return -1;
                return System.Convert.ToInt32(result);
            }
            catch (Exception e)
            {
                Logger.Error($"washer_id: {washer_id}", e);
                return -1;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }
        void ThreadWasher(object obj)
        {
            WPBase washerObj = null;
            Washer washer = (Washer)obj;
            try
            {
                if (obj == null)
                {
                    Logger.Error($"washer object is null!");
                    return;
                }
                Logger.Info($"** THREAD STARTED ** for: {washer.SerialNumber} {washer.Description}");
                if (washer.PollingTime <= 0)
                {
                    Logger.Warn($"Polling " +
                        $"washer.Description {washer.Description} " +
                        $"(washer.Code: {washer.Code} " +
                        $"washer.ID: {washer.ID}) " +
                        $"polling time less than zero setting 60 seconds");
                    washer.PollingTime = 60;
                    ResetPollingTime(washer.ID);
                }
                washerObj = WPBase.Get(washer);
                if (washerObj == null)
                {
                    Logger.Error($"washer object is null for washer " +
                        $"washer.Description {washer.Description} " +
                        $"(washer.Code: {washer.Code} " +
                        $"washer.ID: {washer.ID})");
                    return;
                }
                //init se richiesto
                if (!washerObj.Start())
                    throw new ApplicationException("unable to start washer");
                while (!_terminateThreadEvent.WaitOne(washer.PollingTime * 1000))
                {
                    Logger.Info($"Polling {washer.SerialNumber} {washer.Description}");
                    try
                    {
                        bool errors = false;
                        if (washerObj.FileSystemWasher && washer.FolderOrFileName != null)
                        {
                            if (washer.FolderOrFileName.Length > 0)
                            {
                                if (!Directory.Exists(washer.FolderOrFileName))
                                {
                                    Logger.Warn($"cannot access directory for " +
                                        $"washer.Description {washer.Description} " +
                                        $"(washer.Code: {washer.Code} " +
                                        $"washer.ID: {washer.ID}) --> " +
                                        $"washer.FolderOrFileName: {washer.FolderOrFileName}");
                                    errors = true;
                                }
                            }
                        }
                        if (errors)
                            continue;
                        List<WasherCycle> cycles = washerObj.GetCycles(washer, GetLastDate(washer.ID));
                        if (cycles == null)
                        {
                            Logger.Warn($"cycles are null for " +
                                        $"washer.Description {washer.Description} " +
                                        $"(washer.Code: {washer.Code} " +
                                        $"washer.ID: {washer.ID}) " +
                                        $"WASHER CYCLES ARE NULL!");
                            continue;
                        }
                        foreach (var cycle in cycles)
                        {
                            if (cycle.WasherID != washer.ID)
                            {
                                Logger.Error($"Error washer mismatch for " +
                                        $"washer.Description {washer.Description} " +
                                        $"(washer.Code: {washer.Code} " +
                                        $"washer.ID: {washer.ID}) " +
                                        $"cycle.WasherID: {cycle.WasherID} " +
                                        $"cycle.Filename: { cycle.Filename}");
                                continue;
                            }
                            if (!StateTransactions.Add(cycle, GetWasherIdSede(washer.ID)))
                                Logger.Error($"Error state transaction failed " +
                                        $"washer.Description {washer.Description} " +
                                        $"(washer.Code: {washer.Code} " +
                                        $"washer.ID: {washer.ID}) " +
                                        $"cycle.WasherID {cycle.WasherID}, " +
                                        $"cycle.DeviceID {cycle.DeviceID} " +
                                        $"not added!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                if (washerObj != null)
                    if (!washerObj.Stop())
                        Logger.Error("unable to stop washer");
            }
        }
        private static void ResetPollingTime(int washerid)
        {

            DbConnection conn = null;
            try
            {
                string query = $"UPDATE ARMADI_LAVATRICI SET POLLINGTIME = 60 WHERE ID = {washerid}";
                conn = new DbConnection();
                conn.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                Logger.Error($"washerid: {washerid} {Environment.NewLine} Exception: {e}");
            }
        }
        DateTime GetLastDate(int idWasher)
        {
            //Davide - aggiungo il filtro su ciclocompleto perchè le sterizzatrici scrivono sullo stesso scontrino quindi se lo inserisco la prima volta 
            //         quello incompleto poi non lo processo nuovamente
            DateTime max = DateTime.MinValue;
            string query = "SELECT MAX(FILENAMEDATA) AS MaxData FROM SterilizzatriciParsing WHERE CicloCompleto = 1 AND IDSterilizzatrice = " + idWasher.ToString();

            try
            {
                DbConnection conn = new DbConnection();
                DbRecordset dataset = conn.ExecuteReader(query);
                if (dataset.Count == 1)
                {
                    string maxdate = dataset[0].GetString("MaxData");
                    if (maxdate != null && maxdate.Length == 14)
                        max = new DateTime(Convert.ToInt32(maxdate.Substring(0, 4)),
                                           Convert.ToInt32(maxdate.Substring(4, 2)),
                                           Convert.ToInt32(maxdate.Substring(6, 2)),
                                           Convert.ToInt32(maxdate.Substring(8, 2)),
                                           Convert.ToInt32(maxdate.Substring(10, 2)),
                                           Convert.ToInt32(maxdate.Substring(12, 2)));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return max;
        }
        public const string _UNKNOWN_OPERATOR = "OPERATORE SCONOSCIUTO";
        private void CheckUnknownOperator()
        {
            try
            {
                Logger.Info("Checking unknown operator");
                DbConnection dbconn = new DbConnection();
                var query_ins = $"INSERT INTO OPERATORI (MATRICOLA, COGNOME, TAG, DISATTIVATO) " +
                    $"VALUES ('{_UNKNOWN_OPERATOR}', '{_UNKNOWN_OPERATOR}', '{_UNKNOWN_OPERATOR}', 0)";
                var query_count = $"SELECT COUNT(*) FROM OPERATORI WHERE COGNOME = '{_UNKNOWN_OPERATOR}'";
                int unknown_op_records = System.Convert.ToInt32(dbconn.ExecuteScalar(query_count));
                switch (unknown_op_records)
                {
                    case 1:
                        return;
                    case 0:
                        dbconn.ExecuteNonQuery(query_ins);
                        return;
                    default:
                        throw new ApplicationException($"there are {unknown_op_records} UNKNOWN OPERATOR RECORDS, should be 1!");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
        public static Response SetWasherConfiguration()
        {
            try
            {
                Logger.Info("Start");
                Instance.Stop();
                Instance.Start();
                Logger.Info("End");
                return new Response { Successed = true };
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new Response { Successed = false };
            }
        }
    }
}
