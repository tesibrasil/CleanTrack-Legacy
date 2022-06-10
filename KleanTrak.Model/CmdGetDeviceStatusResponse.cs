using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdGetDeviceStatusResponse : Response
    {
        [DataMember]
        public string Description { set; get; }
        [DataMember]
        public string Status { set; get; }
    }
}
