using LibLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace amrfidmgrex
{
    public class StepManager : DBObject
    {
        private static int DelayRefreshData = 10000;
        private static int DelayRestartStep = 5000;
        private static int DelayTimeout = 20000;
        private static System.Threading.Timer RestartStepTimer = (System.Threading.Timer)null;
        private static RFIDHelper Helper = (RFIDHelper)null;
        private static bool ContinueRun = false;
        private static int IdStepType = -1;
        private static int IdExamToSave = -1;
        private static List<int> ListAllowedType = (List<int>)null;
        private static string hostName = "";
        private static int Cleaner = -1;

        public static event StepManager.UserDetectedHandler UserDetected;

        public static event StepManager.DeviceDetectedHandler DeviceDetected;

        public static event StepManager.BadgeDetectedHandler BadgeDetected;

        public static event StepManager.DataCollectionCompleteHandler DataCollectionCompleted;

        public static bool Init(string address, string odbcConnString)
        {
            RFIDManager.writeLog("init");
            try
            {
                StepManager.hostName = Dns.GetHostName();
                DBObject.ODBCConnectionString = odbcConnString;
                RFIDHelper.Address = address;
                RFIDHelper.HostName = StepManager.hostName;
                StepManager.Helper = RFIDHelper.Get();
                return true;
            }
            catch (Exception exc)
            {
                RFIDManager.writeLog(exc.Message);
                return false;
            }
        }

        public static bool Reset()
        {
            RFIDManager.writeLog("reset");
            RFIDHelper.Reset();
            StepManager.Helper = RFIDHelper.Get();
            return true;
        }

        public static bool isRFIDHelperInitiated()
        {
            RFIDManager.writeLog("isrfidhelperinitiated");
            return RFIDHelper.isInitiated();
        }

        public static bool isInitiated()
        {
            RFIDManager.writeLog("isinitiated");
            if (RFIDHelper.isInitiated() && DBObject.ODBCConnectionString != null)
                return DBObject.ODBCConnectionString.Length > 0;
            return false;
        }

        public static void Finish()
        {
            RFIDManager.writeLog("finish");
            StepManager.Helper.StopAnticollisionLoop();
            StepManager.Helper.Stop();
        }

        public static void RestartStep(object state)
        {
            RFIDManager.writeLog("restartstep");
            RFIDManager.writeLog(StepManager.hostName, "Cleantrack.Service", "Start new Step", (byte[])null, Logger.LogLevel.Info);
            if (StepManager.RestartStepTimer != null)
                StepManager.RestartStepTimer.Dispose();
            Step step = new Step(StepManager.IdStepType, StepManager.hostName);
            RFIDManager.writeLog(StepManager.hostName, "Cleantrack.Service", ">>>>>", (byte[])null, Logger.LogLevel.Info);
            step.UserBadgeReaded += new Step.UserDetectedHandler(StepManager.step_UserBadgeReaded);
            step.DeviceBadgeReaded += new Step.DeviceDetectedHandler(StepManager.step_DeviceBadgeReaded);
            step.Start(new Types.CompleteDelegate(StepManager.EndStepCallback), StepManager.DelayRefreshData, StepManager.DelayTimeout, StepManager.ListAllowedType, StepManager.Cleaner, StepManager.IdExamToSave);
        }

        public static void OneStep()
        {
            RFIDManager.writeLog("onestep");
            Step step = new Step(-1, StepManager.hostName);
            step.BadgeReaded += new Step.BadgeDetectedHandler(StepManager.step_BadgeReaded);
            StepManager.ContinueRun = false;
            step.StartOnce();
        }

        private static void step_BadgeReaded(string id)
        {
            RFIDManager.writeLog("badgeread");
            if (StepManager.BadgeDetected == null)
                return;
            StepManager.BadgeDetected(id);
        }

        private static void step_DeviceBadgeReaded(string id)
        {
            RFIDManager.writeLog("devicebadgeread");
            if (StepManager.DeviceDetected == null)
                return;
            StepManager.DeviceDetected(id);
        }

        private static void step_UserBadgeReaded(string id)
        {
            RFIDManager.writeLog("userbadgeread");
            if (StepManager.UserDetected == null)
                return;
            StepManager.UserDetected(id);
        }

        private static void EndStepCallback(Types.Info info)
        {
            RFIDManager.writeLog("endstepcallback");
            int success;
            switch (info.Result)
            {
                case Types.Result.Success:
                    success = 1;
                    break;
                case Types.Result.Timeout:
                    success = 2;
                    break;
                default:
                    success = 0;
                    break;
            }
            if (StepManager.DataCollectionCompleted != null)
                StepManager.DataCollectionCompleted(success);
            RFIDManager.writeLog(StepManager.hostName, "Cleantrack.Service", "<<<<<", (byte[])null, Logger.LogLevel.Info);
            if (!StepManager.ContinueRun)
                return;
            StepManager.RestartStepTimer = new System.Threading.Timer(new TimerCallback(StepManager.RestartStep), (object)null, StepManager.DelayRestartStep, -1);
        }

        private static void writeLog(string text)
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

        public static bool isReaderConnected()
        {
            RFIDManager.writeLog("isreaderconnected");
            return StepManager.Helper.isReaderConnected();
        }

        public static void RunLoop(int cleaner, int idStepType, List<int> listAllowedType)
        {
            RFIDManager.writeLog("runloop");
            StepManager.IdStepType = idStepType;
            StepManager.ListAllowedType = listAllowedType;
            StepManager.Cleaner = cleaner;
            StepManager.ContinueRun = true;
            StepManager.RestartStep((object)null);
        }

        public static void ReadForExam(int idStepType, int AllowedType, int ExamId)
        {
            RFIDManager.writeLog("read4exam");
            StepManager.IdStepType = idStepType;
            StepManager.IdExamToSave = ExamId;
            StepManager.ListAllowedType = new List<int>();
            StepManager.ListAllowedType.Add(AllowedType);
            StepManager.Cleaner = -1;
            StepManager.ContinueRun = false;
            StepManager.RestartStep((object)null);
        }

        public static void Stop()
        {
            StepManager.ContinueRun = false;
        }

        public delegate void UserDetectedHandler(string id);

        public delegate void DeviceDetectedHandler(string id);

        public delegate void BadgeDetectedHandler(string id);

        public delegate void DataCollectionCompleteHandler(int success);
    }
}
