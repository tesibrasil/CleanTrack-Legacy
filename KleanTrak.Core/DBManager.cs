using System;
using System.Data.Odbc;

namespace KleanTrak.Core
{
	public class DBManager
	{
		protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public string Driver = "Microsoft Access Driver (*.mdb)";
		public string UID { get; set; }
		public string PWD { get; set; }
		public string DBQ { get; set; }
		public virtual string GetConnString()
		{
			return "Driver={" + Driver + "};UID=" + UID + ";PWD=" + PWD + ";DBQ=" + DBQ;
		}
		public int GetAutoNumber(OdbcDataReader reader, string colName, int null_value = -1)
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? null_value : Convert.ToInt32(reader.GetInt32(ordinal));
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
		public int GetNumber(OdbcDataReader reader, string colName, int null_value = -1)
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? null_value : reader.GetInt32(ordinal);
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
		public bool GetBool(OdbcDataReader reader, string colName, bool null_value = false)
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? null_value : reader.GetBoolean(ordinal);
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
		public string GetYN(OdbcDataReader reader, string colName, string null_value = "")
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? null_value : reader.GetString(ordinal);
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
		public string GetString(OdbcDataReader reader, string colName, string null_value = "")
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? null_value : reader.GetString(ordinal);
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
		public DateTime GetDateTime(OdbcDataReader reader, string colName)
		{
			try
			{
				var ordinal = reader.GetOrdinal(colName);
				return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
			}
			catch (Exception e)
			{
				Logger.Error($"colName: {colName}", e);
				throw;
			}
		}
	}
}
