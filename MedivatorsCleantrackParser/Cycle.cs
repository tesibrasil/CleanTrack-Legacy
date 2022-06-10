using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MedivatorsCleantrackParser
{
	public class Cycle
	{
		public string Key { get; set; }
		public int MachineID { get; set; }
		public string Station { get; set; }
		public DateTime StartTimestamp { get; set; }
		public DateTime EndTimestamp { get; set; }
		public int CycleCount { get; set; }
		public int DisinfectantCycleCount { get; set; }
		public int OperatorID { get; set; }
		public int PatientID { get; set; }
		public int PhysicianID { get; set; }
		public int ScopeID { get; set; }
		public string SSGRatio { get; set; }
		public DateTime CreateDateTime { get; set; }

		public List<AdditionalInfo> AdditionalInfoList = new List<AdditionalInfo>();

	}
}
