using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KleanTrak.Server
{
    class ServiceManagement
    {
        [DllImport("Advapi32.dll")]
        protected static extern UInt32 OpenSCManager(string lpMachineName,
                                                     string lpDatabaseName,
                                                     UInt32 dwDesiredAccess);

        [DllImport("Advapi32.dll")]
        protected static extern UInt32 CreateService(UInt32 hSCManager,
                                                     string lpServiceName,
                                                     string lpDisplayName,
                                                     UInt32 dwDesiredAccess,
                                                     UInt32 dwServiceType,
                                                     UInt32 dwStartType,
                                                     UInt32 dwErrorControl,
                                                     string lpBinaryPathName,
                                                     string lpLoadOrderGroup,
                                                     UInt32 lpdwTagId,
                                                     string lpDependencies,
                                                     string lpServiceStartName,
                                                     string lpPassword);
        [DllImport("Advapi32.dll")]
        protected static extern UInt32 OpenService(UInt32 hSCManager,
                                                   string lpServiceName,
                                                   UInt32 dwDesiredAccess);

        [DllImport("Advapi32.dll")]
        protected static extern bool CloseServiceHandle(UInt32 hSCObject);

        [DllImport("Advapi32.dll")]
        protected static extern bool DeleteService(UInt32 hService);

        [DllImport("Kernel32.dll")]
        protected static extern UInt32 GetLastError();

        const int STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        const int SERVICE_QUERY_CONFIG = 0x0001;
        const int SERVICE_CHANGE_CONFIG = 0x0002;
        const int SERVICE_QUERY_STATUS = 0x0004;
        const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        const int SERVICE_START = 0x0010;
        const int SERVICE_STOP = 0x0020;
        const int SERVICE_PAUSE_CONTINUE = 0x0040;
        const int SERVICE_INTERROGATE = 0x0080;
        const int SERVICE_USER_DEFINED_CONTROL = 0x0100;

        const int SC_MANAGER_CONNECT = 0x0001;
        const int SC_MANAGER_CREATE_SERVICE = 0x0002;
        const int SC_MANAGER_ENUMERATE_SERVICE = 0x0004;
        const int SC_MANAGER_LOCK = 0x0008;
        const int SC_MANAGER_QUERY_LOCK_STATUS = 0x0010;
        const int SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020;

        const int SC_MANAGER_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                                           SC_MANAGER_CONNECT |
                                           SC_MANAGER_CREATE_SERVICE |
                                           SC_MANAGER_ENUMERATE_SERVICE |
                                           SC_MANAGER_LOCK |
                                           SC_MANAGER_QUERY_LOCK_STATUS |
                                           SC_MANAGER_MODIFY_BOOT_CONFIG);

        const int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                                        SERVICE_QUERY_CONFIG |
                                        SERVICE_CHANGE_CONFIG |
                                        SERVICE_QUERY_STATUS |
                                        SERVICE_ENUMERATE_DEPENDENTS |
                                        SERVICE_START |
                                        SERVICE_STOP |
                                        SERVICE_PAUSE_CONTINUE |
                                        SERVICE_INTERROGATE |
                                        SERVICE_USER_DEFINED_CONTROL);

        const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        const int SERVICE_AUTO_START = 0x00000002;
        const int SERVICE_ERROR_NORMAL = 0x00000001;

        public void Install(string strServiceName, bool bNetworkService, string args, string dependencies)
        {
            string pServicePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            UInt32 schSCManager = OpenSCManager(null,
                                                null,
                                                SC_MANAGER_CREATE_SERVICE);
            if (schSCManager == 0)
            {
                Console.WriteLine("Error during opening service manager!\r\nCheck if user is an administrator!");
                return;
            }

            string binaryPath = (args.Length > 0 ? pServicePath + " " + args : pServicePath);

            UInt32 schService = CreateService(schSCManager,
                                              strServiceName,
                                              strServiceName,
                                              SERVICE_ALL_ACCESS,
                                              SERVICE_WIN32_OWN_PROCESS,
                                              SERVICE_AUTO_START,
                                              SERVICE_ERROR_NORMAL,
                                              binaryPath,
                                              null,
                                              0,
                                              dependencies,
                                              bNetworkService ? "NT AUTHORITY\\NetworkService" : null,
                                              null);
            if (schService == 0)
            {
                Console.WriteLine("Error during creating service: Error " + GetLastError().ToString());
                CloseServiceHandle(schSCManager);
                return;
            }

            Console.WriteLine("Service " + strServiceName + " successfully installed!");
            CloseServiceHandle(schService);
            CloseServiceHandle(schSCManager);
        }

        public void Uninstall(string strServiceName)
        {
            UInt32 schSCManager = OpenSCManager(null,
                                                null,
                                                SC_MANAGER_ALL_ACCESS);
            if (schSCManager == 0)
            {
                Console.WriteLine("Error during opening service manager!\r\nCheck if user is an administrator!");
                return;
            }

            UInt32 schService = OpenService(schSCManager,
                                            strServiceName,
                                            SERVICE_ALL_ACCESS);

            if (schService == 0)
            {
                Console.WriteLine("Error during opening service: Error " + GetLastError().ToString());
                CloseServiceHandle(schSCManager);
                return;
            }

            if (!DeleteService(schService))
                Console.WriteLine("Error uninstalling service: Error " + GetLastError().ToString());
            else
                Console.WriteLine("Service " + strServiceName + " successfully uninstalled!");

            CloseServiceHandle(schService);
            CloseServiceHandle(schSCManager);
        }
    }
}
