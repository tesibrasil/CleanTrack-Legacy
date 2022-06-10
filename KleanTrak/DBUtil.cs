using LibLog;
using OdbcExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
    public class DBUtil
    {
        public enum LogOperation
        {
            Insert,
            Update,
            Delete
        }
        public static void InsertDbLog(string table_name,
            LogOperation operation,
            int id,
            string classe_elemento = "",
            string nome_campo = "",
            string valore_originale = "",
            string valore_modificato = "")
        {
            OdbcCommand cmd = null;
            try
            {
                string str_operation = "";
                switch (operation)
                {
                    case LogOperation.Insert:
                        str_operation = "INSERT";
                        break;
                    case LogOperation.Update:
                        str_operation = "UPDATE";
                        break;
                    case LogOperation.Delete:
                        str_operation = "DELETE";
                        break;
                    default:
                        str_operation = "LOG_ERROR";
                        break;
                }
                cmd = new OdbcCommand($"INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, CLASSEELEMENTO, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO) " +
                    $"VALUES " +
                    $"('{KleanTrak.Globals.m_strUser}', " +
                    $"'{table_name}', '{str_operation}', " +
                    $"'{KleanTrak.Globals.ConvertDateTime(DateTime.Now)}', '{id}', " +
                    $"'{classe_elemento}', '{nome_campo}', '{valore_originale}', '{valore_modificato}')",
                    GetODBCConnection());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LibLog
                    .Logger
                    .Get()
                    .Write(KleanTrak.Globals.GetLocalIPAddress(),
                        "Dbutil.insertdblog",
                        e.ToString(),
                        null,
                        LibLog.Logger.LogLevel.Error);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static OdbcConnection GetODBCConnection()
        {
            OdbcConnection odbcconnectionReturn = new OdbcConnection(KleanTrak.Globals.strDatabase);

            try
            {
                odbcconnectionReturn.Open();
            }
            catch (Exception e)
            {
                odbcconnectionReturn = null;
                MessageBox.Show(e.Message);
            }

            return odbcconnectionReturn;
        }
        public static string GetDescrSede(int id_sede)
        {

            OdbcCommand cmd = null;
            try
            {
                string query = $"SELECT DESCRIZIONE FROM SEDIESAME WHERE ID = {id_sede}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                return (string)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Globals.Log(e, $"id_sede: {id_sede}");
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool IsUserAnOperator(uint user_id)
        {

            OdbcCommand cmd = null;
            try
            {
                string query = $"SELECT COUNT(*) FROM OPERATORI WHERE ID_UTENTE = {Globals.m_iUserID}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                return cmd.ExecuteScalarInt() > 1;
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static List<Query.Ciclo> GetCycles(int idDisp, int idOp, int idSter, string dateFrom, string dateTo, int success, string connectionString)
        {
            List<Query.Ciclo> res = new List<Query.Ciclo>();
            OdbcDataReader rdr = null;
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand();
                cmd.Connection = GetODBCConnection();
                string queryString = "SELECT * FROM CICLI";
                bool more = false;
                if (idDisp > 0 || idOp > 0 || idSter > 0 || dateFrom != "" || dateTo != "")
                {
                    queryString = queryString + " WHERE ";
                    if (idDisp > 0)
                    {
                        queryString = more ? queryString + " AND " : queryString;
                        queryString = queryString + "IDDISPOSITIVO = " + idDisp;
                        more = true;
                    }
                    if (idOp > 0)
                    {
                        queryString = more ? queryString + " AND " : queryString;
                        queryString = queryString + "IDOPERATOREINIZIOSTERILIZZAZIO = " + idOp;
                        more = true;
                    }
                    if (idSter > 0)
                    {
                        queryString = more ? queryString + " AND " : queryString;
                        queryString = queryString + "IDSTERILIZZATRICE = " + idSter;
                        more = true;
                    }
                    if (dateFrom != "")
                    {
                        queryString = more ? queryString + " AND " : queryString;
                        queryString = queryString + "DATAINIZIOSTERILIZZAZIONE >= '" + dateFrom + "'";
                        more = true;
                    }
                    if (dateTo != "")
                    {
                        queryString = more ? queryString + " AND " : queryString;
                        queryString = queryString + "DATAINIZIOSTERILIZZAZIONE <= '" + dateTo + "'";
                        more = true;
                    }
                }
                if (success >= 0)
                {
                    queryString = more ? queryString + " AND " : queryString + " WHERE ";
                    queryString = queryString + "FAILED=" + (success == 1 ? 0 : 1);
                }
                cmd.CommandText = queryString;
                rdr = cmd.ExecuteReader();
                Core.DbConnection.ConnectionString = KleanTrak.Globals.strDatabase;
                while (rdr.Read())
                {
                    try
                    {
                        int id_op_inizio = rdr.GetIntEx("IDOPERATOREINIZIOSTERILIZZAZIO");
                        int id_op_fine = rdr.GetIntEx("IDOPERATOREFINESTERILIZZAZIONE");
                        int id_sterilizzatrice = rdr.GetIntEx("IDSTERILIZZATRICE");
                        Model.Device device = Core.Devices.FromID(rdr.GetIntEx("IDDISPOSITIVO"));
                        Model.Operator operator_start = Core.Operators.FromID(id_op_inizio);
                        Model.Operator operator_end = Core.Operators.FromID(id_op_fine);
                        Model.Washer washer = Core.Washers.FromId(id_sterilizzatrice);
                        Query.Ciclo ciclo = new Query.Ciclo();
                        ciclo.DataInizio = rdr.GetStringEx("DATAINIZIOSTERILIZZAZIONE");
                        ciclo.DataFine = rdr.GetStringEx("DATAFINESTERILIZZAZIONE");
                        ciclo.Id = rdr.GetIntEx("ID");
                        ciclo.Dispositivo = device.Description;
                        ciclo.OperatoreInizio = $"{operator_start?.LastName} {operator_start?.FirstName}";
                        ciclo.OperatoreFine = $"{operator_end?.LastName} {operator_end?.FirstName}";
                        ciclo.Sterilizzatrice = washer.Description;
                        ciclo.Esito = (rdr.GetBoolEx("FAILED")) ? "FAILED" : "OK";
                        res.Add(ciclo);
                    }
                    catch(Exception exc)
                    {
                        LibLog.Logger.Get().Write("", "Dbutil.GetCycles", exc.ToString(), null, LibLog.Logger.LogLevel.Error);
                        continue;
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                LibLog.Logger.Get().Write("", "Dbutil.getCycles", e.ToString(), null, LibLog.Logger.LogLevel.Error);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static Query.Ciclo GetCycle(int cycleId)
        {
            List<Query.Ciclo> res = new List<Query.Ciclo>();
            OdbcDataReader rdr = null;
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand();
                cmd.Connection = GetODBCConnection();
                string queryString = $"SELECT * FROM CICLI WHERE ID = {cycleId}";                
                cmd.CommandText = queryString;
                rdr = cmd.ExecuteReader();
                Core.DbConnection.ConnectionString = KleanTrak.Globals.strDatabase;
                while (rdr.Read())
                {
                    try
                    {
                        int id_op_inizio = rdr.GetIntEx("IDOPERATOREINIZIOSTERILIZZAZIO");
                        int id_op_fine = rdr.GetIntEx("IDOPERATOREFINESTERILIZZAZIONE");
                        int id_sterilizzatrice = rdr.GetIntEx("IDSTERILIZZATRICE");
                        Model.Device device = Core.Devices.FromID(rdr.GetIntEx("IDDISPOSITIVO"));
                        Model.Operator operator_start = Core.Operators.FromID(id_op_inizio);
                        Model.Operator operator_end = Core.Operators.FromID(id_op_fine);
                        Model.Washer washer = Core.Washers.FromId(id_sterilizzatrice);
                        Query.Ciclo ciclo = new Query.Ciclo();
                        ciclo.DataInizio = rdr.GetStringEx("DATAINIZIOSTERILIZZAZIONE");
                        ciclo.DataFine = rdr.GetStringEx("DATAFINESTERILIZZAZIONE");
                        ciclo.Id = rdr.GetIntEx("ID");
                        ciclo.Dispositivo = device.Description;
                        ciclo.OperatoreInizio = $"{operator_start?.LastName} {operator_start?.FirstName}";
                        ciclo.OperatoreFine = $"{operator_end?.LastName} {operator_end?.FirstName}";
                        ciclo.Sterilizzatrice = washer.Description;
                        ciclo.Esito = (rdr.GetBoolEx("FAILED")) ? "FAILED" : "OK";
                        res.Add(ciclo);
                    }
                    catch (Exception exc)
                    {
                        LibLog.Logger.Get().Write("", "Dbutil.GetCycles", exc.ToString(), null, LibLog.Logger.LogLevel.Error);
                        continue;
                    }
                }
                return res.Count == 1 ? res[0] : null;
            }
            catch (Exception e)
            {
                LibLog.Logger.Get().Write("", "Dbutil.getCycle", e.ToString(), null, LibLog.Logger.LogLevel.Error);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }


        public static List<Query.ComboboxItem> LoadWashers(int id_sede, bool include_storage = true, bool dummyWasher = false)
        {
            List<Query.ComboboxItem> retlist = new List<Query.ComboboxItem>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            if (dummyWasher)
            {
                Query.ComboboxItem dummy = new Query.ComboboxItem()
                {
                    Text = "",
                    Value = 0
                };
                retlist.Add(dummy);
            }
            try
            {
                string query = $"SELECT * FROM ARMADI_LAVATRICI WHERE DISMESSO = 0 " +
                    $"AND IDSEDE = {id_sede}";
                if (!include_storage)
                    query += $" AND TIPO NOT IN ({(int)KleanTrak.Model.WasherStorageTypes.Storage_Cantel_EDC})";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    retlist.Add(new Query.ComboboxItem
                    {
                        Value = rdr.GetIntEx("ID"),
                        Text = rdr.GetStringEx("DESCRIZIONE")
                    });
                }
                return retlist;
            }
            catch (Exception e)
            {
                Logger.Get().Write("",
                    $"Dbutil.LoadWashers id_sede: {id_sede}",
                    e.ToString(),
                    null,
                    Logger.LogLevel.Error);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static List<Query.ComboboxItem> LoadOperatori(int id_sede)
        {

            var res = new List<Query.ComboboxItem>();
            res.Add(new KleanTrak.Query.ComboboxItem
            {
                Text = "",
                Value = 0
            });
            OdbcDataReader rdr = null;
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT ID, NOME, COGNOME " +
                    $"FROM OPERATORI INNER JOIN OPERATORI_SEDI " +
                    $"ON OPERATORI_SEDI.IDOPERATORE = OPERATORI.ID " +
                    $"WHERE DISATTIVATO = 0 AND OPERATORI_SEDI.IDSEDE = {id_sede}",
                    GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    KleanTrak.Query.ComboboxItem item = new Query.ComboboxItem();
                    try
                    {
                        item.Value = rdr.GetIntEx("ID");
                        item.Text = $"{rdr.GetStringEx("NOME")} {rdr.GetStringEx("COGNOME")}";
                    }
                    catch
                    {
                        item = new Query.ComboboxItem();
                    }
                    res.Add(item);
                }
                return res;
            }
            catch (Exception e)
            {
                Logger.Get().Write("",
                    $"Dbutil.LoadOperatori id_sede: {id_sede}",
                    e.ToString(),
                    null,
                    Logger.LogLevel.Error);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static List<Query.ComboboxItem> LoadDispositivi(int id_sede)
        {

            List<Query.ComboboxItem> res = new List<Query.ComboboxItem>();
            Query.ComboboxItem dummy = new Query.ComboboxItem()
            {
                Text = "",
                Value = 0
            };
            res.Add(dummy);
            OdbcDataReader rdr = null;
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT ID, DESCRIZIONE " +
                    $"FROM DISPOSITIVI " +
                    $"WHERE DISMESSO = 0 AND ELIMINATO = 0 " +
                    $"AND IDSEDE = {id_sede}", GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int Id = 0;
                    string desc = "";
                    Query.ComboboxItem item = new Query.ComboboxItem();
                    try
                    {
                        Id = rdr.GetIntEx("ID");
                        desc = rdr.GetStringEx("DESCRIZIONE");
                        item.Value = Id;
                        item.Text = desc;
                    }
                    catch
                    {
                        item = new Query.ComboboxItem();
                    }
                    res.Add(item);
                }
                return res;
            }
            catch (Exception e)
            {
                Logger.Get().Write("",
                    $"Dbutil.LoadDispositivi id_sede: {id_sede}",
                    e.ToString(),
                    null,
                    Logger.LogLevel.Error);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static List<Query.ComboboxItem> GetSeeds()
        {
            List<Query.ComboboxItem> retlist = new List<Query.ComboboxItem>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                string query = "SELECT * FROM SEDIESAME WHERE ELIMINATO = 0";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    retlist.Add(new Query.ComboboxItem
                    {
                        Value = rdr.GetIntEx("ID"),
                        Text = rdr.GetStringEx("DESCRIZIONE")
                    });
                }
                return retlist;
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public class UoSedi { public int iduo { get; set; } public int idsede { get; set; } }
        public static List<UoSedi> GetSeedsUo()
        {
            List<UoSedi> retlist = new List<UoSedi>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                string query = "SELECT * FROM UO_SEDI";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    retlist.Add(new UoSedi { iduo = rdr.GetIntEx("IDUO"), idsede = rdr.GetIntEx("IDSEDE") });
                return retlist;
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static List<Query.ComboboxItem> GetUoList()
        {
            List<Query.ComboboxItem> retlist = new List<Query.ComboboxItem>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                string query = "SELECT * FROM UO WHERE ELIMINATO = 0";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    retlist.Add(new Query.ComboboxItem
                    {
                        Value = rdr.GetIntEx("ID"),
                        Text = rdr.GetStringEx("DESCRIZIONE")
                    });
                }
                return retlist;
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool AddUo(string text, out int newid)
        {
            OdbcCommand cmd = null;
            newid = -1;
            try
            {
                cmd = new OdbcCommand("SELECT MAX(ID) FROM UO", GetODBCConnection());
                newid = cmd.ExecuteScalarInt();
                newid++;
                cmd.CommandText = $"INSERT INTO UO (ID, DESCRIZIONE) VALUES ({newid}, ?)";
                cmd.Parameters.Add(new OdbcParameter("descrizione", text));
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with id {newid} not inserted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "AddUo");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static bool DeleteUo(int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"UPDATE UO SET ELIMINATO = 1 WHERE ID = {id}", GetODBCConnection());
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with id {id} not deleted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "DeleteUo");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool RenameUo(int id, string description)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"UPDATE UO SET DESCRIZIONE = ? WHERE ID = {id}", GetODBCConnection());
                cmd.Parameters.Add(new OdbcParameter("descrizione", description));
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row description with id {id} not updated");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "RenameUo");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static bool AddSite(string text, out int newid)
        {
            OdbcCommand cmd = null;
            newid = -1;
            try
            {
                cmd = new OdbcCommand("SELECT MAX(ID) FROM SEDIESAME", GetODBCConnection());
                newid = cmd.ExecuteScalarInt();
                newid++;
                cmd.CommandText = $"INSERT INTO SEDIESAME (ID, DESCRIZIONE) VALUES ({newid}, ?)";
                cmd.Parameters.Add(new OdbcParameter("descrizione", text));
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with id {newid} not inserted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "AddSite");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool DeleteSite(int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"UPDATE SEDIESAME SET ELIMINATO = 1 WHERE ID = {id}", GetODBCConnection());
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with id {id} not deleted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "DeleteSite");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool RenameSite(int id, string description)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"UPDATE SEDIESAME SET DESCRIZIONE = ? WHERE ID = {id}", GetODBCConnection());
                cmd.Parameters.Add(new OdbcParameter("descrizione", description));
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row description with id {id} not updated");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "RenameSite");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool DeleteSiteUo(int iduo, int idsite)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"DELETE FROM UO_SEDI WHERE IDUO = {iduo} AND IDSEDE = {idsite}", GetODBCConnection());
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with idUO {iduo} and idsite {idsite} not deleted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "DeleteSiteUo");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static bool AddSiteUo(int iduo, int idsite)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"INSERT INTO UO_SEDI (IDUO, IDSEDE) VALUES ({iduo}, {idsite})", GetODBCConnection());
                if (cmd.ExecuteNonQuery() != 1)
                    throw new ApplicationException($"row with idUO {iduo} and idsite {idsite} not inserted");
                return true;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "AddSiteUo");
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static int GetOperatorsCount(int idsite)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT COUNT(*) FROM  OPERATORI_SEDI WHERE IDSEDE = {idsite}", GetODBCConnection());
                return cmd.ExecuteScalarInt();
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "GetOperatorsCount");
                return 1;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static int GetWashersCount(int idsite)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT COUNT(*) FROM  ARMADI_LAVATRICI WHERE IDSEDE = {idsite}", GetODBCConnection());
                return cmd.ExecuteScalarInt();
            }
            catch (Exception ex)
            {
                Globals.Log(ex, "GetWashersCount");
                return 1;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public static bool GetLastCycleId(int id_device, out int id_last_cycle)
        {
            OdbcCommand cmd = null;
            id_last_cycle = -1;
            try
            {
                string query = $"SELECT MAX(ID) FROM CICLI WHERE IDDISPOSITIVO = {id_device}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                var result = cmd.ExecuteScalar();
                if (result == DBNull.Value)
                    return false;
                id_last_cycle = (int)result;
                return true;
            }
            catch (Exception e)
            {
                Globals.Log(e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        public static bool GetPrevCycleId(int id_device, int id_sel_cycle, out int id_prev_cycle, out string desc, out string tipo, out string matricola)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            id_prev_cycle = -1;
            desc = "";
            tipo = "";
            matricola = "";
            try
            {
                string query = $"SELECT MAX(ID) FROM CICLI WHERE IDDISPOSITIVO = {id_device} AND ID < {id_sel_cycle}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                var result = cmd.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    MessageBox.Show(Globals.strTable[156]);
                    return false;
                }
                id_prev_cycle = System.Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = $"SELECT DISPOSITIVI.DESCRIZIONE, TIPIDISPOSITIVI.DESCRIZIONE AS TIPO, MATRICOLA " +
                    $"FROM DISPOSITIVI LEFT OUTER JOIN TIPIDISPOSITIVI ON DISPOSITIVI.IDTIPO = TIPIDISPOSITIVI.ID " +
                    $"WHERE DISPOSITIVI.ID = {id_device}";
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    throw new ApplicationException("device not found");
                desc = rdr.GetStringEx("DESCRIZIONE").Trim();
                tipo = rdr.GetStringEx("TIPO").Trim();
                matricola = rdr.GetStringEx("MATRICOLA").Trim();
                return true;
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
                return false;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }

    }
}
