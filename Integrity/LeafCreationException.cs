using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Integrity
{
    internal class LeafCreationException : Exception
    {
        public LeafCreationException(string sMsg):base(sMsg)
        {

        }
    }
}
