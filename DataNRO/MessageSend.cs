using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNRO
{
    internal class MessageSend : IMessage, IDisposable
    {
        List<byte> buffer;
        sbyte cmd;

        public sbyte Command => cmd;
        public byte[] Buffer => buffer.ToArray();
        public long DataLength => buffer.LongCount();
        public long CurrentPosition => DataLength;

        internal MessageSend(sbyte command)
        {
            cmd = command;
            buffer = new List<byte>();
        }

        internal void WriteBool(bool value) => buffer.AddRange(BitConverter.GetBytes(value));
        internal void WriteByte(byte value) => buffer.Add(value);
        internal void WriteSByte(sbyte value) => buffer.Add((byte)value);
        internal void WriteShort(short value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteUShort(ushort value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteChar(char value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteInt(int value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteUInt(uint value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteLong(long value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteULong(ulong value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        internal void WriteBytes(byte[] value) => buffer.AddRange(value);

        internal void WriteString(string value)
        {
            char[] chars = value.ToCharArray();
            WriteShort((short)chars.Length);
            buffer.AddRange(chars.Cast<byte>());
        }

        internal void WriteStringUTF(string value)
        {
            byte[] array = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(65001), Encoding.Unicode.GetBytes(value));
            WriteShort((short)array.Length);
            WriteBytes(array);
        }

        public void Dispose()
        {
            buffer.Clear();
        }
    }
}
