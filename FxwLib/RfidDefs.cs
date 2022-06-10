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
    public const byte FXW_TYPE_EXTENDED_TAGID = 227;
    public const byte FXW_TYPE_UWB_DISTANCE = 240;
    public const byte FXW_TYPE_UWB_EVENT = 241;
    public const byte FXW_TYPE_UWB_EVENT_TRANSPARENT = 242;
    public const byte FXW_TYPE_INPUT_CHANGE = 229;
    public const byte FXW_TYPE_245_TAG = 230;
    public const byte FXW_TYPE_BULK = 231;
    public const byte FXW_TYPE_245_TRANSPARENT_END = 232;
    public const byte FXW_TYPE_245_REPLY_JUNGHEINRICH = 233;
    public const byte FXW_TYPE_GPIO_INPUT_CHANGE = 234;
    public const byte FXW_TYPE_ID_SCADUTO = 235;
    public const byte FXW_TYPE_ID_PHOTO = 236;
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
      return true;
    }

    public enum TagHfType : byte
    {
      ICODE = 2,
      TAGIT = 66,
    }

    public enum Tag245Type
    {
      NORMAL_AND_ANTITACKLING = 0,
      ANTITACKLING_CHECKED = 16,
      TEMPERATURE = 32,
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
      JUNG_GET_PARAM = 16,
      JUNG_SET_PARAM = 17,
      EXT_PAR_SET = 56,
      EXT_PAR_GET = 57,
      FIELD_ON = 64,
      FIELD_OFF = 65,
      UWB_SET_PARAM = 71,
      UWB_GET_PARAM = 72,
      UWB_TRANSPARENT = 73,
      TRANSPARENT_ISO = 74,
      TRANSPARENT_245 = 75,
      RF_LINK = 76,
      ANTICOLL = 77,
      SET_PARAM = 78,
      GET_PARAM = 79,
      IO_SET_OUTPUT = 80,
      IO_GET_INPUT = 81,
      SA_CONFIG_IO = 81,
      DB_LOG = 96,
      SYSTEM = 97,
      DB_UTENTI = 98,
      ALR245 = 99,
      UHF = 99,
      DB_FASCIA = 100,
      BULK_TXFR = 101,
    }

    public enum SystemFunctions : byte
    {
      INIT_DEV = 1,
      RADIO_TESTS = 2,
      GET_ERROR_CODES = 3,
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

    public enum FxwUWBParam : byte
    {
      UWB_DEVICE_TYPE = 17,
      UWB_DEVICE_ID = 18,
      UWB_PREAMBLE_LEN = 19,
      UWB_TRANSMIT_PWR = 20,
      UWB_ANTENNA_DLY = 21,
      UWB_RF_CHANNEL = 22,
      UWB_TX_ID_RATE = 23,
      UWB_MIN_DISTANCE = 24,
      UWB_MAX_DISTANCE = 25,
    }

    public enum FxwJungParam : byte
    {
      MSG_NORMAL_SPEED = 1,
      MSG_LOW_SPEED = 2,
      OPERATOR_ID = 3,
      KART_ID = 4,
      TIMING_GATE = 5,
      TIMING_USER = 6,
      TX_ATTENUATION = 7,
      USER_TIMEOUT = 8,
      RATE_THRESHOLD = 9,
      RATE_WINDOW_MSEC = 10,
      TX_SETTINGS = 11,
      BUZZER_SIGNAL = 12,
      SLAVE_REPLY_DELAY = 13,
    }

    public enum FxwExtParam
    {
      EXTPAR_FW_RELEASE = 4,
      EXTPAR_RTC_TIME = 5,
      EXTPAR_DEV_CODE = 6,
      EXTPAR_DEV_TYPE = 7,
      EXTPAR_DIAG_STATO = 8,
      EXTPAR_UART_SPEED = 9,
      EXTPAR_RF_ADDRESS = 10,
      EXTPAR_RF_ADDRESS_LEN = 11,
      EXTPAR_RF_CHANNEL = 12,
      EXTPAR_RF_DATA_RATE = 13,
      EXTPAR_RF_PAYLOAD_LEN = 14,
      EXTPAR_RF_RX_MODE = 15,
      EXTPAR_RF_ATTENUATION = 16,
      EXTPAR_OUT_PROFILE = 17,
      EXTPAR_INPUT_CONFIG = 18,
      EXTPAR_INPUT_EVENT_VALIDITY = 19,
      EXTPAR_INPUT_READ = 20,
      EXTPAR_COMM_EVENT_MODE = 21,
      EXTPAR_PRESENCE_TIMEOUT = 22,
      EXTPAR_SA_ENABLED = 23,
      EXTPAR_EVENT_ENABLED = 24,
      EXTPAR_VALIDITY_CHECK = 25,
      EXTPAR_JUNG_MSG_RADIO_1 = 4097,
      EXTPAR_JUNG_MSG_RADIO_2 = 4098,
      EXTPAR_JUNG_TIMING_TX_1 = 4099,
      EXTPAR_JUNG_TIMING_TX_2 = 4100,
      EXTPAR_JUNG_TX_SETTINGS = 4101,
      EXTPAR_JUNG_CMD_LOW_SPEED = 4102,
      EXTPAR_JUNG_CMD_HIGH_SPEED = 4103,
      EXTPAR_JUNG_KART_ID = 4104,
      EXTPAR_JUNG_OPERATOR_ID = 4105,
      EXTPAR_JUNG_LAST_SPEED_CMD = 4106,
      EXTPAR_JUNG_LVL_1_WINDOW = 4107,
      EXTPAR_JUNG_LVL_2_WINDOW = 4108,
      EXTPAR_JUNG_LVL_1_THRESHOLD = 4109,
      EXTPAR_JUNG_LVL_2_THRESHOLD = 4110,
      EXTPAR_JUNG_USER_TIMEOUT = 4111,
      EXTPAR_JUNG_GATE_BLIND = 4112,
      EXTPAR_JUNG_FEEDBACK_TIME = 4113,
      EXTPAR_JUNG_EVALUATION_TIME = 4114,
      EXTPAR_JUNG_ACK_THRESHOLD = 4115,
      EXTPAR_JUNG_BATTERY_THRESHOLD = 4116,
      EXTPAR_JUNG_USER_RETRIGGER = 4117,
      EXTPAR_JUNG_ID_MASK = 4118,
      EXTPAR_JUNG_SLOW_PULSE = 4119,
      EXTPAR_JUNG_RADIO_RAID = 4120,
      EXTPAR_JUNG_LANE_MSEC = 4121,
      EXTPAR_LAST = 65535,
    }

    public enum FxwParam : byte
    {
      RTC_TIME = 6,
      DEV_CODE = 7,
      DEV_STATO = 8,
      DEV_MODE = 9,
      DEV_DIAG_STATO = 10,
      FW_RELEASE = 11,
      TAG_CONF = 12,
      RF_SETTINGS = 13,
      DEV_TYPE = 14,
      IO_INPUT_SETTINGS = 15,
      RF_POWER = 15,
      START_FREQ = 16,
      STOP_FREQ = 17,
      CURR_FREQ = 18,
      RETRY = 19,
      HOPSEQ = 20,
      MOD_INDEX = 21,
      REG_MODE = 22,
      REGION_TYPE = 23,
      TAG_VALIDITY = 24,
      OPERATING_MODE = 25,
      ALR245_PRESENCE_ALARM_TIMEOUT_S = 33,
      ANTITERRORISM = 34,
      BATTERY_VOLTAGE = 112,
      BATTERY_CURRENT = 113,
      BATTERY_CAPACITY = 114,
      BATTERY_TEMP = 115,
      SENSIRION_TEMP = 116,
      SENSIRION_HUMI = 117,
      ALR245_DATARATE = 128,
      ALR245_STOP_EVENTS = 129,
      ALR245_RF_OPTION = 130,
      ALR245_RF_CH = 131,
      ALR245_RF_POWER = 132,
      ALR245_RF_ADD_LEN = 133,
      ALR245_RF_TX_ADD = 134,
      ALR245_RF_RX_ADD_P0 = 135,
      ALR245_RF_PAY_LEN = 136,
      ALR245_READER_TYPE = 137,
      ALR245_TAG_VAL_TIME = 138,
      ALR245_RF_CH_1 = 139,
      ALR245_RF_CH_2 = 140,
      ALR245_ATTENUATOR_1_VALUE = 141,
      ALR245_ATTENUATOR_2_VALUE = 142,
      ALR245_RF_ATTENUATION = 143,
      ALR245_TEMP_THRESHOLD = 144,
      ALR245AP_RF_CH_RX = 145,
      ALR245AP_RF_CH_TX = 146,
      ALR245AP_RF_WINTIME_RX = 147,
      ALR245AP_RF_WINTIME_TX = 148,
      ALR245AP_DYNAMIC_PAR = 149,
      ALR245_SENSIBILITY_A = 150,
      ALR245_SENSIBILITY_B = 151,
      ALR245_SENSIBILITY_MODE = 152,
      ALR245_HASH_BLACKLIST = 153,
      ALR245_IOS_T_OK_ON = 163,
      ALR245_IOS_T_OK_OFF = 164,
      ALR245_IOS_T_ERROR_ON = 165,
      ALR245_SLEEPING_ALARM_TIME = 165,
      ALR245_IOS_T_ERROR_OFF = 166,
      ALR245_SLEEPING_TIME_ON = 167,
      ALR245_CHAIN_ALLOWED_ZEROS = 176,
      ALR245_CHAIN_ALLOWED_AT_LOOPS = 177,
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
        if ((int) antenna == 0)
          this._address = (byte) 64;
        if ((int) antenna == 1)
          this._address = (byte) 128;
        if ((int) antenna == 2)
          this._address = (byte) 64;
        if ((int) antenna != 3)
          return;
        this._address = (byte) 128;
      }

      public Antenna245Config(byte antenna, bool enable, ushort time, byte power)
      {
        this._status = !enable ? (byte) 0 : (byte) 1;
        this._time = time;
        this._power = power;
        if ((int) antenna == 0)
          this._address = (byte) 64;
        if ((int) antenna == 1)
          this._address = (byte) 128;
        if ((int) antenna == 2)
          this._address = (byte) 64;
        if ((int) antenna != 3)
          return;
        this._address = (byte) 128;
      }

      public byte getAntenna()
      {
        return (int) this._address == 64 ? (byte) 0 : (byte) 1;
      }

      public byte getPower()
      {
        return this._power;
      }

      public bool isEnabled()
      {
        return (int) this._status == 1;
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
      UNIQUE_125KHZ = 16,
      T5552_125KHZ = 32,
      UNKNOWN = 255,
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
      ERR_WRONG_CMD = 16,
      ERR_WRONG_PAR = 17,
      ERR_WRONG_FUNCT = 18,
      ERR_WRONG_VALUE = 19,
      ERR_WRONG_TYPE = 20,
      ERR_WRONG_FLAGS = 21,
      ERR_EMPTY = 32,
      ERR_FULL = 33,
      ERR_OVERFLOW = 34,
      ERR_FRAME = 49,
      ERR_TIMEOUT = 50,
      ERR_CRC = 51,
      ERR_RECEIVE = 52,
      ERR_TRANSMIT = 53,
      ERR_MEM_ACC = 64,
      ERR_MEM_WRITE = 65,
      ERR_MEM_READ = 66,
    }

    internal enum FxwWeekDay : byte
    {
      HOLIDAY = 7,
    }

    public enum FxwUserCmd : byte
    {
      ADD = 1,
      DEL = 2,
      GET = 3,
      CLEAR_DB = 4,
      COUNT = 5,
      GET_N = 6,
      SET_NEW = 7,
      DEL_NEW = 8,
      GET_NEW = 9,
      GET_ALL = 10,
    }

    internal enum FxwDBlogCmd : byte
    {
      READ_CURRENT = 1,
      DEL_CURRENT = 2,
      CLEAR = 3,
      READ_AND_DELETE = 4,
      GET_N = 5,
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
      END_TXFR_HASH = 128,
    }

    public enum FxwTimeframeCmd : byte
    {
      WRITE = 1,
      READ = 2,
      INS_HOLIDAY = 3,
      HOLI_TBL_READ = 4,
      HOLI_TBL_CLEAR = 5,
      NEW_SET = 6,
      NEW_GET = 7,
    }
  }
}
