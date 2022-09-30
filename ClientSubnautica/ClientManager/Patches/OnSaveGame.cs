using System.Net.Sockets;
using System.Threading;
using ClientSubnautica.MultiplayerManager.SendData;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UWE;

namespace ClientSubnautica.ClientManager.Patches
{
    class SaveGame
    {
        [HarmonyPatch(typeof(IngameMenu), "SaveGame")]
        public class Patches
        {
            [HarmonyPostfix]
            static bool Prefix(IngameMenu __instance)
            {

                GameObject mainPanel = __instance.mainPanel;
                Button save = __instance.saveButton;
                
                save.onClick.AddListener(SaveButtonClick);
                
                mainPanel.SetActive(true);

                return false;
            }
            private static void SaveButtonClick()
            {
                Player player = Player.main;
                Vector3 position = player.transform.position;

                SendOnSaveGame.SendPosition(position);
            }
        }
    }
}