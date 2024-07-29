using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNRO
{
    internal interface ISession
    {
        IMessageReceiver MessageReceiver { get; set; }
        IMessageWriter MessageWriter { get; set; }
        string Host { get; }
        ushort Port { get; }
        bool IsConnected { get; }
        void Connect();
        void SendMessage(MessageSend message);
        void Disconnect();
    }
}
