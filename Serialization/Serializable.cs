using LibZNI.Serialization.ZNIFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization
{
    public abstract class Serializable
    {
        public abstract void save(Folder f);
        public abstract void load(Folder f);
    }
}
