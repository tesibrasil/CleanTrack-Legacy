using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class ComboItem
    {
        public int Id { get; set; } = 0;
        public string Description { get; set; } = "";
        public override string ToString() => Description;
    }
}
