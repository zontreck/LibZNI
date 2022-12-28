using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class IntTag : Tag
    {

        public IntTag() : this(null, 0)
        { }
        public IntTag(int Value) : this(null, Value)
        {
        }
        public IntTag(string _Name, int Val)
        { 
            this.Name = _Name;
            IntVal = Val;
        }
        private int IntVal;
        public int Value
        {
            get
            {
                return IntVal;
            }
        }
        public override TagType Type
        {
            get
            {
                return TagType.INTEGER;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            IntVal = br.ReadInt32();
            return true;
        }


        public override void SkipTag(BinaryReader br)
        {
            _ = new IntTag().ReadTag(br);
        }
        public override void WriteTag(BinaryWriter bw)
        {
            bw.Write(((int)Type));
            bw.Write(Name);
            bw.Write(Value);
        }

        public override void WriteData(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new IntTag(Name, Value);
        }
    }
}
