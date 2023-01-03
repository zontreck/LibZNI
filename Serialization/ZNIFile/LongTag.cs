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

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            LongVal = br.ReadInt64();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new LongTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);

            bw.Write(Value);
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);
        }

        public override object Clone()
        {
            return new LongTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
