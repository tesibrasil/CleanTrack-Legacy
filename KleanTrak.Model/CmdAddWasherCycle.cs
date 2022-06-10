using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class CmdAddWasherCycle : Request
    {
        public List<WasherCycle> WasherCycleList { set; get; }
    }
}
