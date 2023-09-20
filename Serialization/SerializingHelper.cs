using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAC.Serialization
{
    public static class SerializingHelper
    {
        private static int SEGMENT_BITS = 0x7F;
        private static int CONTINUE_BIT = 0x80;
        /// <summary>
        /// Adapted from code from wiki.vg
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="value"></param>
        public static void WriteVarInt(this BinaryWriter bw, int value)
        {
            while (true)
            {
                if ((value & ~SEGMENT_BITS) == 0)
                {
                    bw.Write((byte)value);
                    return;
                }

                byte b = (byte)((value & SEGMENT_BITS) | CONTINUE_BIT);
                bw.Write(b);

                value >>>= 7;
            }
        }

        public static int ReadVarInt(this BinaryReader br)
        {
            int value = 0;
            int position = 0;
            byte curByte;

            while (true)
            {
                curByte = br.ReadByte();
                value |= (curByte & SEGMENT_BITS) << position;
                if ((curByte & CONTINUE_BIT) == 0) break;

                position += 7;

                if (position >= 32) throw new VarIntSizeException("Too large");
            }

            return value;
        }

        public static void WriteVarLong(this BinaryWriter bw, long value)
        {
            while (true)
            {
                if ((value & ~((long)SEGMENT_BITS)) == 0)
                {
                    byte bx = (byte)value;
                    bw.Write(bx);
                    return;
                }
                byte b = (byte)((value & SEGMENT_BITS) | CONTINUE_BIT);
                bw.Write(b);
                value >>>= 7;
            }
        }

        public static long ReadVarLong(this BinaryReader br)
        {
            long value = 0;
            int position = 0;
            byte curByte;
            while (true)
            {
                curByte = br.ReadByte();
                value |= (long)(curByte & SEGMENT_BITS) << position;

                if ((curByte & CONTINUE_BIT) == 0) break;

                position += 7;

                if (position >= 64) throw new VarLongSizeException("Too large");
            }

            return value;
        }

        /// <summary>
        /// Applies ZigZag Encoding to the integer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ZigZag(this int i)
        {
            return Math.Abs((i + i) + ((i < 0) ? 1 : 0));
        }
        /// <summary>
        /// Undoes the zigzag encoding on a integer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int DeZigZag(this int i)
        {
            if ((i % 2) == 0)
            {
                // Even number. Divide by two
                return (i / 2);
            }
            else
            {
                int x = i - 1;
                x = -i;
                return x;
            }
        }


        /// <summary>
        /// Applies ZigZag Encoding to the integer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static long ZigZag(this long i)
        {
            return Math.Abs((i + i) + ((i < 0L) ? 1L : 0L));
        }
        /// <summary>
        /// Undoes the zigzag encoding on a integer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static long DeZigZag(this long i)
        {
            if ((i % 2) == 0)
            {
                // Even number. Divide by two
                return (i / 2L);
            }
            else
            {
                long x = i - 1L;
                x = -i;
                return x;
            }
        }


    }

    public class VarIntSizeException : Exception
    {
        public VarIntSizeException(string Message) : base(Message) { }
    }
    public class VarLongSizeException : Exception
    {
        public VarLongSizeException(string Message) : base(Message) { }
    }
}
