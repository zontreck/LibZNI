using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class Folder : Tag, ICollection<Tag>, ICollection
    {
        public Folder() { }
        public Folder(string name)
        {
            this.Name = name;
        }
        public Collection<Tag> Tags { get; set; } = new Collection<Tag>();
        public override TagType Type
        {
            get
            {
                return TagType.FOLDER;
            }
        }

        public int Count
        {
            get
            {
                return Tags.Count;
            }
        }

        public bool IsReadOnly
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
                return (Tags as ICollection).SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public void Add(Tag item)
        {
            if (item == null) throw new Exception("Bad item!");

            Tags.Add(item);
            item.Parent = this;
        }

        public override Tag this[int index]
        {
            get
            {
                return Tags.ElementAt(index);
            }
            set
            {
                try
                {

                    string TagName = Tags.ElementAt(index).Name;
                    this[TagName] = value;
                }
                catch(Exception e)
                {
                    Tags.Add(value);
                }

            }
        }
        public override Tag this[string index]
        {
            get
            {
                return Tags.Where(x => x.Name == index).FirstOrDefault();
            }
            set
            {
                if(Tags.Select(x=>x.Name == index).Count() != 0)
                {
                    Tags.RemoveAt(Tags.IndexOf(Tags.Where(x => x.Name == index).FirstOrDefault()));
                }

                Tags.Add(value);
            }
        }

        public void Clear()
        {
            foreach(Tag t in Tags)
            {
                t.Parent = null;
            }
            Tags = new Collection<Tag>();
        }

        public bool Contains(Tag item)
        {
            return Tags.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
            Tags.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo((Tag[])array, index);
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return Tags.GetEnumerator();
        }

        public bool Remove(Tag item)
        {
            return Tags.Remove(item);
        }


        public override bool ReadTag(NBTReader br)
        {
            // Aria: Removed a return on parent not being null because that is how the ZNI Parsing system works.
            if (!(Parent != null && Parent.Type == TagType.LIST))
                Name = br.ReadString();


            while (true)
            {
                TagType next = br.ReadTagType();
                Tag _next = null;
                switch (next)
                {
                    case TagType.FOLDER:
                        _next = new Folder();
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
                    case TagType.END:
                        return true;

                    case TagType.SHORT:
                        _next = new ShortTag();
                        break;
                }
                _next.Parent = this;

                if (_next.ReadTag(br))
                {
                    if (_next.Type == TagType.FOLDER)
                    {
                        Folder NextTag = _next as Folder;
                        if (NextTag.HasNamedTag("_virtcast_"))
                        {
                            ByteTag bt = NextTag["_virtcast_"] as ByteTag;
                            next = (TagType)bt.Value;
                            Tag temp = null;
                            switch (next)
                            {
                                case TagType.BOOL:
                                    temp = new BoolTag();
                                    temp.CastFrom(NextTag);
                                    break;
                                case TagType.ULONG:
                                    temp = new uLongTag();
                                    temp.CastFrom(NextTag);
                                    break;
                                case TagType.UUID:
                                    temp = new UUIDTag();
                                    temp.CastFrom(NextTag);
                                    break;
                            }
                            _next = temp;
                        }

                    }
                    Tags.Add(_next);
                }
            }
            return true;
        }

        public bool HasNamedTag(string Name)
        {
            foreach(Tag t in Tags)
            {
                if(t.Name == Name) return true;
            }
            return false;
        }


        public override void SkipTag(NBTReader br)
        {
            _ = new Folder().ReadTag(br);
        }


        public override void WriteData(NBTWriter bw)
        {
            if (!(Parent != null && Parent.Type == TagType.LIST))
                bw.Write(Name);
            foreach (Tag t in Tags)
            {
                t.WriteTag(bw);
                t.WriteData(bw);
            }
            bw.Write(TagType.END);
        }

        public override void WriteTag(NBTWriter bw)
        {
            bw.Write(Type); // Write int (0), folder

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Rename(string old, string newName)
        {
            // do nothing. The folder's collection will be automatically updated.
        }

        public override object Clone()
        {
            return new Folder(this);
        }

        public Folder(Folder existing)
        {
            Name = existing.Name;
            Tags = new Collection<Tag>(Tags.ToArray());
        }

        public override void CastFrom(Folder F)
        {
            throw new NotImplementedException();
        }
    }
}
