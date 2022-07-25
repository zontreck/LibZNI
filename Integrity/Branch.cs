using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Integrity
{
    internal class Branch
    {
        private Leaf lA;
        private Leaf lB;

        public Branch(Leaf a, Leaf b)
        {
            lA = a;
            lB = b;
        }

        public byte[] GetNodeHash()
        {
            byte[] h1 = Tools.SHA256HashBytes(lA.InputBytes);
            byte[] h2 = Tools.SHA256HashBytes(lB.InputBytes);

            byte[] h3 = new byte[h1.Length + h2.Length];
            h1.CopyTo(h3, 0);
            h2.CopyTo(h3, h1.Length);

            byte[] b = Tools.SHA256HashBytes(Tools.SHA256HashBytes(h3));
            return b;
        }
        public override string ToString()
        {
            byte[] i = GetNodeHash();
            return Tools.Hash2String(i);
        }
    }
}
