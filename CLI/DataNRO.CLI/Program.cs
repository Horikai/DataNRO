﻿using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DataNRO.Interfaces;
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
            string data = Environment.GetEnvironmentVariable("DATA");
            string[] datas = data.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string d in datas)
                LoginAndGetData(d);
        }

        static void LoginAndGetData(string data)
        {
            string[] arr = data.Split('|');
            string type = arr[0];
            string host = arr[1];
            ushort port = ushort.Parse(arr[2]);
            string unregisteredUser = arr[3];
            string account = arr[4];
            string password = arr[5];
            bool requestAndSaveIcon = bool.Parse(arr[6]);
            string folderName = arr[7];
            string dataPath = $"Data\\{type}\\{folderName}";
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
            ISession session;
            try
            {
                Console.WriteLine($"Creating session type \"{type}\" from assembly \"DataNRO.{type}.dll\"...");
                Assembly assembly = Assembly.LoadFrom($"DataNRO.{type}.dll");
                Console.WriteLine($"Loaded assembly: {assembly.FullName}");
                Console.WriteLine($"Assembly name: {assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title}");
                Console.WriteLine($"Assembly description: {assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description}");
                Console.WriteLine($"Assembly company: {assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company}");
                Console.WriteLine($"Assembly copyright: {assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright}");
                string sessionTypeName = $"DataNRO.{type}.{type}Session";
                Console.WriteLine($"Creating session type: {sessionTypeName}...");
                session = (ISession)Activator.CreateInstance(assembly.GetType(sessionTypeName), new object[] { host, port });
                Console.WriteLine($"Session type \"{type}\" has been created successfully!");
            }
            catch
            {
                Console.WriteLine($"The main assembly for server type \"{type}\" (DataNRO.{type}.dll) does not exist!");
                return;
            }
            session.Data.Path = dataPath;
            session.Data.SaveIcon = requestAndSaveIcon;
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
                Console.WriteLine($"[{session.Host}:{session.Port}] Login as {unregisteredUser.Substring(0, 4)}{new string('*', unregisteredUser.Length - 4)}");
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
            Location location;
            do
            {
                location = session.Player.location;
            }
            while (location == null || string.IsNullOrEmpty(location.mapName));
            Console.WriteLine($"Current map: {location.mapName} [{location.mapId}], zone {location.zoneId}");
            Thread.Sleep(1000);
            TryGoOutsideIfAtHome(session);
            Console.WriteLine($"[{session.Host}:{session.Port}] Disconnect from {session.Host}:{session.Port} in 10s...");
            writer.Chat("DataNRO by ElectroHeavenVN");
            Thread.Sleep(5000);
            writer.Chat("GitHub dot com slash ElectroHeavenVN slash DataNRO");
            Thread.Sleep(5000);
            //request icon here
            session.Disconnect();
            Console.WriteLine($"[{session.Host}:{session.Port}] Writing data to {session.Data.Path}\\...");
            Formatting formatting = Formatting.Indented;
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.Maps)}.json", JsonConvert.SerializeObject(session.Data.Maps, formatting));
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.NpcTemplates)}.json", JsonConvert.SerializeObject(session.Data.NpcTemplates, formatting));
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.MobTemplates)}.json", JsonConvert.SerializeObject(session.Data.MobTemplates, formatting));
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.ItemOptionTemplates)}.json", JsonConvert.SerializeObject(session.Data.ItemOptionTemplates, formatting));
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.NClasses)}.json", JsonConvert.SerializeObject(session.Data.NClasses, formatting));
            File.WriteAllText($"{session.Data.Path}\\{nameof(GameData.ItemTemplates)}.json", JsonConvert.SerializeObject(session.Data.ItemTemplates, formatting));
            File.WriteAllText($"{session.Data.Path}\\LastUpdated", DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
            Thread.Sleep(3000);
            session.Dispose();
        }

        static void TryGoOutsideIfAtHome(ISession session)
        {
            IMessageWriter writer = session.MessageWriter;
            Location location = session.Player.location;
            while (location.mapId <= 23 && location.mapId >= 21)
            {
                Console.WriteLine("The player is at home, trying to go outside...");
                int x = 0, y = 336;
                switch (location.mapId)
                {
                    case 21:
                        x = 495;
                        break;
                    case 22:
                        x = 205;
                        break;
                    case 23:
                        x = 475;
                        break;
                }
                writer.CharMove(x, y);
                Thread.Sleep(200);
                writer.CharMove(x, ++y);
                Thread.Sleep(200);
                writer.CharMove(x, --y);
                Thread.Sleep(200);
                writer.GetMapOffline();
                Thread.Sleep(1000);
                writer.FinishLoadMap();
                Thread.Sleep(3000);
                location = session.Player.location;
                Console.WriteLine($"Current map: {location.mapName} [{location.mapId}], zone {location.zoneId}");
            }
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
                        Console.WriteLine($"Failed to connect to the server! Retry with proxy {Regex.Replace(proxyHost, "[0-9]", "*")}:{proxyPort}...");
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
                                Console.WriteLine("Failed to connect to the server through the provided proxy!");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to connect to the server!");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
