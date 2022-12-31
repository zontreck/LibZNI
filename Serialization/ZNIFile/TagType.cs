using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public enum TagType
    {
        FOLDER = 10,
        STRING = 8,
        INTEGER = 3,
        LIST = 9, // List can be any valid Tag Type
        END = 0, // Present at the end of a folder or list
        BOOL = 13,
        DOUBLE = 6,
        FLOAT = 5,
        LONG = 4,
        BYTE = 1,
        BYTEARRAY = 7,
        INTARRAY = 11,
        LONGARRAY=12,
        ULONG=14,
        UUID=15,



        INVALID=99
    }
}
