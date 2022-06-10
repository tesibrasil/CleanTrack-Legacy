using System;
using System.Collections.Generic;

using System.Text;

namespace ImageSI.Configuration
{
    public class ConfigurationHelper
    {
        public class ConfigValue
        {
            public string Value { set; get; }
            public bool Encrypted { set; get; }
        }

        protected bool SettingsLoaded = false;

        protected SerializableDictionary<string, ConfigValue> Configuration = new SerializableDictionary<string, ConfigValue>();

        //
        public string Set(string strSection, string strKey, string strValue, bool encrypted = false)
        {
            return Set(strSection + "_" + strKey, strValue, encrypted);
        }

        //
        public string Set(string sectionKey, string strValue, bool encrypted)
        {
            string result = strValue;
            try
            {
                if (Configuration.ContainsKey(sectionKey))
                {
                    var val = new ConfigValue()
                    {
                        Encrypted = encrypted,
                        Value = encrypted ? Crypto.Encrypt(strValue) : strValue
                    };

                    Configuration[sectionKey] = val;
                }
                else
                {
                    var val = new ConfigValue()
                    {
                        Encrypted = encrypted,
                        Value = encrypted ? Crypto.Encrypt(strValue) : strValue
                    };

                    Configuration.Add(sectionKey, val);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        //
        public string Get(string strSection, string strKey, string strDefault, bool ToEncrypt = false)
        {
            if (strKey.IndexOf("_") != -1)
                throw new ArgumentException();

            string strReturn = strDefault;

            try
            {
                string key = strSection + "_" + strKey;
                strReturn = Get(key, strDefault, ToEncrypt);
            }
            catch (Exception)
            {
            }

            return strReturn;
        }


        //
        public string Get(string strSectionKey, string strDefault, bool ToEncrypt = false)
        {
            string strReturn = strDefault;

            try
            {
                if (Configuration.ContainsKey(strSectionKey))
                {
                    var val = Configuration[strSectionKey];
                    strReturn = val.Encrypted ? Crypto.Decrypt(val.Value) : val.Value;

                    if ((ToEncrypt == true) && 
                        (val.Encrypted == false))
                        Configuration[strSectionKey] = new ConfigValue() { Encrypted = true, Value = Crypto.Encrypt(strReturn) };
                }
                else
                {
                    var val = new ConfigValue()
                    {
                        Value = ToEncrypt ? Crypto.Encrypt(strDefault) : strDefault,
                        Encrypted = ToEncrypt
                    };

                    Configuration.Add(strSectionKey, val);
                }
            }
            catch (Exception)
            {
            }

            return strReturn;
        }

        //
        public SettingList GetSettingList()
        {
            var result = new SettingList();
            foreach (var item in Configuration)
            {
                result.Add(new Setting()
                {
                    Section = item.Key.Substring(0, item.Key.IndexOf('_', 0)),
                    Key = item.Key.Substring(item.Key.IndexOf('_', 0) + 1),
                    Value = item.Value.Encrypted ? Crypto.Decrypt(item.Value.Value) : item.Value.Value,
                    Encrypted = item.Value.Encrypted
                });
            }

            return result;
        }

    }
}
