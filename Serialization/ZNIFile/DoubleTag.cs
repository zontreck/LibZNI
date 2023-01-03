using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class DoubleTag : Tag
    {
        public override TagType Type
        {
            get
            {
                return TagType.DOUBLE;
            }
        }

        private double DoubleVal;
        public double Value
        {
            get
            {
                return DoubleVal;
            }
        }

        public DoubleTag() : this(null, 0) { }
        public DoubleTag(double Val) : this(null, Val) { }
        public DoubleTag(string DName, double val)
        {
            Name = DName;
            DoubleVal = val;
        }

        public override object Clone()
        {
            return new DoubleTag(Name, Value);
        }
        public override bool ReadTag(NBTReader br)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            DoubleVal = br.ReadDouble();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new DoubleTag().ReadTag(br);
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

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
