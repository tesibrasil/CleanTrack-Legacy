using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("312FC329-D499-363D-A7B4-1F142F5C15E4")]
    public interface IRFIDCycleStep
    {
        int ID { get; set; }

        string Description { get; set; }

        string OperatorSurname { get; set; }

        string OperatorName { get; set; }

        string DateTime { get; set; }
    }
}
