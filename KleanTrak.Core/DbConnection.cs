using KleanTrak.Model;
using System;
using System.Data;
using System.Data.Odbc;
using OdbcExtensions;

namespace KleanTrak.Core
{
    public class DbConnection
    {
        public static string ConnectionString { set; get; }

        public DbRecordset ExecuteReader(string query)
        {
            if (ConnectionString == null)
                throw new KleanTrackException("DatabaseHelper Connection string missing!");
            DbRecordset ret = new DbRecordset();
            OdbcConnection conn = new OdbcConnection(ConnectionString);
            OdbcCommand cmd = new OdbcCommand(query, conn);
            OdbcDataReader reader = null;
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DbRecord record = new DbRecord();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        record.Add(reader.GetName(i).ToUpper(), reader.IsDBNull(i) ? null : reader.GetValue(i));
                    }

                    ret.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new KleanTrackException(ex.Message + "\r\n\r\nQuery:\r\n" + query + "\r\n\r\nStack trace:\r\n" + ex.StackTrace);
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    reader.Dispose();
                }
                cmd.Dispose();
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }

            return ret;
        }
        public object ExecuteScalar(string query)
        {
            OdbcConnection conn = null;
            OdbcCommand cmd = null;
            try
            {
                conn = new OdbcConnection(ConnectionString);
                conn.Open();
                cmd = new OdbcCommand(query, conn);
                return cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }
        public void ExecuteNonQuery(string query)
        {
            if (ConnectionString == null)
                throw new KleanTrackException("DatabaseHelper Connection string missing!");

            OdbcConnection conn = new OdbcConnection(ConnectionString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = conn;

            for (int i = 0; ; i++)
            {
                int start = query.IndexOf("##START_STRING_PARAMETER");
                if (start >= 0)
                {
                    int end = query.IndexOf("END_STRING_PARAMETER##", start);
                    if (end >= 0)
                    {
                        string parameterValue = query.Substring(start + "##START_STRING_PARAMETER".Length, end - start - "##START_STRING_PARAMETER".Length);
                        query = query.Substring(0, start) + "?" + query.Substring(end + "END_STRING_PARAMETER##".Length);

                        OdbcParameter param = new OdbcParameter("PARAM" + i.ToString(), OdbcType.Text);
                        param.Direction = ParameterDirection.Input;
                        param.Value = parameterValue;
                        cmd.Parameters.Add(param);
                    }
                    else
                        break;
                }
                else
                    break;
            }

            cmd.CommandText = query;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new KleanTrackException(ex.Message + "\r\n\r\nQuery:\r\n" + query + "\r\n\r\nStack trace:\r\n" + ex.StackTrace);
            }
            finally
            {
                cmd.Dispose();

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }
        public static int GetMaxFieldValue(string table_name, string key_name, string filter = "")
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT MAX({key_name}) " +
                    $"FROM {table_name}" +
                    $"{((filter.Length > 0) ? " WHERE " + filter : "")}",
                new OdbcConnection(ConnectionString));
                object result = cmd.ExecuteScalar();
                if (result == DBNull.Value)
                    return -1;
                return System.Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

    }
}
