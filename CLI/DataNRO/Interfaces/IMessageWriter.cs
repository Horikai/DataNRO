namespace DataNRO.Interfaces
{
    public interface IMessageWriter
    {
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
