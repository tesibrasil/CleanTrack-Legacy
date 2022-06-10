using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using KleanTrak.Model;
using OdbcExtensions;

namespace KleanTrak.Core
{
    public class Devices
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Device FromBarcode(string sTag)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader($"SELECT DISPOSITIVI.*, " +
                $"STATO.DESCRIZIONE AS DESCRIZIONESTATO " +
                $"FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID " +
                $"WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND TAG = '{sTag.Replace("'", "''").ToUpper()}'");
            if (dataset.Count() >= 1)
            {
                return new Device(dataset.First().GetInt("IDSEDE").Value)
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }
            return null;
        }

        public static Device FromSerial(string sSerial)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader($"SELECT DISPOSITIVI.*, " +
                $"STATO.DESCRIZIONE AS DESCRIZIONESTATO " +
                $"FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID " +
                $"WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND SERIALE = '{sSerial.Replace("'", "''").ToUpper()}'");
            if (dataset.Count() >= 1)
            {
                return new Device(dataset.First().GetInt("IDSEDE").Value)
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }
            return null;
        }

        public static Device FromMat(string sMatricola)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader($"SELECT DISPOSITIVI.*, STATO.DESCRIZIONE AS DESCRIZIONESTATO " +
                $"FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID " +
                $"WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND MATRICOLA = '{sMatricola.Replace("'", "''").ToUpper()}'");
            if (dataset.Count() >= 1)
            {
                return new Device(dataset.First().GetInt("IDSEDE").Value)
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }

            return null;
        }

        public static Device FromID(int id)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader($"SELECT DISPOSITIVI.*, " +
                $"STATO.DESCRIZIONE AS DESCRIZIONESTATO " +
                $"FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID " +
                $"WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND DISPOSITIVI.ID = {id}");

            if (dataset.Count() >= 1)
            {
                return new Device(dataset.First().GetInt("IDSEDE").Value)
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }

