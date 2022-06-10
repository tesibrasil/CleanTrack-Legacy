using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("4648D6DE-CB69-30C3-BC62-0F336597D6AA")]
    public class RFIDCycleEx : IRFIDCycleEx
	{
        public string Description { get; set; }
        public string WasherDescription { get; set; }

        public RFIDCycleStep[] Steps { get; set; }
    }
}
