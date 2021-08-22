using HarmonyLib;
using System.Net.Sockets;
using System.Threading;
namespace ClientSubnautica
{
    class StartMultiplayer
    {
        [HarmonyPatch(typeof(Player), "Awake")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                //Thread sender
                Thread threadSender = new Thread(o => HandleMultiplayer.SendData((TcpClient)o));
                threadSender.Start(MainMenuBegin.client);              
            }
        }
    }
}
