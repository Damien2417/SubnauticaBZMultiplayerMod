using HarmonyLib;
using System.Net.Sockets;
using System.Threading;

namespace ClientSubnautica
{
    class StartMultiplayer
    {
        public static bool threadStarted = false;
        public static TcpClient client = new TcpClient();
        [HarmonyPatch(typeof(Player), "Awake")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {              
                //Thread sender                    
                client = HandleMultiplayer.startServer();
                bool isconnected = client.Connected;
                NetworkStream ns = client.GetStream();
                if (!threadStarted)
                {
                    //Thread receiver
                    Thread threadReceiver = new Thread(o => HandleMultiplayer.ReceiveData((TcpClient)o));
                    threadReceiver.Start(client);

                    //Thread sender
                    Thread threadSender = new Thread(o => HandleMultiplayer.SendData((TcpClient)o));
                    threadSender.Start(client);

                    threadStarted = true;
                }
            }
        }
    }
}
