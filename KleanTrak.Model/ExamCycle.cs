using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class ExamCycle
    {
        [DataMember]
        public int CycleId { set; get; }

        [DataMember]
        public string DeviceBarcode { set; get; }

        [DataMember]
        public string DeviceDescription { set; get; }

        [DataMember]
        public List<CycleLog> CycleLogs { get; set; }

        [DataMember]
        public bool Bypass { set; get; }

        [DataMember]
        public string BypassCause { set; get; }
    }
}
