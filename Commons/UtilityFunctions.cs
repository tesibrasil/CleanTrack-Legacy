using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Commons
{
    public static class UtilityFunctions
    {
        public static Version GetAssemblyVersion() => Assembly.GetEntryAssembly().GetName().Version;
        public static string GetShortAssemblyVersion()
        {
            var version = GetAssemblyVersion();
            return $"{version.Major}.{version.Minor}";
        }
		public static DateTime GetDate(this string value)
		{
			return new DateTime(int.Parse(value.Substring(0, 4)),
				int.Parse(value.Substring(4, 2)),
				int.Parse(value.Substring(6, 2)),
				int.Parse(value.Substring(8, 2)),
				int.Parse(value.Substring(10, 2)),
				int.Parse(value.Substring(12, 2)));
		}
	}
}
