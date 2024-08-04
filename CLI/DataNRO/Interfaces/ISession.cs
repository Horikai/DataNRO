using System;
using Starksoft.Net.Proxy;

namespace DataNRO.Interfaces
{
    public interface ISession : IDisposable
    {
        IMessageReceiver MessageReceiver { get; }
        IMessageWriter MessageWriter { get; }
        string Host { get; }
        ushort Port { get; }
        bool IsConnected { get; }
        void Connect();
        void Connect(string proxyHost, ushort proxyPort, string proxyUsername, string proxyPassword, ProxyType proxyType);
        void SendMessage(MessageSend message);
        void Disconnect();
        GameData Data { get; }
        Player Player { get; }
    }
}
