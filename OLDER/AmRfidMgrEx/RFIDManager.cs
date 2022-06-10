using LibLog;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class RFIDManager : IRFIDManager
    {
        private string connectionDBString = "";
        private IRFIDEvents m_badge_listener;
        private IRFIDEvents m_user_listener;
        private IRFIDEvents m_device_listener;
        private IRFIDEvents m_completed_listener;

        public long testDatabaseConnection()
        {
            return DBUtilities.testConnection(this.connectionDBString) ? 1L : 0L;
        }

        public long testRFIDAddress(string address)
        {
            return new Ping().Send(address).Status == IPStatus.Success ? 1L : 0L;
        }

        public void setConnectionString(string connectionString)
        {
            this.connectionDBString = connectionString;
        }

        public void addCompletedListener(IRFIDEvents evt)
        {
            this.m_completed_listener = evt;
        }

        public void addBadgeListener(IRFIDEvents evt)
        {
            this.m_badge_listener = evt;
        }

        public void addUserListener(IRFIDEvents evt)
        {
            this.m_user_listener = evt;
        }

        public void addDeviceListener(IRFIDEvents evt)
        {
            this.m_device_listener = evt;
        }

        public void cleanUpListeners()
        {
            this.cleanInternalListenersReferences();
        }

        public void startListening(string address, int fromState, int examId)
        {
            writeLog("StartListening");
            if (StepManager.isInitiated())
            {
                writeLog("StepManagerIsInitiated");
                StepManager.Reset();
                StepManager.DeviceDetected += new StepManager.DeviceDetectedHandler(this.StepManager_DeviceDetected);
                StepManager.UserDetected += new StepManager.UserDetectedHandler(this.StepManager_UserDetected);
                StepManager.DataCollectionCompleted += new StepManager.DataCollectionCompleteHandler(this.StepManager_DataCollectionCompleted);
            }
            else if (this.connectionDBString != null)
            {
                writeLog("connectionDBString != null");
                if (this.connectionDBString != "")
                {
                    writeLog("connectionDBString != ''");
                    try
                    {
                        writeLog("GetHostName");
                        Dns.GetHostName();
                    }
                    catch
                    {
                    }
                    writeLog("PreInit != ''");
                    StepManager.Init(address, this.connectionDBString);
                    StepManager.DeviceDetected += new StepManager.DeviceDetectedHandler(this.StepManager_DeviceDetected);
                    StepManager.UserDetected += new StepManager.UserDetectedHandler(this.StepManager_UserDetected);
                    StepManager.DataCollectionCompleted += new StepManager.DataCollectionCompleteHandler(this.StepManager_DataCollectionCompleted);
                }
            }

            writeLog("SetLogger");

            writeLog("StepManager.ReadForExam");
            StepManager.ReadForExam(2, fromState, examId);
        }

        public void readData(string address)
        {
            try
            {
                Dns.GetHostName();
            }
            catch
            {
            }
            if (!StepManager.isRFIDHelperInitiated())
            {
                StepManager.Init(address, this.connectionDBString);
                StepManager.BadgeDetected += new StepManager.BadgeDetectedHandler(this.StepManager_BadgeDetected);
            }
            else
            {
                StepManager.Reset();
                StepManager.BadgeDetected += new StepManager.BadgeDetectedHandler(this.StepManager_BadgeDetected);
            }
            StepManager.OneStep();
        }

        private void StepManager_BadgeDetected(string id)
        {
            this.stopListening();
            if (this.m_badge_listener == null)
                return;
            this.m_badge_listener.BadgeDetected(id);
        }

        public string getDeviceDesc(int id)
        {
            return DBUtilities.getDeviceDescFromId(id, this.connectionDBString);
        }

        public string getUserName(int id)
        {
            return DBUtilities.getOperatorNameFromId(id, this.connectionDBString);
        }

        public string getUserSurname(int id)
        {
            return DBUtilities.getOperatorSurnameFromId(id, this.connectionDBString);
        }

        public long getUserIdFromMat(string mat)
        {
            return (long)DBUtilities.getOperatoreIdFromMat(mat, this.connectionDBString);
        }

        public long getDeviceIdFromMat(string mat)
        {
            return (long)DBUtilities.getDeviceIdFromMat(mat, this.connectionDBString);
        }

        public long checkUserValidity(string op)
        {
            return DBUtilities.checkUser(op, this.connectionDBString) ? 1L : 0L;
        }

        public long checkAndInsertManualCycle(int stateToMatch, int op, int device, int ExamId)
        {
            if (this.connectionDBString == null || !(this.connectionDBString != ""))
                return 0;
            int state = DBUtilities.getState(device, this.connectionDBString);
            return (stateToMatch < 0 || state == stateToMatch) && DBUtilities.insertnewCycle(DBUtilities.getDeviceTagFromId(device, this.connectionDBString), DBUtilities.getOperatorTagFromId(op, this.connectionDBString), -1, 2, stateToMatch, ExamId, this.connectionDBString) ? 1L : 0L;
        }

        public RFIDCiclo GetCycleData(int iIDEsame, int iPrevious)
        {
            return DBUtilities.GetCycleData(iIDEsame, this.connectionDBString, iPrevious > 0);
        }

        public RFIDCiclo GetCycleDataFromEsameDispositivo(int iIDEsame, int iIDDispositivo, int iPrevious)
        {
            return DBUtilities.GetCycleDataFromEsameDispositivo(iIDEsame, iIDDispositivo, this.connectionDBString, iPrevious > 0);
        }

        public string getCycleAdditionalInfo(int examId, long previous)
        {
            return DBUtilities.getCycleAdditionalInfo(examId, this.connectionDBString, previous > 0L);
        }

        public string getSeparator()
        {
            return "$3P4R470R";
        }

        public void stopListening()
        {
            StepManager.Finish();
        }

        private void StepManager_DataCollectionCompleted(int success)
        {
            if (this.connectionDBString == null || !(this.connectionDBString != "") || this.m_completed_listener == null)
                return;
            this.m_completed_listener.Completed((long)success);
        }

        private void StepManager_UserDetected(string id)
        {
            if (this.connectionDBString == null || !(this.connectionDBString != "") || this.m_user_listener == null)
                return;
            this.m_user_listener.UserDetected(DBUtilities.getOperatorNameFromTag(id, this.connectionDBString), DBUtilities.getOperatorSurnameFromTag(id, this.connectionDBString), (long)DBUtilities.getOperatoreIdFromTag(id, this.connectionDBString));
        }

        private void StepManager_DeviceDetected(string id)
        {
            if (this.connectionDBString == null || !(this.connectionDBString != "") || this.m_device_listener == null)
                return;
            this.m_device_listener.DeviceDetected(DBUtilities.getDeviceDescFromTag(id, this.connectionDBString), (long)DBUtilities.getDispIdFromTag(id, this.connectionDBString));
        }

        public static void writeLog(string hostname, string origin, string text, byte[] attach, Logger.LogLevel lev)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter("C:\\TESILOG\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true);
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
                streamWriter.Close();
            }
            catch
            {
            }
        }

        public static void writeLog(string text)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter("C:\\TESILOG\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true);
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
                streamWriter.Close();
            }
            catch
            {
            }
        }
        private void cleanInternalListenersReferences()
        {
            StepManager.BadgeDetected -= new StepManager.BadgeDetectedHandler(this.StepManager_BadgeDetected);
            StepManager.DataCollectionCompleted -= new StepManager.DataCollectionCompleteHandler(this.StepManager_DataCollectionCompleted);
            StepManager.DeviceDetected -= new StepManager.DeviceDetectedHandler(this.StepManager_DeviceDetected);
            StepManager.UserDetected -= new StepManager.UserDetectedHandler(this.StepManager_UserDetected);
        }
    }
}
