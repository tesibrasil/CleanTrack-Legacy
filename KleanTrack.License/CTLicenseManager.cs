using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseManager;
using LicenseManager.Model;
using LocalLicenseChecker;
using log4net;
namespace KleanTrack.License
{
	public static class CTLicenseManager
	{
        public delegate void LicenseLoanExpiredHandler(string connectionId);
        private static event LicenseLoanExpiredHandler LicenseLoanExpired;
		private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ILicenseChecker licenseCheckerInstance = null;
        private static string key = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPm1UUDFLRXFZWTFQenBEZzMzOEczV1U3NWtnOThJTUw3V2kzVm1PZDYyQkxXRDV6VFFGNzBZaVI5elVEeCs5RlJaaEZUeWVQQ1hYOEVXWjNrSjRaOVd4WkdqU2F6RkJNZjZjQmNETEEwZWhmanlnQ3p1TmxpY2hTQ1Zqd0NmZ1N4cUlPeFhqdWdwQ3Zjblhhc1VYQ3BUMW83SGxDRzFRUzZXaXFsV3pvSEVlem5aNDg0eUtYUkErYVR3SUk0TTJZMTdHRWhNR0RlUjMzazZTUWxsdEhiTERMNDRUQWtpMzlKWnJSaUpJQnF3Uk1BR3JsSExVWUlTeXloejVDM2xlaWlyMGxLeHhRZ0hJbm0zZmdwY3BpVVFiQzk0QjRlWnlBSXI3NGJGVDNBTXkzNksybHA0SGVjRUFhelY5VWNIemxiaFp6Z0JEN05hTU5Vdnh1cmZKU0pwUT09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==";
        private static string productID = "CT"; 
        public static void Initialize(string path = "") => licenseCheckerInstance = LocalLicenseCheckerFactory.InitApplicationLicenseChecker(10, Path.Combine(path, "license.lic"), OnLicenseLoanExpired);
        public static void OnLicenseLoanExpired(object sender, LicenseLoanExpiredArgs e) => LicenseLoanExpired?.Invoke(e.SessionId);
        public static void Dispose() => licenseCheckerInstance.Dispose();
        public static void CheckinLicense(string sessionID) => licenseCheckerInstance.CheckinLicense(productID, sessionID);
        public static List<UoClaims> uo_claims = null;
        public static LicenseCheck CheckoutLicense (string sessionID)
        {
			try
			{
                var license = licenseCheckerInstance.CheckoutLicense(productID, key, sessionID);
                if (license.CheckStatus != LicenseCheck.LICENSE_VALID)
                    return license.CheckStatus;
                uo_claims = license.LicenseInfo.GetClaims<IList<UoClaims>>() as List<UoClaims>;
                return license.CheckStatus;
			}
			catch (Exception e)
			{
                Logger.Error(sessionID, e);
                throw;
			}
        }

        public static void RenewLicense (string sessionID)
        {
            try
            {
                var licensecheck = licenseCheckerInstance.RenewLicense(productID, sessionID);
                if (licensecheck != LicenseCheck.LICENSE_VALID)
                    LicenseLoanExpired?.Invoke(sessionID);
            }
            catch (Exception e)
            {
                Logger.Error(sessionID, e);
            }
        }
    }
}
