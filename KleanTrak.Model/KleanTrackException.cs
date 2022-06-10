using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class KleanTrackException : Exception
    {
        public KleanTrackException(string message)
            : base(message)
        {

        }
    }
}
