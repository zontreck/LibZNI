using LibZNI.Serialization;
using LibZNI.Serialization.ZNIFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI
{
    public class VersionNumberDifferentException : Exception
    {
        public VersionNumberDifferentException(Version one, Version two) :
            base($"The version numbers are not identical. Current version {one} ; Other {two}")
        {
        }
    }
    public class Version : Serializable
    {
        public List<int> ver { get; set; } = new List<int>();

        public Version() { }
        public Version(int major, int minor, int revision, int build, int cycleStatus, int cycleNum)
        {
            ver = new List<int>();
            ver.Add(major);
            ver.Add(minor);
            ver.Add(revision);
            ver.Add(build);
            ver.Add(cycleStatus);
            ver.Add(cycleNum);
        }
        public int Compare(Version other)
        {
            for(int i=0;i<ver.Count;i++)
            {
                int cur = ver[i];
                int oth = other.ver[i];

                if (cur < oth) return 1;
                else if (cur > oth) return 2;
            }

            return 0;
        }
        public Version(string versionStr)
        {
            ver = new List<int>();
            List<string> split = versionStr.llParseStringKeepNulls(new string[] { "." }, new string[] { "R", "A", "B", "RC", "DEV" });
            for(int i=0;i<split.Count;i++)
            {
                if (i == 4)
                {
                    switch (split[i])
                    {
                        case "R":
                            ver.Add(0);
                            break;
                        case "A":
                            ver.Add(1);
                            break;
                        case "B":
                            ver.Add(2);
                            break;
                        case "RC":
                            ver.Add(3);
                            break;
                        case "DEV":
                            ver.Add(4);
                            break;
                        default:
                            ver.Add(4);
                            break;
                    }
                }
                else ver.Add(int.Parse(split[i]));
            }
        }
        public override string ToString()
        {
            string CYCLE = "";
            switch (ver[4])
            {
                case 0:
                    CYCLE = "R";
                    break;
                case 1:
                    CYCLE = "A";
                    break;
                case 2:
                    CYCLE = "B";
                    break;
                case 3:
                    CYCLE = "RC";
                    break;
                case 4:
                    CYCLE = "DEV";
                    break;
                default:
                    CYCLE = "DEV";
                    break;
            }

            return $"{ver[0]}.{ver[1]}.{ver[2]}.{ver[3]}.{CYCLE}.{ver[5]}";
        }
        public override void load(Folder f)
        {
            ListTag lt = f["Version"] as ListTag;
            ver = new List<int>();
            foreach(Tag tag in lt)
            {
                IntTag it = tag as IntTag;
                ver.Add(it.IntValue);
            }
        }

        public override void save(Folder f)
        {
            ListTag lt = new ListTag(TagType.INTEGER, "Version");
            foreach(int v in ver)
            {
                IntTag i = new IntTag(v);
                lt.Add(i);
            }
            f.Add(lt);
        }
    }
}
