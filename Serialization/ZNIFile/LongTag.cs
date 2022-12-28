using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class LongTag : Tag
    {
        private long LongVal;
        public long Value
        {
            get
            {
                return LongVal;
            }
        }
        
        public LongTag(string _Name, long val)
        {
            Name = _Name;
            LongVal = val;
        }
        public LongTag(long _LongVal) : this(null, _LongVal)
        {
        }
        public LongTag() : this(null, 0) { }

        public override TagType Type
        {
            get
            {
                return TagType.LONG;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            LongVal = br.ReadInt64();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new LongTag().ReadTag(br);
        }

        public override void WriteData(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void WriteTag(BinaryWriter bw)
        {
            bw.Write(((int)Type));
            bw.Write(Name);

            bw.Write(Value);
        }

        public override object Clone()
        {
            return new LongTag(Name, Value);
        }
    }
}
