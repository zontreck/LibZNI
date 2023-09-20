using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LibAC.Serialization.ACFile
{
    public class ListTag : Tag, IList<Tag>, IList
    {
        private TagType _subtype = TagType.INVALID;
        private List<Tag> _tags;
        public List<Tag> Value
        {
            get
            {
                return _tags;
            }
        }

        public ListTag() : this(TagType.STRING, null, new List<Tag>())
        {
        }
        public ListTag(TagType sub) : this(sub, null, new List<Tag>()) { }
        public ListTag(TagType sub, string name) : this(sub,name, new List<Tag>()) { }
        public ListTag(TagType sub, string name, List<Tag> tags)
        {
            _tags = tags;
            Name = name;
            setSubtype(sub);
        }
        public void setSubtype(TagType itemTypes)
        {
            _subtype = itemTypes;
        }
        public TagType getListType()
        {
            return _subtype;
        }
        public override TagType Type
        {
            get
            {
                return TagType.LIST;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return _tags.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((IList)_tags).SyncRoot;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return _tags[index];
            }
            set
            {
                if (_tags.Count >= index) _tags[index] = (Tag)value;
                else _tags.Add((Tag)value);
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override bool ReadTag(NBTReader br)
        {
            //_subtype = (TagType)br.ReadInt32();
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();
            _subtype = br.ReadTagType();
            int count = br.ReadInt32();
            TagType next = _subtype;
            for (int i = 0; i < count; i++)
            {
                Tag _next = null;
                // read sub-tags
                switch (next)
                {
                    case TagType.FOLDER:
                        _next = new Folder();
                        break;
                    case TagType.BOOL:
                        _next = new BoolTag();
                        break;
                    case TagType.BYTE:
                        _next = new ByteTag();
                        break;
                    case TagType.DOUBLE:
                        _next = new DoubleTag();
                        break;
                    case TagType.FLOAT:
                        _next = new FloatTag();
                        break;
                    case TagType.INTEGER:
                        _next = new IntTag();
                        break;
                    case TagType.LIST:
                        _next = new ListTag();
                        break;
                    case TagType.LONG:
                        _next = new LongTag();
                        break;
                    case TagType.STRING:
                        _next = new StringTag();
                        break;
                    case TagType.BYTEARRAY:
                        _next = new ByteArrayTag();
                        break;
                    case TagType.INTARRAY:
                        _next = new IntArrayTag();
                        break;
                    case TagType.LONGARRAY:
                        _next = new LongArrayTag();
                        break;
                    case TagType.SHORT:
                        _next = new ShortTag();
                        break;
                }
                _next.Parent = this;
                if (_next.ReadTag(br))
                {
                    if (_next.Type == TagType.FOLDER)
                    {
                        Folder nxt = _next as Folder;
                        if (nxt.HasNamedTag("_virtcast_"))
                        {
                            TagType tag = (TagType)nxt["_virtcast_"].ByteValue;
                            Tag temp = null;
                            switch (tag)
                            {
                                case TagType.BOOL:
                                    temp = new BoolTag();
                                    temp.CastFrom(nxt);
                                    break;
                                case TagType.ULONG:
                                    temp = new uLongTag();
                                    temp.CastFrom(nxt);
                                    break;
                                case TagType.UUID:
                                    temp = new UUIDTag();
                                    temp.CastFrom(nxt);
                                    break;

                            }
                            _next = temp;
                        }
                    }
                    _tags.Add(_next);
                }


            }
            return true;
        }

        public override void Rename(string old, string newName)
        {
            throw new NotImplementedException();
        }

        public override void SkipTag(NBTReader br)
        {
            _ = new ListTag(_subtype).ReadTag(br);
        }

        public override void WriteData(NBTWriter bw)
        {

            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);
            bw.Write(_subtype);
            bw.Write(_tags.Count);

            foreach (Tag x in _tags)
            {
                x.WriteData(bw);
            }
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type);


            //bw.Write(((int)TagType.END));
        }

        public int Add(object value)
        {
            if (value is Tag)
            {
                Tag tx = (Tag)value;
                Add(tx);
                return _tags.IndexOf(tx);
            }
            else return -1;
        }

        public void Clear()
        {
            foreach(Tag x in _tags)
            {
                x.Parent = null;
            }
            _tags.Clear();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            if (!(value is Tag)) return -1;
            return _tags.IndexOf((Tag)value);
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _tags.GetEnumerator();
        }

        public int IndexOf(Tag item)
        {
            return _tags.IndexOf(item);
        }

        public void Insert(int index, Tag item)
        {
            throw new NotImplementedException();
        }

        public void Add(Tag item)
        {
            item.Parent = this;
            _tags.Add(item);
        }

        public bool Contains(Tag item)
        {
            return _tags.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Tag item)
        {
            if (Contains(item))
            {
                item.Parent = null;
                _tags.Remove(item);
                return true;
            }
            else return false;
        }

        IEnumerator<Tag> IEnumerable<Tag>.GetEnumerator()
        {
            return _tags.GetEnumerator();
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
