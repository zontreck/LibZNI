using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class FloatTag : Tag
    {
        private float FloatVal;
        public float Value
        {
            get
            {
                return FloatVal;
            }
        }
        
        public FloatTag(string _Name, float val)
        {
            Name = _Name;
            FloatVal = val;
        }
        public FloatTag(float _FloatVal) : this(null, _FloatVal)
        {
        }
        public FloatTag() : this(null, 0f) { }

        public override TagType Type
        {
            get
            {
                return TagType.FLOAT;
            }
        }

        public override bool ReadTag(BinaryReader br)
        {
            Name = br.ReadString();
            FloatVal = br.ReadSingle();
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(BinaryReader br)
        {
            _ = new FloatTag().ReadTag(br);
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
            return new FloatTag(Name, Value);
        }
    }
}
