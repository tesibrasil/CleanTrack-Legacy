using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class CmdGetDeviceStatus : Request
    {
        public string DeviceBarcode { set; get; }

        public string DeviceSerial { set; get; }
    }
}
