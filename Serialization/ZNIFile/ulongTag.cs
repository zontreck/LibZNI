using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
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

        public override bool ReadTag(NBTReader br)
        {
            throw new Exception("Not allowed"); // This type must be virtual casted
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new uLongTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            Folder vCast = new Folder(Name);
            vCast.Add(new ByteTag("_virtcast_", (byte)Type));
            vCast.Add(new StringTag("val", Value.ToString()));
            if (Parent != null)
            {
                if (Parent.Type == TagType.LIST)
                {
                    vCast.Add(new StringTag("name", Name));
                }
            }
            vCast.WriteTag(bw);
            vCast.WriteData(bw);
        }

        public override void WriteTag(NBTWriter bw)
        {

        }

        public override object Clone()
        {
            return new uLongTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            if (!F.HasNamedTag("name"))
                Name = F.Name;
            else Name = F["name"].StringValue;
            LongVal = ulong.Parse(F["val"].StringValue);
        }
    }
}
