using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class ShortTag : Tag
    {
        private short FloatVal;
        public short Value
        {
            get
            {
                return FloatVal;
            }
        }
        
        public ShortTag(string _Name, short val)
        {
            Name = _Name;
            FloatVal = val;
        }
        public ShortTag(short _FloatVal) : this(null, _FloatVal)
        {
        }
        public ShortTag() : this(null, 0) { }

        public override TagType Type
        {
            get
            {
                return TagType.SHORT;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            FloatVal = br.ReadInt16();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new ShortTag().ReadTag(br);
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
            return new ShortTag(Name, Value);
        }
    }
}
