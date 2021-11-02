using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using HarmonyLib;
using System;
using System.Net.Sockets;
using System.Text;
namespace ClientSubnautica.ClientManager
{
    class OnDrop
    {
        [HarmonyPatch(typeof(Inventory), "InternalDropItem")]
        public class Patches
        {
            [HarmonyPostfix]
            static void Postfix(Pickupable pickupable)
            {
                NetworkStream ns2 = StartMultiplayer.client.GetStream();
                
                byte[] msgresponse = Encoding.ASCII.GetBytes("");
                Array.Clear(msgresponse, 0, msgresponse.Length);
                msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SpawnPiece")+":" + pickupable.GetTechName() + "/END/");
                // Position envoyé !
                ns2.Write(msgresponse, 0, msgresponse.Length);
            }
        }
    }
}
