using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ExtModules
{
	/// <summary>
	/// Summary description for AMLogin.
	/// </summary>
	/// 

	public class InteropAmLogin
	{
#if DEBUG
        const string _ModuleName = "AmLoginD.dll";
#else
        const string _ModuleName = "AmLogin.dll";
#endif
        [DllImport(_ModuleName)]
		public static extern bool AmLogin_Login_OLD(string szUsernameMod, string szComputerName, string szInstallationName, string lpLoginTitle, string pConnectionString);
        [DllImport(_ModuleName)]
		public static extern bool AmLogin_EnableSelfLocalization(string programName, int language);
        [DllImport(_ModuleName)]
		public static extern bool AmLogin_DisableSelfLocalization();
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_Logout();
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_ChangePassword(string szUsernameMod, string szComputerName, string szInstallationName);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_AdministrationDlg(string szUsernameMod, string szComputerName, string szInstallationName);
		[DllImport(_ModuleName)]
		private static extern bool AmLogin_GetUsernameOld([MarshalAs(UnmanagedType.LPStr)] StringBuilder pUsername, int iBufferSize);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_GetUserID(ref uint pUserID);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_GetUserPermission(ref ulong pPermission);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_GetUserSetting(string pSetting, [MarshalAs(UnmanagedType.LPStr)] string pValue, int iBufferSize, string pDefault);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_SetUserSetting(string pSetting, string pValue);
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_IsAdministratorUser();
		[DllImport(_ModuleName)]
		public static extern bool AmLogin_IsSysAdminUser();
		//[DllImport(_ModuleName)]
		//public static extern bool AMLogin_StartScreenSaver(string pMessage, uint hParentWnd);
		//[DllImport(_ModuleName)]
		//public static extern bool AMLogin_StopScreenSaver();
		[DllImport(_ModuleName)]
		public static extern void AmLogin_ViewUserRights();
		[DllImport(_ModuleName)]
		public static extern void AmLogin_SetAvailableRights(UInt64 dwAvailableRights);
		[DllImport(_ModuleName)]
		public static extern void AmLogin_SetPrivilegeDescription(ulong dwPermission, string pDescription);
		public static bool AmLogin_GetUsername(ref string strUsername)
		{
			StringBuilder pUsername = new StringBuilder(255);
			bool bReturn = InteropAmLogin.AmLogin_GetUsernameOld(pUsername, pUsername.Capacity);
			strUsername = pUsername.ToString();
			return bReturn;
		}
		public static bool IsAdmin()
		{
			return AmLogin_IsSysAdminUser() || AmLogin_IsAdministratorUser();
		}
	}
}
