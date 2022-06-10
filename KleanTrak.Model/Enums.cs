using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public enum BarcodeTypes
    {
        Unknown,
        Operator,
        Device,
        Operation,
        Accessory
    }

    public enum DeviceReadersTypes
    {
        Pi,
        Rfid
    }

    public enum WasherStorageTypes
    {
        Washer_IMS7 = 0,
		Washer_ISAWD = 1,
		Washer_Cantel_Medivators_XXX = 2,
		Washer_Steelco = 3,
		Washer_Mirth = 4,
		Washer_Cantel_RapidAir = 5,
		Storage_Cantel_EDC = 6,
		Washer_Steelco_ManualEnd = 7, // Sandro 04/09/2017 // BUG 834 //
		Washer_Cantel_Medivators_Serial = 8,
        Washer_ICT_DbConnect = 9, //washer con dati ciclo su db
        PreWasher_Cantel_MDG = 10, // pompe di lavaggio
        Washer_Cantel_AdvantagePassThrough_PV_3_0_0_16 = 11, // Washer Cantel Pass Through protocol version 3.0.0.16 and up (serial in 11th field, wrapped in '^' character)
        Storage_Cantel_EndoDry = 12, // storage cantel endodry 
        Washer_Cantel_AdvantagePassThrough_PV_3_0_0_16_Old = 13, // Washer Cantel Pass Through protocol version 3.0.0.16 and up (serial in 1st field)
		PreWasher_Steelco_EPW100 = 14
    }
}
