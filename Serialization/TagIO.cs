using LibZNI.Serialization.ZNIFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization
{
    /// <summary>
    /// This class contains helper functions for interacting with streams and files that are encoded using ZNIFile's structure
    /// Additionally, this provides a overlay as it interacts with the final data set, and can compress and or encrypt.
    /// </summary>
    public static class TagIO
    {
        public static void WriteOnStream(Stream s, Tag x)
        {
            BinaryWriter bw = new BinaryWriter(s);
            x.WriteTag(bw);
        }

        public static Folder ReadFromStream(Stream s)
        {
            try
            {

                Folder folder = new Folder();
                BinaryReader br = new BinaryReader(s);
                TagType type = (TagType)br.ReadInt32();
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

        public static void SaveToFile(string FileName, Tag x)
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            WriteOnStream(fs, x);
            fs.Close();
        }
        public static Folder ReadFromFile(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            Folder f = ReadFromStream(fs);
            fs.Close();
            return f;
        }
    }
}
