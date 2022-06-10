using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KleanTrak.Model
{
    public class CmdSetPiConfiguration : Request
    {
        public PiConfiguration Configuration { set; get; }
    }
}
