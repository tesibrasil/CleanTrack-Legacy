using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using KleanTrak.Models;

namespace KleanTrak.Managers
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

        public void ExecuteNonQuery(string query)
        {
            if (ConnectionString == null)
                throw new KleanTrackException("DatabaseHelper Connection string missing!");

            OdbcConnection conn = new OdbcConnection(ConnectionString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = conn;

            for (int i = 0;; i++)
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
    }
}
