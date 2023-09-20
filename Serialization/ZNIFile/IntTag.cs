using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
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

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            
            IntVal = br.ReadInt32();
            return true;
        }


        public override void SkipTag(NBTReader br)
        {
            _ = new IntTag().ReadTag(br);
        }
        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);

        }

        public override void WriteData(NBTWriter bw)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);
            bw.Write(Value);
        }
        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new IntTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
