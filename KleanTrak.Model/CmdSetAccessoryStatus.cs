using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class CmdSetAccessoryStatus : Request
    {
        public string AccessoryBarcode { set; get; }

        public string UserBarcode { set; get; }

        public int WorklistItemID { set; get; }
    }
}
