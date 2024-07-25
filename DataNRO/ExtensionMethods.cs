using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNRO
{
    internal static class ExtensionMethods
    {
        internal static byte[] Reverse(this byte[] b) => b.Reverse<byte>().ToArray();
        internal static ushort ReadUInt16BE(this BinaryReader binRdr) => BitConverter.ToUInt16(binRdr.ReadBytesRequired(sizeof(ushort)).Reverse(), 0);
        internal static short ReadInt16BE(this BinaryReader binRdr) => BitConverter.ToInt16(binRdr.ReadBytesRequired(sizeof(short)).Reverse(), 0);
        internal static uint ReadUInt32BE(this BinaryReader binRdr) => BitConverter.ToUInt32(binRdr.ReadBytesRequired(sizeof(uint)).Reverse(), 0);
        internal static int ReadInt32BE(this BinaryReader binRdr) => BitConverter.ToInt32(binRdr.ReadBytesRequired(sizeof(int)).Reverse(), 0);
        internal static ulong ReadUInt64BE(this BinaryReader binRdr) => BitConverter.ToUInt64(binRdr.ReadBytesRequired(sizeof(ulong)).Reverse(), 0);
        internal static long ReadInt64BE(this BinaryReader binRdr) => BitConverter.ToInt64(binRdr.ReadBytesRequired(sizeof(long)).Reverse(), 0);

        internal static byte[] ReadBytesRequired(this BinaryReader reader, int byteCount)
        {
            var result = reader.ReadBytes(byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

            return result;
        }
    }
}
