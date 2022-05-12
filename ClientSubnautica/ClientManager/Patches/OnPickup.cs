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
                    SendOnPickup.send(pickupable);
                }
            }
        }
    }
}
