using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Commons
{
	public static class ObjectExtensions
	{
		public static string GetStringForNull(this object o)
		{
			return (o == null) ? "is null" : o.ToString();
		}

		public static string Stringify(this object o)
		{
			var type = o.GetType();
			var retval = "";
			foreach(PropertyInfo pi in type.GetProperties())
			{
				retval += $"{pi.Name} {pi.GetValue(o).GetStringForNull()} {Environment.NewLine}";
			}
			return retval;
		}

	}
}
