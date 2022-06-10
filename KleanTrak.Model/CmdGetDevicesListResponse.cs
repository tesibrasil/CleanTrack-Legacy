using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KleanTrak.Model
{
	[DataContract]
	public class CmdGetDevicesListResponse : Response
	{
		[DataMember]
		public List<Device> Devices { get; set; } = new List<Device>();
	}
}
