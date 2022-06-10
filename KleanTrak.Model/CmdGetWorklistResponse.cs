using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdGetWorklistResponse : Response
    {
        public CmdGetWorklistResponse()
        {
            Items = new List<WorklistItem>();
        }
        [DataMember]
        public List<WorklistItem> Items { private set; get; }
    }
}
