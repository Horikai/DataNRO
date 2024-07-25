using System;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace DataNRO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ISession session = new TeaMobiSession(Environment.GetEnvironmentVariable("SERVER"), ushort.Parse(Environment.GetEnvironmentVariable("PORT")))
            { 
                MessageReceiver = new TeaMobiMessageReceiver(),
                MessageWriter = new TeaMobiMessageWriter()
            };
            session.Connect();
            IMessageWriter writer = session.MessageWriter;
            writer.SetClientType();
            Thread.Sleep(500);
            writer.ImageSource();
            Thread.Sleep(1000);
            //writer.GetResource(1);
            //Thread.Sleep(200);
            //writer.GetResource(2);
            //Thread.Sleep(12000);
            //writer.GetResource(3);
            //Thread.Sleep(1000);
            string unregisteredUser = Environment.GetEnvironmentVariable("USERAO");
            if (!string.IsNullOrEmpty(unregisteredUser))
                writer.Login(unregisteredUser, "", 1);
            else 
                writer.Login(Environment.GetEnvironmentVariable("ACCOUNT"), Environment.GetEnvironmentVariable("PASSWORD"), 0);
            Thread.Sleep(1000);
            writer.UpdateMap();
            writer.UpdateSkill();
            writer.UpdateItem();
            writer.UpdateData();
            Thread.Sleep(100);
            writer.ClientOk();
            writer.FinishUpdate();
            Thread.Sleep(500);
            writer.FinishLoadMap();
            Console.WriteLine($"Disconnect from {session.Host}:{session.Port} in 15s...");
            Thread.Sleep(15000);
            session.Disconnect();
            File.WriteAllText("Data.json", JsonConvert.SerializeObject(new
            {
                GameData.MapNames,
                GameData.NpcTemplates,
                GameData.MobTemplates,
                GameData.ItemOptionTemplates,
                GameData.NClasses,
                GameData.ItemTemplates
            }, Formatting.None));
        }
    }
}
