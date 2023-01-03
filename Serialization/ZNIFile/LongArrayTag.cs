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

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            int count = br.ReadVarInt();
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

        public override void SkipTag(NBTReader br)
        {
            _ = new LongArrayTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);
            bw.Write(Value.Length);

            foreach (long i in Value)
            {
                bw.Write(i);
            }
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);
        }

        public override object Clone()
        {
            return new LongArrayTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
