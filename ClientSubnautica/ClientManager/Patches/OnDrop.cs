using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using HarmonyLib;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

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
                pickupable.gameObject.AddComponent<UniqueGuid>();
                pickupable.gameObject.GetComponent<UniqueGuid>().guid = System.Guid.NewGuid().ToString();

                SendOnDrop.send(pickupable);
            }
        }
    }
}
