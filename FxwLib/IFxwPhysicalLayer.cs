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

    public abstract byte connect(string connectionString, bool useAsciiTransport, object transportChannel, int txDelayMs);

    public abstract void disconnect();

    public abstract bool isValid();

    public abstract bool isConnected();

    public abstract byte sendData(byte[] dataBuff, int dataLen);
  }
}
