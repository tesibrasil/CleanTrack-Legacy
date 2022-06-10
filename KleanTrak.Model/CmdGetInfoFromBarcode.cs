using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class CmdGetInfoFromBarcode : Request
    {
        public string Barcode { set; get; }
    }
}
