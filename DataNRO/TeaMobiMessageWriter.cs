﻿using System;
using System.IO;

namespace DataNRO
{
    internal class TeaMobiMessageWriter : IMessageWriter
    {
        static readonly string VERSION = "2.4.0";

        TeaMobiSession session;

        public void SetSession(TeaMobiSession session)
        {
            this.session = session;
        }

        public void UpdateMap()
        {
            MessageSend message = MessageNotMap(6);
            session.SendMessage(message);
        }

        public void UpdateItem()
        {
            MessageSend message = MessageNotMap(8);
            session.SendMessage(message);
        }

        public void UpdateSkill()
        {
            MessageSend message = MessageNotMap(7);
            session.SendMessage(message);
        }

        public void UpdateData()
        {
            MessageSend message = new MessageSend(-87);
            session.SendMessage(message);
        }

        public void ClientOk()
        {
            MessageSend message = MessageNotMap(13);
            session.SendMessage(message);
        }

        public void SetClientType()
        {
            MessageSend message = MessageNotLogin(2);
            message.WriteByte(4);
            message.WriteByte(2);
            message.WriteBool(false);
            message.WriteInt(512);
            message.WriteInt(300);
            message.WriteBool(true);
            message.WriteBool(true);
            message.WriteStringUTF("Pc platform xxx|" + VERSION);
            Stream stream = typeof(TeaMobiMessageWriter).Assembly.GetManifestResourceStream("DataNRO.Resources.info");
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            message.WriteShort((short)array.Length);
            message.WriteBytes(array);
            stream.Close();
            session.SendMessage(message);
        }

        public void ImageSource()
        {
            MessageSend message = new MessageSend(-111);
            message.WriteShort(0);
            session.SendMessage(message);
        }

        public void Login(string username, string pass, sbyte type)
        {
            MessageSend message = MessageNotLogin(0);
            message.WriteStringUTF(username);
            message.WriteStringUTF(pass);
            message.WriteStringUTF(VERSION);
            message.WriteSByte(type);
            session.SendMessage(message);
        }

        public void FinishUpdate()
        {
            MessageSend message = new MessageSend(-38);
            session.SendMessage(message);
        }

        public void FinishLoadMap()
        {
            MessageSend message = new MessageSend(-39);
            session.SendMessage(message);
        }

        MessageSend MessageNotMap(sbyte command)
        {
            MessageSend message = new MessageSend(-28);
            message.WriteSByte(command);
            return message;
        }

        MessageSend MessageNotLogin(sbyte command)
        {
            MessageSend message = new MessageSend(-29);
            message.WriteSByte(command);
            return message;
        }

    }
}
