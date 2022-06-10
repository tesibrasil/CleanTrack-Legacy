namespace It.IDnova.Fxw.AFR600
{
  public class AFR600OutputProfile
  {
    public const int RAW_PROFILE_LENGTH = 12;

    public AFR600OutputProfile.OUTPUT_MODE OutputMode { get; set; }

    public AFR600OutputProfile.TIMER_MODE TimerMode { get; set; }

    public int OnPeriod { get; set; }

    public int OffPeriod { get; set; }

    public int NRepeat { get; set; }

    public static AFR600OutputProfile parseProfile(byte[] rawProfile)
    {
      if (rawProfile.Length != 12)
        return (AFR600OutputProfile) null;
      return new AFR600OutputProfile()
      {
        OutputMode = (AFR600OutputProfile.OUTPUT_MODE) rawProfile[1],
        TimerMode = (AFR600OutputProfile.TIMER_MODE) rawProfile[2],
        OnPeriod = (int) rawProfile[3] << 24 | (int) rawProfile[4] << 16 | (int) rawProfile[5] << 8 | (int) rawProfile[6],
        OffPeriod = (int) rawProfile[7] << 24 | (int) rawProfile[8] << 16 | (int) rawProfile[9] << 8 | (int) rawProfile[10],
        NRepeat = (int) rawProfile[11]
      };
    }

    public byte[] encodeProfile()
    {
      return new byte[12]
      {
        (byte) 0,
        (byte) this.OutputMode,
        (byte) this.TimerMode,
        (byte) (((long) this.OnPeriod & 4278190080L) >> 24),
        (byte) ((this.OnPeriod & 16711680) >> 16),
        (byte) ((this.OnPeriod & 65280) >> 8),
        (byte) (this.OnPeriod & (int) byte.MaxValue),
        (byte) (((long) this.OffPeriod & 4278190080L) >> 24),
        (byte) ((this.OffPeriod & 16711680) >> 16),
        (byte) ((this.OffPeriod & 65280) >> 8),
        (byte) (this.OffPeriod & (int) byte.MaxValue),
        (byte) this.NRepeat
      };
    }

    public enum TIMER_MODE : byte
    {
      NORMAL,
      RETRIGGERABLE,
    }

    public enum OUTPUT_MODE : byte
    {
      ON,
      OFF,
      BLINK,
      ONESHOT,
      N_SHOT,
      BLINK_DUTY,
      ACTIVATE,
    }
  }
}
