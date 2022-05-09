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

                NetworkStream ns2 = MainMenuBegin.client.GetStream();
                
                byte[] msgresponse = Encoding.ASCII.GetBytes("");
                Array.Clear(msgresponse, 0, msgresponse.Length);
                msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SpawnPiece")+":" + pickupable.GetTechName() + ";" + pickupable.gameObject.GetComponent<UniqueGuid>().guid + "/END/");
                // Position envoyé !
                ns2.Write(msgresponse, 0, msgresponse.Length);
            }
        }
    }
}
