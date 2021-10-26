using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

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
            ZHash.Instance.NewKey();
            ZHash.Instance.Key = ToHash;
            return ZHash.Instance.Key;
        }

        public static string ZSR(string ToSerialize)
        {
            ZHash.Instance.NewSerial();
            ZHash.Instance.Key = ToSerialize;
            return ZHash.Instance.Key;
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



    public sealed class ZHash
    {
        private static readonly object _lock = new object();
        private static ZHash _inst = new ZHash();
        static ZHash() { }

        public static ZHash Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_inst == null) _inst = new ZHash();
                    return _inst;
                }
            }
        }


        public string _key;
        public string Key
        {
            set
            {
                lock (_lock)
                {

                    if (value != "")
                        CalculateKey(value);
                    else NewKey();
                }
            }
            get
            {
                return _key;
            }
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
                    for (int ii = 0; ii < K.Length; ii++)
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

        public void NewKey()
        {
            lock (_lock)
            {

                _key = "".PadLeft(10, '0');
                _key += "-";
                _key += "".PadRight(4, '0');
                _key += "-";
                _key += "".PadRight(6, '0');
                _key += "-";
                _key += "".PadRight(8, '0');
            }
        }

        public void NewSerial()
        {
            lock (_lock)
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
        }

        public void SetKey(string Key)
        {
            _key = Key;
        }
    }

    public static class ZNILSLTools
    {
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
                        string[] newbuff = y.Split(z);
                        foreach(string V in newbuff)
                        {
                            tmpBuffer.Add(V);
                            tmpBuffer.Add(z);
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
