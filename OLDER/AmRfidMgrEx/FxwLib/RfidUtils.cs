// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.RfidUtils
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;
using System.Text.RegularExpressions;

namespace It.IDnova.Fxw
{
  public class RfidUtils
  {
    public static byte[] concatArray(byte[] array1, byte[] array2)
    {
      byte[] numArray = new byte[array1.Length + array2.Length];
      Buffer.BlockCopy((Array) array1, 0, (Array) numArray, 0, array1.Length);
      Buffer.BlockCopy((Array) array2, 0, (Array) numArray, array1.Length, array2.Length);
      return numArray;
    }

    public static byte[] ushortToBigEndianBytes(ushort value)
    {
      return new byte[2]
      {
        (byte) ((uint) value >> 8),
        (byte) value
      };
    }

    public static byte[] ushortToLittleEndianBytes(ushort value)
    {
      return new byte[2]
      {
        (byte) value,
        (byte) ((uint) value >> 8)
      };
    }

    public static byte[] intToBigEndianBytes(int value)
    {
      return new byte[4]
      {
        (byte) (value >> 24),
        (byte) (value >> 16),
        (byte) (value >> 8),
        (byte) value
      };
    }

    public static byte[] intToLittleEndianBytes(int value)
    {
      return new byte[4]
      {
        (byte) value,
        (byte) (value >> 8),
        (byte) (value >> 16),
        (byte) (value >> 24)
      };
    }

    public static byte[] longToBigEndianBytes(long value)
    {
      return new byte[8]
      {
        (byte) (value >> 56),
        (byte) (value >> 58),
        (byte) (value >> 40),
        (byte) (value >> 32),
        (byte) (value >> 24),
        (byte) (value >> 16),
        (byte) (value >> 8),
        (byte) value
      };
    }

    public static byte[] asciiStringToByteArray(string s)
    {
      byte[] numArray = new byte[s.Length];
      int index = 0;
      foreach (char ch in s)
      {
        numArray[index] = (byte) ch;
        ++index;
      }
      return numArray;
    }

    public static string byteArrayToAsciiString(byte[] byteArray)
    {
      string str = "";
      foreach (byte num in byteArray)
        str += (string) (object) (char) num;
      return str;
    }

    public static string byteArrayToHexString(byte[] byteArray)
    {
      string str = "";
      foreach (byte num in byteArray)
        str += num.ToString("X2");
      return str;
    }

    public static byte[] hexStringToByteArray(string s)
    {
      if (s.Length != 0)
      {
        if (s.Length % 2 == 0)
        {
          try
          {
            byte[] numArray = new byte[s.Length / 2];
            for (int index = 0; index < numArray.Length; ++index)
              numArray[index] = Convert.ToByte(s.Substring(index * 2, 2), 16);
            return numArray;
          }
          catch (Exception ex)
          {
            return (byte[]) null;
          }
        }
      }
      return (byte[]) null;
    }

    public static ushort[] hexStringToUshortArray(string s)
    {
      if (s.Length != 0)
      {
        if (s.Length % 4 == 0)
        {
          try
          {
            ushort[] numArray = new ushort[s.Length / 4];
            for (int index = 0; index < numArray.Length; ++index)
              numArray[index] = Convert.ToUInt16(s.Substring(index * 4, 4), 16);
            return numArray;
          }
          catch (Exception ex)
          {
            return (ushort[]) null;
          }
        }
      }
      return (ushort[]) null;
    }

    public static ushort bigEndianBytesToUInt16(byte[] data)
    {
      if (data.Length != 2)
        return 0;
      return (ushort) (((uint) data[0] << 8) + (uint) data[1]);
    }

    public static uint bigEndianBytesToUInt32(byte[] data)
    {
      if (data.Length != 4)
        return 0;
      return (uint) (((int) data[0] << 24) + ((int) data[1] << 16) + ((int) data[2] << 8)) + (uint) data[3];
    }

    public static ulong bigEndianBytesToUInt64(byte[] data)
    {
      if (data.Length != 8)
        return 0;
      return (ulong) (((long) data[0] << 56) + ((long) data[1] << 48) + ((long) data[2] << 40) + ((long) data[3] << 32) + ((long) data[4] << 24) + ((long) data[5] << 16) + ((long) data[6] << 8)) + (ulong) data[7];
    }

    public static ushort littleEndianBytesToUint16(byte[] data)
    {
      if (data.Length != 2)
        return 0;
      return (ushort) (((uint) data[1] << 8) + (uint) data[0]);
    }

    public static short littleEndianBytesToInt16(byte[] data)
    {
      if (data.Length != 2)
        return 0;
      return (short) (((int) data[1] << 8) + (int) data[0]);
    }

    public static uint littleEndianBytesToUInt32(byte[] data)
    {
      if (data.Length != 4)
        return 0;
      return (uint) (((int) data[3] << 24) + ((int) data[2] << 16) + ((int) data[1] << 8)) + (uint) data[0];
    }

    public static ulong littleEndianBytesToUInt64(byte[] data)
    {
      if (data.Length != 8)
        return 0;
      return (ulong) (((long) data[7] << 56) + ((long) data[6] << 48) + ((long) data[5] << 40) + ((long) data[4] << 32) + ((long) data[3] << 24) + ((long) data[2] << 16) + ((long) data[1] << 8)) + (ulong) data[0];
    }

    public static byte encodeBcd(byte aByte)
    {
      return (byte) ((uint) (byte) ((uint) aByte % 10U) | (uint) (byte) ((int) aByte / 10 << 4));
    }

    public static string decodeBcd(byte bcd)
    {
      return ((byte) (((int) bcd & 240) >> 4)).ToString() + ((byte) ((uint) bcd & 15U)).ToString();
    }

    public static byte byteTime(byte hh, byte mm)
    {
      byte num = 0;
      if ((int) hh < 24 || (int) mm < 60)
        num = (byte) ((uint) (byte) ((uint) hh << 3) | (uint) (byte) ((uint) mm / 10U));
      return num;
    }

    public static short twoComplement14BitsToShort(ushort valueIn2Compl14Bits)
    {
      if (((int) valueIn2Compl14Bits & 8192) != 0)
        return (short)(-(short) ((int) ~valueIn2Compl14Bits + 1 & 16383));
      return (short) valueIn2Compl14Bits;
    }

    public static bool isHexString(string s)
    {
      return Regex.IsMatch(s, "\\A\\b[0-9a-fA-F]+\\b\\Z");
    }
  }
}
