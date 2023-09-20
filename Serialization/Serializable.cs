using LibAC.Serialization.ACFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization
{
    public abstract class Serializable
    {
        public abstract void save(Folder f);
        public abstract void load(Folder f);
    }
}
