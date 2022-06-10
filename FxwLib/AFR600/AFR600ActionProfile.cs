using System;

namespace It.IDnova.Fxw.AFR600
{
  public class AFR600ActionProfile
  {
    internal static readonly int MASK_PROFILE_RELE2 = Convert.ToInt32("0000111000000000", 2);
    internal static readonly int MASK_PROFILE_RELE1 = Convert.ToInt32("0000000111000000", 2);
    internal static readonly int MASK_PROFILE_BUZZ = Convert.ToInt32("0000000000111000", 2);
    internal static readonly int MASK_USER_PROFILE = Convert.ToInt32("0000000000000100", 2);
    internal static readonly int MASK_EVENTS = Convert.ToInt32("0000000000000010", 2);
    internal static readonly int MASK_LOGS = Convert.ToInt32("0000000000000001", 2);
    internal const int SHIFT_PROFILE_RELE2 = 9;
    internal const int SHIFT_PROFILE_RELE1 = 6;
    internal const int SHIFT_PROFILE_BUZZER = 3;
    internal const int SHIFT_USER_PROFILE = 2;
    internal const int SHIFT_EVENTS = 1;
    internal const int SHIFT_LOGS = 0;

    public ushort RawProfile { get; set; }

    public int BuzzerProfile { get; set; }

    public int Rele1Profile { get; set; }

    public int Rele2Profile { get; set; }

    public bool EventsEnabled { get; set; }

    public bool LogsEnabled { get; set; }

    public AFR600ActionProfile.USER_PROFILE UserProfile { get; set; }

    public static AFR600ActionProfile parseProfile(ushort actionProfile)
    {
      return new AFR600ActionProfile()
      {
        RawProfile = actionProfile,
        Rele2Profile = ((int) actionProfile & AFR600ActionProfile.MASK_PROFILE_RELE2) >> 9,
        Rele1Profile = ((int) actionProfile & AFR600ActionProfile.MASK_PROFILE_RELE1) >> 6,
        BuzzerProfile = ((int) actionProfile & AFR600ActionProfile.MASK_PROFILE_BUZZ) >> 3,
        UserProfile = (AFR600ActionProfile.USER_PROFILE) (((int) actionProfile & AFR600ActionProfile.MASK_USER_PROFILE) >> 2),
        EventsEnabled = ((int) actionProfile & AFR600ActionProfile.MASK_EVENTS) >> 1 == 1,
        LogsEnabled = ((int) actionProfile & AFR600ActionProfile.MASK_LOGS) == 1
      };
    }

    public ushort encodeProfile()
    {
      return (ushort) (this.Rele2Profile << 9 & AFR600ActionProfile.MASK_PROFILE_RELE2 | this.Rele1Profile << 6 & AFR600ActionProfile.MASK_PROFILE_RELE1 | this.BuzzerProfile << 3 & AFR600ActionProfile.MASK_PROFILE_BUZZ | (int) this.UserProfile << 2 & AFR600ActionProfile.MASK_USER_PROFILE | (this.EventsEnabled ? 1 : 0) << 1 & AFR600ActionProfile.MASK_EVENTS | (this.LogsEnabled ? 1 : 0) & AFR600ActionProfile.MASK_LOGS);
    }

    public enum USER_PROFILE
    {
      USER,
      GENERIC,
    }

    public enum ACTIONS : ushort
    {
      USER_UNKNOWN,
      MODE_0_OK,
      MODE_0_TIME_INVALID,
      MODE_1_IN_RANGE_OK,
      MODE_1_IN_RANGE_TIME_INVALID,
      MODE_1_OUT_OF_RANGE_OK,
      MODE_1_OUT_OF_RANGE_TIME_INVALID,
      MODE_1_REREAD_OK,
      INPUT_NOT_SATISFIED,
      PHOTO_0_UP,
      PHOTO_0_DOWN,
      PHOTO_1_UP,
      PHOTO_1_DOWN,
    }
  }
}
