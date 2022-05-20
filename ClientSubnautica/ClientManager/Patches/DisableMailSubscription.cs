using HarmonyLib;
using UWE;

namespace ClientSubnautica.ClientManager
{
    class DisableMailSubscription
    {
        [HarmonyPatch(typeof(MainMenuEmailHandler), "Subscribe")]
        public class Patches
        {
            [HarmonyPrefix]
            static bool Prefix()
            {
                return false;
            }
        }
    }
}
