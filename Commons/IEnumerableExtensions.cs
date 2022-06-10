using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Commons
{
	public static class IEnumerableExtensions
	{
		public static string ToCsvString<T>(this IEnumerable<T> list, string separator = "")
		{
			if (list == null)
				return "is null";
			string retval = "";
			separator = (separator.Length == 0) ? Environment.NewLine : separator;
			foreach (T item in list)
				foreach (PropertyInfo p in typeof(T).GetProperties())
					retval += p.Name + ": " + (p.GetValue(item) ?? "null");
			return retval;
		}
	}
}
