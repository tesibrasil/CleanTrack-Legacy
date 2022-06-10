using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("DEB8E4C2-5300-30B3-BC12-46AC325177E6")]
    public class RFIDCycleStep : IRFIDCycleStep
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string OperatorSurname { get; set; }

        public string OperatorName { get; set; }

        public string DateTime { get; set; }
    }
}
