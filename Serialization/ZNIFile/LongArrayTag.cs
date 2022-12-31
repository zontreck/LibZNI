using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class LongArrayTag : Tag
    {
        private long[] BArrVal;
        public long[] Value
        {
            get
            {
                return BArrVal;
            }
        }
        
        public LongArrayTag(string _Name, long[] val)
        {
            Name = _Name;
            BArrVal = val;
        }
        public LongArrayTag(long[] boolVal) : this(null, boolVal)
        {
        }
        public LongArrayTag() : this(null, new long[] {}) { }

        public override TagType Type
        {
            get
            {
                return TagType.LONGARRAY;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            int count = br.ReadInt32();
            BArrVal = new long[count];
            for(int i = 0; i < count; i++)
            {
                BArrVal[i] = br.ReadInt64();
            }

            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new LongArrayTag().ReadTag(br);
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
            return new LongArrayTag(Name, Value);
        }
    }
}
