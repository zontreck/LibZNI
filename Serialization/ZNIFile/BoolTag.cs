using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
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

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            BoolVal = br.ReadBoolean();
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

            bw.Write(Value);
        }

        public override object Clone()
        {
            return new BoolTag(Name, Value);
        }
    }
}
