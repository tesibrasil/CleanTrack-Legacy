using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdGetDeviceExams : Request
	{
		[DataMember]
		public int DeviceId { get; set; }
		[DataMember]
		public int UoId { get; set; }
	}
}
