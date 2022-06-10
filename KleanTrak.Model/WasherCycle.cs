using System;
using System.Collections.Generic;
using Commons;
using System.Reflection;

namespace KleanTrak.Model
{
	public class WasherCycle : ObjectSerializeHelper
    {
        public bool Completed { get; set; } = false;
        public bool Failed { get; set; } = false;
        public string WasherExternalID { get; set; }
        public int WasherID { get; set; }
        public string DeviceExternalID { get; set; }
        public int DeviceID { get; set; }
        public string OperatorStartExternalID { get; set; }
		public int OperatorStartID { get; set; } = -1;
        public string OperatorEndExternalID { get; set; }
		public int OperatorEndID { get; set; } = -1;
		public DateTime StartTimestamp { get; set; } = DateTime.MinValue;
		public DateTime EndTimestamp { get; set; } = DateTime.MinValue;
        public string StationName { get; set; }
        public string CycleCount { get; set; } = "";
        public string CycleType { get; set; }
        public string Filename { get; set; }
        public DateTime FileDatetime { get; set; }
        public string FileContent { get; set; }

		public bool IsStorage { get; set; } = false;
        public bool IsPreWash { get; set; } = false;

        public List<WasherCycleInfo> AdditionalInfoList = new List<WasherCycleInfo>();

		public int? DesiredDestinationState { get; set; } = null;

		// Sandro 04/09/2017 // BUG 834 //
		public bool bSteelcoFineLavaggioManuale = false;

		public enum ParsingErrors
        {
            NoError,
            MissingTranscode,
            DateWrong,
        }

        public WasherCycle Clone() 
        {
            var retval = new WasherCycle();
            foreach (PropertyInfo pi in typeof(WasherCycle).GetProperties())
                pi.SetValue(retval, pi.GetValue(this));
            return retval;
        }

        public override string ToString() => this.Stringify();

		public bool ValidData()
        {
            //adesso nel caso l'operatore non sia noto operatorstartid = 0 o null, si usa operatore sconosciuto.
            return WasherID > 0 && DeviceID > 0 && StartTimestamp > DateTime.MinValue;
            //return WasherID > 0 && DeviceID > 0 && OperatorStartID > 0 && StartTimestamp > DateTime.MinValue;
        }
    }
}
