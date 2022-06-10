using System;
using System.Collections.Generic;
using System.Text;

namespace Commons
{
	public static class StringExtensions
	{
		public static string PrepareForQuery(this string value)
		{
			value.Replace("'", "''");
			return value;
		}
		public static string Separator
		{
			get
			{
				return (Environment.OSVersion.Platform == PlatformID.Unix) ? "/" : @"\";
			}
		}
		public static string AttachArguments(this string value, params object[] args)
		{
			foreach (object arg in args)
			{
				string cur_value = (arg == null) ? "" : arg.ToString();
				value += $"{Environment.NewLine}{nameof(arg)}: {cur_value}";
			}
			return value;
		}

		/// <summary>
		/// Ritorna il path costituito con i parametri passati
		/// </summary>
		/// <param name="value">la stringa iniziale (può essere anche vuota)</param>
		/// <param name="pathslices">i pezzi del path da concatenare</param>
		public static string ComposePathToDir(this string value, params string[] pathslices)
		{
			if (value.Length > 0 && value.Substring(value.Length - 1, 1) != Separator)
				value += Separator;

			foreach (string slice in pathslices)
			{
				if (slice.Length == 0)
					continue;

				var newvalue = "";
				newvalue = slice;

				if (newvalue.Substring(0, 1) == Separator)
				{
					if (newvalue.Length == 1)
						continue;
					newvalue = newvalue.Substring(1, newvalue.Length - 1);
				}

				if (newvalue.Substring(newvalue.Length - 1, 1) != Separator)
				{
					newvalue += Separator;
				}

				value += newvalue;
			}

			return value;
		}

		/// <summary>
		/// Ritorna il path costituito con i parametri passati, di cui l'ultimo è il nome
		/// del file, pertanto viene omesso l'ultimo '/'
		/// </summary>
		/// <param name="value">la stringa iniziale</param>
		/// <param name="paths_and_filename">i pezzi del path e come ultimo parametro il nome del file</param>
		public static string ComposePathToFile(this string value, params string[] paths_and_filename)
		{
			value = value.ComposePathToDir(paths_and_filename);
			return (value.Length > 0) ? value.Substring(0, value.Length - 1) : value;
		}

		/// <summary>
		/// Ritorna un nome file senza il path
		/// </summary>
		/// <param name="include_extension">se true include l'estensione del file, altrimenti no</param>
		/// <returns></returns>
		public static string GetFileNameWithoutPath(this string value, bool include_extension = true)
		{
			if (value.Length == 0)
				return value;
			var retval = value;
			if (value.IndexOf(Separator) > 0)
				retval = value.Substring(value.LastIndexOf(Separator) + 1);
			if (!include_extension && retval.IndexOf('.') > 0)
				retval = retval.Substring(0, retval.Length - retval.LastIndexOf('.') + 1);
			return retval;
		}

	}
}
