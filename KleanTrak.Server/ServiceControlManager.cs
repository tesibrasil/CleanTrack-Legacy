using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Permissions;

namespace KleanTrak.Server
{
    #region Win32 API Declarations

    [Flags]
    enum ServiceControlAccessRights : uint
    {
        SC_MANAGER_CONNECT = 0x0001, // Required to connect to the service control manager. 
        SC_MANAGER_CREATE_SERVICE = 0x0002, // Required to call the CreateService function to create a service object and add it to the database. 
        SC_MANAGER_ENUMERATE_SERVICE = 0x0004, // Required to call the EnumServicesStatusEx function to list the services that are in the database. 
        SC_MANAGER_LOCK = 0x0008, // Required to call the LockServiceDatabase function to acquire a lock on the database. 
        SC_MANAGER_QUERY_LOCK_STATUS = 0x0010, // Required to call the QueryServiceLockStatus function to retrieve the lock status information for the database.
        SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020, // Required to call the NotifyBootConfigStatus function. 
        SC_MANAGER_ALL_ACCESS = 0xF003F // Includes STANDARD_RIGHTS_REQUIRED, in addition to all access rights in this table. 
    }

    [Flags]
    enum ServiceAccessRights : uint
    {
        SERVICE_QUERY_CONFIG = 0x0001, // Required to call the QueryServiceConfig and QueryServiceConfig2 functions to query the service configuration. 
        SERVICE_CHANGE_CONFIG = 0x0002, // Required to call the ChangeServiceConfig or ChangeServiceConfig2 function to change the service configuration. Because this grants the caller the right to change the executable file that the system runs, it should be granted only to administrators. 
        SERVICE_QUERY_STATUS = 0x0004, // Required to call the QueryServiceStatusEx function to ask the service control manager about the status of the service. 
        SERVICE_ENUMERATE_DEPENDENTS = 0x0008, // Required to call the EnumDependentServices function to enumerate all the services dependent on the service. 
        SERVICE_START = 0x0010, // Required to call the StartService function to start the service. 
        SERVICE_STOP = 0x0020, // Required to call the ControlService function to stop the service. 
        SERVICE_PAUSE_CONTINUE = 0x0040, // Required to call the ControlService function to pause or continue the service. 
        SERVICE_INTERROGATE = 0x0080, // Required to call the ControlService function to ask the service to report its status immediately. 
        SERVICE_USER_DEFINED_CONTROL = 0x0100, // Required to call the ControlService function to specify a user-defined control code.
        SERVICE_ALL_ACCESS = 0xF01FF // Includes STANDARD_RIGHTS_REQUIRED in addition to all access rights in this table. 
    }

    enum ServiceConfig2InfoLevel : uint
    {
        SERVICE_CONFIG_DESCRIPTION = 0x00000001, // The lpBuffer parameter is a pointer to a SERVICE_DESCRIPTION structure.
        SERVICE_CONFIG_FAILURE_ACTIONS = 0x00000002 // The lpBuffer parameter is a pointer to a SERVICE_FAILURE_ACTIONS structure.
    }

    enum SC_ACTION_TYPE : uint
    {
        SC_ACTION_NONE = 0x00000000, // No action.
        SC_ACTION_RESTART = 0x00000001, // Restart the service.
        SC_ACTION_REBOOT = 0x00000002, // Reboot the computer.
        SC_ACTION_RUN_COMMAND = 0x00000003 // Run a command.
    }

    //struct SERVICE_FAILURE_ACTIONS
    //{
    //    [MarshalAs(UnmanagedType.U4)]
    //    public UInt32 dwResetPeriod;
    //    // [MarshalAs(UnmanagedType.LPStr)]
    //    // public String lpRebootMsg;
    //    // [MarshalAs(UnmanagedType.LPStr)]
    //    // public String lpCommand;
    //    [MarshalAs(UnmanagedType.U4)]
    //    public UInt32 cActions;
    //    public IntPtr lpsaActions;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct SERVICE_FAILURE_ACTIONS
    {
        public int dwResetPeriod;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpRebootMsg;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpCommand;
        public int cActions;
        public IntPtr lpsaActions;
    }

    struct SC_ACTION
    {
        [MarshalAs(UnmanagedType.U4)]
        public SC_ACTION_TYPE Type;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 Delay;
    }

    #endregion

    #region Native Methods

    class NativeMethods
    {
        private NativeMethods() { }

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManager")]
        public static extern IntPtr OpenSCManager(
            string machineName,
            string databaseName,
            ServiceControlAccessRights desiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle")]
        public static extern int CloseServiceHandle(IntPtr hSCObject);

        [DllImport("advapi32.dll", EntryPoint = "OpenService")]
        public static extern IntPtr OpenService(
            IntPtr hSCManager,
            string serviceName,
            ServiceAccessRights desiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "QueryServiceConfig2")]
        public static extern int QueryServiceConfig2(
            IntPtr hService,
            ServiceConfig2InfoLevel dwInfoLevel,
            IntPtr lpBuffer,
            uint cbBufSize,
            out uint pcbBytesNeeded);

        [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeServiceConfig2(
            IntPtr hService,
            ServiceConfig2InfoLevel dwInfoLevel,
            IntPtr lpInfo);
    }

    #endregion

    public class ServiceControlManager : IDisposable
    {
        private IntPtr SCManager;
        private bool disposed;

