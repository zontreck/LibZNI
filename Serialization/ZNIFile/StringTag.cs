using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class StringTag : Tag
    {
        private string StrVal;
        public string Value
        {
            get
            {
                return StrVal;
            }
        }

        public override TagType Type
        {
            get
            {
                return TagType.STRING;
            }
        }

        public StringTag() : this(null, "")
        { }
        public StringTag(string Value) : this(null, Value)
        {
        }
        public StringTag(string _Name, string Val)
        {
            this.Name = _Name;
            StrVal = Val;
        }

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            StrVal = br.ReadString();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new StringTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);

            bw.Write(StrVal);
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);
        }

        public override object Clone()
        {
            return new StringTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
