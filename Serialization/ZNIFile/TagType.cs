using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public enum TagType
    {
        END = 0, // Present at the end of a folder or list
        BYTE = 1,
        SHORT = 2,
        INTEGER = 3,
        LONG = 4,
        FLOAT = 5,
        DOUBLE = 6,
        BYTEARRAY = 7,
        STRING = 8,
        LIST = 9, // List can be any valid Tag Type
        FOLDER = 10,
        INTARRAY = 11,
        LONGARRAY = 12,
        BOOL = 13,
        ULONG=14,
        UUID=15,



        INVALID=99
    }
}
