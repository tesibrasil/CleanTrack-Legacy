using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class Response : ObjectSerializeHelper
    {
        [DataMember]
        public bool Successed { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
