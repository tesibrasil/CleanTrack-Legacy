// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.RfidDefs
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;

namespace It.IDnova.Fxw
{
  public class RfidDefs
  {
    private static DateTime expirationDate = new DateTime(2019, 9, 1, 0, 0, 0);
    public const byte TAG_EPC_LEN = 12;
    public const byte TAG_EPC_MAX_LEN = 62;
    public const byte EPC_ACCESS_PWD_LEN = 4;
    public const byte FXW_ISO_BLOCKLEN_BYTES = 4;
    public const byte FXW_TYPE_TAGID = 224;
    public const byte FXW_COMM_ERROR = 225;
    public const byte FXW_TYPE_INPUT_CHANGE = 229;
    public const byte FXW_TYPE_245_TAG = 230;
    public const byte FXW_TYPE_BULK = 231;
    public const byte FXW_TYPE_GPIO_INPUT_CHANGE = 234;
    public const byte FXW_TYPE_ERRCODE = 255;
    public const byte FXW_RES_OK = 0;
    public const byte FXW_RES_ERR_NOT_VALID = 1;
    public const byte FXW_RES_ERR_ARG = 2;
    public const byte FXW_RES_ERR_OPEN = 3;
    public const byte FXW_RES_ERR_SEND = 4;
    public const byte FXW_RES_TBD = 254;
    public const byte FXW_RES_ERR_GENERIC = 255;
    internal const byte FXW_PROTO_SOF = 42;
    internal const byte ERR_CODE_MASK = 1;
    internal const byte FXW_MSG_MIN_LEN = 5;
    internal const byte FXW_PROTO_FLAGS_OK = 0;
    internal const byte FXW_PROTO_FLAGS_245_TRANSP_WAIT_REPLY = 16;
    internal const byte FXW_PROTO_FLAGS_WRITE = 128;
    internal const byte FXW_PROTO_ISO_RFU = 0;
    internal const byte FXW_PROTO_ISO_READ_ID = 38;
    internal const byte FXW_PROTO_ISO_FLAGS_DEFAULT = 2;
    internal const byte FXW_PROTO_ISO_FLAGS_ADDRESSED = 32;
    internal const byte FXW_PROTO_ISO_FLAGS_TAGIT = 64;
    internal const byte FXW_PROTO_ISO_FLAGS_ICODE = 0;
    internal const byte FXW_PROTO_ISO_INVENTORY = 1;
    internal const byte FXW_PROTO_ISO_RD_BLOCK = 32;
    internal const byte FXW_PROTO_ISO_WR_BLOCK = 33;

    internal static bool isLibValid()
    {
      bool flag = true;
      // if (DateTime.Compare(DateTime.Now, RfidDefs.expirationDate) == 1)
        // flag = false;
      return flag;
    }

    public enum TagHfType : byte
    {
      ICODE = 2,
      TAGIT = 66, // 0x42
    }

    public enum Tag245Type
    {
      NORMAL_AND_ANTITACKLING = 0,
      ANTITACKLING_CHECKED = 16, // 0x00000010
      TEMPERATURE = 32, // 0x00000020
    }

    public enum Tag245APType
    {
      ID_ONLY,
      TEMPERATURE,
      TEMPERATURE_LOG,
      TEMPERATURE_LOG_EXT,
    }

    public enum FxwCommands : byte
    {
      NOP = 0,
      SLEEP = 1,
      ANTICOLL_LOOP_START = 2,
      ANTICOLL_LOOP_STOP = 3,
      ANTICOLL_LOOP_START_TID = 4,
      ANTICOLL_LOOP_STOP_TID = 5,
      FIELD_ON = 64, // 0x40
      FIELD_OFF = 65, // 0x41
      TRANSPARENT_ISO = 74, // 0x4A
      TRANSPARENT_245 = 75, // 0x4B
      ANTICOLL = 77, // 0x4D
      SET_PARAM = 78, // 0x4E
      GET_PARAM = 79, // 0x4F
      IO_SET_OUTPUT = 80, // 0x50
      IO_GET_INPUT = 81, // 0x51
      SA_CONFIG_IO = 81, // 0x51
      DB_LOG = 96, // 0x60
      SYSTEM = 97, // 0x61
      DB_UTENTI = 98, // 0x62
      ALR245 = 99, // 0x63
      UHF = 99, // 0x63
      DB_FASCIA = 100, // 0x64
      BULK_TXFR = 101, // 0x65
    }

