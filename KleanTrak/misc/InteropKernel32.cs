using System.Runtime.InteropServices;
using System.Text;

namespace ExtModules
{
	public class InteropKernel32
	{
		[DllImport("Kernel32.dll")]
		public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, uint nDefault, string lpFileName);
		[DllImport("Kernel32.dll")]
		public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

		public static string GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpFileName)
		{
			StringBuilder pReturnString = new StringBuilder(255);
			GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, pReturnString, pReturnString.Capacity, lpFileName);
			return pReturnString.ToString();
		}

		public static System.Drawing.Color GetPrivateProfileColor(string lpAppName, string lpKeyName, System.Drawing.Color clDefault, string lpFileName)
		{
			int R = (int)GetPrivateProfileInt(lpAppName, "R" + lpKeyName, clDefault.R, lpFileName);
			int G = (int)GetPrivateProfileInt(lpAppName, "G" + lpKeyName, clDefault.G, lpFileName);
			int B = (int)GetPrivateProfileInt(lpAppName, "B" + lpKeyName, clDefault.B, lpFileName);
			return System.Drawing.Color.FromArgb(R, G, B);
		}

		public static void WritePrivateProfileColor(string lpAppName, string lpKeyName, System.Drawing.Color clColor, string lpFileName)
		{
			WritePrivateProfileString(lpAppName, "R" + lpKeyName, clColor.R.ToString(), lpFileName);
			WritePrivateProfileString(lpAppName, "G" + lpKeyName, clColor.G.ToString(), lpFileName);
			WritePrivateProfileString(lpAppName, "B" + lpKeyName, clColor.B.ToString(), lpFileName);
		}

		[DllImport("Kernel32.dll")]
		private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [MarshalAs(UnmanagedType.LPStr)] StringBuilder lpReturnedString, int nSize, string lpFileName);
	}

}

