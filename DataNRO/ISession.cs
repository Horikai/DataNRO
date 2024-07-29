using Starksoft.Net.Proxy;

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
        void Connect(string proxyHost, ushort proxyPort, string proxyUsername, string proxyPassword, ProxyType proxyType);
        void SendMessage(MessageSend message);
        void Disconnect();
    }
}