            return null;
        }
        public static int GetDeviceWard(string serial_number, string code)
        {
            if (serial_number.Length == 0 && code.Length == 0)
                throw new ApplicationException("serial_number and code are both empty");
            OdbcCommand cmd = null;
            try
            {
                string filter = "";
                if (serial_number.Length > 0)
                    filter = $"SERIALE = '{serial_number}'";
                if (code.Length > 0)
                {
                    filter += (filter.Length == 0) ? "" : " AND ";
                    filter += $"MATRICOLA = '{code}'";
                }
                string query = $"SELECT IDSEDE FROM DISPOSITIVI WHERE {filter}";
                cmd = new OdbcCommand(query, new OdbcConnection(DbConnection.ConnectionString));
                object value = cmd.ExecuteScalar();
                if (value == DBNull.Value)
                    throw new ApplicationException("id_sede not found");
                return (int)value;
            }
            catch (Exception e)
            {
                Logger.Error($"serial_number: {serial_number} code: {code}", e);
                return -1;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static List<DeviceExam> GetDeviceExams(int deviceId, int uoId)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                var retlist = new List<DeviceExam>();
                string query = $"SELECT * FROM CICLI " +
                    $"WHERE IDDISPOSITIVO = {deviceId} AND IDUOESAME = {uoId}";
                Logger.Debug($"GetDeviceExams query {query}");
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    retlist.Add(new DeviceExam
                    {
                        ExamId = rdr.GetIntEx("IDESAME"),
                        SiteId = rdr.GetIntEx("IDSEDEESAME"),
                        UoId = rdr.GetIntEx("IDUOESAME")
                    });
                }
                return retlist;
            }
            catch (Exception e)
            {
                Logger.Error($"deviceId {deviceId}, " +
                    $"uoId {uoId}, " +
                    Environment.NewLine +
                    $"Exception {e}");
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

        public static List<Device> GetDevicesList(int? siteId, int? uoId, string deviceName, bool includeDismissed)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection dbconn = null;
            try
            {
                var retlist = new List<Device>();
                string query = $"SELECT DISPOSITIVI.ID, " +
                    $"DISPOSITIVI.IDSEDE, " +
                    $"DISPOSITIVI.MATRICOLA, " +
                    $"DISPOSITIVI.DESCRIZIONE, " +
                    $"DISPOSITIVI.TAG, " +
                    $"DISPOSITIVI.STATO, " +
                    $"STATO.DESCRIZIONE AS 'DESCRIZIONESTATO' " +
                    $"FROM DISPOSITIVI " +
                    $"LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID";
                string filter = "DISPOSITIVI.ELIMINATO = 0";
                if (siteId != null)
                {
                    filter += (filter.Length > 0) ? " AND " : "";
                    filter += $"IDSEDE = ${siteId}";
                }
                if (uoId != null && siteId == null)
                {
                    filter += (filter.Length > 0) ? " AND " : "";
                    filter += $"IDSEDE IN (SELECT IDSEDE FROM UO_SEDI WHERE IDUO = {uoId})";
                }
                if (deviceName.Length > 0)
                {
                    filter += (filter.Length > 0) ? " AND " : "";
                    filter += $"DESCRIZIONE LIKE '%{deviceName}%'";
                }
                if (!includeDismissed)
                {
                    filter += (filter.Length > 0) ? " AND " : "";
                    filter += "DISPOSITIVI.DISMESSO = 0";
                }
                query += (filter.Length > 0) ? $" WHERE {filter}" : "";
                Logger.Debug($"GetDevicesList query {query}");
                dbconn = new OdbcConnection(DbConnection.ConnectionString);
                dbconn.Open();
                cmd = new OdbcCommand(query, dbconn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    retlist.Add(new Device(rdr.GetIntEx("IDSEDE"))
                    {
                        ID = rdr.GetIntEx("ID"),
                        Code = rdr.GetStringEx("MATRICOLA"),
                        Description = rdr.GetStringEx("DESCRIZIONE"),
                        Tag = rdr.GetStringEx("TAG"),
                        StateID = rdr.GetIntEx("STATO"),
                        StateDescription = rdr.GetStringEx("DESCRIZIONESTATO")
                    });
                }
                return retlist;
            }
            catch (Exception e)
            {
                Logger.Error($"siteId {siteId}, " +
                    $"uoId {uoId}, " +
                    $"deviceName {deviceName}, " +
                    $"includeDismissed {includeDismissed}" +
                    Environment.NewLine +
                    $"Exception {e}");
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

        public static CmdGetDeviceStatusResponse GetDeviceStatus(CmdGetDeviceStatus req)
        {
            Logger.Info("input: " + req.DeviceBarcode + " " + req.DeviceSerial);

            Device dev = null;

            try
            {
                dev = req.DeviceBarcode != null ? Devices.FromBarcode(req.DeviceBarcode) : Devices.FromMat(req.DeviceSerial);

                // Sandro 09/06/2017 // modifica fatta al volo per Petardo a Pordenone //
                if (dev == null)
                    dev = FromMat(req.DeviceBarcode);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new CmdGetDeviceStatusResponse() { Successed = false, ErrorMessage = ex.Message };
            }

            if (dev != null)
            {
                Logger.Info("output: " + dev.Description + " " + dev.StateDescription);
                return new CmdGetDeviceStatusResponse() { Successed = true, Description = dev.Description, Status = dev.StateDescription };
            }

            Logger.Error("output: " + Dictionary.Instance["deviceNotFound"]);
            return new CmdGetDeviceStatusResponse() { Successed = false, ErrorMessage = Dictionary.Instance["deviceNotFound"] };
        }

        public static Response SetDeviceStatus(CmdSetDeviceStatus req)
        {
            Logger.Info("input: barcode: " + req.DeviceBarcode + " serial: " + req.DeviceSerial);

            try
            {
                Operation reader_operation = req.OperationBarcode != null ? Operations.FromBarcode(req.OperationBarcode) : Operations.FromID(req.OperationID);
                if (reader_operation == null)
                {
                    Logger.Error(Dictionary.Instance["operationNotFound"]);
                    return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["operationNotFound"] };
                }

                Device dev = null;
                if (req.DeviceBarcode != null)
                {
                    dev = Devices.FromBarcode(req.DeviceBarcode);
                    if (dev == null) // Sandro 09/06/2017 // modifica fatta al volo per Petardo a Pordenone //
                        dev = FromMat(req.DeviceBarcode);
                }

                if (dev == null && req.DeviceSerial != null)
                    dev = FromMat(req.DeviceSerial);

                if (dev == null)
                {
                    Logger.Error(Dictionary.Instance["deviceNotFound"]);
                    return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["deviceNotFound"] };
                }

                Operator user = Operators.FindOperator(req.UserBarcode);
                if (user == null)
                {
                    Logger.Error(Dictionary.Instance["operatorNotFound"]);
                    return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["operatorNotFound"] };
                }
                // controllo cicli sonda da EW
                if (req.ExamID.HasValue && req.SiteId.HasValue && req.UoId.HasValue)
                {
                    if (CycleManager.GetExamCycleId(dev.ID, req.ExamID.Value, req.SiteId.Value, req.UoId.Value) > 0)
                        return new Response
                        {
                            Successed = false,
                            // NON ELIMINARE STRINGA DEVICE_PRESENT_ERROR IN ERROR MESSAGE SERVE AD EW!!!
                            ErrorMessage = "DEVICE_PRESENT_ERROR" + Dictionary.Instance["cycle_already_present"]
                        };
                }
                //si forza lo stato e si trascurano le transazioni configurate
                if (req.ForceState.HasValue && req.ForceState.Value)
                {
                    var set_device_result = StateTransactions.SetDeviceState(dev.ID,
                        user.ID,
                        reader_operation.ID,
                        DateTime.Now,
                        dev.Id_sede,
                        req.ExamID,
                        req.SiteId,
                        req.UoId);
                    if (set_device_result)
                        return new Response() { Successed = true };
                    return new Response() { Successed = false, ErrorMessage = "StateTransaction.SetDeviceState return false" };
                }
                //Se la transazione non è valida e non sto forzando la fase...
                if (!StateTransactions.IsValid(dev.StateID, reader_operation.ID, dev.Id_sede))
                {
                    string stateAccepted = (from s in StateTransactions.Get() where s.IDStateOld == dev.StateID select s.StateNew).Aggregate((i, j) => i + ";" + j);
                    Logger.Error(string.Format(Dictionary.Instance["stateNotValid2"], dev.Tag, dev.Description, dev.StateDescription, stateAccepted));
                    return new Response() { Successed = false, ErrorMessage = string.Format(Dictionary.Instance["stateNotValid2"], dev.Tag, dev.Description, dev.StateDescription, stateAccepted) };
                }
                Logger.Info("output: true");
                var result = StateTransactions.Add(dev.ID,
                    user.ID,
                    reader_operation.ID,
                    DateTime.Now,
                    dev.Id_sede,
                    false,
                    req.ExamID,
                    req.SiteId,
                    req.UoId);
                return new Response() { Successed = result };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new Response() { Successed = false, ErrorMessage = ex.Message };
            }
        }
        public static bool TryToFindDeviceBarcode(string sTag, ref string sDescrizione)
        {
            Logger.Info("input: " + sTag);

            try
            {
                Device device = FromBarcode(sTag);
                if (device != null)
                {
                    sDescrizione = device.Description;
                    Logger.Info("return: " + sDescrizione);
                    return true;
                }
                else
                {
                    // Sandro 09/06/2017 // modifica fatta al volo per Petardo a Pordenone //
                    device = FromMat(sTag);
                    if (device != null)
                    {
                        sDescrizione = device.Description;
                        Logger.Info("FROMSERIAL return: " + sDescrizione);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Info("return: false");
            return false;
        }

        public static int GetDeviceIdFromBarcode(string sTag)
        {
            Logger.Info("input: " + sTag);
            try
            {
                Device device = FromBarcode(sTag);
                if (device != null)
                {
                    Logger.Info("return: " + device.Description);
                    return device.ID;
                }
                else
                {
                    device = FromMat(sTag);
                    if (device != null)
                    {
                        Logger.Info("FROMSERIAL return: " + device.Description);
                        return device.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            Logger.Info("return: false");
            return -1;
        }

    }
}
