using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class WorklistItem
    {
        public int ID { set; get; }

        public DateTime? Date { set; get; }

        public string Patient { set; get; }

        public DateTime? BirthDate { set; get; }

        public string Description { set; get; }
    }
}
