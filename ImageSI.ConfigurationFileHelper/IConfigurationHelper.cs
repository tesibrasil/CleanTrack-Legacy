using System;
using System.Collections.Generic;

using System.Text;

namespace ImageSI.Configuration
{
    public interface IConfigurationHelper
    {
        string Set(string strSection, string strKey, string strValue, bool encrypted = false);

        string Set(string sectionKey, string strValue, bool encrypted = false);

        string Get(string strSection, string strKey, string strDefault, bool EncryptDefault = false);

        string Get(string strSectionKey, string strDefault, bool EncryptDefault = false);

        string GetODBCConnectionString(string defaultValue);

        SettingList GetSettingList();

        void LoadSettings();

        void SaveSettings();
    }
}
