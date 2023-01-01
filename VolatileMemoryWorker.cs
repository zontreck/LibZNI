using LibZNI.Serialization;
using LibZNI.Serialization.ZNIFile;
using System.IO;

/// <summary>
/// RAM-like memory
/// Memory is regularly saved to a temporary file called VM.RAM
/// The memory in this class should only be used for temporary persistance between tasks. It should be assumed that this data will purge every restart.
/// </summary>
namespace LibZNI
{
        
    public class VolatileMemory : Serializable
    {
        public static Folder Memory = new Folder("MEM");

        public override void save(Folder f)
        {
            f.Add(VolatileMemory.Memory);
        }
        public override void load(Folder f)
        {
            VolatileMemory.Memory = f["MEM"] as Folder;
        }


        public void FullSave()
        {

            Folder f = new Folder("root");
            save(f);

            TagIO.SaveToFile(ConfigLocation.GetPath()+".volatile", f);
        }
    }
}