using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UWE;

namespace ClientSubnautica.MultiplayerManager
{
    internal class InitializeConnection
    {
        public static TcpClient client = new TcpClient();
        public static bool threadStarted = false;

        public void start(string ip)
        {
            //Thread sender

            client = ConnectToServer.start(ip);

            bool isconnected = client.Connected;
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];

            ErrorMessage.AddMessage("Downloading map... 0%");
            byte[] data = downloadMap(ns);
            ErrorMessage.AddMessage("Downloading map... 100%");

            string outDirectoryPath = importMap(data);

            ErrorMessage.AddMessage("Map downloaded !");


            byte[] receivedBytes2 = new byte[1024];
            int byte_count;

            byte_count = ns.Read(receivedBytes2, 0, receivedBytes2.Length);

            string message2 = Encoding.ASCII.GetString(receivedBytes2, 0, byte_count);
            string[] arr = message2.Split('$');

            if (arr != null)
            {

                GameModePresetId gameMode = GameModePresetId.Survival;
                switch (arr[2])
                {
                    /* 
                    case "1":
                        gameMode = GameModePresetId.Freedom;
                        break;
                     
                    case "2":
                        gameMode = GameModePresetId.Hardcore;
                        break;
                    */
                    case "4":
                        gameMode = GameModePresetId.Creative;
                        break;
                }

                ErrorMessage.AddMessage("Loading map ...");
                CoroutineHost.StartCoroutine(LoadMap.loadMap(uGUI_MainMenu.main, outDirectoryPath, arr[0], arr[1], gameMode, new GameOptions(),arr[3], returnValue =>
                    {


                        byte[] test = Encoding.ASCII.GetBytes("ok");

                        ns.Write(test, 0, test.Length);

                        //Thread receiver
                        Thread threadReceiver = new Thread(o => ReceiveDataFromServer.start((TcpClient)o));
                        threadReceiver.Start(client);

                        threadStarted = true;
                    }));
            }
            
        }

        public static byte[] downloadMap(NetworkStream ns)
        {
            byte[] fileSizeBytes = new byte[4];
            int bytes = ns.Read(fileSizeBytes, 0, 4);
            int dataLength = BitConverter.ToInt32(fileSizeBytes, 0);

            int bytesLeft = dataLength;
            byte[] data = new byte[dataLength];

            int bufferSize = 1024;
            int bytesRead = 0;

            while (bytesLeft > 0)
            {
                int curDataSize = System.Math.Min(bufferSize, bytesLeft);
                if (client.Available < curDataSize)
                    curDataSize = client.Available; //This saved me

                bytes = ns.Read(data, bytesRead, curDataSize);

                bytesRead += curDataSize;
                bytesLeft -= curDataSize;
            }
            return data;
        }

        public static string importMap(byte[] data)
        {
            string[] outPath = { MainPatcher.location, "SNAppData", "SavedGames", "MultiplayerSave" };
            if (PlatformServicesEpic.IsPresent())
            {
                outPath[0] = Application.persistentDataPath;
                outPath[1] = "SubnauticaZero";
            }
            string outDirectoryPath = Path.Combine(outPath);
            if (Directory.Exists(outDirectoryPath))
                Directory.Delete(outDirectoryPath, true);

            Directory.CreateDirectory(outDirectoryPath);
            string[] outPath2 = { outDirectoryPath, "world.zip" };
            string outZipPath = Path.Combine(outPath2);
            File.WriteAllBytes(outZipPath, data);
            ZipFile.ExtractToDirectory(outZipPath, outDirectoryPath);
            File.Delete(outZipPath);

            return outDirectoryPath;
        }
    }
}
