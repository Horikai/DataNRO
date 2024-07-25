using System;
using System.IO;
using System.Text;

namespace DataNRO
{
    internal class MessageReceive : IMessage, IDisposable
    {
        byte[] buffer;
        sbyte cmd;
        BinaryReader reader;

        public sbyte Command => cmd;
        public byte[] Buffer => buffer;
        public long DataLength => buffer.GetLongLength(0);
        public long CurrentPosition => reader.BaseStream.Position;

        internal MessageReceive(sbyte command, byte[] buffer)
        {
            cmd = command;
            this.buffer = buffer;
            reader = new BinaryReader(new MemoryStream(buffer));
        }

        internal MessageReceive(sbyte command, sbyte[] buffer)
        {
            cmd = command;
            this.buffer = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                this.buffer[i] = (byte)buffer[i];
            reader = new BinaryReader(new MemoryStream(this.buffer));
        }

        internal bool ReadBool() => reader.ReadBoolean();
        internal byte ReadByte() => reader.ReadByte();
        internal sbyte ReadSByte() => reader.ReadSByte();
        internal short ReadShort() => reader.ReadInt16BE();
        internal ushort ReadUShort() => reader.ReadUInt16BE();
        internal char ReadChar() => reader.ReadChar();
        internal int ReadInt() => reader.ReadInt32BE();
        internal uint ReadUInt() => reader.ReadUInt32BE();
        internal long ReadLong() => reader.ReadInt64BE();
        internal ulong ReadULong() => reader.ReadUInt64BE();
        internal byte[] ReadBytes(int count) => reader.ReadBytes(count);

        internal sbyte[] ReadSBytes(int count)
        {
            byte[] data = reader.ReadBytes(count);
            sbyte[] result = new sbyte[count];
            for (int i = 0; i < count; i++)
                result[i] = (sbyte)data[i];
            return result;
        }

        internal string ReadString()
        {
            short length = ReadShort();
            byte[] data = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(data);
        }

        public void Dispose() => reader.Dispose();
    }
}
