using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdRemoveExamCycleResponse : Response
	{
		[DataMember]
		public string ResetStateName { get; set; }
		[DataMember]
		public bool NotLastCycle { get; set; } = false;
	}
}
