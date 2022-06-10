using System;
using System.IO;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	public class Dictionary
	{
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            Logger.Info("start");

            instance = new Dictionary();
			var dictionaryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Dictionary.xml";

			DictionaryBase storedDictionary = null;

			try
			{
				var serializedXml = File.ReadAllText(dictionaryPath);
				storedDictionary = (DictionaryBase)DictionaryBase.ReadObjectFromXml(serializedXml);
			}
			catch (Exception ex)
			{
                Logger.Error(ex);
            }

			if (storedDictionary == null)
            {
                storedDictionary = new DictionaryBase();
                Logger.Info("Starting with new dictionary...");
            }

            storedDictionary.UpdateWithDefaultValues();
			instance.DictionaryBase = storedDictionary;
			instance.Save();

            Logger.Info("end");
        }

        public void Save()
		{
            Logger.Info("start");

            var dictionaryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Dictionary.xml";

			try
			{
				File.WriteAllText(dictionaryPath, instance.DictionaryBase.SaveObjectToXml());
			}
			catch (Exception)
			{
			}

            Logger.Info("end");
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
