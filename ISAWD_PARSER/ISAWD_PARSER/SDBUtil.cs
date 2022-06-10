using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;

namespace ISAWD_PARSER
{
    class SDBUtil
    {
        public static bool update(string tabella, int id, string attributo, string valore, string keyField, string connectionToString)
        {
            bool flag = true;

            if (valore != null && valore != "" && valore != "''")
            {


                OdbcConnection connection = new OdbcConnection(connectionToString);

                connection.Open();
                OdbcCommand cmd = new OdbcCommand();
                cmd.Connection = connection;
                cmd.CommandText = "UPDATE " + tabella + " SET " + attributo + "=" + valore + " WHERE " + keyField + "=" + id;
                try
                {
                    OdbcDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    dr.Close();
                }
                catch (Exception)
                {
                    flag = false;
                }

                connection.Close();
            }
            else
            {
                return false;
            }

            return flag;
        }

        public static int InsertNewCyclePlus(int key, string connectionString)
        {
            // int id = -1;

            OdbcConnection connection = new OdbcConnection(connectionString);
            OdbcDataReader dr1 = null;
            try
            {

              
                if (key>0)
                {
                    connection.Open();
                    OdbcCommand cmdEsa = new OdbcCommand();
                    cmdEsa.CommandType = System.Data.CommandType.Text;
                    cmdEsa.Connection = connection;
                    cmdEsa.CommandText = "INSERT INTO CICLIEXT (IDCICLO) VALUES (" + key + ")";

                    dr1 = cmdEsa.ExecuteReader();
                    dr1.Read();
                    dr1.Close();
                }
            }
            catch (Exception)
            {

                // id = -1;

            }

            if (dr1 != null && !dr1.IsClosed)
            {
                dr1.Close();
            }

            connection.Close();

            return grabMax("ID","CICLIEXT",connectionString);
        }

        public static int grabMax(string campo, string table, string connectionToString)
        {
            int MAX = 0;

            OdbcConnection connection = new OdbcConnection(connectionToString);


            connection.Open();
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = connection;

            cmd.CommandText = "SELECT MAX(" + campo + ") FROM " + table;
            OdbcDataReader dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                if (!dr.IsDBNull(0))
                {
                    MAX = dr.GetInt32(0);
                }
            }
            catch (Exception) { }

            dr.Close();
            connection.Close();
            return MAX;
        }

    }
}

  