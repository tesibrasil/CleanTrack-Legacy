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
		private int _downloadLogRetryCounter = 0;
		private RfidReader.DownloadLogStatus _downloadLogStatus = RfidReader.DownloadLogStatus.IDLE;
		private int _downloadedLogRecords = 0;
		private TextWriter _readerLogWriter = (TextWriter)null;
		private string _logFilename = (string)null;
		private System.Timers.Timer _keybEmuDebouncingTimer = (System.Timers.Timer)null;
		private System.Timers.Timer _downloadLogTimer = (System.Timers.Timer)null;
		private RfidDefs.PollingType _pollingType = RfidDefs.PollingType.HF_UHF_SINGLE;
		private bool _automaticLogDownload = false;
		public const string DEFAULT_READER_LOG_FILENAME = "readerLog.txt";
		private const int _DOWNLOAD_LOG_TIMER_INTERVAL = 2300;
		private const int _DOWNLOAD_LOG_NRETRY = 13;
		private const string _REGISTRY_COMPORT_KEY = "HARDWARE\\DEVICEMAP\\SERIALCOMM";
		private const string _REGISTRY_VCOMPORT_ID = "VCP";
		private static DateTime _lastReadTime;
		private const int _TIMEFRAME_ARRAY_LEN = 4;

		public RfidReader(FxwPhysicalType theRdrType)
		  : base(theRdrType)
		{
			_automaticLogDownload = false;
			_downloadLogTimer = new System.Timers.Timer();
			_downloadLogTimer.Interval = 1313.0;
			_downloadLogTimer.Elapsed += new ElapsedEventHandler(onDownloadLogTimeout);
			_keybEmuDebouncingTimer = new System.Timers.Timer();
			_keybEmuDebouncingTimer.Interval = 2000.0;
			_keybEmuDebouncingTimer.Elapsed += new ElapsedEventHandler(onDebouncingEvent);
			RfidReader._lastReadTime = DateTime.Now.Subtract(AbstractRfidReader._KEYB_EMU_WAIT_TIME_MS);
		}

		public static string[] autoDetect(FxwPhysicalType theRdrType, bool notUseRegistry)
		{
			List<string> stringList = new List<string>();
			SerialPort serialPort = (SerialPort)null;
			if (theRdrType != FxwPhysicalType.USB)
				return (string[])null;
			string[] strArray = (string[])null;
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
							string str = (string)registryKey.GetValue(name);
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
				return (string[])null;
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
							for (int index = 2; index < 2 + (int)buffer2[1]; ++index)
								num2 += buffer2[index];
							if ((int)buffer2[7] != (int)byte.MaxValue && (int)num2 == (int)buffer2[2 + (int)buffer2[1]])
							{
								stringList.Add(portName);
								AbstractRfidReader._logger.debug("[RfidReader] Device detected on " + portName);
							}
							serialPort.Close();
						}
						serialPort = (SerialPort)null;
					}
				}
				catch
				{
					AbstractRfidReader._logger.debug("[RfidReader] Device NOT detected on " + portName);
				}
			}
			if (serialPort != null && serialPort.IsOpen)
				serialPort.Close();
			return stringList.Count > 0 ? stringList.ToArray() : (string[])null;
		}

		public override byte disconnect()
		{
			if (!isValid())
				return 1;
			closeLog(RfidReader.DownloadLogStatus.TIMED_OUT);
			return base.disconnect();
		}

		public byte setPollingParams(RfidDefs.PollingType theType, int interval_ms)
		{
			if (!isValid())
				return 1;
			_pollingType = theType;
			return _readerWorker.setPollingParams(_pollingType, interval_ms);
		}

		public RfidDefs.PollingType getPollingType()
		{
			if (!isValid())
				return RfidDefs.PollingType.UNKNOWN;
			return _pollingType;
		}

		public double getPollingInterval()
		{
			if (!isValid())
				return 0.0;
			return _readerWorker.getPollingInterval();
		}

		public override int inventory(AbstractRfidReader.InventoryMode invMode)
		{
			if (!isValid())
				return 1;

			byte aCmdId = (byte)77;
			switch (invMode)
			{
				case AbstractRfidReader.InventoryMode.START_LOOP:
				{
					aCmdId = (byte)2;
					_isInventorying = true;
					break;
				}
				case AbstractRfidReader.InventoryMode.STOP_LOOP:
				{
					aCmdId = (byte)3;
					_isInventorying = false;
					break;
				}
			}

			RfidMsg rfidMsg = new RfidMsg(aCmdId, (byte)_pollingType, (byte[])null, (byte)0);
			if ((int)aCmdId == 77)
				return _readerWorker.rawSend(rfidMsg);
			else
				return _readerWorker.send(rfidMsg);
		}

		public override int inventoryTID(AbstractRfidReader.InventoryMode invMode)
		{
			if (!isValid())
				return 1;

			byte aCmdId = 77;
			switch (invMode)
			{
				case AbstractRfidReader.InventoryMode.START_LOOP:
				{
					aCmdId = (byte)4;
					_isInventorying = true;
					break;
				}
				case AbstractRfidReader.InventoryMode.STOP_LOOP:
				{
					aCmdId = (byte)5;
					_isInventorying = false;
					break;
				}
			}

			RfidMsg rfidMsg = new RfidMsg(aCmdId, (byte)(_pollingType | RfidDefs.PollingType.UNIQUE_125KHZ), (byte[])null, (byte)0);
			if ((int)aCmdId == 77)
				return _readerWorker.rawSend(rfidMsg);
			else
				return _readerWorker.send(rfidMsg);
		}

		protected override void onWorkerReceivedData(RfidMsg rcvMsg)
		{
			if ((int)rcvMsg.CommandIdentifier == 96)
			{
				if (_automaticLogDownload)
				{
					resetDownloadTimer();
					if (rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_EMPTY)
					{
						closeLog(RfidReader.DownloadLogStatus.SUCCEDED);
						AbstractRfidReader._logger.debug("Log downloading terminated: " + (object)_downloadedLogRecords + " records got");
					}
					else if (rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_MEM_WRITE || rcvMsg.decodeErrorCode() == RfidDefs.FxwProtoRes.ERR_MEM_READ)
					{
						closeLog(RfidReader.DownloadLogStatus.ERROR);
						AbstractRfidReader._logger.debug("Log downloading error");
					}
					else if (_readerLogWriter != null && rcvMsg.getPayloadLen() > 0)
					{
						AbstractRfidReader._logger.debug("Log record succesfully extracted !");
						_readerLogWriter.WriteLine(rcvMsg.getPayloadHexString(false));
						_downloadedLogRecords = _downloadedLogRecords + 1;
						int num = (int)deleteLog();
					}
					else
					{
						int num1 = (int)readLog();
					}
				}
				base.onWorkerReceivedData(rcvMsg);
			}
			else
				base.onWorkerReceivedData(rcvMsg);
		}

		public override int getKeyboardEmulationDebouncing()
		{
			return (int)_keybEmuDebouncingTimer.Interval;
		}

		public override void setKeyboardEmulationDebouncing(int interval_ms)
		{
			if ((uint)interval_ms > 0U)
			{
				AbstractRfidReader._DEBOUNCING_ENABLED = true;
				_keybEmuDebouncingTimer.Interval = (double)interval_ms;
			}
			else
				AbstractRfidReader._DEBOUNCING_ENABLED = false;
		}

		protected override void emulateKeyboard(string data)
		{
			AbstractRfidReader._logger.debug("Emulating keyboard with data: '" + data + "'");
			if (!AbstractRfidReader._DEBOUNCING_ENABLED)
			{
				if (data.Equals(_lastTagIdRead) && DateTime.Now - RfidReader._lastReadTime < AbstractRfidReader._KEYB_EMU_WAIT_TIME_MS)
				{
					RfidReader._lastReadTime = DateTime.Now;
					return;
				}
			}
			else
			{
				if (data.Equals(_lastTagIdRead))
					return;
				_keybEmuDebouncingTimer.Stop();
			}
			RfidReader._lastReadTime = DateTime.Now;
			_lastTagIdRead = data;
			if (_keyEmuAddTimestamp)
				data = DateTime.Now.ToString("s").Replace("T", " ") + " " + data;
			foreach (char ch in data)
			{
				_keybEmulator.press((int)ch);
				_keybEmulator.release((int)ch);
			}
			if (_keybEmuMode == FxwKbd.KeybEmuMode.DATA_CRLF)
			{
				_keybEmulator.press(13);
				_keybEmulator.release(13);
				_keybEmulator.press(10);
				_keybEmulator.release(10);
			}
			if (!AbstractRfidReader._DEBOUNCING_ENABLED)
				return;
			_keybEmuDebouncingTimer.Start();
		}

		private void onDownloadLogTimeout(object source, ElapsedEventArgs e)
		{
			_downloadLogTimer.Stop();
			if (_downloadLogRetryCounter < 13)
			{
				_downloadLogRetryCounter = _downloadLogRetryCounter + 1;
				int num = (int)downloadLog(_logFilename);
				_downloadLogTimer.Start();
			}
			else
				closeLog(RfidReader.DownloadLogStatus.TIMED_OUT);
		}

		private void resetDownloadTimer()
		{
			_downloadLogTimer.Stop();
			_downloadLogRetryCounter = 0;
		}

		public void setLogFilename(string logFilename)
		{
			if (logFilename == null || logFilename.Length <= 0)
				return;
			_logFilename = logFilename;
		}

		public string getLogFilename()
		{
			return _logFilename;
		}

		public byte downloadLog(string filename)
		{
			_automaticLogDownload = true;
			if (!isValid())
				return 1;
			_logFilename = filename != null && filename.Length >= 5 && filename.Contains(".log") ? filename : "readerLog.txt";
			if (_readerLogWriter == null && _downloadLogStatus != RfidReader.DownloadLogStatus.BUSY)
			{
				AbstractRfidReader._logger.debug("Downloading log to file" + _logFilename + "...");
				_readerLogWriter = (TextWriter)new StreamWriter(_logFilename, true);
				_downloadLogStatus = RfidReader.DownloadLogStatus.BUSY;
				_downloadedLogRecords = 0;
			}
			return readLog();
		}

		public RfidReader.DownloadLogStatus getLogStatus()
		{
			return _downloadLogStatus;
		}

		public byte clearLog()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)96, (byte)0, new byte[1]
			{
		(byte) 3
			}, (byte)1));
			return 0;
		}

		public byte readAndDeleteLog()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)96, (byte)0, new byte[1]
			{
		(byte) 4
			}, (byte)1));
			return 0;
		}

		private void closeLog(RfidReader.DownloadLogStatus closingStatus)
		{
			if (_readerLogWriter == null)
				return;
			_downloadLogStatus = closingStatus;
			_readerLogWriter.Flush();
			_readerLogWriter.Close();
			_readerLogWriter = (TextWriter)null;
		}

		public byte setTempThreshold(byte tempThresholdC)
		{
			if ((int)tempThresholdC < 0)
				return 2;
			ushort num = (ushort)((double)tempThresholdC / (1.0 / 32.0));
			return setParam(RfidDefs.FxwParam.ALR245_TEMP_THRESHOLD, new byte[2]
			{
		(byte) (((int) num & 65280) >> 8),
		(byte) ((uint) num & (uint) byte.MaxValue)
			});
		}

		public byte getTempThreshold()
		{
			return getParam(RfidDefs.FxwParam.ALR245_TEMP_THRESHOLD);
		}

		public byte setCategory(byte categoryId, byte categoryDay, byte[] inTime, byte[] outTime)
		{
			if (!isValid())
				return 1;
			if (inTime == null || inTime.Length != 4 || (outTime == null || outTime.Length != 4) || ((int)categoryId < 0 || (int)categoryId > 30 || (int)categoryDay < 0) || (int)categoryDay > 7)
				return 2;
			foreach (byte num in inTime)
			{
				if ((int)num < 0 || (int)num > 192)
					return 2;
			}
			foreach (byte num in outTime)
			{
				if ((int)num < 0 || (int)num > 192)
					return 2;
			}
			byte[] aPayload = new byte[11];
			int num1 = 0;
			byte[] numArray1 = aPayload;
			int index1 = num1;
			int num2 = 1;
			int num3 = index1 + num2;
			int num4 = 1;
			numArray1[index1] = (byte)num4;
			byte[] numArray2 = aPayload;
			int index2 = num3;
			int num5 = 1;
			int num6 = index2 + num5;
			int num7 = (int)categoryId;
			numArray2[index2] = (byte)num7;
			byte[] numArray3 = aPayload;
			int index3 = num6;
			int num8 = 1;
			int num9 = index3 + num8;
			int num10 = (int)categoryDay;
			numArray3[index3] = (byte)num10;
			foreach (byte num11 in inTime)
				aPayload[num9++] = num11;
			foreach (byte num11 in outTime)
				aPayload[num9++] = num11;
			_readerWorker.send(new RfidMsg((byte)100, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte resetCategoryDay(byte categoryId, byte categoryDay)
		{
			return setCategory(categoryId, categoryDay, new byte[4], new byte[4]
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
			for (byte categoryDay = 0; (int)categoryDay <= 7; ++categoryDay)
			{
				if ((uint)resetCategoryDay(categoryId, categoryDay) > 0U)
					num = byte.MaxValue;
			}
			return num;
		}

		public byte getCategory(byte categoryId, byte categoryDay)
		{
			if (!isValid())
				return 1;
			if ((int)categoryId < 0 || (int)categoryId > 30 || (int)categoryDay < 0 || (int)categoryDay > 7)
				return 2;
			_readerWorker.send(new RfidMsg((byte)100, (byte)0, new byte[3]
			{
		(byte) 2,
		categoryId,
		categoryDay
			}, (byte)3));
			return 0;
		}

		public byte addHoliday(byte aMonth, byte aDay)
		{
			if (!isValid())
				return 1;
			byte num = (byte)(((uint)(byte)((uint)aDay / 10U) << 4) + (uint)(byte)((uint)aDay % 10U));
			_readerWorker.send(new RfidMsg((byte)100, (byte)0, new byte[3]
			{
		(byte) 3,
		(byte) (((uint) (byte) ((uint) aMonth / 10U) << 4) + (uint) (byte) ((uint) aMonth % 10U)),
		num
			}, (byte)3));
			return 0;
		}

		public byte clearHoliday()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)100, (byte)0, new byte[1]
			{
		(byte) 5
			}, (byte)1));
			return 0;
		}

		public byte readHolidayTable()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)100, (byte)0, new byte[1]
			{
		(byte) 4
			}, (byte)1));
			return 0;
		}

		public static byte encodeUserProfileHf(byte categoryId, byte ioActions)
		{
			return (byte)(((int)categoryId & 31) << 3 | (int)ioActions & 7);
		}

		public byte writeUserIdHf(RfidDefs.TagHfType tagType, ushort userId, byte[] uid)
		{
			if (uid.Length != 8)
				return 2;
			byte[] buffer = new byte[10];
			buffer[0] = (byte)(((int)userId & 65280) >> 8);
			buffer[1] = (byte)((uint)userId & (uint)byte.MaxValue);
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
			return writeBlock(false, (byte[])null, (byte)tagType, (byte)0, blockData);
		}

		public static byte encodeUserProfile245(byte categoryId, bool checkAbsence, byte outActions, byte inActions)
		{
			byte num = 0;
			if (checkAbsence)
				num = (byte)16;
			return (byte)(((int)categoryId & 7) << 5 | (int)num | ((int)outActions & 3) << 2 | (int)inActions & 3);
		}

		public byte setUserRaw(byte[] userDataRaw)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[userDataRaw.Length + 1];
			aPayload[0] = (byte)1;
			Array.Copy((Array)userDataRaw, 0, (Array)aPayload, 1, userDataRaw.Length);
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte setUser(ushort userId, byte userProfile)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[4]
			{
		(byte) 1,
		userProfile,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte delUser(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[4]
			{
		(byte) 2,
		(byte) 0,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getUser(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[4]
			{
		(byte) 3,
		(byte) 0,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getNumUsers()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, new byte[1]
			{
		(byte) 5
			}, (byte)1));
			return 0;
		}

		public byte clearUserDb()
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[1] { (byte)4 };
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getInputStatus(byte aChannel)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)81, (byte)0, new byte[1]
			{
		aChannel
			}, (byte)1));
			return 0;
		}

		public byte setOutput(AbstractRfidReader.TypeIO anIOtype, byte aChannel, AbstractRfidReader.ModeIO mode, int onTime, int offTime, byte numRepeat)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)80, (byte)0, new byte[8]
			{
		(byte) anIOtype,
		aChannel,
		(byte) mode,
		(byte) ((onTime & 65280) >> 8),
		(byte) (onTime & (int) byte.MaxValue),
		(byte) ((offTime & 65280) >> 8),
		(byte) (offTime & (int) byte.MaxValue),
		numRepeat
			}, (byte)8));
			return 0;
		}

		public byte setOutputTest(AbstractRfidReader.TypeIO anIOtype, byte aChannel, AbstractRfidReader.ModeIO mode, int onTime, int offTime, byte numRepeat)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)80, (byte)128, new byte[8]
			{
		(byte) anIOtype,
		aChannel,
		(byte) mode,
		(byte) ((onTime & 65280) >> 8),
		(byte) (onTime & (int) byte.MaxValue),
		(byte) ((offTime & 65280) >> 8),
		(byte) (offTime & (int) byte.MaxValue),
		numRepeat
			}, (byte)8));
			return 0;
		}

		public byte getIoConfig(byte channel, StandAloneIoDefs.IoType channelType)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)81, (byte)channelType, new byte[2]
			{
		(byte) 0,
		channel
			}, (byte)2));
			return 0;
		}

		public byte setIoConfig(StandAloneIoDefs.IoSettings ioSettings)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)81, (byte)ioSettings.getFlags(), ioSettings.getRawData(), (byte)ioSettings.getRawData().Length));
			return 0;
		}

		public byte sendRawMessage(RfidMsg msg)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(msg);
			return 0;
		}

		public byte activateOutput(byte channel)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)81, (byte)0, new byte[2]
			{
		(byte) 2,
		channel
			}, (byte)2));
			return 0;
		}

		public byte immediateReadInputs()
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)81, (byte)1, new byte[2]
			{
		(byte) 3,
		(byte) 0
			}, (byte)2));
			return 0;
		}

		public byte chain245_getMask()
		{
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, new byte[3]
			{
		(byte) 6,
		(byte) 0,
		(byte) 0
			}, (byte)3));
			return 0;
		}

		public byte chain245_setMask(byte firstTagId, byte numTags)
		{
			if (!isValid())
				return 1;
			if ((int)firstTagId + (int)numTags - 1 > 64)
				return 2;
			long num1 = 0;
			for (byte index = 0; (int)index < (int)numTags; ++index)
				num1 += (long)Math.Pow(2.0, (double)((int)index + (int)firstTagId - 1));
			byte[] bigEndianBytes = RfidUtils.longToBigEndianBytes(num1);
			if (bigEndianBytes.Length != 8)
				return 2;
			byte num2 = RfidReader.encodeUserProfile245((byte)0, true, (byte)3, (byte)0);
			byte[] aPayload = new byte[7 + bigEndianBytes.Length];
			aPayload[0] = (byte)1;
			aPayload[1] = (byte)32;
			aPayload[2] = (byte)64;
			aPayload[3] = (byte)0;
			aPayload[4] = (byte)((int)firstTagId + (int)numTags - 1);
			for (int index = 0; index < bigEndianBytes.Length; ++index)
				aPayload[5 + index] = bigEndianBytes[index];
			aPayload[13] = num2;
			aPayload[14] = (byte)0;
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte chain245_getAllowedZeros()
		{
			return getParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_ZEROS);
		}

		public byte chain245_setAllowedZeros(byte allowedZeros)
		{
			return setParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_ZEROS, new byte[1]
			{
		allowedZeros
			});
		}

		public byte chain245_setAllowedAntitacklingLoops(byte allowedAntitacklingLoops)
		{
			return setParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_AT_LOOPS, new byte[1]
			{
		allowedAntitacklingLoops
			});
		}

		public byte chain245_getAllowedAntitacklingLoops()
		{
			return getParam(RfidDefs.FxwParam.ALR245_CHAIN_ALLOWED_AT_LOOPS);
		}

		public byte readLog()
		{
			if (_automaticLogDownload)
				_downloadLogTimer.Start();
			_readerWorker.send(new RfidMsg((byte)96, (byte)0, new byte[1]
			{
		(byte) 1
			}, (byte)1));
			return 0;
		}

		public byte deleteLog()
		{
			_readerWorker.send(new RfidMsg((byte)96, (byte)0, new byte[1]
			{
		(byte) 2
			}, (byte)1));
			return 0;
		}

		public byte getNLogs()
		{
			_readerWorker.send(new RfidMsg((byte)96, (byte)0, new byte[1]
			{
		(byte) 5
			}, (byte)1));
			return 0;
		}

		private void onDebouncingEvent(object source, ElapsedEventArgs e)
		{
			_lastTagIdRead = "NO ID DEF";
		}

		public byte bulkTxfrBeginUpload(ushort replyDelayMs, byte replyChannel, byte[] tagId)
		{
			byte[] numArray = new byte[32];
			numArray[0] = (byte)1;
			numArray[1] = (byte)7;
			numArray[2] = (byte)1;
			numArray[3] = (byte)((uint)replyDelayMs >> 8);
			numArray[4] = (byte)replyDelayMs;
			numArray[5] = replyChannel;
			for (int index = 6; index < 18; ++index)
				numArray[index] = (byte)0;
			numArray[6] = (byte)154;
			Array.Copy((Array)tagId, 0, (Array)numArray, 18, 8);
			byte[] aPayload = new byte[1 + numArray.Length];
			Array.Copy((Array)numArray, 0, (Array)aPayload, 1, numArray.Length);
			aPayload[0] = (byte)1;
			_readerWorker.send(new RfidMsg((byte)101, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte bulkTxfrEndUpload(byte[] hash)
		{
			if (hash == null || hash.Length != 20)
				throw new Exception("L'hash deve essere non nullo e di lunghezza pari a 20 bytes");
			byte[] aPayload = new byte[33];
			aPayload[0] = (byte)3;
			aPayload[1] = (byte)128;
			aPayload[2] = (byte)0;
			Array.Copy((Array)hash, 0, (Array)aPayload, 3, hash.Length);
			_readerWorker.send(new RfidMsg((byte)101, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte bulkTxfrUploadBlock(byte[] blockData)
		{
			int num1 = 30;
			int num2 = 3;
			if (blockData == null || blockData.Length > num1)
				throw new Exception("Block size must be <= " + (object)num1);
			byte[] aPayload = new byte[num1 + 2 + 1];
			aPayload[0] = (byte)2;
			aPayload[1] = (byte)0;
			aPayload[2] = (byte)(blockData.Length / num2);
			for (int index = 0; index < blockData.Length; ++index)
				aPayload[index + 3] = blockData[index];
			_readerWorker.send(new RfidMsg((byte)101, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte setUserNew(ushort userId, ushort userProfile)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[5]
			{
		(byte) 7,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue),
		(byte) (((int) userProfile & 65280) >> 8),
		(byte) ((uint) userProfile & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte setActionNew(ushort userId, ushort userProfile)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[5]
			{
		(byte) 7,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue),
		(byte) (((int) userProfile & 65280) >> 8),
		(byte) ((uint) userProfile & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)1, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte delUserNew(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[3]
			{
		(byte) 8,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte delActionNew(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[3]
			{
		(byte) 8,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)1, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getUserNew(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[3]
			{
		(byte) 9,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getActionNew(ushort userId)
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[3]
			{
		(byte) 9,
		(byte) (((int) userId & 65280) >> 8),
		(byte) ((uint) userId & (uint) byte.MaxValue)
			};
			_readerWorker.send(new RfidMsg((byte)98, (byte)1, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte getAllUsers()
		{
			if (!isValid())
				return 1;
			byte[] aPayload = new byte[1] { (byte)10 };
			_readerWorker.send(new RfidMsg((byte)98, (byte)0, aPayload, (byte)aPayload.Length));
			return 0;
		}

		public byte setOutputNew(AbstractRfidReader.TypeIO anIOtype, byte aChannel, byte mode, int onTime, int offTime, byte numRepeat)
		{
			if (!isValid())
				return 1;
			_readerWorker.send(new RfidMsg((byte)80, (byte)0, new byte[8]
			{
		(byte) anIOtype,
		aChannel,
		mode,
		(byte) ((onTime & 65280) >> 8),
		(byte) (onTime & (int) byte.MaxValue),
		(byte) ((offTime & 65280) >> 8),
		(byte) (offTime & (int) byte.MaxValue),
		numRepeat
			}, (byte)8));
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
