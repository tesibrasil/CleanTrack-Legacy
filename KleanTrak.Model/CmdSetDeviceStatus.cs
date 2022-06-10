using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdSetDeviceStatus : Request
    {
        [DataMember]
        public string OperationBarcode { set; get; }
        public string OperationID { set; get; }

        [DataMember]
        public bool? ForceState { get; set; } //Richiede di forzare il passaggio allo stato richiesto

        [DataMember]
        public string DeviceBarcode { set; get; }
        public string DeviceSerial { set; get; }

        [DataMember]
        public string UserBarcode { set; get; }
        public string UserSerial { set; get; }

        public int WorklistItemID { set; get; }        

        [DataMember]
        public int? ExamID { get; set; }
        [DataMember]
        public int? SiteId { get; set; }
        [DataMember]
        public int? UoId { get; set; }
    }
}
