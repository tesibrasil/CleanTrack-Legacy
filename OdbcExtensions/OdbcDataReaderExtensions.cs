using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbcExtensions
{
	public static class OdbcDataReaderExtensions
	{
		public static long GetLongEx(this OdbcDataReader rdr, string field_name, long value_for_null = 0)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			try
			{
				if (rdr.IsDBNull(ordinal))
					return value_for_null;
				return System.Convert.ToInt64(rdr.GetValue(ordinal));
			}
			catch (Exception e)
			{
				throw new ApplicationException($"exception for field {field_name}", e);
			}
		}
		public static int GetIntEx(this OdbcDataReader rdr, string field_name, int value_for_null = 0)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			try
			{
				if (rdr.IsDBNull(ordinal))
					return value_for_null;
				return System.Convert.ToInt32(rdr.GetValue(ordinal));
			}
			catch (Exception e) 
			{
				throw new ApplicationException($"exception for field {field_name}", e);
			}
		}
		public static string GetStringEx(this OdbcDataReader rdr, string field_name, string value_for_null = "") =>
			(rdr.IsDBNull(rdr.GetOrdinal(field_name))) ? value_for_null : rdr.GetString(rdr.GetOrdinal(field_name));
		public static bool GetBoolEx(this OdbcDataReader rdr, string field_name, bool value_for_null = false)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			try
			{
				if (rdr.IsDBNull(ordinal))
					return value_for_null;
				return System.Convert.ToBoolean(rdr.GetValue(ordinal));
			}
			catch (Exception e)
			{
				throw new ApplicationException($"exception for field {field_name}", e);
			}
		}
	}
}
