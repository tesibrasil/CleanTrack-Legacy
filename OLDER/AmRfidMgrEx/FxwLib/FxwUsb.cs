// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.FxwUsb
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;
using System.IO.Ports;

namespace It.IDnova.Fxw
{
  internal class FxwUsb : IFxwPhysicalLayer
  {
    private FxwLog _logger = FxwLog.getInstance();
    public const int DEFAULT_BAUDRATE = 115200;
    public const Parity DEFAULT_PARITY = Parity.None;
    public const int DEFAULT_DATABITS = 8;
    public const StopBits DEFAULT_STOPBITS = StopBits.One;
    private SerialPort _theCom;
    private int _baudRate;
    private string _port;

    public FxwUsb(PhysicalReceiveDataHandler callback)
      : base(callback)
    {
      this._theCom = (SerialPort) null;
    }

    public override bool isValid()
    {
      if (this._theCom == null)
        return false;
      return this._theCom.IsOpen;
    }

    public override bool isConnected()
    {
      return this.isValid();
    }

    public override byte connect(string connString)
    {
      if (connString != null)
      {
        if (!connString.Equals(""))
        {
          try
          {
            string[] strArray = connString.Split(':');
            this._port = strArray[0];
            this._baudRate = strArray.Length != 2 ? 115200 : int.Parse(strArray[1]);
            this._theCom = new SerialPort(this._port, this._baudRate, Parity.None, 8, StopBits.One);
            this._theCom.DtrEnable = false;
            this._theCom.ErrorReceived += new SerialErrorReceivedEventHandler(this.onErrorReceived);
            this._theCom.Open();
            GC.SuppressFinalize((object) this._theCom.BaseStream);
            this._theCom.DataReceived += new SerialDataReceivedEventHandler(this.onReceivedData);
            this._logger.debug("[FxwUsb] Connection established");
            return 0;
          }
          catch (Exception ex)
          {
            this._logger.debug("[FxwUsb] Connection creation exception");
            this._theCom = (SerialPort) null;
            return byte.MaxValue;
          }
        }
      }
      return byte.MaxValue;
    }

    private void onErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
      this._logger.debug("!!!!!!!! SERIAL PORT ERROR: " + (object) e.EventType + " - " + e.ToString());
    }

    public override void disconnect()
    {
      if (!this.isValid() || this._theCom == null)
        return;
      if (!this._theCom.IsOpen)
        return;
      try
      {
        this._logger.debug("[FxwUsb] Terminating USB connection...");
        this._logger.debug("[FxwUsb] Discarding in buffer...");
        this._theCom.DiscardInBuffer();
        this._logger.debug("[FxwUsb] Discarding out buffer...");
        this._theCom.DiscardOutBuffer();
        this._logger.debug("[FxwUsb] Closing...");
        this._theCom.Close();
        this._logger.debug("[FxwUsb] Connection terminated");
      }
      catch (Exception ex)
      {
        this._logger.debug("[FxwUsb] Connection termination exception");
      }
      this._theCom = (SerialPort) null;
    }

    public override byte sendData(byte[] dataBuff, int dataLen)
    {
      if (!this.isValid())
        return 1;
      try
      {
        if (this._theCom != null)
        {
          this._theCom.Write(dataBuff, 0, dataLen);
          this._logger.debug("[FxwUsb] Msg sent over USB");
        }
      }
      catch
      {
        this._logger.debug("[FxwUsb] sendData Error");
      }
      return 0;
    }

    protected virtual void onReceivedData(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        int dataLen = 0;
        if (this._theCom == null)
          return;
        if (this._theCom != null)
          dataLen = this._theCom.BytesToRead;
        byte[] dataBuff = new byte[dataLen];
        for (int index = 0; index < dataLen; ++index)
        {
          if (this._theCom == null)
            return;
          dataBuff[index] = (byte) this._theCom.ReadByte();
        }
        string str = "";
        foreach (byte num in dataBuff)
          str = str + num.ToString("X2") + " ";
        this._logger.debug("[FxwUsb] Received " + (object) dataLen + " bytes: " + str);
        this.receivedDataCallback(dataBuff, dataLen);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
