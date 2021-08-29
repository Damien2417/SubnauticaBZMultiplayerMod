using HarmonyLib;
using UWE;

namespace ClientSubnautica.ClientManager
{
    class DisablePause
    {
        [HarmonyPatch(typeof(IngameMenu), "OnSelect")]
        public class Patches
        {
            [HarmonyPostfix]
            static void Postfix()
            {
                FreezeTime.End(FreezeTime.Id.IngameMenu);
            }
        }
    }
}
