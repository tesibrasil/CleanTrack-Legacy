using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class Reader
    {
        public int ID { get; set; }
		public string Description { get; set; }
		public string IP { get; set; }
		public int Port { get; set; }
		public int DefaultStateID { get; set; }
		public DeviceReadersTypes Type { get; set; }
		public bool Deleted { get; set; }
		public int Timeout { get; set; }
    }
}
