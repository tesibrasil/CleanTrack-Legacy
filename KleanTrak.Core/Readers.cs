using KleanTrak.Model;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace KleanTrak.Core
{
	public class Readers
    {
		private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public static List<Reader> Get(int siteid)
		{
			List<Reader> ret = new List<Reader>();
			try
			{
				DbConnection db = new DbConnection();
				DbRecordset dataset = db.ExecuteReader($"SELECT * FROM LETTORI WHERE IDSEDE = {siteid}");
				foreach (var record in dataset)
					ret.Add(CreateReader(record));
				return ret;
			}
			catch (Exception e)
			{
				Logger.Error($"siteid {siteid}", e);
				return null;
			}
		}
        public static List<Reader> Get()
        {
            List<Reader> ret = new List<Reader>();
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT * FROM LETTORI WHERE (IDSTATODEFAULT IS NOT NULL AND IDSTATODEFAULT > 0) AND TIPO = " + ((int)(DeviceReadersTypes.Rfid)).ToString() + " AND ELIMINATO = 0");

            foreach (var record in dataset)
            {
                if (Enum.IsDefined(typeof(WasherStorageTypes), record.GetInt("TIPO")))
					ret.Add(CreateReader(record));
			}
            return ret;
        }

		private static Reader CreateReader(DbRecord record)
		{
			Reader rTemp = new Reader();
			rTemp.ID = record.GetInt("ID").Value;
			rTemp.Description = record.GetString("DESCRIZIONE");
			rTemp.IP = record.GetString("IP");
			rTemp.Port = record.GetInt("PORTA").Value;
			rTemp.DefaultStateID = record.GetInt("IDSTATODEFAULT").Value;
			rTemp.Type = (DeviceReadersTypes)record.GetInt("TIPO");
			rTemp.Deleted = record.GetBoolean("ELIMINATO").Value;
			int? timeoutTemp = record.GetInt("TIMEOUTCOMPLETAMENTOSTEP");
			if (timeoutTemp == null)
				rTemp.Timeout = 10;
			else
				rTemp.Timeout = timeoutTemp.Value;
			return rTemp;
		}
	}
}
