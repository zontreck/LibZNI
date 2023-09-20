using LibAC.Serialization.ACFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization
{
    /// <summary>
    /// This class contains helper functions for interacting with streams and files that are encoded using ZNIFile's structure
    /// Additionally, this provides a overlay as it interacts with the final data set, and can compress and or encrypt.
    /// </summary>
    public static class TagIO
    {
        public static void WriteOnStream(Stream s, Tag x)
        {
            NBTWriter bw = new NBTWriter(s, true);
            x.WriteTag(bw);
            x.WriteData(bw);
        }

        public static Folder ReadFromStream(Stream s)
        {
            try
            {

                Folder folder = new Folder();
                NBTReader br = new NBTReader(s,true);
                TagType type = (TagType)br.ReadByte();
                if (type == TagType.FOLDER)
                {
                    // Read the file!
                    folder.ReadTag(br);
                }

                return folder;
            }catch(Exception e)
            {
                return new Folder();
            }
        }

        public static void SaveToFile(string FileName, Tag x, bool gz=false)
        {
            if(File.Exists(FileName))File.Delete(FileName);
            Stream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (gz){
                fs = new GZipStream(fs, CompressionLevel.SmallestSize);
            }
            WriteOnStream(fs, x);
            fs.Close();
        }
        public static Folder ReadFromFile(string FileName, bool gz = false)
        {
            Stream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (gz)
            {
                fs = new GZipStream(fs, CompressionMode.Decompress);
            }
            Folder f = ReadFromStream(fs);
            fs.Close();
            return f;
        }
    }
}
