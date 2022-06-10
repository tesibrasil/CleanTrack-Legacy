using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdGetCurrentExamCycleIdResponse : Response
	{
		[DataMember]
		public int CycleId { get; set; }
	}
}
