using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class ByteArrayTag : Tag
    {
        private byte[] BArrVal;
        public byte[] Value
        {
            get
            {
                return BArrVal;
            }
        }
        
        public ByteArrayTag(string _Name, byte[] val)
        {
            Name = _Name;
            BArrVal = val;
        }
        public ByteArrayTag(byte[] boolVal) : this(null, boolVal)
        {
        }
        public ByteArrayTag() : this(null, new byte[] {}) { }

        public override TagType Type
        {
            get
            {
                return TagType.BYTEARRAY;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            int count = br.ReadInt32();
            BArrVal = br.ReadBytes(count);
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new BoolTag().ReadTag(br);
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

            bw.Write(Value);
        }

        public override object Clone()
        {
            return new ByteArrayTag(Name, Value);
        }
    }
}
