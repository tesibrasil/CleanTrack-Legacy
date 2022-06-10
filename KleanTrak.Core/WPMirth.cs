using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleanTrak.Model;

namespace KleanTrak.Core
{
    public class WPMirth : WPBase
    {
        public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
            return null;
        }

        public void Prepare(WasherCycle cycle)
        {
            cycle.WasherID = TranscodeWasher(cycle.WasherExternalID);

            cycle.DeviceID = TranscodeDevice(cycle.DeviceExternalID);
            if (cycle.DeviceID <= 0)
                cycle.DeviceID = TranscodeDevice("UNKNOWN");

            cycle.OperatorStartID = TranscodeOperator(cycle.OperatorStartExternalID);
            if (cycle.OperatorStartID <= 0)
                cycle.OperatorStartID = TranscodeOperator("UNKNOWN");

            cycle.OperatorEndID = TranscodeOperator(cycle.OperatorEndExternalID);
            if (cycle.OperatorEndID <= 0)
                cycle.OperatorEndID = TranscodeOperator("UNKNOWN");
        }
    }
}
