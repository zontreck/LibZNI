using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class IntArrayTag : Tag
    {
        private int[] BArrVal;
        public int[] Value
        {
            get
            {
                return BArrVal;
            }
        }
        
        public IntArrayTag(string _Name, int[] val)
        {
            Name = _Name;
            BArrVal = val;
        }
        public IntArrayTag(int[] boolVal) : this(null, boolVal)
        {
        }
        public IntArrayTag() : this(null, new int[] {}) { }

        public override TagType Type
        {
            get
            {
                return TagType.INTARRAY;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            int count = br.ReadInt32();
            BArrVal = new int[count];
            for(int i = 0; i < count; i++)
            {
                BArrVal[i] = br.ReadInt32();
            }

            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new IntArrayTag().ReadTag(br);
        }

        public override void WriteData(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void WriteTag(BinaryWriter bw)
        {
            bw.Write(((int)Type));
            bw.Write(Name);
            bw.Write(Value.Length);

            foreach(int i in Value)
            {
                bw.Write(i);
            }
        }

        public override object Clone()
        {
            return new IntArrayTag(Name, Value);
        }
    }
}
