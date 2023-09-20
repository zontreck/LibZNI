using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
{
    public class IntArrayTag : Tag
    {
        private int[] BArrVal;
        public int[] Value
        {
            get
            {
                return BArrVal;
            }
        }
        
        public IntArrayTag(string _Name, int[] val)
        {
            Name = _Name;
            BArrVal = val;
        }
        public IntArrayTag(int[] boolVal) : this(null, boolVal)
        {
        }
        public IntArrayTag() : this(null, new int[] {}) { }

        public override TagType Type
        {
            get
            {
                return TagType.INTARRAY;
            }
        }

        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();

            int count = br.ReadInt32();

            BArrVal = new int[count];
            for (int i = 0; i < count; i++)
            {
                BArrVal[i] = br.ReadInt32();
            }
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new IntArrayTag().ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {

            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);
            bw.Write(Value.Length);

            foreach (int i in Value)
            {
                bw.Write(i);
            }
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);

        }

        public override object Clone()
        {
            return new IntArrayTag(Name, Value);
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
