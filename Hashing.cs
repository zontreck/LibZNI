using LibZNI.Serialization;
using LibZNI.Serialization.ZNIFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibZNI
{
    public class Hash : Serializable
    {
        public byte[] hashed;
        private readonly byte signature = 0x9f;
        public int passnum = 0;

        public Hash(int len)
        {
            hashed = new byte[len];
            //hashed[0] = signature;

            passnum = 0;
        }

        public byte get_sig_at_pass(int pass)
        {
            byte start = signature;
            for (int i = 0; i < pass; i++)
            {
                start = (byte)wrap_add((int)start, (int)signature);
            }

            return start;
        }

        public Hash(byte[] bs, int pass)
        {
            hashed = bs;
            passnum = pass;
        }

        public static Hash compute(int len, byte[] data)
        {
            if (len <= 0) throw new ArgumentOutOfRangeException();
            byte[] arr = new byte[len];
            int p = 0;//pointer to position in [arr]

            foreach (byte b in data)
            {
                //arr[p] += b;// treat as a number.
                int exist = arr[p];
                exist += b;
                while (exist > 255) exist -= 255;

                if (exist < 0) exist *= -1;
                arr[p] = (byte)exist;

                p++;
                if (p >= len)
                {
                    p = 0;// reset the pointer
                }
            }


            return new Hash(arr, 1);

        }

        public static Hash operator +(Hash a, Hash b)
        {
            Hash ret = new Hash(a.hashed, a.passnum);
            int p = 2; // We do want to add the position signed bit. As that information is unique to a piece of source data

            for (p = 0; p < b.hashed.Length; p++)
            {
                int exist = a.hashed[p];
                exist = wrap_add(exist, b.hashed[p]);

                ret.hashed[p] = (byte)exist;
            }

            ret.passnum++;


            return ret;
        }

        /// <summary>
        /// Wrap adds, with a cap at 255
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Wrapped integer</returns>
        public static int wrap_add(int a, int b)
        {
            a = a + b;
            while (a > 255) a -= 255;
            if (a < 0) a *= -1;
            return a;
        }
        /// <summary>
        /// Wrap subtracts with a cap of 255
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Wrapped integer</returns>
        public static int wrap_sub(int a, int b)
        {
            int ret = a - b;
            if (ret < 0) ret += 255;
            return ret;
        }

        public static Hash operator -(Hash a, Hash b)
        {
            Hash ret = new Hash(a.hashed, a.passnum);
            int p = 1; // We do want to add the position signed bit. As that information is unique to a piece of source data

            for (p = 0; p < b.hashed.Length; p++)
            {
                int exist = a.hashed[p];
                exist = wrap_sub(exist, b.hashed[p]);

                ret.hashed[p] = (byte)exist;
            }

            ret.passnum--;
            /*
            if (ret.get_sig_at_pass(ret.passnum) == ret.hashed[0])
            {
                // success
            }
            else throw new Exception("Fatal error in pass. Validation failed");
            */


            return ret;


        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in serialize())
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public byte[] serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(passnum);
            bw.Write(hashed.Length);
            bw.Write(hashed);

            return ms.ToArray();
        }

        public void deserialize(byte[] streams)
        {
            MemoryStream ms = new MemoryStream(streams);
            BinaryReader br = new BinaryReader(ms);
            passnum = br.ReadInt32();
            hashed = br.ReadBytes(br.ReadInt32());

            br.Close();
        }

        public override void save(Folder f)
        {
            f.Add(new IntTag("pass", passnum));
            f.Add(new ByteArrayTag("hash",hashed));
        }

        public override void load(Folder f)
        {
            passnum = f["pass"].IntValue;
            hashed = (f["hash"] as ByteArrayTag).Value;
        }
    }
}
