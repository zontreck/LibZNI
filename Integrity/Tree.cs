

#define MERKLE_VERBOSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Integrity
{
    public class Tree
    {
        private List<Leaf> Leaves;
        public string MasterHash;
        public Tree()
        {
            Leaves = new List<Leaf>();
        }

        public void NewLeaf(byte[] b = null, string s = null)
        {
            if(b != null)
            {
                Leaves.Add(new Leaf(b));
                return;
            }
            if(s != null)
            {
                Leaves.Add(new Leaf(s));
                return;
            }

            throw new LeafCreationException("New leaf cannot be null!");
        }

        /// <summary>
        /// This process will lock the program up for a few moments while the tree is processed
        /// </summary>
        public void ProcessTree()
        {
            List<Branch> branches = new List<Branch>();
            if((Leaves.Count % 2) != 0)
            {
                Leaves.Add(Leaves.Last());
            }
            for(int i=0;i< Leaves.Count; i += 2)
            {
                Branch br = new Branch(Leaves[i], Leaves[i+1]);
                bool iDo = false;
#if MERKLE_VERBOSE
iDo = true;
#endif
                if(iDo)
                {
                    Console.WriteLine($"New Branch: {br}");
                }
                branches.Add(br);
            }
            while(branches.Count > 1)
            {
                if((branches.Count % 2) != 0)
                {
                    branches.Add(branches.Last());
                }
                Branch[] copy = branches.ToArray();
                branches.Clear();
                for(int i = 0; i < copy.Length; i += 2)
                {
                    Leaf a = new Leaf(copy[i].GetNodeHash());
                    Leaf b = new Leaf(copy[i + 1].GetNodeHash());
                    bool iDo = false;
                    Branch br = new Branch(a,b);
#if MERKLE_VERBOSE
iDo=true;
#endif
                    if (iDo)
                    {
                        Console.WriteLine($"New Branch: {br}");
                    }
                    branches.Add(br);
                }
            }
            MasterHash = branches[0].ToString();
        }
    }
}
