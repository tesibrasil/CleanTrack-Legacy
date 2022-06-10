// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.RfidReader
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace It.IDnova.Fxw
{
  public class RfidReader : AbstractRfidReader
  {
    public const string DEFAULT_READER_LOG_FILENAME = "readerLog.txt";
    private const int _DOWNLOAD_LOG_TIMER_INTERVAL = 2300;
    private const int _DOWNLOAD_LOG_NRETRY = 13;
    private const string _REGISTRY_COMPORT_KEY = "HARDWARE\\DEVICEMAP\\SERIALCOMM";
    private const string _REGISTRY_VCOMPORT_ID = "VCP";
    private const int _TIMEFRAME_ARRAY_LEN = 4;
    private int _downloadLogRetryCounter;
    private RfidReader.DownloadLogStatus _downloadLogStatus;
    private int _downloadedLogRecords;
    private TextWriter _readerLogWriter;
    private string _logFilename;
    private System.Timers.Timer _keybEmuDebouncingTimer;
    private System.Timers.Timer _downloadLogTimer;
    private static DateTime _lastReadTime;
    private RfidDefs.PollingType _pollingType;
    private bool _automaticLogDownload;

    public RfidReader(FxwPhysicalType theRdrType)
      : base(theRdrType)
    {
      this._automaticLogDownload = false;
      this._downloadLogTimer = new System.Timers.Timer();
      this._downloadLogTimer.Interval = 1313.0;
      this._downloadLogTimer.Elapsed += new ElapsedEventHandler(this.onDownloadLogTimeout);
      this._keybEmuDebouncingTimer = new System.Timers.Timer();
      this._keybEmuDebouncingTimer.Interval = 2000.0;
      this._keybEmuDebouncingTimer.Elapsed += new ElapsedEventHandler(this.onDebouncingEvent);
      RfidReader._lastReadTime = DateTime.Now.Subtract(AbstractRfidReader._KEYB_EMU_WAIT_TIME_MS);
    }

    public static string[] autoDetect(FxwPhysicalType theRdrType, bool notUseRegistry)
    {
      List<string> stringList = new List<string>();
      SerialPort serialPort = (SerialPort) null;
      if (theRdrType != FxwPhysicalType.USB)
        return (string[]) null;
      string[] strArray = (string[]) null;
      if (!notUseRegistry)
      {
        AbstractRfidReader._logger.debug("[RfidReader] Autodetecting using registry...");
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM");
        string[] valueNames = registryKey.GetValueNames();
        int length = 0;
        foreach (string str in valueNames)
        {
          if (str.Contains("VCP"))
            ++length;
        }
        if (length > 0)
        {
          strArray = new string[length];
          int num = 0;
          foreach (string name in valueNames)
          {
            if (name.Contains("VCP"))
            {
              string str = (string) registryKey.GetValue(name);
              strArray[num++] = str;
            }
          }
        }
      }
      else
      {
        AbstractRfidReader._logger.debug("[RfidReader] Autodetecting WITHOUT using registry...");
        strArray = SerialPort.GetPortNames();
      }
      if (strArray == null || strArray.Length == 0)
      {
        AbstractRfidReader._logger.debug("[RfidReader] No ports detected");
        return (string[]) null;
      }
      string str1 = "";
      foreach (string str2 in strArray)
        str1 = str1 + str2 + " ";
      AbstractRfidReader._logger.debug("[RfidReader] Pre-detection result: " + str1);
      byte[] buffer1 = new byte[6]
      {
        (byte) 42,
        (byte) 3,
        (byte) 79,
        (byte) 0,
        (byte) 14,
        (byte) 0
      };
      byte[] buffer2 = new byte[8];
      for (int index = 2; index < 5; ++index)
        buffer1[5] += buffer1[index];
      foreach (string portName in strArray)
      {
        try
        {
          serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
          serialPort.DtrEnable = false;
          if (serialPort != null)
          {
            AbstractRfidReader._logger.debug("[RfidReader] Trying " + portName + "...");
            serialPort.Open();
            if (serialPort != null && serialPort.IsOpen)
            {
              for (int index = 0; index < 8; ++index)
                buffer2[index] = byte.MaxValue;
              serialPort.Write(buffer1, 0, 6);
              serialPort.ReadTimeout = 1000;
              int num1 = 0;
              while (serialPort.BytesToRead < 1 && ++num1 < 313)
                Thread.Sleep(3);
              if (num1 >= 313)
                throw new Exception();
              serialPort.Read(buffer2, 0, 8);
              byte num2 = 0;
              for (int index = 2; index < 2 + (int) buffer2[1]; ++index)
                num2 += buffer2[index];
              if (buffer2[7] != byte.MaxValue && (int) num2 == (int) buffer2[2 + (int) buffer2[1]])
              {
                stringList.Add(portName);
                AbstractRfidReader._logger.debug("[RfidReader] Device detected on " + portName);
              }
              serialPort.Close();
            }
            serialPort = (SerialPort) null;
          }
        }
        catch
        {
          AbstractRfidReader._logger.debug("[RfidReader] Device NOT detected on " + portName);
        }
      }
      if (serialPort != null && serialPort.IsOpen)
        serialPort.Close();
      if (stringList.Count <= 0)
        return (string[]) null;
      return stringList.ToArray();
    }

    public override byte disconnect()
    {
      if (!this.isValid())
        return 1;
      this.closeLog(RfidReader.DownloadLogStatus.TIMED_OUT);
      return base.disconnect();
    }

    public byte setPollingParams(RfidDefs.PollingType theType, int interval_ms)
    {
      if (!this.isValid())
        return 1;
      this._pollingType = theType;
      return this._readerWorker.setPollingParams(this._pollingType, interval_ms);
    }

    public RfidDefs.PollingType getPollingType()
    {
      if (!this.isValid())
        return RfidDefs.PollingType.UNKNOWN;
      return this._pollingType;
    }

    public double getPollingInterval()
    {
      if (!this.isValid())
        return 0.0;
      return this._readerWorker.getPollingInterval();
    }

    public override byte inventory(AbstractRfidReader.InventoryMode invMode)
    {
      if (!this.isValid())
        return 1;
      byte aCmdId;
      switch (invMode)
      {
        case AbstractRfidReader.InventoryMode.START_LOOP:
          aCmdId = (byte) 2;
          this._isInventorying = true;
          break;
        case AbstractRfidReader.InventoryMode.STOP_LOOP:
          aCmdId = (byte) 3;
          this._isInventorying = false;
          break;
        default:
          aCmdId = (byte) 77;
          break;
      }
      RfidMsg rfidMsg = new RfidMsg(aCmdId, (byte) this._pollingType, (byte[]) null, (byte) 0);
      if (aCmdId == (byte) 77)
        this._readerWorker.rawSend(rfidMsg);
      else
        this._readerWorker.send(rfidMsg);
      return 0;
    }

    public override byte inventoryTID(AbstractRfidReader.InventoryMode invMode)
    {
      if (!this.isValid())
        return 1;
      byte aCmdId;
      switch (invMode)
      {
        case AbstractRfidReader.InventoryMode.START_LOOP:
          aCmdId = (byte) 4;
          this._isInventorying = true;
          break;
        case AbstractRfidReader.InventoryMode.STOP_LOOP:
          aCmdId = (byte) 5;
          this._isInventorying = false;
          break;
        default:
          aCmdId = (byte) 77;
          break;
      }
      RfidMsg rfidMsg = new RfidMsg(aCmdId, (byte) (this._pollingType | RfidDefs.PollingType.UNIQUE_125KHZ), (byte[]) null, (byte) 0);
      if (aCmdId == (byte) 77)
        this._readerWorker.rawSend(rfidMsg);
      else
        this._readerWorker.send(rfidMsg);
      return 0;
    }

    protected override void onWorkerReceivedData(RfidMsg rcvMsg)
    {
      if (rcvMsg.CommandIdentifier == (byte) 96)
      {
        if (this._automaticLogDownload)
        {
          this.resetDownloadTimer();
          if (rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_EMPTY)
          {
            this.closeLog(RfidReader.DownloadLogStatus.SUCCEDED);
            AbstractRfidReader._logger.debug("Log downloading terminated: " + (object) this._downloadedLogRecords + " records got");
          }
          else if (rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_MEM_WRITE || rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_MEM_READ)
          {
            this.closeLog(RfidReader.DownloadLogStatus.ERROR);
            AbstractRfidReader._logger.debug("Log downloading error");
          }
          else if (this._readerLogWriter != null && rcvMsg.getPayloadLen() > 0)
          {
            AbstractRfidReader._logger.debug("Log record succesfully extracted !");
            this._readerLogWriter.WriteLine(rcvMsg.getPayloadHexString(false));
            ++this._downloadedLogRecords;
            int num = (int) this.deleteLog();
          }
          else
          {
            int num1 = (int) this.readLog();
          }
        }
        base.onWorkerReceivedData(rcvMsg);
      }
      else
        base.onWorkerReceivedData(rcvMsg);
    }

    public override int getKeyboardEmulationDebouncing()
    {
      return (int) this._keybEmuDebouncingTimer.Interval;
    }

    public override void setKeyboardEmulationDebouncing(int interval_ms)
    {
      if (interval_ms != 0)
      {
        AbstractRfidReader._DEBOUNCING_ENABLED = true;
        this._keybEmuDebouncingTimer.Interval = (double) interval_ms;
      }
      else
        AbstractRfidReader._DEBOUNCING_ENABLED = false;
    }

    protected override void emulateKeyboard(string data)
    {
      AbstractRfidReader._logger.debug("Emulating keyboard with data: '" + data + "'");
      if (!AbstractRfidReader._DEBOUNCING_ENABLED)
      {
        if (data.Equals(this._lastTagIdRead) && DateTime.Now - RfidReader._lastReadTime < AbstractRfidReader._KEYB_EMU_WAIT_TIME_MS)
        {
          RfidReader._lastReadTime = DateTime.Now;
          return;
        }
      }
      else
      {
        if (data.Equals(this._lastTagIdRead))
          return;
        this._keybEmuDebouncingTimer.Stop();
      }
      RfidReader._lastReadTime = DateTime.Now;
      this._lastTagIdRead = data;
      if (this._keyEmuAddTimestamp)
        data = DateTime.Now.ToString("s").Replace("T", " ") + " " + data;
      foreach (char ch in data)
      {
        this._keybEmulator.press((int) ch);
        this._keybEmulator.release((int) ch);
      }
      if (this._keybEmuMode == FxwKbd.KeybEmuMode.DATA_CRLF)
      {
        this._keybEmulator.press(13);
        this._keybEmulator.release(13);
        this._keybEmulator.press(10);
        this._keybEmulator.release(10);
      }
      if (!AbstractRfidReader._DEBOUNCING_ENABLED)
        return;
      this._keybEmuDebouncingTimer.Start();
    }

    private void onDownloadLogTimeout(object source, ElapsedEventArgs e)
    {
      this._downloadLogTimer.Stop();
      if (this._downloadLogRetryCounter < 13)
      {
        ++this._downloadLogRetryCounter;
        int num = (int) this.downloadLog(this._logFilename);
        this._downloadLogTimer.Start();
      }
      else
        this.closeLog(RfidReader.DownloadLogStatus.TIMED_OUT);
    }

    private void resetDownloadTimer()
    {
      this._downloadLogTimer.Stop();
      this._downloadLogRetryCounter = 0;
    }

    public void setLogFilename(string logFilename)
    {
      if (logFilename == null || logFilename.Length <= 0)
        return;
      this._logFilename = logFilename;
    }

    public string getLogFilename()
    {
      return this._logFilename;
    }

    public byte downloadLog(string filename)
    {
      this._automaticLogDownload = true;
      if (!this.isValid())
        return 1;
      this._logFilename = filename == null || filename.Length < 5 || !filename.Contains(".log") ? "readerLog.txt" : filename;
      if (this._readerLogWriter == null && this._downloadLogStatus != RfidReader.DownloadLogStatus.BUSY)
      {
        AbstractRfidReader._logger.debug("Downloading log to file" + this._logFilename + "...");
        this._readerLogWriter = (TextWriter) new StreamWriter(this._logFilename, true);
        this._downloadLogStatus = RfidReader.DownloadLogStatus.BUSY;
        this._downloadedLogRecords = 0;
      }
      return this.readLog();
    }

    public RfidReader.DownloadLogStatus getLogStatus()
    {
      return this._downloadLogStatus;
    }

    public byte clearLog()
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 96, (byte) 0, new byte[1]
      {
        (byte) 3
      }, (byte) 1));
      return 0;
    }

    private void closeLog(RfidReader.DownloadLogStatus closingStatus)
    {
      if (this._readerLogWriter == null)
        return;
      this._downloadLogStatus = closingStatus;
      this._readerLogWriter.Flush();
      this._readerLogWriter.Close();
      this._readerLogWriter = (TextWriter) null;
    }

    public byte setTempThreshold(byte tempThresholdC)
    {
      if (tempThresholdC < (byte) 0)
        return 2;
      ushort num = (ushort) ((double) tempThresholdC / (1.0 / 32.0));
      return this.setParam(RfidDefs.FxwParam.ALR245_TEMP_THRESHOLD, new byte[2]
      {
        (byte) (((int) num & 65280) >> 8),
        (byte) ((uint) num & (uint) byte.MaxValue)
      });
    }

    public byte getTempThreshold()
    {
      return this.getParam(RfidDefs.FxwParam.ALR245_TEMP_THRESHOLD);
    }

    public byte setCategory(byte categoryId, byte categoryDay, byte[] inTime, byte[] outTime)
    {
      if (!this.isValid())
        return 1;
      if (inTime == null || inTime.Length != 4 || (outTime == null || outTime.Length != 4) || (categoryId < (byte) 0 || categoryId > (byte) 30 || (categoryDay < (byte) 0 || categoryDay > (byte) 7)))
        return 2;
      foreach (byte num in inTime)
      {
        if (num < (byte) 0 || num > (byte) 192)
          return 2;
      }
      foreach (byte num in outTime)
      {
        if (num < (byte) 0 || num > (byte) 192)
          return 2;
      }
      byte[] aPayload = new byte[11];
      int num1 = 0;
      byte[] numArray1 = aPayload;
      int index1 = num1;
      int num2 = index1 + 1;
      numArray1[index1] = (byte) 1;
      byte[] numArray2 = aPayload;
      int index2 = num2;
      int num3 = index2 + 1;
      int num4 = (int) categoryId;
      numArray2[index2] = (byte) num4;
      byte[] numArray3 = aPayload;
      int index3 = num3;
      int num5 = index3 + 1;
      int num6 = (int) categoryDay;
      numArray3[index3] = (byte) num6;
      foreach (byte num7 in inTime)
        aPayload[num5++] = num7;
      foreach (byte num7 in outTime)
        aPayload[num5++] = num7;
      this._readerWorker.send(new RfidMsg((byte) 100, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte resetCategoryDay(byte categoryId, byte categoryDay)
    {
      return this.setCategory(categoryId, categoryDay, new byte[4], new byte[4]
      {
        (byte) 192,
        (byte) 192,
        (byte) 192,
        (byte) 192
      });
    }

    public byte resetCategory(byte categoryId)
    {
      byte num = 0;
      for (byte categoryDay = 0; categoryDay <= (byte) 7; ++categoryDay)
      {
        if (this.resetCategoryDay(categoryId, categoryDay) != (byte) 0)
          num = byte.MaxValue;
      }
      return num;
    }

    public byte getCategory(byte categoryId, byte categoryDay)
    {
      if (!this.isValid())
        return 1;
      if (categoryId < (byte) 0 || categoryId > (byte) 30 || (categoryDay < (byte) 0 || categoryDay > (byte) 7))
        return 2;
      this._readerWorker.send(new RfidMsg((byte) 100, (byte) 0, new byte[3]
      {
        (byte) 2,
        categoryId,
        categoryDay
      }, (byte) 3));
      return 0;
    }

    public byte addHoliday(byte aMonth, byte aDay)
    {
      if (!this.isValid())
        return 1;
      byte num = (byte) (((uint) (byte) ((uint) aDay / 10U) << 4) + (uint) (byte) ((uint) aDay % 10U));
      this._readerWorker.send(new RfidMsg((byte) 100, (byte) 0, new byte[3]
      {
        (byte) 3,
        (byte) (((uint) (byte) ((uint) aMonth / 10U) << 4) + (uint) (byte) ((uint) aMonth % 10U)),
        num
      }, (byte) 3));
      return 0;
    }

    public byte clearHoliday()
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 100, (byte) 0, new byte[1]
      {
        (byte) 5
      }, (byte) 1));
      return 0;
    }

    public byte readHolidayTable()
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 100, (byte) 0, new byte[1]
      {
        (byte) 4
      }, (byte) 1));
      return 0;
    }

    public static byte encodeUserProfileHf(byte categoryId, byte ioActions)
    {
      return (byte) (((int) categoryId & 31) << 3 | (int) ioActions & 7);
    }

    public byte writeUserIdHf(RfidDefs.TagHfType tagType, ushort userId, byte[] uid)
    {
      if (uid.Length != 8)
        return 2;
      byte[] buffer = new byte[10];
      buffer[0] = (byte) (((int) userId & 65280) >> 8);
      buffer[1] = (byte) ((uint) userId & (uint) byte.MaxValue);
      int index = 2;
      foreach (byte num in uid)
      {
        buffer[index] = num;
        ++index;
      }
      ushort num1 = Signature.SignatureCalcHfApriporta(buffer);
      byte[] blockData = new byte[4]
      {
        (byte) (((int) num1 & 65280) >> 8),
        (byte) ((uint) num1 & (uint) byte.MaxValue),
        (byte) (((int) userId & 65280) >> 8),
        (byte) ((uint) userId & (uint) byte.MaxValue)
      };
      return this.writeBlock(false, (byte[]) null, (byte) tagType, (byte) 0, blockData);
    }

    public static byte encodeUserProfile245(
      byte categoryId,
      bool checkAbsence,
      byte outActions,
      byte inActions)
    {
      byte num = 0;
      if (checkAbsence)
        num = (byte) 16;
      return (byte) (((int) categoryId & 7) << 5 | (int) num | ((int) outActions & 3) << 2 | (int) inActions & 3);
    }

    public byte setUserRaw(byte[] userDataRaw)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[userDataRaw.Length + 1];
      aPayload[0] = (byte) 1;
      Array.Copy((Array) userDataRaw, 0, (Array) aPayload, 1, userDataRaw.Length);
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte setUser(ushort userId, byte userProfile)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[4]
      {
        (byte) 1,
        userProfile,
        (byte) (((int) userId & 65280) >> 8),
        (byte) ((uint) userId & (uint) byte.MaxValue)
      };
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte delUser(ushort userId)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[4]
      {
        (byte) 2,
        (byte) 0,
        (byte) (((int) userId & 65280) >> 8),
        (byte) ((uint) userId & (uint) byte.MaxValue)
      };
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte getUser(ushort userId)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[4]
      {
        (byte) 3,
        (byte) 0,
        (byte) (((int) userId & 65280) >> 8),
        (byte) ((uint) userId & (uint) byte.MaxValue)
      };
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte getNumUsers()
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, new byte[1]
      {
        (byte) 5
      }, (byte) 1));
      return 0;
    }

    public byte clearUserDb()
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[1]{ (byte) 4 };
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte getInputStatus(byte aChannel)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 81, (byte) 0, new byte[1]
      {
        aChannel
      }, (byte) 1));
      return 0;
    }

    public byte setOutput(
      AbstractRfidReader.TypeIO anIOtype,
      byte aChannel,
      AbstractRfidReader.ModeIO mode,
      int onTime,
      int offTime,
      byte numRepeat)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 80, (byte) 0, new byte[8]
      {
        (byte) anIOtype,
        aChannel,
        (byte) mode,
        (byte) ((onTime & 65280) >> 8),
        (byte) (onTime & (int) byte.MaxValue),
        (byte) ((offTime & 65280) >> 8),
        (byte) (offTime & (int) byte.MaxValue),
        numRepeat
      }, (byte) 8));
      return 0;
    }

    public byte getIoConfig(byte channel, StandAloneIoDefs.IoType channelType)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 81, (byte) channelType, new byte[2]
      {
        (byte) 0,
        channel
      }, (byte) 2));
      return 0;
    }

    public byte setIoConfig(StandAloneIoDefs.IoSettings ioSettings)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 81, (byte) ioSettings.getFlags(), ioSettings.getRawData(), (byte) ioSettings.getRawData().Length));
      return 0;
    }

    public byte activateOutput(byte channel)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 81, (byte) 0, new byte[2]
      {
        (byte) 2,
        channel
      }, (byte) 2));
      return 0;
    }

    public byte chain245_getMask()
    {
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, new byte[3]
      {
        (byte) 6,
        (byte) 0,
        (byte) 0
      }, (byte) 3));
      return 0;
    }

    public byte chain245_setMask(byte firstTagId, byte numTags)
    {
      if (!this.isValid())
        return 1;
      if ((int) firstTagId + (int) numTags - 1 > 64)
        return 2;
      long num1 = 0;
      for (byte index = 0; (int) index < (int) numTags; ++index)
        num1 += (long) Math.Pow(2.0, (double) ((int) index + (int) firstTagId - 1));
      byte[] bigEndianBytes = RfidUtils.longToBigEndianBytes(num1);
      if (bigEndianBytes.Length != 8)
        return 2;
      byte num2 = RfidReader.encodeUserProfile245((byte) 0, true, (byte) 3, (byte) 0);
      byte[] aPayload = new byte[7 + bigEndianBytes.Length];
      aPayload[0] = (byte) 1;
      aPayload[1] = (byte) 32;
      aPayload[2] = (byte) 64;
      aPayload[3] = (byte) 0;
      aPayload[4] = (byte) ((int) firstTagId + (int) numTags - 1);
      for (int index = 0; index < bigEndianBytes.Length; ++index)
        aPayload[5 + index] = bigEndianBytes[index];
      aPayload[13] = num2;
      aPayload[14] = (byte) 0;
      this._readerWorker.send(new RfidMsg((byte) 98, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte chain245_getAllowedZeros()
    {
      return this.getParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_ZEROS);
    }

    public byte chain245_setAllowedZeros(byte allowedZeros)
    {
      return this.setParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_ZEROS, new byte[1]
      {
        allowedZeros
      });
    }

    public byte chain245_setAllowedAntitacklingLoops(byte allowedAntitacklingLoops)
    {
      return this.setParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_AT_LOOPS, new byte[1]
      {
        allowedAntitacklingLoops
      });
    }

    public byte chain245_getAllowedAntitacklingLoops()
    {
      return this.getParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_AT_LOOPS);
    }

    public byte readLog()
    {
      if (this._automaticLogDownload)
        this._downloadLogTimer.Start();
      this._readerWorker.send(new RfidMsg((byte) 96, (byte) 0, new byte[1]
      {
        (byte) 1
      }, (byte) 1));
      return 0;
    }

    public byte deleteLog()
    {
      this._readerWorker.send(new RfidMsg((byte) 96, (byte) 0, new byte[1]
      {
        (byte) 2
      }, (byte) 1));
      return 0;
    }

    private void onDebouncingEvent(object source, ElapsedEventArgs e)
    {
      this._lastTagIdRead = "NO ID DEF";
    }

    public byte bulkTxfrBeginUpload(ushort replyDelayMs, byte replyChannel, byte[] tagId)
    {
      byte[] numArray = new byte[32];
      numArray[0] = (byte) 1;
      numArray[1] = (byte) 7;
      numArray[2] = (byte) 1;
      numArray[3] = (byte) ((uint) replyDelayMs >> 8);
      numArray[4] = (byte) replyDelayMs;
      numArray[5] = replyChannel;
      for (int index = 6; index < 18; ++index)
        numArray[index] = (byte) 0;
      numArray[6] = (byte) 154;
      Array.Copy((Array) tagId, 0, (Array) numArray, 18, 8);
      byte[] aPayload = new byte[1 + numArray.Length];
      Array.Copy((Array) numArray, 0, (Array) aPayload, 1, numArray.Length);
      aPayload[0] = (byte) 1;
      this._readerWorker.send(new RfidMsg((byte) 101, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte bulkTxfrEndUpload(byte[] hash)
    {
      if (hash == null || hash.Length != 20)
        throw new Exception("L'hash deve essere non nullo e di lunghezza pari a 20 bytes");
      byte[] aPayload = new byte[33];
      aPayload[0] = (byte) 3;
      aPayload[1] = (byte) 128;
      aPayload[2] = (byte) 0;
      Array.Copy((Array) hash, 0, (Array) aPayload, 3, hash.Length);
      this._readerWorker.send(new RfidMsg((byte) 101, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte bulkTxfrUploadBlock(byte[] blockData)
    {
      int num1 = 30;
      int num2 = 3;
      if (blockData == null || blockData.Length > num1)
        throw new Exception("Block size must be <= " + (object) num1);
      byte[] aPayload = new byte[num1 + 2 + 1];
      aPayload[0] = (byte) 2;
      aPayload[1] = (byte) 0;
      aPayload[2] = (byte) (blockData.Length / num2);
      for (int index = 0; index < blockData.Length; ++index)
        aPayload[index + 3] = blockData[index];
      this._readerWorker.send(new RfidMsg((byte) 101, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public enum DownloadLogStatus : byte
    {
      IDLE,
      SUCCEDED,
      BUSY,
      TIMED_OUT,
      ERROR,
    }
  }
}
