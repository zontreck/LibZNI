using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public enum TagType
    {
        FOLDER = 0,
        STRING = 1,
        INTEGER = 2,
        LIST = 3, // List can be any valid Tag Type
        END = 4, // Present at the end of a folder or list
        BOOL = 5,
        DOUBLE = 6,
        FLOAT = 7,
        LONG = 8,
        BYTE = 9,
        BYTEARRAY = 10,



        INVALID=99
    }
}
