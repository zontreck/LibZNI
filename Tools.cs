using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

[assembly: LibZNI.AutoUpdater("/job/LibZNI", "library.tar")]
namespace LibZNI
{
    public class Tools
    {

        public static Int32 getTimestamp()
        {
            return int.Parse(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        }

        public static string GetOSShortID()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (isWindows) return "windows";
            if (isLinux) return "linux";
            if (isMac) return "osx";


            return "unknown";
        }


        public static string Hash2String(byte[] Hash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5Hash(string ToHash)
        {
            byte[] Source = UTF8Encoding.UTF8.GetBytes(ToHash);
            byte[] Hash = new MD5CryptoServiceProvider().ComputeHash(Source);
            return Tools.Hash2String(Hash);
        }

        public static string MD5Hash(byte[] ToHash)
        {
            return Tools.Hash2String(new MD5CryptoServiceProvider().ComputeHash(ToHash));
        }

        public static string SHA256Hash(string ToHash)
        {
            SHA256 hasher = SHA256.Create();
            return Tools.Hash2String(hasher.ComputeHash(UTF8Encoding.UTF8.GetBytes(ToHash)));
        }

        public static string SHA256Hash(byte[] ToHash)
        {
            SHA256 Hasher = SHA256.Create();
            return Tools.Hash2String(Hasher.ComputeHash(ToHash));
        }

        public static string ZHX(string ToHash)
        {
            ZHash tmp = new ZHash();
            tmp.NewKey();
            tmp.CalculateKey(ToHash);
            return tmp._key;
        }

        public static string ZSR(string ToSerialize)
        {
            ZHash tmp = new ZHash();
            tmp.NewSerial();
            tmp.CalculateKey(ToSerialize);
            return tmp._key;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GoUpOneLevel(string path)
        {
            string[] paths = path.Split(new[] { '/', '\\' });

            List<string> pathList = paths.ToList();
            pathList.Remove(pathList[pathList.Count-1]);

            string pathListStr = "";
            foreach(string s in pathList)
            {
                pathListStr += s + "/";
            }
            return pathListStr;
        }
    }

    [Serializable()]
    public class ZHash
    {
        [JsonRequired(), JsonProperty(PropertyName = "value")]
        public string _key;
        [JsonIgnore()]
        public string _template;

        public void Reset()
        {
            _key = _template;
        }

        public override string ToString()
        {
            return _key;
        }


        public void NewKey()
        {

            _key = "".PadLeft(10, '0');
            _key += "-";
            _key += "".PadRight(4, '0');
            _key += "-";
            _key += "".PadRight(6, '0');
            _key += "-";
            _key += "".PadRight(8, '0');

        }

        public void NewSerial()
        {
            _key = "".PadLeft(10, '0');
            _key += "-";
            _key += "".PadRight(6, '0');
            _key += "-";
            _key += "".PadRight(4, '0');
            _key += "-";
            _key += "".PadRight(4, '0');
            _key += "-";
            _key += "".PadRight(2, '0');
            _key += "-";
            _key += "".PadRight(4, '0');
            _key += "-";
            _key += "".PadRight(8, '0');

        }
        public void CalculateKey(string K)
        {
            string valid = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ=.+/\\][{}';:?><,_-)(*&^%$#@!`~|";
            while (valid.Length < K.Length)
            {
                valid += valid;
            }
            StringBuilder tmp = new StringBuilder(_key);

            for (int i = 0; i < _key.Length; i++)
            {
                char V = _key[i];
                if (V != '-')
                {
                    MD5 MDHash = MD5.Create();
                    for (int ii = 0; ii < ((K.Length > _key.Length) ? _key.Length : K.Length); ii++)
                    {
                        byte[] md5Data = MDHash.ComputeHash(Encoding.UTF8.GetBytes((K + i.ToString() + valid[i].ToString() + valid[ii].ToString()).ToCharArray()));
                        // Replace digit with MD5'd  char from String K encoded alongside (i)
                        StringBuilder hashData = new StringBuilder();
                        foreach (byte b in md5Data)
                        {
                            hashData.Append(b.ToString("X2"));
                        }
                        string Hash = hashData.ToString();
                        tmp[i] = Hash[(i > 31 ? 1 : i)];
                        //Console.Write("\r" + tmp.ToString() + "\r");
                    }
                }
            }
            //Console.WriteLine("\r\n");
            _key = tmp.ToString();
        }

        public static byte[] HashToBytes(string Key)
        {
            return Enumerable.Range(0, Key.Length).Where(x=>x % 2 == 0).Select(x=>Convert.ToByte(Key.Substring(x,2),16)).ToArray();
        }
        public static ZHash Bytes2Hash(byte[] key)
        {
            ZHash itm = new ZHash();
            foreach(byte b in key)
            {
                itm._key += b.ToString("X2");
            }
            itm._template = itm._key;
            return itm;
        }

        public static string Bytes2HashStr(byte[] key)
        {
            return Bytes2Hash(key)._key;
        }
    }

    public static class LinqExtensions
    {
        public static string ReplaceAtIndex(this string a, int b, string c)
        {
            string sSplice = "";
            if(b == 0)
            {
                sSplice = $"{c}{a.Substring(1)}";
            }else
            {
                sSplice = $"{a.Substring(0,b)}{c}{a.Substring(b+1)}";
            }
            return sSplice;
        }
    }

    public static class ZNILSLTools
    {
        public static bool Compare<T>(this List<string> itx, List<string> itx2)
        {
            if (itx.Count != itx2.Count) return false;
            for(int i = 0; i < itx.Count; i++)
            {
                if(itx[i] != itx2[i]) return false;
            }

            return true;
        }

        public static List<string> llParseString2List(this string item, string[] opts, string[] keepopts)
        {
            return ParseString2List(item, opts, keepopts);
        }
        internal static string[] Augment(this string[] itm, string x)
        {
            List<string> working = new List<string>(itm);
            List<string> buffer = new List<string>();
            for(int i = 0; i < working.Count; i++)
            {
                if (String.IsNullOrEmpty(working[i])) break;
                buffer.Add(working[i]);
                
                if (i == working.Count - 1) break;
                else
                    buffer.Add(x);

            }
            return buffer.ToArray();
        }
        public static List<string> ParseString2List(this string item, string[] opts, string[] keepopts)
        {
            List<string> entries = new List<string>();
            List<string> tmpBuffer = new List<string>();
            List<string> buffer = new List<string>();
            buffer.Add(item);
            foreach(string x in opts)
            {
                for(int i=0;i<buffer.Count;i++)
                {
                    string y = buffer[i];
                    if (y.Contains(x))
                    {
                        string[] newbufferItem = y.Split(x);
                        foreach (string V in newbufferItem)
                        {
                            tmpBuffer.Add(V);
                        }
                    }
                    else tmpBuffer.Add(y);
                }
                buffer = tmpBuffer;
                tmpBuffer = new List<string>();
            }

            // Now re-run the buffer through the keep opts list
            tmpBuffer = new List<string>();
            foreach(string z in keepopts)
            {
                for(int i=0;i<buffer.Count;i++)
                {
                    string y = buffer[i];
                    if (y.Contains(z))
                    {
                        string[] newbuff = y.Split(z).Augment(z);
                        foreach(string V in newbuff)
                        {
                            tmpBuffer.Add(V);
                        }
                    }else
                    {
                        tmpBuffer.Add(y);
                    }
                }
                buffer = tmpBuffer;
                tmpBuffer = new List<string>();
            }



            entries = buffer;
            buffer = new List<string>();

            return entries;
        }
    }
}
