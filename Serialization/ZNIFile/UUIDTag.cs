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
        public static UUIDTag Random(string sName)
        {
            UUIDTag rnd = new UUIDTag(sName, Guid.NewGuid());

            return rnd;
        }

        public static UUIDTag Empty(string sName)
        {
            UUIDTag z = new UUIDTag(sName, Guid.Empty);
            return z;
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

        public override bool ReadTag(NBTReader br)
        {
            throw new Exception("Must be virtcasted!");
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new UUIDTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            Folder vCast = new Folder(Name);
            vCast.Add(new ByteTag("_virtcast_", (byte)Type));
            vCast.Add(new ByteArrayTag("val", Value.ToByteArray()));
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
            return new UUIDTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            if (!F.HasNamedTag("name"))
                Name = F.Name;
            else Name = F["name"].StringValue;
            ByteArrayTag bat = F["val"] as ByteArrayTag;
            LongVal = new Guid(bat.Value);
        }
    }
}
