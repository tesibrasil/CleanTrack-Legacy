// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.FxwLog
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;
using System.Net.Sockets;
using System.Text;

namespace It.IDnova.Fxw
{
  public class FxwLog : AbstractFxwLog
  {
    private const string DEFAULT_ADDRESS = "127.0.0.1";
    private const int DEFAULT_PORT = 10313;
    private static UdpClient _theClient;

    public static FxwLog getInstance()
    {
      return FxwLog.getInstance("127.0.0.1", 10313);
    }

    public static FxwLog getInstance(string toAddress, int toPort)
    {
      if (AbstractFxwLog._instance == null)
        AbstractFxwLog._instance = new FxwLog(toAddress, toPort);
      return AbstractFxwLog._instance;
    }

    internal override void debug(string msg)
    {
      if (!AbstractFxwLog._isDebugEnabled)
        return;
      byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
      FxwLog._theClient.Send(bytes, bytes.Length);
    }

    public override void log(string msg)
    {
      if (!AbstractFxwLog._isLogEnabled)
        return;
      byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
      FxwLog._theClient.Send(bytes, bytes.Length);
    }

    private FxwLog(string toAddress, int toPort)
    {
      try
      {
        FxwLog._theClient = new UdpClient();
        FxwLog._theClient.Connect(toAddress, toPort);
      }
      catch (Exception)
      {
        FxwLog._theClient = (UdpClient) null;
      }
    }
  }
}
