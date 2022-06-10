using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KleanTrak.Models;

namespace KleanTrak.Server
{
    public class Dictionary
    {
        public DictionaryBase DictionaryBase { private set; get; }
        private static Dictionary instance;

        private Dictionary()
        {
            DictionaryBase = new DictionaryBase();
        }

        public static void Init()
        {
            if (instance != null)
                return;

            instance = new Dictionary();
            var dictionaryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Dictionary.xml";

            DictionaryBase storedDictionary = null;

            try
            {
                var serializedXml = File.ReadAllText(dictionaryPath);
                storedDictionary = (DictionaryBase)DictionaryBase.ReadObjectFromXml(serializedXml);
            }
            catch (Exception)
            {
            }

            if (storedDictionary == null)
                storedDictionary = new DictionaryBase();

            storedDictionary.UpdateWithDefaultValues();
            instance.DictionaryBase = storedDictionary;
            instance.Save();
        }

        public void Save()
        {
            var dictionaryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Dictionary.xml";

            try
            {
                File.WriteAllText(dictionaryPath, instance.DictionaryBase.SaveObjectToXml());
            }
            catch (Exception)
            {
            }
        }

        public static Dictionary Instance
        {
            get
            {
                return instance;
            }
        }

        public string this[string index]
        {
            get
            {
                return DictionaryBase[index];
            }
        }
    }
}
