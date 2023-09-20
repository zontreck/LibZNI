using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
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

        public override bool ReadTag(NBTReader br)
        {

            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            ByteVal = br.ReadByte();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new ByteTag().ReadTag(br);
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
            return new ByteTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }

    }
}
