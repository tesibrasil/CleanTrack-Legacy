using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Threading;

namespace amrfidmgrex
{
	[ComVisible(true)]
	[Guid("B3D68A45-4035-3DA9-A455-4C0A78565166")]
	public class StepManager : DBObject
	{
		private static int DelayRefreshData = 10000;
		private static int DelayRestartStep = 5000;
		private static int DelayTimeout = 20000;
		private static Timer RestartStepTimer = null;
		private static RFIDHelper Helper = null;
		private static bool ContinueRun = false;
		private static int IdStepType = -1;
		private static int IdExamToSave = -1;

		private static List<int> ListAllowedType = null;
		private static string hostName = "";
		private static int Cleaner = -1;

		public delegate void UserDetectedHandler(string id);
		public static event UserDetectedHandler UserDetected;

		public delegate void DeviceDetectedHandler(string id);
		public static event DeviceDetectedHandler DeviceDetected;

		public delegate void BadgeDetectedHandler(string id);
		public static event BadgeDetectedHandler BadgeDetected;

		public delegate void DataCollectionCompleteHandler(int success);
		public static event DataCollectionCompleteHandler DataCollectionCompleted;

		public static bool Init(string address, string odbcConnString)
		{
			Logger.Info("StepManager.Init...");
			hostName = GetHostName();

			Operator.ODBCConnectionString = odbcConnString;
			Device.ODBCConnectionString = odbcConnString;
			ODBCConnectionString = odbcConnString;
			RFIDHelper.Address = address;
			RFIDHelper.HostName = hostName;
			Helper = RFIDHelper.Get();
			Logger.Info("...StepManager.Init");
			return true;
		}

		private static string GetHostName()
		{
			try
			{
				return System.Net.Dns.GetHostName();
			}
			catch (Exception ex)
			{
				Logger.Info(ex);
			}

			return "";
		}

		public static bool Reset()
		{
			Logger.Info("StepManager.Reset...");
			RFIDHelper.Reset();
			Helper = RFIDHelper.Get();
			Logger.Info("...StepManager.Reset");
			return true;
		}

		public static bool isRFIDHelperInitiated()
		{
			Logger.Info("StepManager.isRFIDHelperInitiated");
			return RFIDHelper.isInitiated();
		}

		public static bool isInitiated()
		{
			bool result = RFIDHelper.isInitiated() && ODBCConnectionString != null && ODBCConnectionString.Length > 0;
			Logger.Info("StepManager.isInitiated: " + result.ToString());
			return result;
		}

		public static void Finish()
		{
			Logger.Info("StepManager.Finish...");
			Helper.StopAnticollisionLoop();
			Helper.Stop();
			Logger.Info("...StepManager.Finish");
		}

		public static void RestartStep(object state)
		{
			Logger.Info("StepManager.RestartStep...");

			if (RestartStepTimer != null)
				RestartStepTimer.Dispose();

			Step step = new Step(IdStepType, hostName);
			Logger.Info(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

			step.UserBadgeReaded += step_UserBadgeReaded;
			step.DeviceBadgeReaded += step_DeviceBadgeReaded;

			step.Start(EndStepCallback,
						  DelayRefreshData,
						  DelayTimeout,
						  ListAllowedType, Cleaner, IdExamToSave);

			Logger.Info("...StepManager.RestartStep");
		}

		public static void OneStep()
		{
			Logger.Info("StepManager.OneStep...");
			Step step = new Step(-1, hostName);
			step.BadgeReaded += step_BadgeReaded;
			ContinueRun = false;
			step.StartOnce();
			Logger.Info("...StepManager.OneStep");
		}

		static void step_BadgeReaded(string id)
		{
			Logger.Info("StepManager.step_BadgeReaded...");
			BadgeDetected?.Invoke(id);
			Logger.Info("...StepManager.step_BadgeReaded");
		}

		static void step_DeviceBadgeReaded(string id)
		{
			Logger.Info("StepManager.step_DeviceBadgeReaded...");
			DeviceDetected?.Invoke(id);
			Logger.Info("...StepManager.step_DeviceBadgeReaded");
		}

		static void step_UserBadgeReaded(string id)
		{
			Logger.Info("StepManager.step_UserBadgeReaded...");
			UserDetected?.Invoke(id);
			Logger.Info("...StepManager.step_UserBadgeReaded");
		}

		private static void EndStepCallback(Types.Info info)
		{
			Logger.Info("StepManager.EndStepCallback...");
			int result = 0;
			switch (info.Result)
			{
				case (amrfidmgrex.Types.Result.Success):
					result = 1;
					break;
				case (amrfidmgrex.Types.Result.Timeout):
					result = 2;
					break;
				default:
					result = 0;
					break;
			}
			DataCollectionCompleted?.Invoke(result);
			Logger.Info("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
			if (ContinueRun)
				RestartStepTimer = new Timer(RestartStep, null, DelayRestartStep, System.Threading.Timeout.Infinite);
			Logger.Info("...StepManager.step_UserBadgeReaded");
		}

		public static bool isReaderConnected()
		{
			Logger.Info("StepManager.isReaderConnected");
			return Helper.isReaderConnected();
		}

		public static void RunLoop(int cleaner, int idStepType, List<int> listAllowedType)
		{
			Logger.Info("StepManager.RunLoop...");

			IdStepType = idStepType;
			ListAllowedType = listAllowedType;
			Cleaner = cleaner;
			ContinueRun = true;
			RestartStep(null);

			Logger.Info("...StepManager.RunLoop");
		}

		public static void ReadForExam(int idStepType, int AllowedType, int ExamId)
		{
			Logger.Info("StepManager.ReadForExam...");
			IdStepType = idStepType;
			IdExamToSave = ExamId;
			ListAllowedType = new List<int>();
			ListAllowedType.Add(AllowedType);
			Cleaner = -1;
			ContinueRun = false;
			RestartStep(null);
			Logger.Info("...StepManager.ReadForExam");
		}

		public static void Stop()
		{
			Logger.Info("StepManager.Stop");
			ContinueRun = false;
		}
	}
}
