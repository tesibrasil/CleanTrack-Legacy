using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KleanTrack.License
{
    public class UoClaims
    {
        public int Iduo = 0;
        public Dictionary<int, string> Sedi = new Dictionary<int, string>(); // < id , description >
        public Dictionary<string, int> Claims = new Dictionary<string, int>();
    }
}
