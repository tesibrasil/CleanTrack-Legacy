using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Xml.Serialization;

namespace ImageSI.Configuration
{
    public class ConfigurationFile : ConfigurationHelper, IConfigurationHelper
    {
        private bool AbsoluteConfigPath = false;

        private string _strFilename;

        //
        public ConfigurationFile(string strFilename, bool absolutePath = false)
        {
            if (strFilename == null || strFilename.Length == 0)
                throw new ArgumentException();

            AbsoluteConfigPath = absolutePath;
            _strFilename = strFilename;
        }

        //
        public void LoadSettings()
        {
            if (SettingsLoaded)
                return;

            var path = GetSettingsPath();
            FileStream fStream = null;

            try
            {
                fStream = new FileStream(path, FileMode.Open);

                var xmlReader = new XmlSerializer(Configuration.GetType());
                Configuration = ((SerializableDictionary<string, ConfigValue>)xmlReader.Deserialize(fStream));
            }
            catch (Exception)
            {
                if (fStream != null)
                    fStream.Close();

                SaveSettings();
            }

            if (fStream != null)
                fStream.Close();

            SettingsLoaded = true;
        }

        //
        public void SaveSettings()
        {
            var path = GetSettingsPath();
            FileStream fStream = null;

            try
            {
                fStream = new FileStream(path, FileMode.Create);
                var xmlWriter = new XmlSerializer(Configuration.GetType());
                xmlWriter.Serialize(fStream, Configuration);
            }
            catch (Exception)
            {
            }

            if (fStream != null)
            {
                fStream.Close();
                fStream.Dispose();
            }
        }

        //
        private string GetSettingsPath()
        {
            if (AbsoluteConfigPath)
                return _strFilename;

            var uri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var path = new FileInfo(uri.LocalPath).DirectoryName;
            return path + "\\" + _strFilename;
        }

        //
        public string GetODBCConnectionString(string defaultValue)
        {
            return Get("General", "ODBCConnectionString", defaultValue, true);
        }
    }
}
