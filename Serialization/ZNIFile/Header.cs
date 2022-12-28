using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI.Serialization.ZNIFile
{
    public class Header : Serializable
    {
        public const string SIGNATURE = "ZNIFile";
        public Version VERSION = new Version(1,0,0,1,0,0);

        public override void load(Folder f)
        {

            Folder x = f["Header"] as Folder;
            if (x == null) return;

            if (x["Signature"].StringValue == SIGNATURE)
            {
                Version ver = new Version();
                ver.load(f);
                if (VERSION.Compare(ver) == 0)
                {
                    return;
                } else throw new VersionNumberDifferentException(VERSION, ver);
                
            }else
            {
                throw new Exception("Header failed validation");
            }
        }

        public override void save(Folder f)
        {
            Folder x = new Folder("Header");
            x.Add(new StringTag("Signature", SIGNATURE));
            VERSION.save(x);

            f.Add(x);
        }

        public static Folder GetHeader()
        {
            Folder f = new Folder("temp"); // Initialize a temporary header
            Header x = new Header();
            x.save(f);
            return f["Header"] as Folder;
        }
    }
}
