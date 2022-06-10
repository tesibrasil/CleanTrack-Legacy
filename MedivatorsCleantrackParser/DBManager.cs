using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;
using System.Threading;

namespace MedivatorsCleantrackParser
{
	public class DBManager
	{
		public string Driver = "Microsoft Access Driver (*.mdb)";
		public string UID { get; set; }
		public string PWD { get; set; }
		public string DBQ { get; set; }

		public virtual string GetConnString()
		{
			return "Driver={" + Driver + "};UID=" + UID + ";PWD=" + PWD + ";DBQ=" + DBQ;
		}

		public int GetAutoNumber(OdbcDataReader reader, string colName)
		{
			try
			{
				return Convert.ToInt32(reader.GetInt64(reader.GetOrdinal(colName)));
			}
			catch (InvalidCastException)
			{
				return -1;
			}
		}

		public int GetNumber(OdbcDataReader reader, string colName)
		{
			try
			{
				return reader.GetInt32(reader.GetOrdinal(colName));
			}
			catch (InvalidCastException)
			{
				return -1;
			}
		}

		public bool GetBool(OdbcDataReader reader, string colName)
		{
			try
			{
				return reader.GetBoolean(reader.GetOrdinal(colName));
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		public string GetYN(OdbcDataReader reader, string colName)
		{
			try
			{
				return reader.GetValue(reader.GetOrdinal(colName)).ToString();
			}
			catch (InvalidCastException)
			{
				return "";
			}
		}

		public string GetString(OdbcDataReader reader, string colName)
		{
			try
			{
				return reader.GetString(reader.GetOrdinal(colName));
			}
			catch (InvalidCastException)
			{
				return "";
			}
		}

		public DateTime GetDateTime(OdbcDataReader reader, string colName)
		{
			try
			{
				return reader.GetDateTime(reader.GetOrdinal(colName));
			}
			catch (Exception)
			{
				return DateTime.Today;
			}
		}

	}
}
