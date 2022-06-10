// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.FxwTcp
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;
using System.Net;
using System.Net.Sockets;

namespace It.IDnova.Fxw
{
  internal class FxwTcp : IFxwPhysicalLayer
  {
    private FxwLog _logger = FxwLog.getInstance();
    private const int RECEIVE_BUF_SIZE = 2048;
    private Socket _theSocket;
    private byte[] _receiveBuf;

    public FxwTcp(PhysicalReceiveDataHandler callback)
      : base(callback)
    {
      this._theSocket = (Socket) null;
      this._receiveBuf = new byte[2048];
    }

    public override bool isValid()
    {
      if (this._theSocket == null)
        return false;
      return this._theSocket.Connected;
    }

    public override bool isConnected()
    {
      bool flag = false;
      if (this._theSocket == null)
        return flag;
      bool blocking = this._theSocket.Blocking;
      try
      {
        this._theSocket.Blocking = false;
        this._theSocket.Send(new byte[1], 0, SocketFlags.None);
        flag = true;
      }
      catch (SocketException ex)
      {
        if (ex.NativeErrorCode.Equals(10035))
          flag = true;
        else
          this._logger.debug("[FxwTcp] Disconnected: err[0]" + (object) ex.NativeErrorCode);
      }
      finally
      {
        this._theSocket.Blocking = blocking;
      }
      return flag;
    }

    public override byte connect(string aConnString)
    {
      if (aConnString == null || aConnString.Equals(""))
        return byte.MaxValue;
      string[] strArray = aConnString.Split(':');
      if (strArray.Length != 2)
        return 2;
      try
      {
        string ipString = strArray[0];
        int port = int.Parse(strArray[1]);
        this._theSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this._theSocket.Connect((EndPoint) new IPEndPoint(IPAddress.Parse(ipString), port));
        byte[] buffer = new byte[2048];
        int num1 = 10;
        int num2 = 0;
        this._logger.debug("[FxwTcp] Receiving useless input buffer...");
        while (num1 > 0)
        {
          int num3;
          try
          {
            this._theSocket.ReceiveTimeout = 1000;
            num3 = this._theSocket.Receive(buffer, SocketFlags.None);
          }
          catch
          {
            if (!this._theSocket.Connected)
            {
              this._theSocket.Close();
              this._theSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
              this._theSocket.Connect((EndPoint) new IPEndPoint(IPAddress.Parse(ipString), port));
              break;
            }
            break;
          }
          num2 += num3;
          --num1;
          if (num3 < 512)
            break;
        }
        this._logger.debug("[FxwTcp] Discarded data: " + (object) num2 + " bytes");
        this._theSocket.ReceiveTimeout = 0;
        this._theSocket.BeginReceive(this._receiveBuf, 0, 2048, SocketFlags.None, new AsyncCallback(this.onReceivedData), (object) this);
        this._logger.debug("[FxwTcp] Connection established");
      }
      catch (Exception)
      {
        this._logger.debug("[FxwTcp] Connection exception");
        if (this._theSocket != null)
        {
          this._theSocket.Disconnect(false);
          this._theSocket.Close();
          this._logger.debug("[FxwTcp] Socket disconnected and closed");
          this._theSocket = (Socket) null;
        }
        return 3;
      }
      return 0;
    }

    public override void disconnect()
    {
      try
      {
        if (this.isValid() && this._theSocket != null)
        {
          this._logger.debug("[FxwTcp] Terminating TCP/IP connection...");
          this._logger.debug("[FxwTcp] Closing...");
          this._theSocket.Close();
        }
        this._logger.debug("[FxwTcp] Socket disconnected");
      }
      catch (Exception)
      {
        this._logger.debug("[FxwTcp] Connection termination exception");
      }
      this._theSocket = (Socket) null;
    }

    public override byte sendData(byte[] dataBuff, int dataLen)
    {
      byte num = 1;
      if (!this.isValid())
        return num;
      try
      {
        this._logger.debug("[FxwTcp] Msg sent over TCP (" + (object) this._theSocket.Send(dataBuff, SocketFlags.None) + " bytes)");
        return 0;
      }
      catch (SocketException)
      {
        this._logger.debug("[FxwTcp] Socket error sending data");
        return 4;
      }
    }

    private void onReceivedData(IAsyncResult asynchRes)
    {
      try
      {
        if (this._theSocket == null)
          return;
        int num1 = this._theSocket.EndReceive(asynchRes);
        if (num1 > 0)
        {
          int dataLen = num1;
          byte[] dataBuff = new byte[dataLen];
          for (int index = 0; index < dataLen; ++index)
            dataBuff[index] = this._receiveBuf[index];
          string str = "";
          foreach (byte num2 in dataBuff)
            str = str + num2.ToString("X2") + " ";
          this._logger.debug("[FxwTcp] Received " + (object) dataLen + " bytes: " + str);
          this.receivedDataCallback(dataBuff, dataLen);
        }
        if (this._theSocket == null)
          return;
        this._theSocket.BeginReceive(this._receiveBuf, 0, 2048, SocketFlags.None, new AsyncCallback(this.onReceivedData), (object) this);
      }
      catch (SocketException)
      {
        this._logger.debug("[FxwTcp] Socket error receiving data");
      }
      catch (ObjectDisposedException)
      {
        this._logger.debug("[FxwTcp] Data receiving interrupted (socket closed)");
      }
    }
  }
}
