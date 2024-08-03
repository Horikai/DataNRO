using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Starksoft.Net.Proxy;

namespace DataNRO
{
    internal class Program
    {
        static string proxyData = "";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            proxyData = Environment.GetEnvironmentVariable("PROXY");
            string[] datas = Environment.GetEnvironmentVariable("DATA").Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string data in datas)
                LoginAndGetData(data);
        }

        static void LoginAndGetData(string data)
        {
            GameData.Reset();
            string[] arr = data.Split('|');
            string host = arr[0];
            ushort port = ushort.Parse(arr[1]);
            string unregisteredUser = arr[2];
            string account = arr[3];
            string password = arr[4];
            string folderName = arr[5];
            if (!Directory.Exists("Data\\" + folderName))
                Directory.CreateDirectory("Data\\" + folderName);
            ISession session = new TeaMobiSession(host, port)
            {
                MessageReceiver = new TeaMobiMessageReceiver(),
                MessageWriter = new TeaMobiMessageWriter()
            };
            Console.WriteLine($"Connecting to {session.Host}:{session.Port}...");
            if (!TryConnect(session))
                return;
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
            //writer.Chat("DataNRO by ElectroHeavenVN");
            Console.WriteLine($"[{session.Host}:{session.Port}] Disconnect from {session.Host}:{session.Port} in 10s...");
            Thread.Sleep(10000);
            session.Disconnect();
            Console.WriteLine($"[{session.Host}:{session.Port}] Writing data to {folderName}\\...");
            Formatting formatting = Formatting.Indented;
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.Maps)}.json", JsonConvert.SerializeObject(GameData.Maps, formatting));
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.NpcTemplates)}.json", JsonConvert.SerializeObject(GameData.NpcTemplates, formatting));
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.MobTemplates)}.json", JsonConvert.SerializeObject(GameData.MobTemplates, formatting));
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.ItemOptionTemplates)}.json", JsonConvert.SerializeObject(GameData.ItemOptionTemplates, formatting));
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.NClasses)}.json", JsonConvert.SerializeObject(GameData.NClasses, formatting));
            File.WriteAllText($"Data\\{folderName}\\{nameof(GameData.ItemTemplates)}.json", JsonConvert.SerializeObject(GameData.ItemTemplates, formatting));
            File.WriteAllText($"Data\\{folderName}\\LastUpdated", DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
            Thread.Sleep(3000);
        }

        static bool TryConnect(ISession session)
        {
            int retryTimes = 0;
            try
            {
                session.Connect();
            }
            catch
            {
                while (!session.IsConnected)
                {
                    try
                    {
                        session.Connect();
                    }
                    catch
                    {
                        if (retryTimes >= 3)
                            break;
                        Console.WriteLine($"Retry {retryTimes + 1}...");
                        Thread.Sleep(1000);
                    }
                    retryTimes++;
                }
                if (!session.IsConnected)
                {
                    if (!string.IsNullOrEmpty(proxyData))
                    {
                        string[] arrP = proxyData.Split(':');
                        ProxyType proxyType = (ProxyType)Enum.Parse(typeof(ProxyType), arrP[0]);
                        string proxyHost = arrP[1];
                        ushort proxyPort = ushort.Parse(arrP[2]);
                        string proxyUsername = "";
                        string proxyPassword = "";
                        if (arrP.Length > 3)
                            proxyUsername = arrP[3];
                        if (arrP.Length > 4)
                            proxyPassword = arrP[4];
                        retryTimes = 0;
                        Console.WriteLine($"Failed to connect! Retry with proxy {Regex.Replace(proxyHost, "[0-9]", "*")}:{proxyPort}...");
                        try
                        {
                            session.Connect(proxyHost, proxyPort, proxyUsername, proxyPassword, proxyType);
                        }
                        catch
                        {
                            while (!session.IsConnected)
                            {
                                try
                                {
                                    session.Connect(proxyHost, proxyPort, proxyUsername, proxyPassword, proxyType);
                                }
                                catch
                                {
                                    if (retryTimes >= 3)
                                        break;
                                    Console.WriteLine($"Retry {retryTimes + 1}...");
                                    Thread.Sleep(1000);
                                }
                                retryTimes++;
                            }
                            if (!session.IsConnected)
                            {
                                Console.WriteLine($"Failed to connect!");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to connect!");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
