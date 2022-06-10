using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbcExtensions
{
	public static class OdbcCommandExtensions
	{
		public static int ExecuteScalarInt(this OdbcCommand cmd, int int_for_null = -1)
		{
			var result = cmd.ExecuteScalar();
			if (result == null || result == DBNull.Value)
				return int_for_null;
			return System.Convert.ToInt32(result);
		}
		public static long ExecuteScalarLong(this OdbcCommand cmd, long long_for_null = -1)
		{
			var result = cmd.ExecuteScalar();
			if (result == null || result == DBNull.Value)
				return long_for_null;
			return System.Convert.ToInt64(result);
		}
		public static int GetMaxKeyValue(this OdbcCommand cmd, string table_name, string key_name)
		{
			cmd.CommandText = $"SELECT MAX({key_name}) FROM {table_name}";
			return cmd.ExecuteScalarInt();
		}
	}
}
