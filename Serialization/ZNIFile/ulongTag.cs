using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class uLongTag : Tag
    {
        private ulong LongVal;
        public ulong Value
        {
            get
            {
                return LongVal;
            }
        }
        
        public uLongTag(string _Name, ulong val)
        {
            Name = _Name;
            LongVal = val;
        }
        public uLongTag(ulong _LongVal) : this(null, _LongVal)
        {
        }
        public uLongTag() : this(null, 0) { }

        public override TagType Type
        {
            get
            {
                return TagType.ULONG;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            LongVal = br.ReadUInt64();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new uLongTag().ReadTag(br);
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
            return new uLongTag(Name, Value);
        }
    }
}
