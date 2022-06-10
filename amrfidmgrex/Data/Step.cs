using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace amrfidmgrex
{

	public class Step : DBObject
	{
        protected int Id = -1;
		protected Operator CurrentOperator = null;
		protected Device CurrentDevice = null;

		private int DelayTimeout = -1;
		private Timer RefreshDataTimer = null;
		private Timer InStepTimeoutTimer = null;
		private int Cleaner = -1;
		private int IdExamToSave = -1;

		protected Types.CompleteDelegate CompleteCallback = null;
		public static List<int> ListAllowedType
        {
            get; set;
        }

		protected string hostName = "";

		public delegate void BadgeDetectedHandler(string id);
		public event BadgeDetectedHandler BadgeReaded;

		public delegate void UserDetectedHandler(string id);
		public event UserDetectedHandler UserBadgeReaded;

		public delegate void DeviceDetectedHandler(string id);
		public event DeviceDetectedHandler DeviceBadgeReaded;

		public Step(int idStep, string hostName = "")
		{
			Logger.Info("Step.Step...");
			Id = idStep;
			this.hostName = hostName;
			Logger.Info("...Step.Step");
		}

		private void StartTimeoutTimer()
		{
			Logger.Info("Step.StartTimeoutTimer, value:" + DelayTimeout.ToString() + "ms...");

			if (InStepTimeoutTimer != null)
				InStepTimeoutTimer.Dispose();

			InStepTimeoutTimer = new Timer(TimeoutTick, this, DelayTimeout, System.Threading.Timeout.Infinite);

			Logger.Info("...Step.StartTimeoutTimer");
		}

		protected void AnticollisionCallback(string str)
		{
			Logger.Info("Step.AnticollisionCallback...");

			StartTimeoutTimer();

			if (Operator.Exist(str))
			{
				CurrentOperator = Operator.Get(str);
				Logger.Info("Found Operator: " + CurrentOperator.sMatricola);
				RFIDHelper.Get().Led(RFIDHelper.LedType.Orange);
				UserBadgeReaded(str);
			}

			if (Device.Exist(str))
			{
				CurrentDevice = Device.Get(str);
				Logger.Info("Found Device: " + CurrentDevice.Code);
				RFIDHelper.Get().Led(RFIDHelper.LedType.Orange);
				DeviceBadgeReaded(str);
			}

			if ((CurrentOperator != null) || (CurrentDevice != null))
			{
				var time = 0;
				time += (CurrentOperator != null) ? 200 : 0;
				time += (CurrentDevice != null) ? 200 : 0;
				RFIDHelper.Get().Buzzer(time);
			}

			if ((CurrentOperator != null) && (CurrentDevice != null))
				Complete();

			Logger.Info("...Step.AnticollisionCallback...");
		}


        public void DirectUpdateDB(string devTag, string opTag)
        {
            Logger.Info("Step.DirectUpdateDB...");

            Device.ODBCConnectionString = ODBCConnectionString;
            Device.Refresh();
            Operator.ODBCConnectionString = ODBCConnectionString;
            Operator.Refresh();


            if (Operator.Exist(opTag))
            {
                CurrentOperator = Operator.Get(opTag);
                Logger.Info("Found Operator: " + CurrentOperator.sMatricola);
            }

            if (Device.Exist(devTag))
            {
                CurrentDevice = Device.Get(devTag);
                Logger.Info("Found Device: " + CurrentDevice.Code);
            }

            if ((CurrentOperator != null) && 
                (CurrentDevice != null) &&
                IsStateDeviceCompatible())
                UpdateDB(devTag, opTag, -1, this.Id, this.CurrentDevice.State, -1, ODBCConnectionString);

            Logger.Info("...Step.DirectUpdateDB");
        }

        protected void AnticollisionReadOnceCallback(string str)
		{
			Logger.Info("Step.AnticollisionReadOnceCallback");

			if (BadgeReaded != null)
			{
				//BUZZER
				RFIDHelper.Get().Buzzer(100);
				System.Threading.Thread.Sleep(200);

				BadgeReaded(str);
			}

			CompleteBadge();
		}

		private static void RefreshTick(object state)
		{
			Logger.Info("Step.RefreshTick");

			string hostName = "";
			try
			{
				hostName = System.Net.Dns.GetHostName();
			}
			catch (Exception ex)
            {
                Logger.Error(ex);
            }

			string opMessage = Operator.Refresh();
			if (opMessage != "")
				Logger.Info(opMessage);

			string devMessage = Device.Refresh();
			if (devMessage != "")
				Logger.Info(devMessage);
		}

		private void Complete()
		{
			Logger.Info("Step.Complete...");

			RefreshDataTimer.Dispose();
			if (InStepTimeoutTimer != null)
			{
				InStepTimeoutTimer.Dispose();
				InStepTimeoutTimer = null;
			}

			RFIDHelper.Get().StopAnticollisionLoop();

			if (!IsStateDeviceCompatible())
			{
				CompleteError();
				return;
			}

			CompleteSuccess();
			Logger.Info("...Step.Complete");
		}

		private bool IsStateDeviceCompatible()
		{
			Logger.Info("Step.IsStateDeviceCompatible");
			var result = (ListAllowedType.FindIndex(delegate (int id) { return (id == CurrentDevice.State); }) >= 0);

            if (!result)
                Logger.Error(">> CurrentDevice State: " + CurrentDevice.State.ToString() + " !!NOTFOUND!!");
            else
                Logger.Info(">> CurrentDevice State: " + CurrentDevice.State.ToString() + " FOUND!");

            return result;
		}

		private void CompleteError()
		{
			Logger.Info("Step.CompleteError");

			// UpdateDB();
			RFIDHelper.Get().Led(RFIDHelper.LedType.Red);

			if (CompleteCallback != null)
				CompleteCallback(new Types.Info()
				{
					Result = Types.Result.Error,
					Description = "ERROR",
					IdStepType = this.Id
				});

			Logger.Info(CurrentDevice.State + " --> " + Id + " : State transition not allowed");
		}

		private void CompleteTimeoutError()
		{
			Logger.Info("Step.CompleteTimeoutError");

			RFIDHelper.Get().Led(RFIDHelper.LedType.Red);

			if (CompleteCallback != null)
				CompleteCallback(new Types.Info()
				{
					Result = Types.Result.Timeout,
					Description = "ERROR",
					IdStepType = this.Id
				});

			Logger.Info("Reading Timeout");
		}

		private void CompleteSuccess()
		{
			Logger.Info("Step.CompleteSuccess");

			UpdateDB();
			RFIDHelper.Get().Led(RFIDHelper.LedType.Green);

			if (CompleteCallback != null)
				CompleteCallback(new Types.Info()
				{
					Result = Types.Result.Success,
					Description = "OK",
					IdStepType = this.Id
				});
		}

		private void CompleteBadge()
		{
			Logger.Info("Step.CompleteBadge");

			RFIDHelper.Get().Led(RFIDHelper.LedType.Green);

			if (CompleteCallback != null)
				CompleteCallback(new Types.Info()
				{
					Result = Types.Result.Success,
					Description = "OK",
					IdStepType = this.Id
				});
		}

		private void TimeoutTick(object state)
		{
			Logger.Info("Step.TimeoutTick...");

			RFIDHelper.Get().StopAnticollisionLoop();
			Step step = (Step)state;
			step.RefreshDataTimer.Dispose();
			if (step.InStepTimeoutTimer != null)
			{
				step.InStepTimeoutTimer.Dispose();
				step.InStepTimeoutTimer = null;
			}

			step.CompleteTimeoutError();
			RFIDHelper.Get().Buzzer(600);

			Logger.Info("...Step.TimeoutTick: Timeout reading tag");
		}

		public void Start(Types.CompleteDelegate completeCallback,
								int delayRefreshData,
								int delayTimeout,
								List<int> listAllowedType, int cleaner, int IdExamToSave = -1)
		{
			Logger.Info("Step.Start...");

			this.Cleaner = cleaner;
			this.IdExamToSave = IdExamToSave;
			CompleteCallback = completeCallback;
			DelayTimeout = delayTimeout;
			ListAllowedType = listAllowedType;

			Logger.Info("AllowedTypes:");
			foreach (var item in ListAllowedType)
				Logger.Info("item: " + item);

			RefreshDataTimer = new Timer(RefreshTick, null, 0, delayRefreshData);

			bool res = RFIDHelper.Get().StartAnticollisionLoop(AnticollisionCallback);
			Logger.Info("...Step.Start");
		}

		public void StartOnce()
		{
			Logger.Info("Step.StartOnce...");
			bool res = RFIDHelper.Get().StartAnticollisionLoop(AnticollisionReadOnceCallback);
			Logger.Info("...Step.StartOnce");
		}


		protected void UpdateDB()
		{
			Logger.Info("Step.UpdateDB...");
			Thread thread = new Thread(() => UpdateDB(CurrentDevice.Tag, CurrentOperator.sTag, Cleaner, Id, CurrentDevice.State, IdExamToSave, ODBCConnectionString));
			thread.Start();
			Logger.Info("...Step.UpdateDB");
		}


		protected void UpdateDB(string deviceTag, string userTag, int cleaner, int state, int oldState, int IdExamToSave, string connectionString)
		{
			Logger.Info("Step.UpdateDBAsync...");
			Logger.Info("Updating DB: DEVICETAG:" + deviceTag + " USERTAG:" + userTag + " CLEANER:" + cleaner + " STATE:" + state + " OLDSTATE:" + oldState + " ENDOX_EXAM:" + IdExamToSave);
			bool success = DBUtilities.insertnewCycleEx(deviceTag, userTag, cleaner, state, oldState, IdExamToSave, connectionString);
			Logger.Info("Updating DB " + success);
			Logger.Info("...Step.UpdateDBAsync");
		}
	}
}
