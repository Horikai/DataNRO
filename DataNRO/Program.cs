using System;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace DataNRO
{
    internal class Program
    {
        static int threadCount;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string[] datas = Environment.GetEnvironmentVariable("DATA").Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string data in datas)
            {
                new Thread(() => LoginAndGetData(data)).Start();
                threadCount++;
                while (threadCount > 2)
                    Thread.Sleep(1000);
            }
        }

        static void LoginAndGetData(string data)
        {
            string[] arr = data.Split('|');
            string host = arr[0];
            ushort port = ushort.Parse(arr[1]);
            string unregisteredUser = arr[2];
            string account = arr[3];
            string password = arr[4];
            string fileName = arr[5];
            ISession session = new TeaMobiSession(host, port)
            {
                MessageReceiver = new TeaMobiMessageReceiver(),
                MessageWriter = new TeaMobiMessageWriter()
            };
            Console.WriteLine($"Connecting to {session.Host}:{session.Port}...");
            int retryTimes = 0;
            try
            {
                session.Connect();
            }
            catch
            {
                while (!session.IsConnected && retryTimes < 5)
                {
                    Console.WriteLine();
                    try
                    {
                        session.Connect();
                    }
                    catch
                    {
                        Console.WriteLine($"Retry {retryTimes + 1}...");
                        Thread.Sleep(1000);
                    }
                    retryTimes++;
                }
                if (!session.IsConnected)
                {
                    Console.WriteLine("Failed to connect!");
                    threadCount--;
                    return;
                }
            }
            Console.WriteLine("Connected successfully!");
            IMessageWriter writer = session.MessageWriter;
            writer.SetClientType();
            Thread.Sleep(500);
            writer.ImageSource();
            Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(unregisteredUser))
            {
                Console.WriteLine($"[{session.Host}:{session.Port}] Login as User{new string('*', unregisteredUser.Length - 4)}");
                writer.Login(unregisteredUser, "", 1);
            }
            else
            {
                Console.WriteLine($"[{session.Host}:{session.Port}] Login as {new string('*', account.Length)}");
                writer.Login(account, password, 0);
            }
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
            Thread.Sleep(3000);
            writer.Chat("DataNRO by ElectroHeavenVN");
            Console.WriteLine($"[{session.Host}:{session.Port}] Disconnect from {session.Host}:{session.Port} in 15s...");
            Thread.Sleep(15000);
            session.Disconnect();
            Console.WriteLine($"[{session.Host}:{session.Port}] Writing data to {fileName}.json...");
            File.WriteAllText($"{fileName}.json", JsonConvert.SerializeObject(new
            {
                GameData.Maps,
                GameData.NpcTemplates,
                GameData.MobTemplates,
                GameData.ItemOptionTemplates,
                GameData.NClasses,
                GameData.ItemTemplates
            }, Formatting.None));
            threadCount--;
        }
    }
}
