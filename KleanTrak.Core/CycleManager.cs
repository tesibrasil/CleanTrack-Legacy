using KleanTrak.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OdbcExtensions;
using System.Data.Odbc;

namespace KleanTrak.Core
{
    public class CycleManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int CmdGetCurrentExamCycle(int exam_id, int exam_site_id, int exam_uo_id)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                string query = $"SELECT ID FROM CICLI WHERE IDESAME = {exam_id} " +
                $"AND IDSEDEESAME = {exam_site_id} " +
                $"AND IDUOESAME = {exam_uo_id} " +
                "ORDER BY ID DESC";
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return rdr.GetIntEx("ID", -1);
                return -1;
            }
            catch (Exception e)
            {
                Logger.Error($"CmdGetCurrentExamCycle error exam_id {exam_id}" +
                    Environment.NewLine +
                    $"exam_site_id {exam_site_id}" +
                    Environment.NewLine +
                    $"exam_uo_id {exam_uo_id}",
                    e);
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
        public static int GetExamCycleId(int device_id, int exam_id, int exam_site_id, int exam_uo_id)
        {

            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                string query = $"SELECT ID FROM CICLI WHERE IDDISPOSITIVO = {device_id} " +
                    $"AND IDESAME = {exam_id} " +
                    $"AND IDSEDEESAME = {exam_site_id} " +
                    $"AND IDUOESAME = {exam_uo_id}";
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return rdr.GetIntEx("ID", -1);
                return -1;
            }
            catch (Exception e)
            {
                var error_message = $"device_id {device_id}, " +
                    $"exam_id {exam_id} " +
                    Environment.NewLine +
                    $"exam_site_id {exam_site_id} " +
                    Environment.NewLine +
                    $"exam_uo_id {exam_uo_id} " +
                    Environment.NewLine +
                    $"Exception {e}";
                Logger.Error(error_message);
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
        public enum RemoveCycleResponse { NotLastCycle, Ok, Errors };
        public static RemoveCycleResponse RemoveExamCycle(int exam_id,
            int exam_site_id,
            int exam_uo_id,
            string device_barcode,
            string operator_barcode,
            out string error_message,
            out string reset_state_name)
        {
            error_message = "";
            reset_state_name = "";
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                var device = Devices.FromBarcode(device_barcode);
                if (device == null)
                    throw new ApplicationException("device not found");
                var last_cycle_id = StateTransactions.GetLastCycleID(device.ID, false, -1);
                var exam_cycle_id = GetExamCycleId(device.ID, exam_id, exam_site_id, exam_uo_id);
                if (last_cycle_id != exam_cycle_id)
                    return RemoveCycleResponse.NotLastCycle;
                (int previousStateId, int latestStateId) = StateTransactions.GetLastDeviceStateTransition(device.ID,
                    exam_cycle_id,
                    out string date,
                    out int operator_id);
                var now = DateTime.Now;
                string str_data = now.Year.ToString() +
                    now.Month.ToString().PadLeft(2, '0') +
                    now.Day.ToString().PadLeft(2, '0') +
                    now.Hour.ToString().PadLeft(2, '0') +
                    now.Minute.ToString().PadLeft(2, '0') +
                    now.Second.ToString().PadLeft(2, '0');
                var query_log = $"INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO ) " +
                    $" VALUES " +
                    $"(' barcode operatore " + operator_barcode + "', " +
                    "'CICLI', " +
                    "'Eliminazione', " +
                    "'" + str_data + "', " +
                    "1, " +
                    "'ID', " +
                    $"'{exam_cycle_id}', " +
                    "'')";
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                cmd = new OdbcCommand(query_log, dbconn);
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM CICLISTATOLOG WHERE IDCICLO = {exam_cycle_id}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM CICLIEXT WHERE IDCICLO = {exam_cycle_id}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"DELETE FROM CICLI WHERE ID = {exam_cycle_id}";
                cmd.ExecuteNonQuery();
                var device_result = StateTransactions.ResetDeviceState(device.ID, previousStateId, operator_id, date);
                if (!device_result)
                    throw new ApplicationException("unable to set device end_wash state");
                reset_state_name = StateTransactions.GetStateName(previousStateId);
                return RemoveCycleResponse.Ok;
            }
            catch (Exception e)
            {
                error_message = $"device_barcode {device_barcode}, " +
                    $"operator_barcode {operator_barcode} " +
                    Environment.NewLine +
                    $"exam_id {exam_id} " +
                    Environment.NewLine +
                    $"exam_site_id {exam_site_id} " +
                    Environment.NewLine +
                    $"exam_uo_id {exam_uo_id} " +
                    Environment.NewLine +
                    $"Exception {e}";
                Logger.Error(error_message);
                return RemoveCycleResponse.Errors;
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

        public static CmdGetExamCyclesResponse GetExamCycles(CmdGetExamCycles req)
        {
            try
            {
                CmdGetExamCyclesResponse response = new CmdGetExamCyclesResponse();
                DbConnection db = new DbConnection();
                string query = $"SELECT ID, IDDISPOSITIVO FROM CICLI WHERE IDESAME = {req.ExamId} ";
                query += (req.SiteId == null) ? "" : $"AND IDSEDEESAME = {req.SiteId.Value} ";
                query += (req.UoId == null) ? "" : $"AND IDUOESAME = {req.UoId.Value} ";
                query += "ORDER BY ID";
                Logger.Debug($"query {query}");
                DbRecordset dataset = db.ExecuteReader(query);
                foreach (var record in dataset)
                {
                    if (response.ExamCycles == null)
                        response.ExamCycles = new List<ExamCycle>();
                    var idciclo = record.GetInt("ID").Value;
                    var idsonda = record.GetInt("IDDISPOSITIVO").Value;
                    var device = Devices.FromID(idsonda);
                    //Mi interessa il ciclo precedente sulla stessa sonda ma su esame differente
                    // anche in questo caso si filtra eventualmente per iduo, ma non per sede
                    string query2 = $"SELECT ID FROM CICLI WHERE IDDISPOSITIVO = {idsonda} AND ID < {idciclo} AND " +
                        $"(IDESAME IS NULL OR " +
                        $"(" +
                        $"IDESAME <> {req.ExamId} " +
                        // si deve eventualmente restare sempre sulla stessa uo
                        ((req.UoId == null) ? "" : $"AND IDUOESAME = {req.UoId.Value} ") +
                        $")) " +
                        $"ORDER BY ID DESC";
                    Logger.Debug($"query {query2}");
                    DbRecordset dataset2 = db.ExecuteReader(query2);
                    if (dataset2.Count > 0)
                    {
                        response.ExamCycles.Add(
                            new ExamCycle()
                            {
                                CycleId = dataset2.First().GetInt("ID").Value,
                                DeviceBarcode = device.Tag,
                                DeviceDescription = device.Description,
                                CycleLogs = GetCycleLog(dataset2.First().GetInt("ID").Value, true)
                            }
                        );
                        //Aggiungo l'evento del nuovo sporco
                        response.ExamCycles.Last().CycleLogs.Add(
                            GetCycleLog(idciclo, false).First()
                        );
                    }
                    else
                    {
                        //Se non ho cicli precedenti, mostro solo l'utilizzo attuale
                        response.ExamCycles.Add(
                            new ExamCycle()
                            {
                                DeviceBarcode = device.Tag,
                                DeviceDescription = device.Description,
                                CycleLogs = GetCycleLog(idciclo, false)
                            }
                        );
                    }
                }
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"GetExamCycles error req.ExamId {req.ExamId}" +
                    Environment.NewLine +
                    $"req.Siteid {req.SiteId}" +
                    Environment.NewLine +
                    $"req.UoId {req.UoId}",
                    e);
                throw;
            }
        }

        private static List<CycleLog> GetCycleLog(int cycleid, bool removefirstdirty)
        {
            List<CycleLog> response = new List<CycleLog>();

            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT CICLISTATOLOG.ID, IDSTATONEW, IDOPERATORE, DATAORA, STATO.INIZIOCICLO FROM CICLISTATOLOG JOIN STATO ON CICLISTATOLOG.IDSTATONEW = STATO.ID WHERE IDCICLO = " + cycleid + " ORDER BY DATAORA");

            foreach (var record in dataset)
            {
                if (removefirstdirty && record.GetBoolean("INIZIOCICLO").Value)
                {
                    removefirstdirty = false;
                    continue;
                }

                var idlog = record.GetInt("ID").Value;
                var operation = Operations.FromID(record.GetInt("IDSTATONEW").Value.ToString());
                var user = Operators.FromID(record.GetInt("IDOPERATORE").Value);
                DateTime dataora = DateTime.ParseExact(record.GetString("DATAORA"), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                response.Add(new CycleLog
                {
                    OperationBarcode = operation != null ? operation.Barcode : "",
                    OperationDescription = operation != null ? operation.ActionDescription : "",
                    UserBarcode = user != null ? user.Tag : "",
                    UserDescription = user != null ? user.LastName + " " + user.FirstName : "",
                    OperationDateTime = dataora
                });
            }

            return response;
        }
    }
}
