using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
{
    public abstract class Tag : ICloneable, IComparable<Tag>
    {
        public Tag Parent { get; internal set; }
        public abstract TagType Type { get; }

        public bool HasValue
        {
            get
            {
                switch(Type)
                {
                    case TagType.FOLDER:
                    case TagType.LIST:
                    case TagType.END:
                    case TagType.INVALID:
                        return false;
                    default:
                        return true;
                    
                }
            }
        }

        internal string _Name="";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value) return;
                if (value == null)
                {
                    value = "";
                }
                Folder f = Parent as Folder;
                if(f != null)
                {
                    f.Rename(_Name, value);
                }
                _Name = value;
            }
        }

        public abstract bool ReadTag(NBTReader br);
        public abstract void SkipTag(NBTReader br);
        public abstract void WriteTag(NBTWriter bw);
        public abstract void WriteData(NBTWriter bw);

        public abstract void CastFrom(Folder F);

        private string Error = "Invalid tag type";
        public virtual Tag this[int index]
        {
            get
            {
                throw new InvalidOperationException(Error);
            }
            set
            {
                throw new InvalidOperationException(Error);
            }
        }

        public virtual Tag this[string index]
        {
            get
            {
                throw new InvalidOperationException(Error);
            }
            set
            {
                throw new InvalidOperationException(Error);
            }
        }

        public static string GetCanonicalName(TagType type)
        {
            switch(type)
            {
                case TagType.FOLDER:
                    {
                        return "Folder";
                    }
                case TagType.STRING:
                    {
                        return "String";
                    }
                case TagType.INTEGER:
                    {
                        return "Integer";
                    }
                case TagType.LIST:
                    {
                        return "List";
                    }
                case TagType.BOOL:
                    {
                        return "Bool";
                    }
                case TagType.DOUBLE:
                    {
                        return "Double";
                    }
                case TagType.FLOAT:
                    {
                        return "Float";
                    }
                case TagType.LONG:
                    {
                        return "Long";
                    }
                case TagType.BYTE:
                    {
                        return "Invalid";
                    }
            }
            return "Invalid";
        }

        public string StringValue
        {
            get
            {
                switch(Type)
                {
                    case TagType.STRING:
                        {
                            return (this as StringTag).Value;
                        }
                    default:
                        {
                            throw new Exception("Invalid type");
                        }
                }
            }
        }

        public int IntValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.INTEGER:
                        return (this as IntTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }

        public bool BoolValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.BOOL:
                        return (this as BoolTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public double DoubleValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.DOUBLE:
                        return (this as DoubleTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public float FloatValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.FLOAT:
                        return (this as FloatTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public long LongValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.LONG:
                        return (this as LongTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public byte ByteValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.BYTE:
                        return (this as ByteTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public ulong uLongValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.ULONG:
                        return (this as uLongTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }
        public Guid UUIDValue
        {
            get
            {
                switch (Type)
                {
                    case TagType.UUID:
                        return (this as UUIDTag).Value;
                    default:
                        throw new Exception("Invalid type");
                }
            }
        }

        public abstract void Rename(string old, string newName);

        public abstract object Clone();

        public int CompareTo(Tag other)
        {
            if (ID == other.ID) return 0;
            else return 1;
        }

        private Guid ID { get; set; } = Guid.NewGuid();
    }
}
