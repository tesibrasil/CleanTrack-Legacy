using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdPing : Request
    {
        [DataMember]
        public string Attachment { set; get; }
    }
}
