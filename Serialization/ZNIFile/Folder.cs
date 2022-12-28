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


        public override bool ReadTag(BinaryReader br)
        {
            if(Parent != null)
            {
                SkipTag(br);
                return false;
            }
            Name = br.ReadString(); // Per ZNIFile standards, each tag reads its own name!

            while(true)
            {
                TagType next = (TagType)br.ReadInt32();
                Tag _next = null;
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
                    case TagType.END:
                        return true;
                }

                if (_next.ReadTag(br))
                {
                    Tags.Add(_next);
                }
                _next.Parent = this;
            }
        }


        public override void SkipTag(BinaryReader br)
        {
            _ = new Folder().ReadTag(br);
        }

        public override void WriteData(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override void WriteTag(BinaryWriter bw)
        {
            bw.Write(((int)Type)); // Write int (0), folder
            bw.Write(Name);

            foreach (Tag t in Tags)
            {
                t.WriteTag(bw);
            }
            bw.Write(((int)TagType.END));
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
    }
}
