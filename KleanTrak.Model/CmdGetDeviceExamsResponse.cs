using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class DeviceExam
	{
		[DataMember]
		public int ExamId { get; set; }
		[DataMember]
		public int UoId { get; set; }
		[DataMember]
		public int SiteId { get; set; }
	}
	[DataContract]
	public class CmdGetDeviceExamsResponse : Response
	{
		[DataMember]
		public List<DeviceExam> DeviceExams { get; set; } = new List<DeviceExam>();
	}
}
