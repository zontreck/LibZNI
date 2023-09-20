using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
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

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            FloatVal = br.ReadInt16();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new ShortTag().ReadTag(br);
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
            return new ShortTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