    public enum SystemFunctions : byte
    {
      INIT_DEV = 1,
    }

    public enum FxwUhfCmd : byte
    {
      WRITE_ID_EPCC1G2 = 1,
      READ_DATA_EPCC1G2 = 2,
      WRITE_DATA_EPCC1G2 = 3,
      APPLY_PARAM = 4,
      GET_SWITCH_PARAM = 5,
      SET_SWITCH_PARAM = 6,
      SET_ACCESS_PASSWORD = 7,
    }

    public enum Fxw245Cmd : byte
    {
      GET_SWITCH_PARAM = 5,
      SET_SWITCH_PARAM = 6,
    }

    public enum FxwParam : byte
    {
      RTC_TIME = 6,
      DEV_CODE = 7,
      DEV_STATO = 8,
      DEV_MODE = 9,
      DEV_DIAG_STATO = 10, // 0x0A
      FW_RELEASE = 11, // 0x0B
      TAG_CONF = 12, // 0x0C
      RF_SETTINGS = 13, // 0x0D
      DEV_TYPE = 14, // 0x0E
      IO_INPUT_SETTINGS = 15, // 0x0F
      RF_POWER = 15, // 0x0F
      START_FREQ = 16, // 0x10
      STOP_FREQ = 17, // 0x11
      CURR_FREQ = 18, // 0x12
      RETRY = 19, // 0x13
      HOPSEQ = 20, // 0x14
      MOD_INDEX = 21, // 0x15
      REG_MODE = 22, // 0x16
      REGION_TYPE = 23, // 0x17
      TAG_VALIDITY = 24, // 0x18
      OPERATING_MODE = 25, // 0x19
      ALR245_PRESENCE_ALARM_TIMEOUT_S = 33, // 0x21
      BATTERY_VOLTAGE = 112, // 0x70
      BATTERY_CURRENT = 113, // 0x71
      BATTERY_CAPACITY = 114, // 0x72
      BATTERY_TEMP = 115, // 0x73
      SENSIRION_TEMP = 116, // 0x74
      SENSIRION_HUMI = 117, // 0x75
      ALR245_STOP_EVENTS = 129, // 0x81
      ALR245_RF_OPTION = 130, // 0x82
      ALR245_RF_CH = 131, // 0x83
      ALR245_RF_POWER = 132, // 0x84
      ALR245_RF_ADD_LEN = 133, // 0x85
      ALR245_RF_TX_ADD = 134, // 0x86
      ALR245_RF_RX_ADD_P0 = 135, // 0x87
      ALR245_RF_PAY_LEN = 136, // 0x88
      ALR245_READER_TYPE = 137, // 0x89
      ALR245_TAG_VAL_TIME = 138, // 0x8A
      ALR245_RF_CH_1 = 139, // 0x8B
      ALR245_RF_CH_2 = 140, // 0x8C
      ALR245_ATTENUATOR_1_VALUE = 141, // 0x8D
      ALR245_ATTENUATOR_2_VALUE = 142, // 0x8E
      ALR245_RF_ATTENUATION = 143, // 0x8F
      ALR245_TEMP_THRESHOLD = 144, // 0x90
      ALR245AP_RF_CH_RX = 145, // 0x91
      ALR245AP_RF_CH_TX = 146, // 0x92
      ALR245AP_RF_WINTIME_RX = 147, // 0x93
      ALR245AP_RF_WINTIME_TX = 148, // 0x94
      ALR245AP_DYNAMIC_PAR = 149, // 0x95
      ALR245_SENSIBILITY_A = 150, // 0x96
      ALR245_SENSIBILITY_B = 151, // 0x97
      ALR245_SENSIBILITY_MODE = 152, // 0x98
      ALR245_HASH_BLACKLIST = 153, // 0x99
      ALR245_IOS_T_OK_ON = 163, // 0xA3
      ALR245_IOS_T_OK_OFF = 164, // 0xA4
      ALR245_IOS_T_ERROR_ON = 165, // 0xA5
      ALR245_SLEEPING_ALARM_TIME = 165, // 0xA5
      ALR245_IOS_T_ERROR_OFF = 166, // 0xA6
      ALR245_SLEEPING_TIME_ON = 167, // 0xA7
      ALR245_CHAIN_ALLOWED_ZEROS = 176, // 0xB0
      ALR245_CHAIN_ALLOWED_AT_LOOPS = 177, // 0xB1
    }