        /// <summary>
        /// Calls the Win32 OpenService function and performs error checking.
        /// </summary>
        /// <exception cref="ComponentModel.Win32Exception">"Unable to open the requested Service."</exception>
        private IntPtr OpenService(string serviceName, ServiceAccessRights desiredAccess)
        {
            // Open the service
            IntPtr service = NativeMethods.OpenService(
                SCManager,
                serviceName,
                desiredAccess);

            // Verify if the service is opened
            if (service == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to open the requested Service.");
            }

            return service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceControlManager"/> class.
        /// </summary>
        /// <exception cref="ComponentModel.Win32Exception">"Unable to open Service Control Manager."</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public ServiceControlManager()
        {
            // Open the service control manager
            SCManager = NativeMethods.OpenSCManager(
                null,
                null,
                ServiceControlAccessRights.SC_MANAGER_CONNECT);

            // Verify if the SC is opened
            if (SCManager == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to open Service Control Manager.");
            }
        }

        /// <summary>
        /// Dertermines whether the nominated service is set to restart on failure.
        /// </summary>
        /// <exception cref="ComponentModel.Win32Exception">"Unable to query the Service configuration."</exception>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public bool HasRestartOnFailure(string serviceName)
        {
            // 8KB is the largest buffer supported by QueryServiceConfig2
            const int bufferSize = 1024 * 8;

            IntPtr service = IntPtr.Zero;
            IntPtr bufferPtr = IntPtr.Zero;
            bool hasRestart = false;

            try
            {
                // Open the service
                service = OpenService(serviceName, ServiceAccessRights.SERVICE_QUERY_CONFIG);

                uint dwBytesNeeded = 0;

                // Allocate memory for struct
                bufferPtr = Marshal.AllocHGlobal(bufferSize);
                int queryResult = NativeMethods.QueryServiceConfig2(
                    service,
                    ServiceConfig2InfoLevel.SERVICE_CONFIG_FAILURE_ACTIONS,
                    bufferPtr,
                    bufferSize,
                    out dwBytesNeeded);

                if (queryResult == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to query the Service configuration.");
                }

                // Cast the buffer to a QUERY_SERVICE_CONFIG struct
                SERVICE_FAILURE_ACTIONS config =
                    (SERVICE_FAILURE_ACTIONS)Marshal.PtrToStructure(bufferPtr, typeof(SERVICE_FAILURE_ACTIONS));

                // Determine whether the service is set to auto restart
                if (config.cActions != 0)
                {
                    SC_ACTION action = (SC_ACTION)Marshal.PtrToStructure(config.lpsaActions, typeof(SC_ACTION));
                    hasRestart = (action.Type == SC_ACTION_TYPE.SC_ACTION_RESTART);
                }

                return hasRestart;
            }
            finally
            {
                // Clean up
                if (bufferPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bufferPtr);
                }

                if (service != IntPtr.Zero)
                {
                    NativeMethods.CloseServiceHandle(service);
                }
            }
        }

        /// <summary>
        /// Sets the nominated service to restart on failure.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void SetRestartOnFailure(string serviceName)
        {
            // Set the number of actions needed
            const int actionCount = 2;
            const uint delay = 0;

            IntPtr service = IntPtr.Zero;
            IntPtr failureActionsPtr = IntPtr.Zero;
            IntPtr actionPtr = IntPtr.Zero;

            try
            {
                // Open the service
                service = OpenService(serviceName,
                    ServiceAccessRights.SERVICE_CHANGE_CONFIG |
                    ServiceAccessRights.SERVICE_START);

                // Allocate memory for the individual actions
                actionPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SC_ACTION)) * actionCount);

                // Set up the restart action
                SC_ACTION action1 = new SC_ACTION();
                action1.Type = SC_ACTION_TYPE.SC_ACTION_RESTART;
                action1.Delay = delay;
                Marshal.StructureToPtr(action1, actionPtr, false);

                // Set up the "do nothing" action
                SC_ACTION action2 = new SC_ACTION();
                action2.Type = SC_ACTION_TYPE.SC_ACTION_RESTART;
                action2.Delay = delay;
                Marshal.StructureToPtr(action2, (IntPtr)((Int64)actionPtr + Marshal.SizeOf(typeof(SC_ACTION))), false);

                // Set up the failure actions
                SERVICE_FAILURE_ACTIONS failureActions = new SERVICE_FAILURE_ACTIONS();
                failureActions.dwResetPeriod = 0;
                failureActions.cActions = actionCount;
                failureActions.lpsaActions = actionPtr;

                failureActionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SERVICE_FAILURE_ACTIONS)));
                Marshal.StructureToPtr(failureActions, failureActionsPtr, false);

                // Make the change
                bool changeResult = NativeMethods.ChangeServiceConfig2(service, ServiceConfig2InfoLevel.SERVICE_CONFIG_FAILURE_ACTIONS, failureActionsPtr);

                // Check that the change occurred
                if (!changeResult)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to change the Service configuration.");
                }
            }
            finally
            {
                // Clean up
                if (failureActionsPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(failureActionsPtr);
                }

                if (actionPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(actionPtr);
                }

                if (service != IntPtr.Zero)
                {
                    NativeMethods.CloseServiceHandle(service);
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements the Dispose(bool) pattern outlined by MSDN and enforced by FxCop.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here
                }

                // Unmanaged resources always need disposing
                if (SCManager != IntPtr.Zero)
                {
                    NativeMethods.CloseServiceHandle(SCManager);
                    SCManager = IntPtr.Zero;
                }
            }

            disposed = true;
        }

        /// <summary>
        /// Finalizer for the <see cref="ServiceControlManager"/> class.
        /// </summary>
        ~ServiceControlManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
