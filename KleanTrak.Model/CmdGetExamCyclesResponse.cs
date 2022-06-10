using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
    [DataContract]
    public class CmdGetExamCyclesResponse : Response
    {
        [DataMember]
        public List<ExamCycle> ExamCycles { get; set; }
    }
}