    public class Antenna245Config
    {
      private byte _status;
      private ushort _time;
      private byte _power;
      private byte _address;

      public Antenna245Config(byte[] rawData)
      {
        this._status = rawData[0];
        this._time = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          rawData[1],
          rawData[2]
        });
        this._power = rawData[3];
        this._address = rawData[4];
      }

      public Antenna245Config(byte antenna, bool enable, ushort time)
      {
        this._status = !enable ? (byte) 0 : (byte) 1;
        this._time = time;
        this._power = (byte) 0;
        if (antenna == (byte) 0)
          this._address = (byte) 64;
        if (antenna != (byte) 1)
          return;
        this._address = (byte) 128;
      }

      public byte getAntenna()
      {
        return this._address == (byte) 64 ? (byte) 0 : (byte) 1;
      }

      public bool isEnabled()
      {
        return this._status == (byte) 1;
      }

      public ushort getTime()
      {
        return this._time;
      }

      internal byte[] getRawData()
      {
        byte[] bigEndianBytes = RfidUtils.ushortToBigEndianBytes(this._time);
        return new byte[5]
        {
          this._status,
          bigEndianBytes[0],
          bigEndianBytes[1],
          this._power,
          this._address
        };
      }
    }

    public enum PollingType : byte
    {
      HF_UHF_SINGLE = 0,
      UHF_MULTI = 2,
      UNIQUE_125KHZ = 16, // 0x10
      T5552_125KHZ = 32, // 0x20
      UNKNOWN = 255, // 0xFF
    }

    public enum MemoryBank : byte
    {
      EPC = 1,
      TID = 2,
      USER = 3,
    }

    public enum FxwProtoRes : byte
    {
      OK_ERR_NONE = 0,
      ERR_GENERIC = 1,
      ERR_NOT_SUPPORTED = 2,
      ERR_NOT_FOUND = 3,
      ERR_WAIT = 4,
      ERR_WRONG_CMD = 16, // 0x10
      ERR_WRONG_PAR = 17, // 0x11
      ERR_WRONG_FUNCT = 18, // 0x12
      ERR_WRONG_VALUE = 19, // 0x13
      ERR_WRONG_TYPE = 20, // 0x14
      ERR_WRONG_FLAGS = 21, // 0x15
      ERR_EMPTY = 32, // 0x20
      ERR_FULL = 33, // 0x21
      ERR_OVERFLOW = 34, // 0x22
      ERR_FRAME = 49, // 0x31
      ERR_TIMEOUT = 50, // 0x32
      ERR_CRC = 51, // 0x33
      ERR_RECEIVE = 52, // 0x34
      ERR_TRANSMIT = 53, // 0x35
      ERR_MEM_ACC = 64, // 0x40
      ERR_MEM_WRITE = 65, // 0x41
      ERR_MEM_READ = 66, // 0x42
    }

    internal enum FxwWeekDay : byte
    {
      HOLIDAY = 7,
    }

    internal enum FxwUserCmd : byte
    {
      ADD = 1,
      DEL = 2,
      GET = 3,
      CLEAR_DB = 4,
      COUNT = 5,
      GET_N = 6,
    }

    internal enum FxwDBlogCmd : byte
    {
      READ_CURRENT = 1,
      DEL_CURRENT = 2,
      CLEAR = 3,
    }

    internal enum FxwBulkTxfrCmd : byte
    {
      BEGIN_UPLOAD = 1,
      UPLOAD_BLOCK = 2,
      END_UPLOAD = 3,
    }

    internal enum FxwBulkTxfrFlags : byte
    {
      NORMAL_USERS_BLOCK = 0,
      END_TXFR_HASH = 128, // 0x80
    }

    internal enum FxwTimeframeCmd : byte
    {
      WRITE = 1,
      READ = 2,
      INS_HOLIDAY = 3,
      HOLI_TBL_READ = 4,
      HOLI_TBL_CLEAR = 5,
    }
  }
}
