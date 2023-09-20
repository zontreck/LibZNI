using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
{
    public class BoolTag : Tag
    {
        private bool BoolVal;
        public bool Value
        {
            get
            {
                return BoolVal;
            }
        }
        
        public BoolTag(string _Name, bool val)
        {
            Name = _Name;
            BoolVal = val;
        }
        public BoolTag(bool boolVal) : this(null, boolVal)
        {
        }
        public BoolTag() : this(null, false) { }

        public override TagType Type
        {
            get
            {
                return TagType.BOOL;
            }
        }
        public override bool ReadTag(NBTReader br)
        {
            throw new NotImplementedException();
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new BoolTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            Folder vFolder = new Folder(Name);
            vFolder.Add(new ByteTag("val", (byte)(Value ? 1 : 0)));
            vFolder.Add(new ByteTag("_virtcast_", (byte)Type));
            if(Parent != null)
            {
                if(Parent.Type == TagType.LIST)
                {
                    vFolder.Add(new StringTag("name", Name));
                }
            }
            vFolder.WriteTag(bw);
            vFolder.WriteData(bw);
        }

        public override void WriteTag(NBTWriter bw)
        {

        }

        public override object Clone()
        {
            return new BoolTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            if (!F.HasNamedTag("name"))
                Name = F.Name;
            else Name = F["name"].StringValue;
            
            int ret = F["val"].IntValue;
            if (ret == 1) BoolVal = true;
            else BoolVal = false;

            
        }

    }
}
