using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class UUIDTag : Tag
    {
        private Guid LongVal;
        public Guid Value
        {
            get
            {
                return LongVal;
            }
        }
        
        public UUIDTag(string _Name, Guid val)
        {
            Name = _Name;
            LongVal = val;
        }
        public UUIDTag(Guid _LongVal) : this(null, _LongVal)
        {
        }
        public UUIDTag() : this(null, Guid.Empty) { }

        public override TagType Type
        {
            get
            {
                return TagType.UUID;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            LongVal = Guid.Parse(br.ReadString());
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new UUIDTag().ReadTag(br);
        }

        public override void WriteData(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void WriteTag(BinaryWriter bw)
        {
            bw.Write(((int)Type));
            bw.Write(Name);

            bw.Write(Value.ToString());
        }

        public override object Clone()
        {
            return new UUIDTag(Name, Value);
        }
    }
}
