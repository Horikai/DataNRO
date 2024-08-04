using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataNRO.Interfaces;

namespace DataNRO
{
    public class MessageSend : IMessage, IDisposable
    {
        List<byte> buffer;
        sbyte cmd;

        public sbyte Command => cmd;
        public byte[] Buffer => buffer.ToArray();
        public long DataLength => buffer.LongCount();
        public long CurrentPosition => DataLength;

        public MessageSend(sbyte command)
        {
            cmd = command;
            buffer = new List<byte>();
        }

        public void WriteBool(bool value) => buffer.AddRange(BitConverter.GetBytes(value));
        public void WriteByte(byte value) => buffer.Add(value);
        public void WriteSByte(sbyte value) => buffer.Add((byte)value);
        public void WriteShort(short value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteUShort(ushort value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteChar(char value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteInt(int value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteUInt(uint value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteLong(long value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteULong(ulong value) => buffer.AddRange(BitConverter.GetBytes(value).Reverse());
        public void WriteBytes(byte[] value) => buffer.AddRange(value);

        public void WriteString(string value)
        {
            char[] chars = value.ToCharArray();
            WriteShort((short)chars.Length);
            buffer.AddRange(chars.Cast<byte>());
        }

        public void WriteStringUTF(string value)
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
