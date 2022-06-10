using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdGetDevicesList : Request
	{
		[DataMember]
		public int? SiteId { get; set; }
		[DataMember]
		public int? UoId { get; set; }
		[DataMember]
		public string DeviceName { get; set; } = "";
		[DataMember]
		public bool IncludeDismissed { get; set; } = false;
	}
}
