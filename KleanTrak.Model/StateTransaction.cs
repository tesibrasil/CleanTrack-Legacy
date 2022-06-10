using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class StateTransaction
    {
        public int ID { set; get; }

        public int IDStateOld { set; get; }

        public string StateOld { set; get; }

        public int IDStateNew { set; get; }

        public string StateNew { set; get; }

        public bool InsertNewCycle { set; get; } = false;
		public int Id_sede { set; get; } = -1;
    }
}
