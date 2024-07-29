namespace DataNRO
{
    internal interface IMessageWriter
    {
        void SetSession(TeaMobiSession session);
        void UpdateMap();
        void UpdateItem();
        void UpdateSkill();
        void UpdateData();
        void ClientOk();
        void SetClientType();
        void ImageSource();
        void Login(string username, string pass, sbyte type);
        void FinishUpdate();
        void FinishLoadMap();
        void Chat(string text);
    }
}
