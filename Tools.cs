using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace LibZNI
{
    public class Tools
    {
        /// <summary>
        /// Finds whether the sum can even be calculated!
        /// </summary>
        /// <param name="targetSum">Target</param>
        /// <param name="nums">List of numbers to use</param>
        /// <returns>True if it is possible</returns>
        public static bool canSum(int targetSum, List<int> nums, Dictionary<int, bool> memo)
        {
            if (memo == null) memo = new Dictionary<int, bool>();
            if (memo.ContainsKey(targetSum)) return memo[targetSum];

            if (targetSum == 0) return true;
            if (targetSum < 0) return false;

            foreach (int element in nums)
            {
                int remain = targetSum - element;
                if (canSum(remain, nums, memo))
                {
                    memo[targetSum] = true;
                    return true;
                }
            }

            memo[targetSum] = false;
            return false;
        }

        /// <summary>
        /// Finds whether the sum can even be calculated!
        /// </summary>
        /// <param name="targetSum">Target</param>
        /// <param name="nums">List of numbers to use</param>
        /// <returns>True if it is possible</returns>
        public static bool canSum(BigInteger targetSum, List<BigInteger> nums, Dictionary<BigInteger,bool> memo, int recurse=0)
        {
            if (memo == null) memo = new Dictionary<BigInteger, bool>();
            if (memo.ContainsKey(targetSum)) return memo[targetSum];

            if (recurse >= 20) throw new Exception("Fatal: Too nested");

            if (targetSum == 0) return true;
            if (targetSum < 0) return false;

            foreach(BigInteger element in nums)
            {
                BigInteger remain = targetSum - element;
                if (canSum(remain, nums, memo, recurse+1))
                {
                    memo[targetSum] = true;
                    return true;
                }
            }

            memo[targetSum] = false;
            return false;
        }
        /// <summary>
        /// Finds the shortest possible way to create the sum and returns that list
        /// </summary>
        /// <param name="elements">The list of items to create {sum} from</param>
        /// <param name="sum">The magical number you want to recreate</param>
        /// <param name="memo">A memory list to reduce the time taken finding the best way to make the sum!</param>
        /// <returns>The list of {elements} recursively that can make the sum</returns>
        public static List<int> BestSum(List<int> elements, int sum, Dictionary<int, List<int>> memo)
        {
            if (memo != null)
            {
                if (memo.ContainsKey(sum)) return memo[sum];
            }
            else memo = new Dictionary<int, List<int>>();

            if (sum == 0) return new List<int>();
            if (sum < 0) return null;
            if (!canSum(sum, elements, null)) return null;


            List<int> shortestCombo = null;

            foreach (int element in elements)
            {
                int remainder = sum - element;

                List<int> remCombo = BestSum(elements, remainder, memo);
                if (remCombo != null)
                {
                    List<int> combo = new List<int>(remCombo);
                    combo.Add(element);
                    if (shortestCombo == null || combo.Count < shortestCombo.Count)
                    {
                        shortestCombo = combo;
                    }
                }
                
            }

            memo[sum] = shortestCombo;
            return shortestCombo;
        }
        /// <summary>
        /// Finds the shortest possible way to create the sum and returns that list
        /// </summary>
        /// <param name="elements">The list of items to create {sum} from</param>
        /// <param name="sum">The magical number you want to recreate</param>
        /// <param name="memo">A memory list to reduce the time taken finding the best way to make the sum!</param>
        /// <returns>The list of {elements} recursively that can make the sum</returns>
        public static List<BigInteger> BestSum(List<BigInteger> elements, BigInteger sum, Dictionary<BigInteger, List<BigInteger>> memo, int recursion=0)
        {
            if (memo != null)
            {
                if (memo.ContainsKey(sum)) return memo[sum];
            }
            else memo = new Dictionary<BigInteger, List<BigInteger>>();
            if (recursion >= 20) throw new Exception("Fatal: Too nested");
            if (sum == 0) return new List<BigInteger>();
            if (sum < 0) return null;
            if (!canSum(sum, elements, null)) return null;

            List<BigInteger> shortestCombo = null;

            foreach (BigInteger element in elements)
            {
                BigInteger remainder = sum - element;
                List<BigInteger> remCombo = BestSum(elements, remainder, memo, recursion+1);
                if (remCombo != null)
                {
                    List<BigInteger> combo = new List<BigInteger>(remCombo);
                    combo.Add(element);
                    if (shortestCombo == null || combo.Count < shortestCombo.Count)
                    {
                        shortestCombo = combo;
                    }
                }
            }

            memo[sum] = shortestCombo;
            return shortestCombo;
        }
        /// <summary>
        /// This function is meant for aiding in attacking the TripleDES encryption for the EDRA algorithm. We intentionally want to break TripleDES since we know for a fact we have one of the correct answers that was used as the encryption key. By doing this, we find the right encryption key with zero knowledge
        /// </summary>
        /// <param name="input">The TripleDES byte array we want to blank</param>
        /// <returns>Zero filled Byte Array</returns>
        public static byte[] MakeBlankKey(byte[] input)
        {
            return new byte[input.Length];
        }

        /// <summary>
        /// This function is meant for aiding in attacking the TripleDES encryption for the EDRA algorithm. We intentionally want to break TripleDES since we know for a fact that we have one of the solutions. This function will increment the current key. If the attack has completed an exception will be thrown
        /// </summary>
        /// <param name="tdes">The key array</param>
        /// <returns>The next key in sequence</returns>
        public static byte[] IncrementAttackVector(byte[] tdes)
        {
            string hstr = Convert.ToHexString(tdes);
            // Loop over the hex to check if every digit is an F
            int Fi = 0;
            for(int i = 0; i < hstr.Length; i++)
            {
                if (hstr[i] == 'F')
                {
                    Fi++;
                }
            }
            if (Fi == hstr.Length) throw new ArgumentException("The operation is already completed");
            BigInteger num = BigInteger.Parse(hstr, System.Globalization.NumberStyles.HexNumber);
            num++;
            hstr = Convert.ToHexString(num.ToByteArray());
            return Convert.FromHexString(hstr);

        }

#pragma warning disable IDE1006 // Naming Styles
        public static Int32 getTimestamp()
#pragma warning restore IDE1006 // Naming Styles
        {
            return int.Parse(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        }

        public static string userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
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

        public static byte[] SHA256HashBytes(byte[] ToHash)
        {
            SHA256 hasher = SHA256.Create();
            return hasher.ComputeHash(ToHash);
        }
        public static byte[] SHA256HashBytes(string ToHash)
        {
            SHA256 hasher = SHA256.Create();
            return hasher.ComputeHash(UTF8Encoding.UTF8.GetBytes(ToHash));
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

        /// <summary>
        /// Format: 
        /// w = week
        /// d = day
        /// h = hour
        /// m = minute
        /// s = second
        /// </summary>
        /// <param name="TimeStr"></param>
        /// <returns></returns>
        public static TimeSpan DecodeTimeNotation(string TimeStr)
        {

            List<string> P = TimeStr.ToLower().llParseString2List(new string[] { }, new string[] { "w", "d", "h", "m", "s" });
            int i = 0;
            //DateTime F = DateTime.Now;

            TimeSpan F = new TimeSpan();
            while (i < P.Count)
            {
                string ENTRY = P[i];
                string TYPE = P[i + 1];
                switch (TYPE)
                {
                    case "w":
                        {

                            F = F.Add(TimeSpan.FromDays(7*double.Parse(ENTRY)));
                            break;
                        }
                    case "d":
                        {
                            F = F.Add(TimeSpan.FromDays(double.Parse(ENTRY)));
                            break;
                        }
                    case "h":
                        {
                            F = F.Add(TimeSpan.FromHours(double.Parse(ENTRY)));
                            break;
                        }
                    case "m":
                        {
                            F = F.Add(TimeSpan.FromMinutes(double.Parse(ENTRY)));
                            break;
                        }
                    case "s":
                        {

                            F = F.Add(TimeSpan.FromSeconds(double.Parse(ENTRY)));
                            break;
                        }
                }


                i += 2;
            }
            return F;
        }

        public static int GetUnixDifference(TimeSpan ts)
        {
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// Encodes a timestamp or difference into ZNI Notation
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>ZNI Time Notation</returns>
        public static string EncodeTimeNotation(TimeSpan ts)
        {
            return EncodeTimeNotation(GetUnixDifference(ts));
        }

        /// <summary>
        /// Encodes a unix timestamp or difference into ZNI Notation
        /// </summary>
        /// <param name="ts"></param>
        /// <returns>ZNI Time Notation</returns>
        public static string EncodeTimeNotation(int ts)
        {
            var ONE_DAY = ((60 * 60) * 24);

            var Days = ts / ONE_DAY;
            ts -= (ONE_DAY * Days);
            var Hours = ts / 60 / 60;
            ts -= (Hours * 60 * 60);
            var Minutes = ts / 60;
            ts -= (Minutes * 60);

            var Weeks = Days / 7;
            Days -= Weeks * 7;

            List<string> X = new List<string>();
            if (Weeks > 0)
            {
                X.Add($"{Weeks}");
                X.Add($"w");
            }
            if (Days > 0)
            {
                X.Add($"{Days}");
                X.Add($"d");
            }
            if (Hours > 0)
            {
                X.Add($"{Hours}");
                X.Add($"h");
            }
            if (Minutes > 0)
            {
                X.Add($"{Minutes}");
                X.Add($"m");
            }
            if (ts > 0)
            {
                X.Add($"{ts}");
                X.Add($"s");
            }
            return String.Join("", X);
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

        /// <summary>
        /// Ensures that the bit (bitToSet) is set on (i)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="bitToSet"></param>
        /// <returns>(i) with (bitToSet) set</returns>
        public static void SetBit(this ref int i, int bitToSet)
        {
            if (!i.BitSet(bitToSet))
            {
                i += bitToSet;
            }
        }
        public static bool BitSet(this ref int i, int bit)
        {
            return (i & bit) != 0;
        }
        /// <summary>
        /// Ensures that the bit (bitToSet) is not set on (i)
        /// </summary>
        /// <param name="bitToSet"></param>
        /// <returns>(i) with (bitToSet) not set</returns>
        public static void UnsetBit(this ref int i, int bitToSet)
        {
            if (i.BitSet(bitToSet))
            {
                i -= bitToSet;
            }
        }

        /// <summary>
        /// Ensures that the bit (bitToSet) is set on (i)
        /// </summary>
        /// <param name="i"></param>
        /// <param name="bitToSet"></param>
        /// <returns>(i) with (bitToSet) set</returns>
        public static void SetBit(this ref byte i, byte bitToSet)
        {
            if (!i.BitSet(bitToSet))
            {
                i += bitToSet;
            }
        }
        public static bool BitSet(this ref byte i, byte bit)
        {
            return (i & bit) != 0;
        }
        /// <summary>
        /// Ensures that the bit (bitToSet) is not set on (i)
        /// </summary>
        /// <param name="bitToSet"></param>
        /// <returns>(i) with (bitToSet) not set</returns>
        public static void UnsetBit(this ref byte i, byte bitToSet)
        {
            if (i.BitSet(bitToSet))
            {
                i -= bitToSet;
            }
        }

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
        public static BigInteger GetAtIndex(this List<BigInteger> X, BigInteger I)
        {
            BigInteger D = -1;
            foreach(BigInteger V in X)
            {
                D++;
                if (D >= I) return V;
            }
            return 0;
        }
        public static int GoesIntoTimes(this BigInteger X, BigInteger Z)
        {
            int nTimes = 0;
            if (X < Z) nTimes++;
            BigInteger XC = X;
            while(XC < Z)
            {
                nTimes++;
                XC = XC + X;
                if (XC >= Z)
                {
                    nTimes--;
                    break;
                }
            }

            return nTimes;
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

#pragma warning disable IDE1006 // Naming Styles
        public static List<string> llParseString2List(this string item, string[] opts, string[] keepopts)
        {
            return ParseString2List(item, opts, keepopts, false);
        }
        public static List<string> llParseStringKeepNulls(this string item, string[] opts, string[] keepopts)
        {
            return ParseString2List(item, opts, keepopts, true);
        }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles
        public static string llDumpList2String<T>(this List<T> items, string delimiter)
#pragma warning restore IDE1006 // Naming Styles
        {
            return String.Join(delimiter, items.Select(t => t.ToString()).ToArray());
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
        /// <summary>
        /// This function was partially taken from the OpenSimulator code and modified to no longer be LSL related and purely function the same way for my own sanity's sake
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="opts"></param>
        /// <param name="keepopts"></param>
        /// <returns></returns>
        public static List<string> ParseString2List(this string item, string[] opts, string[] keepopts, bool keepNulls)
        {
            int sourceLen = item.Length;
            int optionsLength = opts.Length;
            int spacersLen = keepopts.Length;

            int dellen = 0;
            string[] delArray = new string[optionsLength + spacersLen];

            int outlen = 0;
            string[] outArray = new string[sourceLen * 2 + 1];

            int i, j;
            string d;


            for(i=0;i<optionsLength; i++)
            {
                d = opts[i].ToString();
                if(d.Length > 0)
                {
                    delArray[dellen++] = d; 
                }
            }
            optionsLength = dellen;


            for (i = 0; i < spacersLen; i++)
            {
                d = keepopts[i].ToString();
                if(d.Length > 0)
                {
                    delArray[dellen++] = d;
                }
            }


            for(i=0; ;)
            {
                int earliestDel = -1;
                int earliestSrc = sourceLen;
                string earliestStr = null;

                for(j = 0;j < dellen; j++)
                {
                    d = delArray[j];
                    if(d != null)
                    {
                        int idx = item.IndexOf(d, i);
                        if(idx < 0)
                        {
                            delArray[j] = null;
                        }
                        else if(idx < earliestSrc)
                        {
                            earliestSrc = idx;
                            earliestDel = j;
                            earliestStr = d;
                            if (idx == i) break;
                        }
                    }
                }

                if(keepNulls || (earliestSrc > i))
                {
                    outArray[outlen++] = item.Substring(i, earliestSrc - i);
                }
                if (earliestDel < 0) break;
                if(earliestDel >= optionsLength)
                {
                    outArray[outlen++] = earliestStr;
                }
                i = earliestSrc + earliestStr.Length;
            }

            List<string> outList = new List<string>();
            for(i = 0;i<outlen; i++)
            {
                outList.Add(outArray[i]);
            }

            return outList;
            /*
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

            return entries;*/
        }
    }
}
