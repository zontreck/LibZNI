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
        public override bool ReadTag(NBTReader br)
        {
            if(!(Parent!= null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            BArrVal = br.ReadBytes(br.ReadInt32());

            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new ByteArrayTag().ReadTag(br);
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
            return new ByteArrayTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }

    }
}
