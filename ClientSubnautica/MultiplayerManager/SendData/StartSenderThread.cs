using ClientSubnautica.MultiplayerManager.SendData;
using ClientSubnautica.StartMod;
using HarmonyLib;
using System.Net.Sockets;
using System.Threading;

namespace ClientSubnautica.MultiplayerManager
{
    class StartSenderThread
    {
        [HarmonyPatch(typeof(Player), "Awake")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                //Thread sender
                Thread threadSender = new Thread(o => SendMyPos.start((TcpClient)o));
                threadSender.Start(MainMenuBegin.client);              
            }
        }
    }
}
