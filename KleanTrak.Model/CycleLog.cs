using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CycleLog
    {
        [DataMember]
        public string OperationBarcode { get; set; }

        [DataMember]
        public string OperationDescription { get; set; }

        [DataMember]
        public string UserBarcode { get; set; }

        [DataMember]
        public string UserDescription { get; set; }

        [DataMember]
        public DateTime OperationDateTime { get; set; }
    }
}
