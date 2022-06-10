using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
	public class CmdGetCurrentExamCycleId : Request
	{
        [DataMember]
        public int ExamId { get; set; }
        [DataMember]
        public int SiteId { get; set; }
        [DataMember]
        public int UoId { get; set; }
    }
}
