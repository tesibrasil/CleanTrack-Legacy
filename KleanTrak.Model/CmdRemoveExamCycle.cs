using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdRemoveExamCycle : Request
	{
		[DataMember]
		public int ExamId { get; set; }
		[DataMember]
		public int ExamSiteId { get; set; }
		[DataMember]
		public int ExamUoId { get; set; }
		[DataMember]
		public string DeviceBarCode { get; set; }
		[DataMember]
		public string OperatorBarCode { get; set; }
	}
}
