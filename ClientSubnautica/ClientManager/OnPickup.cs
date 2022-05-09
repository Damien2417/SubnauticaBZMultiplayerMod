using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using HarmonyLib;
using System;
using System.Net.Sockets;
using System.Text;
namespace ClientSubnautica.ClientManager
{
    class OnPickup
    {
        [HarmonyPatch(typeof(Inventory), "Pickup")]
        public class Patches
        {
            [HarmonyPostfix]
            static void Postfix(Pickupable pickupable)
            {
                if (pickupable.gameObject.GetComponent<UniqueGuid>() != null)
                {
                    NetworkStream ns2 = MainMenuBegin.client.GetStream();

                    byte[] msgresponse = Encoding.ASCII.GetBytes("");
                    Array.Clear(msgresponse, 0, msgresponse.Length);
                    msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("PickupPiece") + ":" + pickupable.gameObject.GetComponent<UniqueGuid>().guid + "/END/");
                    // Position envoyé !
                    ns2.Write(msgresponse, 0, msgresponse.Length);
                }
            }
        }
    }
}
