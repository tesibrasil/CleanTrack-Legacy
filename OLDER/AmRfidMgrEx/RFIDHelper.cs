using It.IDnova.Fxw;
using LibLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace amrfidmgrex
{
    public class RFIDHelper
    {
        private List<string> AntiCollisionLoopList;
        private RFIDHelper.AnticollisionLoopDataDelegate AnticollisionHandlerCallback;
        private RfidReader Reader;
        private FxwLog FxwLogger;
        private static RFIDHelper Instance;

        public static string Address { get; set; }

        public static string HostName { get; set; }

        public RFIDHelper()
        {
            this.FxwLogger = FxwLog.getInstance();
            this.FxwLogger.logEnabled = true;
            this.FxwLogger.debugEnabled = true;
            if (this.Start())
                return;
            RFIDManager.writeLog(RFIDHelper.HostName, "Cleantrack.Service", "Can' t start RFIDHelper", (byte[])null, Logger.LogLevel.Error);
        }

        public static bool isInitiated()
        {
            if (RFIDHelper.Address != null && RFIDHelper.Address.Length > 0 && RFIDHelper.HostName != null)
                return RFIDHelper.HostName.Length > 0;
            return false;
        }

        public static void Reset()
        {
            RFIDHelper.Instance = new RFIDHelper();
        }

        public static RFIDHelper Get()
        {
            if (RFIDHelper.Instance == null)
                RFIDHelper.Instance = new RFIDHelper();
            return RFIDHelper.Instance;
        }

        public static void Finish()
        {
            if (RFIDHelper.Instance == null)
                return;
            RFIDHelper.Instance.Stop();
        }

        private void onReaderData(RfidMsg msg)
        {
            if (!msg.isValid() || (int)msg.CommandIdentifier != 224 || ((int)msg.CommandFlags != 0 || msg.getPayload() == null) || msg.getPayloadLen() == 0)
                return;
            RFIDHelper.Get().Buzzer(10);
            string payloadHex = RfidUtils.byteArrayToHexString(msg.getPayload());
            if (this.AntiCollisionLoopList.FindIndex((Predicate<string>)(item => item == payloadHex)) >= 0)
                return;
            this.AntiCollisionLoopList.Add(payloadHex);
            if (this.AnticollisionHandlerCallback == null)
                return;
            this.AnticollisionHandlerCallback(payloadHex);
        }

        public void ResetIO()
        {
            RFIDManager.writeLog("resetio");
            this.Led(RFIDHelper.LedType.Orange, false);
            this.Led(RFIDHelper.LedType.Blue, false);
            this.Led(RFIDHelper.LedType.Green, false);
            this.Led(RFIDHelper.LedType.Red, false);
        }

        private bool Start()
        {
            RFIDManager.writeLog("start");
            {

            }
            if (this.Reader != null)
                return false;
            this.Reader = new RfidReader(FxwPhysicalType.TCPIP);
            if ((int)this.Reader.connect(RFIDHelper.Address) != 0)
                return false;
            this.ResetIO();
            return true;
        }

        public void Stop()
        {
            RFIDManager.writeLog("stop");
            if (this.Reader == null)
                return;
            int num = (int)this.Reader.disconnect();
        }

        public bool isReaderConnected()
        {
            RFIDManager.writeLog("isreaderconnected");
            return this.Reader.isConnected();
        }

        public bool StartAnticollisionLoop(RFIDHelper.AnticollisionLoopDataDelegate callback)
        {
            RFIDManager.writeLog("startanticollisionloop");
            if (callback == null)
                return false;
            if (this.Reader.isInventorying())
                this.StopAnticollisionLoop();
            this.ResetIO();
            this.Led(RFIDHelper.LedType.Blue, true);
            this.AntiCollisionLoopList = new List<string>();
            this.AnticollisionHandlerCallback = callback;
            int num = (int)this.Reader.setPollingParams(RfidDefs.PollingType.HF_UHF_SINGLE, 300);
            this.Reader.receivedData += new AbstractRfidReader.ReceivedDataHandler(this.onReaderData);
            return (int)this.Reader.inventory(AbstractRfidReader.InventoryMode.START_LOOP) == 0;
        }

        public bool StopAnticollisionLoop()
        {
            RFIDManager.writeLog("stopanticollisionloop");
            if (!this.Reader.isInventorying())
                return false;
            this.Reader.receivedData -= new AbstractRfidReader.ReceivedDataHandler(this.onReaderData);
            this.AntiCollisionLoopList.Clear();
            bool flag = (int)this.Reader.inventory(AbstractRfidReader.InventoryMode.STOP_LOOP) == 0;
            this.ResetIO();
            return flag;
        }

        private byte GetByteFrom(RFIDHelper.LedType type)
        {
            byte num = 0;
            switch (type)
            {
                case RFIDHelper.LedType.Orange:
                    num = (byte)0;
                    break;
                case RFIDHelper.LedType.Red:
                    num = (byte)1;
                    break;
                case RFIDHelper.LedType.Green:
                    num = (byte)2;
                    break;
                case RFIDHelper.LedType.Blue:
                    num = (byte)3;
                    break;
            }
            return num;
        }

        public void Led(RFIDHelper.LedType type, bool On = true)
        {
            int num = (int)this.Reader.setOutput(AbstractRfidReader.TypeIO.LED, this.GetByteFrom(type), On ? AbstractRfidReader.ModeIO.ON : AbstractRfidReader.ModeIO.OFF, 0, 0, (byte)0);
            Thread.Sleep(100);
        }

        public void Buzzer(int duration)
        {
            int num = (int)this.Reader.setOutput(AbstractRfidReader.TypeIO.BUZZER, (byte)0, AbstractRfidReader.ModeIO.PULSE, duration, 0, (byte)0);
            Thread.Sleep(500);
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

        public delegate void AnticollisionLoopDataDelegate(string tagID);

        public delegate void ConnectionLossHandler();

        public enum LedType
        {
            Unknown,
            Orange,
            Red,
            Green,
            Blue,
        }
    }
}
