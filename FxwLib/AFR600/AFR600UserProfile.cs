using System;

namespace It.IDnova.Fxw.AFR600
{
  public class AFR600UserProfile
  {
    internal static readonly int MASK_CATEGORY = Convert.ToInt32("1110000000000000", 2);
    internal static readonly int MASK_CONTROL_MODE = Convert.ToInt32("0001000000000000", 2);
    internal static readonly int MASK_PROFILE_RELE2 = Convert.ToInt32("0000111000000000", 2);
    internal static readonly int MASK_PROFILE_RELE1 = Convert.ToInt32("0000000111000000", 2);
    internal static readonly int MASK_PROFILE_BUZZ = Convert.ToInt32("0000000000111000", 2);
    internal static readonly int MASK_INPUT_MODE = Convert.ToInt32("0000000000000100", 2);
    internal static readonly int MASK_INPUT_1 = Convert.ToInt32("0000000000000010", 2);
    internal static readonly int MASK_INPUT_0 = Convert.ToInt32("0000000000000001", 2);
    internal const int SHIFT_CATEGORY = 13;
    internal const int SHIFT_CONTROL_MODE = 12;
    internal const int SHIFT_PROFILE_RELE2 = 9;
    internal const int SHIFT_PROFILE_RELE1 = 6;
    internal const int SHIFT_PROFILE_BUZZER = 3;
    internal const int SHIFT_INPUT_MODE = 2;
    internal const int SHIFT_INPUT_1 = 1;
    internal const int SHIFT_INPUT_0 = 0;

    public int Category { get; set; }

    public AFR600UserProfile.CONTROL_MODE ControlMode { get; set; }

    public AFR600UserProfile.INPUT_CHECK_MODE InputCheckMode { get; set; }

    public int BuzzerProfile { get; set; }

    public int Rele1Profile { get; set; }

    public int Rele2Profile { get; set; }

    public bool checkInput0 { get; set; }

    public bool checkInput1 { get; set; }

    public static AFR600UserProfile parseProfile(ushort userProfile)
    {
      AFR600UserProfile afR600UserProfile = new AFR600UserProfile();
      afR600UserProfile.Category = ((int) userProfile & AFR600UserProfile.MASK_CATEGORY) >> 13;
      if (afR600UserProfile.Category < 0 || afR600UserProfile.Category > 7)
        return (AFR600UserProfile) null;
      afR600UserProfile.ControlMode = (AFR600UserProfile.CONTROL_MODE) (((int) userProfile & AFR600UserProfile.MASK_CONTROL_MODE) >> 12);
      afR600UserProfile.Rele2Profile = ((int) userProfile & AFR600UserProfile.MASK_PROFILE_RELE2) >> 9;
      afR600UserProfile.Rele1Profile = ((int) userProfile & AFR600UserProfile.MASK_PROFILE_RELE1) >> 6;
      afR600UserProfile.BuzzerProfile = ((int) userProfile & AFR600UserProfile.MASK_PROFILE_BUZZ) >> 3;
      afR600UserProfile.InputCheckMode = (AFR600UserProfile.INPUT_CHECK_MODE) (((int) userProfile & AFR600UserProfile.MASK_INPUT_MODE) >> 2);
      afR600UserProfile.checkInput1 = ((int) userProfile & AFR600UserProfile.MASK_INPUT_1) >> 1 == 1;
      afR600UserProfile.checkInput0 = ((int) userProfile & AFR600UserProfile.MASK_INPUT_0) == 1;
      return afR600UserProfile;
    }

    public ushort encodeProfile()
    {
      return (ushort) (this.Category << 13 & AFR600UserProfile.MASK_CATEGORY | (int) this.ControlMode << 12 & AFR600UserProfile.MASK_CONTROL_MODE | this.Rele2Profile << 9 & AFR600UserProfile.MASK_PROFILE_RELE2 | this.Rele1Profile << 6 & AFR600UserProfile.MASK_PROFILE_RELE1 | this.BuzzerProfile << 3 & AFR600UserProfile.MASK_PROFILE_BUZZ | (int) this.InputCheckMode << 2 & AFR600UserProfile.MASK_INPUT_MODE | (this.checkInput1 ? 1 : 0) << 1 & AFR600UserProfile.MASK_INPUT_1 | (this.checkInput0 ? 1 : 0) & AFR600UserProfile.MASK_INPUT_0);
    }

    public enum CONTROL_MODE
    {
      DEFAULT,
      PRESENCE_CONTROL,
    }

    public enum INPUT_CHECK_MODE
    {
      AT_LEAST_ONE,
      CHECK_BOTH,
    }
  }
}
