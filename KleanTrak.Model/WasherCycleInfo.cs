using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class WasherCycleInfo
    {
        //public string ForeignKey { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
        public bool isAlarm { get; set; }

    }
}
