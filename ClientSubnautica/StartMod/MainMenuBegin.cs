using ClientSubnautica.MultiplayerManager;
using ClientSubnautica.MultiplayerManager.ReceiveData;
using HarmonyLib;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UWE;

namespace ClientSubnautica.StartMod
{

    // patches
    [HarmonyPatch(typeof(uGUI_MainMenu), "Start")]
    public class MainMenuBegin
    {
        public static TcpClient client = new TcpClient();
        public static bool threadStarted = false;

        class SimpleEnumerator : IEnumerable
        {
            public IEnumerator enumerator;
            public Action prefixAction, postfixAction;
            public Action<object> preItemAction, postItemAction;
            public Func<object, object> itemAction;
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public IEnumerator GetEnumerator()
            {
                prefixAction();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    preItemAction(item);
                    //yield return itemAction(item);
                    postItemAction(item);
                    yield return item;
                }
                postfixAction();
            }
        }

        static void Postfix(ref IEnumerator __result, uGUI_MainMenu __instance)
        {
            Action prefixAction = () => {
                //Thread sender                    
                client =ConnectToServer.start();
                bool isconnected = client.Connected;
                NetworkStream ns = client.GetStream();
                byte[] receivedBytes = new byte[1024];

                ErrorMessage.AddMessage("Downloading map... 0%");
                byte[] data=downloadMap(ns);
                ErrorMessage.AddMessage("Downloading map... 100%");

                string outDirectoryPath = importMap(data);
                
                ErrorMessage.AddMessage("Map downloaded !");


                byte[] receivedBytes2 = new byte[1024];
                int byte_count;

                byte_count = ns.Read(receivedBytes2, 0, receivedBytes2.Length);

                string message2 = Encoding.ASCII.GetString(receivedBytes2, 0, byte_count);
                string[] arr=message2.Split('$');

                if (arr != null)
                {
                    GameMode gm=new GameMode();
                    if (arr[2] == "0")
                    {                      
                        gm = GameMode.Survival;
                    }
                    ErrorMessage.AddMessage("Loading map ...");
                    CoroutineHost.StartCoroutine(LoadMap.loadMap(__instance, outDirectoryPath,arr[0],arr[1],gm,arr[3], returnValue =>
                    {


                        byte[] test = Encoding.ASCII.GetBytes("ok");

                        ns.Write(test, 0, test.Length);

                        //Thread receiver
                        Thread threadReceiver = new Thread(o => ReceiveDataFromServer.start((TcpClient)o));
                        threadReceiver.Start(client);

                        threadStarted = true;
                    }));
                }

                //AccessTools.Method(typeof(uGUI_MainMenu), "StartMostRecentSaveOrNewGame").Invoke(__instance, new object[] { }); 
            };
            Action postfixAction = () => { };
            Action<object> preItemAction = (item) => { };
            Action<object> postItemAction = (item) => { };
            Func<object, object> itemAction = (item) =>
            {
                var newItem = item + "+";

                return newItem;
            };
            var myEnumerator = new SimpleEnumerator()
            {
                enumerator = __result,
                prefixAction = prefixAction,
                postfixAction = postfixAction,
                preItemAction = preItemAction,
                postItemAction = postItemAction,
                itemAction = itemAction
            };
            __result = myEnumerator.GetEnumerator();
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