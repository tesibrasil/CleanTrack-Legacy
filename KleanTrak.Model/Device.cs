using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class Device
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
        public int StateID { get; set; }
        public string StateDescription { get; set; }
		public int Id_sede { get; private set; }
		public Device(int id_sede) => Id_sede = id_sede;
    }
}
