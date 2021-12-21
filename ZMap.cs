using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI
{
    public class VectorList<T, T2, V> : IComparable
    {
        public T Key { get; set; }
        public T2 Key2 { get; set; }
        public V Value { get; set; }

        public VectorList(T x, T2 b, V c)
        {
            Key = x;
            Key2 = b;
            Value = c;
        }

        public int CompareTo(object obj)
        {
            if(obj == null)
            {
                return -1;
            }
            VectorList<T, T2, V> _other = obj as VectorList<T, T2, V>;
            if (Key.Equals(_other.Key))
            {
                if (Key2.Equals(_other.Key2))
                {
                    if (Value.Equals(_other.Value))
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }
    }
    public class ZMap<T,T2,V> : IEnumerable<VectorList<T, T2, V>>
    {
        public List<VectorList<T, T2, V>> _list = new List<VectorList<T, T2, V>>();
        public void Add(VectorList<T,T2,V> item)
        {
            if(!_list.Contains(item))
                _list.Add(item);
        }
        public void Remove(VectorList<T,T2,V> item)
        {
            _list.Remove(item);
        }

        public V GetEntry(T a, T2 b)
        {
            try
            {
                return _list.First(x => x.Key.Equals(a) && x.Key2.Equals(b)).Value;

            }
            catch
            {
                return default(V);
            }
        }

        public T2 GetK2(T a , V b)
        {
            try
            {
                return _list.First(x => x.Key.Equals(a) && x.Value.Equals(b)).Key2;
            }catch
            {
                return default(T2);
            }
        }

        public IEnumerator<VectorList<T, T2, V>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
