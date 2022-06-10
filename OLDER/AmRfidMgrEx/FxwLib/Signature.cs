// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Signature
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

namespace It.IDnova.Fxw
{
  internal class Signature
  {
    private const int POLY_ISO15693 = 33800;
    private const int PRESET_ISO15693 = 65535;

    public static ushort SignatureCalcHfApriporta(byte[] buffer)
    {
      ushort num1 = 33800;
      ushort num2 = ushort.MaxValue;
      int length = buffer.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        num2 ^= (ushort) buffer[index1];
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((int) num2 & 1) != 0)
            num2 = (ushort) ((uint) num2 >> 1 ^ (uint) num1);
          else
            num2 >>= 1;
        }
      }
      return (ushort)~num2;
    }
  }
}
