// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.Step
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using LibLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace amrfidmgrex
{
  public class Step : DBObject
  {
    protected int Id = -1;
    private int DelayTimeout = -1;
    private int Cleaner = -1;
    private int IdExamToSave = -1;
    protected string hostName = "";
    protected Operator CurrentOperator;
    protected Device CurrentDevice;
    private System.Threading.Timer RefreshDataTimer;
    private System.Threading.Timer InStepTimeoutTimer;
    protected Types.CompleteDelegate CompleteCallback;
    protected static List<int> ListAllowedType;

    public event Step.BadgeDetectedHandler BadgeReaded;

    public event Step.UserDetectedHandler UserBadgeReaded;

    public event Step.DeviceDetectedHandler DeviceBadgeReaded;

    public Step(int idStep, string hostName)
    {
      this.Id = idStep;
      this.hostName = hostName;
    }

    private void StartTimeoutTimer()
    {
      if (this.InStepTimeoutTimer != null)
        this.InStepTimeoutTimer.Dispose();
      this.InStepTimeoutTimer = new System.Threading.Timer(new TimerCallback(this.TimeoutTick), (object) this, this.DelayTimeout, -1);
    }

    protected void AnticollisionCallback(string str)
    {
      this.StartTimeoutTimer();
      if (Operator.Exist(str))
      {
        this.CurrentOperator = Operator.Get(str);
        RFIDHelper.Get().Led(RFIDHelper.LedType.Orange, true);
        Logger.Get().Write(this.hostName, "Cleantrack.Service", "Operator detected:" + str, (byte[]) null, Logger.LogLevel.Info);
        this.UserBadgeReaded(str);
      }
      if (Device.Exist(str))
      {
        this.CurrentDevice = Device.Get(str);
        RFIDHelper.Get().Led(RFIDHelper.LedType.Orange, true);
        Logger.Get().Write(this.hostName, "Cleantrack.Service", "Device detected:" + str, (byte[]) null, Logger.LogLevel.Info);
        this.DeviceBadgeReaded(str);
      }
      if (this.CurrentOperator != null || this.CurrentDevice != null)
        RFIDHelper.Get().Buzzer(0 + (this.CurrentOperator != null ? 200 : 0) + (this.CurrentDevice != null ? 200 : 0));
      if (this.CurrentOperator == null || this.CurrentDevice == null)
        return;
      this.Complete();
    }

    protected void AnticollisionReadOnceCallback(string str)
    {
      if (this.BadgeReaded != null)
      {
        RFIDHelper.Get().Buzzer(100);
        Thread.Sleep(200);
        this.BadgeReaded(str);
      }
      this.CompleteBadge();
    }

    private static void RefreshTick(object state)
    {
      Console.WriteLine("Refreshing Data...");
      string host = "";
      try
      {
        host = Dns.GetHostName();
      }
      catch
      {
      }
      string description1 = Operator.Refresh();
      if (description1 != "")
        Logger.Get().Write(host, "Cleantrack.Service", description1, (byte[]) null, Logger.LogLevel.Info);
      string description2 = Device.Refresh();
      if (description2 != "")
        Logger.Get().Write(host, "Cleantrack.Service", description2, (byte[]) null, Logger.LogLevel.Info);
      Console.WriteLine("...Data Refreshed");
    }

    private void Complete()
    {
      this.RefreshDataTimer.Dispose();
      if (this.InStepTimeoutTimer != null)
      {
        this.InStepTimeoutTimer.Dispose();
        this.InStepTimeoutTimer = (System.Threading.Timer) null;
      }
      RFIDHelper.Get().StopAnticollisionLoop();
      if (!this.IsStateDeviceCompatible())
        this.CompleteError();
      else
        this.CompleteSuccess();
    }

    private bool IsStateDeviceCompatible()
    {
      return Step.ListAllowedType.FindIndex((Predicate<int>) (id => id == this.CurrentDevice.Stato)) >= 0;
    }

    private void CompleteError()
    {
      RFIDHelper.Get().Led(RFIDHelper.LedType.Red, true);
      if (this.CompleteCallback != null)
        this.CompleteCallback(new Types.Info()
        {
          Result = Types.Result.Error,
          Description = "ERROR",
          IdStepType = this.Id
        });
      Logger.Get().Write(this.hostName, "Cleantrack.Service", this.CurrentDevice.Stato.ToString() + " --> " + (object) this.Id + " : State transition not allowed", (byte[]) null, Logger.LogLevel.Info);
    }

    private void CompleteTimeoutError()
    {
      RFIDHelper.Get().Led(RFIDHelper.LedType.Red, true);
      if (this.CompleteCallback != null)
        this.CompleteCallback(new Types.Info()
        {
          Result = Types.Result.Timeout,
          Description = "ERROR",
          IdStepType = this.Id
        });
      Logger.Get().Write(this.hostName, "Cleantrack.Service", "Reading Timeout", (byte[]) null, Logger.LogLevel.Info);
    }

    private void CompleteSuccess()
    {
      this.UpdateDB();
      RFIDHelper.Get().Led(RFIDHelper.LedType.Green, true);
      if (this.CompleteCallback == null)
        return;
      this.CompleteCallback(new Types.Info()
      {
        Result = Types.Result.Success,
        Description = "OK",
        IdStepType = this.Id
      });
    }

    private void CompleteBadge()
    {
      RFIDHelper.Get().Led(RFIDHelper.LedType.Green, true);
      if (this.CompleteCallback == null)
        return;
      this.CompleteCallback(new Types.Info()
      {
        Result = Types.Result.Success,
        Description = "OK",
        IdStepType = this.Id
      });
    }

    private void TimeoutTick(object state)
    {
      RFIDHelper.Get().StopAnticollisionLoop();
      Step step = (Step) state;
      step.RefreshDataTimer.Dispose();
      if (step.InStepTimeoutTimer != null)
      {
        step.InStepTimeoutTimer.Dispose();
        step.InStepTimeoutTimer = (System.Threading.Timer) null;
      }
      step.CompleteTimeoutError();
      RFIDHelper.Get().Buzzer(600);
      Logger.Get().Write(this.hostName, "Cleantrack.Service", "Timeout reading tag", (byte[]) null, Logger.LogLevel.Info);
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

    public void Start(
      Types.CompleteDelegate completeCallback,
      int delayRefreshData,
      int delayTimeout,
      List<int> listBannedType,
      int cleaner,
      int IdExamToSave = -1)
    {
      this.Cleaner = cleaner;
      this.IdExamToSave = IdExamToSave;
      this.CompleteCallback = completeCallback;
      this.DelayTimeout = delayTimeout;
      Step.ListAllowedType = listBannedType;
      this.RefreshDataTimer = new System.Threading.Timer(new TimerCallback(Step.RefreshTick), (object) null, 0, delayRefreshData);
      RFIDHelper.Get().StartAnticollisionLoop(new RFIDHelper.AnticollisionLoopDataDelegate(this.AnticollisionCallback));
    }

    public void StartOnce()
    {
      RFIDHelper.Get().StartAnticollisionLoop(new RFIDHelper.AnticollisionLoopDataDelegate(this.AnticollisionReadOnceCallback));
    }

    protected void UpdateDB()
    {
      new Thread((ThreadStart) (() => this.UpdateDBAsync(this.CurrentDevice.Tag, this.CurrentOperator.Tag, this.Cleaner, this.Id, this.CurrentDevice.Stato, this.IdExamToSave, DBObject.ODBCConnectionString))).Start();
    }

    protected void UpdateDBAsync(
      string deviceTag,
      string userTag,
      int cleaner,
      int state,
      int oldState,
      int IdExamToSave,
      string connectionString)
    {
      Logger.Get().Write(this.hostName, "Cleantrack.Service", "Updating DB: DEVICETAG:" + deviceTag + " USERTAG:" + userTag + " CLEANER:" + (object) cleaner + " OLDSTATE:" + (object) oldState + " ENDOX_EXAM:" + (object) IdExamToSave, (byte[]) null, Logger.LogLevel.Info);
      Logger.Get().Write(this.hostName, "Cleantrack.Service", "Updating DB " + (object) DBUtilities.insertnewCycle(deviceTag, userTag, cleaner, state, oldState, IdExamToSave, connectionString), (byte[]) null, Logger.LogLevel.Info);
    }

    public delegate void BadgeDetectedHandler(string id);

    public delegate void UserDetectedHandler(string id);

    public delegate void DeviceDetectedHandler(string id);
  }
}
