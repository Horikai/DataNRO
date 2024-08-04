using System;
using System.IO;
using System.Text;
using DataNRO.Interfaces;

namespace DataNRO
{
    public class MessageReceive : IMessage, IDisposable
    {
        byte[] buffer;
        sbyte cmd;
        BinaryReader reader;

        public sbyte Command => cmd;
        public byte[] Buffer => buffer;
        public long DataLength => buffer.GetLongLength(0);
        public long CurrentPosition => reader.BaseStream.Position;

        public MessageReceive(sbyte command, byte[] buffer)
        {
            cmd = command;
            this.buffer = buffer;
            reader = new BinaryReader(new MemoryStream(buffer));
        }

        public MessageReceive(sbyte command, sbyte[] buffer)
        {
            cmd = command;
            this.buffer = new byte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                this.buffer[i] = (byte)buffer[i];
            reader = new BinaryReader(new MemoryStream(this.buffer));
        }

        public bool ReadBool() => reader.ReadBoolean();
        public byte ReadByte() => reader.ReadByte();
        public sbyte ReadSByte() => reader.ReadSByte();
        public short ReadShort() => reader.ReadInt16BE();
        public ushort ReadUShort() => reader.ReadUInt16BE();
        public char ReadChar() => reader.ReadChar();
        public int ReadInt() => reader.ReadInt32BE();
        public uint ReadUInt() => reader.ReadUInt32BE();
        public long ReadLong() => reader.ReadInt64BE();
        public ulong ReadULong() => reader.ReadUInt64BE();
        public byte[] ReadBytes(int count) => reader.ReadBytes(count);

        public sbyte[] ReadSBytes(int count)
        {
            byte[] data = reader.ReadBytes(count);
            sbyte[] result = new sbyte[count];
            for (int i = 0; i < count; i++)
                result[i] = (sbyte)data[i];
            return result;
        }

        public string ReadString()
        {
            short length = ReadShort();
            byte[] data = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(data);
        }

        public void Dispose() => reader.Dispose();
    }
}
