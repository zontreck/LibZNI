using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Integrity
{
    internal class Leaf
    {
        public byte[] InputBytes;
        public Leaf(byte[] inputBytes)
        {
            InputBytes = inputBytes;
        }
        public Leaf(string input)
        {
            InputBytes = Encoding.UTF8.GetBytes(input);
        }
    }
}
