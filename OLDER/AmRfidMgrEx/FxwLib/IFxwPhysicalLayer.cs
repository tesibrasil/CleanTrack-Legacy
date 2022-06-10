// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.IFxwPhysicalLayer
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

namespace It.IDnova.Fxw
{
  public abstract class IFxwPhysicalLayer
  {
    internal const bool LOG_PHYSICAL = true;
    protected PhysicalReceiveDataHandler receivedDataCallback;

    public IFxwPhysicalLayer(PhysicalReceiveDataHandler callback)
    {
      this.receivedDataCallback = callback;
    }

    public abstract byte connect(string connectionString);

    public abstract void disconnect();

    public abstract bool isValid();

    public abstract bool isConnected();

    public abstract byte sendData(byte[] dataBuff, int dataLen);
  }
}
