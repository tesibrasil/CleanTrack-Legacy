using System;
using System.Collections.Generic;
using System.Text;


namespace ISAWD_PARSER
{
    public class Cycle
    {
        private bool _Completed = false;

        public bool Completed
        {
            get { return _Completed; }
            set { _Completed = value; }
        }

        public string MachineID { get; set; }
        public string Station { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int CycleCount { get; set; }        
        public string OperatorID { get; set; }
        public string ScopeID { get; set; }
        public string Type { get; set; }
        public bool Failed { get; set; }
              
        public List<AdditionalInfo> AdditionalInfoList = new List<AdditionalInfo>();

        public string Dump()
        {
            try
            {
                return "Completed:" + Completed.ToString() + " - MachineID:" + MachineID +
                    " - Station:" + Station + " - Start:" + StartTimestamp.ToString() +
                    " - End:" + EndTimestamp.ToString() + " - CycleCount:" + CycleCount.ToString() +
                    " - OperatorID:" + OperatorID + " - ScopeID:" + ScopeID + " - Type:" + Type +
                    " - Failed:" + Failed.ToString();
            }
            catch (Exception)
            {
            }

            return "DumpError";
        }

    }
}
