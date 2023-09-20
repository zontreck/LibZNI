using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization.ACFile
{
    // Aria: Changed to a type of byte which keeps it to only one byte when writing out in serializing
    public enum TagType : byte
    {
        END = 0x00, // Present at the end of a folder
        BYTE = 0x01,
        SHORT = 0x02,
        INTEGER = 0x03,
        LONG = 0x04,
        FLOAT = 0x05,
        DOUBLE = 0x06,
        BYTEARRAY = 0x07,
        STRING = 0x08,
        LIST = 0x09, // List can be any valid Tag Type
        FOLDER = 0x0A,
        INTARRAY = 0x0B,
        LONGARRAY = 0x0C,
        BOOL = 0x0D,
        ULONG=0x0E,
        UUID=0x0F,



        INVALID=0xFF
    }
}
