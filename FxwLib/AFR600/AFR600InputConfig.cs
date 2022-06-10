namespace It.IDnova.Fxw.AFR600
{
  public class AFR600InputConfig
  {
    public int TimeUp { get; set; }

    public int TimeDown { get; set; }

    public bool LowToHighEnabled { get; set; }

    public bool HighToLowEnabled { get; set; }

    public static AFR600InputConfig parse(byte[] payload)
    {
      if (payload.Length != 5)
        return (AFR600InputConfig) null;
      return new AFR600InputConfig()
      {
        TimeUp = (int) payload[0] << 8 | (int) payload[1],
        TimeDown = (int) payload[2] << 8 | (int) payload[3],
        LowToHighEnabled = ((uint) payload[4] & 1U) > 0U,
        HighToLowEnabled = ((uint) payload[4] & 2U) > 0U
      };
    }

    public byte[] serialze()
    {
      return new byte[5]
      {
        (byte) ((this.TimeUp & 65280) >> 8),
        (byte) (this.TimeUp & (int) byte.MaxValue),
        (byte) ((this.TimeDown & 65280) >> 8),
        (byte) (this.TimeDown & (int) byte.MaxValue),
        (byte) ((this.HighToLowEnabled ? 2 : 0) | (this.LowToHighEnabled ? 1 : 0))
      };
    }
  }
}
