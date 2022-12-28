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

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            StrVal = br.ReadString();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = br.ReadString();
            _ = br.ReadString();
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
            return new StringTag(Name, Value);
        }
    }
}
