using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class ByteTag : Tag
    {
        private byte ByteVal;
        public byte Value
        {
            get
            {
                return ByteVal;
            }
        }
        
        public ByteTag(string _Name, byte val)
        {
            Name = _Name;
            ByteVal = val;
        }
        public ByteTag(byte _ByteVal) : this(null, _ByteVal)
        {
        }
        public ByteTag() : this(null, 0) { }

        public override TagType Type
        {
            get
            {
                return TagType.BYTE;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            ByteVal = br.ReadByte();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new ByteTag().ReadTag(br);
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
            return new ByteTag(Name, Value);
        }
    }
}
