using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Xml.Serialization;

namespace ImageSI.Configuration
{
    public class ConfigurationDB : ConfigurationHelper, IConfigurationHelper
    {
        private string ODBCConnectionString = "";
        private string _Owner = "";

        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        public ConfigurationDB(string odbcConnString, string owner)
        {
            ODBCConnectionString = odbcConnString;
            _Owner = owner;
        }

        public void LoadSettings()
        {
            if (SettingsLoaded)
                return;

            OdbcConnection conn = new OdbcConnection(ODBCConnectionString);
            OdbcDataReader rdr = null;

            int iTimes = 0;
            while (conn.State != ConnectionState.Open && iTimes < 6)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    iTimes++;
                    System.Threading.Thread.Sleep(5000);
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                try
                {
                    OdbcCommand command = new OdbcCommand("select chiave, valore, criptato from configurazione where proprietario = '" + _Owner + "'", conn);
                    rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        string key = rdr.GetString(0);
                        string value = rdr.GetString(1);
                        bool encrypted = rdr.GetBoolean(2);

                        Configuration.Add(key, new ConfigValue() { Value = value, Encrypted = encrypted });
                    }

                    rdr.Close();
                    conn.Close();
                }
                catch (Exception)
                {
                    SaveSettings();
                }
            }

            SettingsLoaded = true;

        }

        //
        public void SaveSettings()
        {
            OdbcConnection conn = new OdbcConnection(ODBCConnectionString);

            try
            {
                conn.Open();

                foreach (var item in Configuration)
                {
                    OdbcCommand command = new OdbcCommand("{call sp_ConfigInsert(?, ?, ?, ?, ?)}", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("result", OdbcType.Int).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("chiave", OdbcType.VarChar, 255).Value = item.Key;
                    command.Parameters.Add("valore", OdbcType.VarChar, 255).Value = item.Value.Value;
                    command.Parameters.Add("criptato", OdbcType.Int).Value = item.Value.Encrypted ? 1 : 0;
                    command.Parameters.Add("proprietario", OdbcType.VarChar, 255).Value = _Owner;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void UpdateSettings()
        {
            OdbcConnection conn = new OdbcConnection(ODBCConnectionString);

            try
            {
                conn.Open();

                foreach (var item in Configuration)
                {
                    OdbcCommand command = new OdbcCommand("{call sp_ConfigUpdate(?, ?, ?, ?, ?)}", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("result", OdbcType.Int).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("chiave", OdbcType.VarChar, 255).Value = item.Key;
                    command.Parameters.Add("valore", OdbcType.VarChar, 255).Value = item.Value.Value;
                    command.Parameters.Add("criptato", OdbcType.Int).Value = item.Value.Encrypted ? 1 : 0;
                    command.Parameters.Add("proprietario", OdbcType.VarChar, 255).Value = _Owner;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        //
        public string GetODBCConnectionString(string defaultValue)
        {
            return ODBCConnectionString;
        }
    }
}
