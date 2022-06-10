using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdGetInfoFromBarcodeResponse : Response
    {
        [DataMember]
        public BarcodeTypes BarcodeType { set; get; }
        [DataMember]
        public string Description { set; get; }
    }
}
