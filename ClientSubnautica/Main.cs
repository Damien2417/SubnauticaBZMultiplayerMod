using HarmonyLib;
using QModManager.API.ModLoading;

namespace SubnauticaModTest
{
    [QModCore]
    public class MainPatcher
    {
        [QModPatch]
        public static void Patch()
        {
            Harmony harmony = new Harmony("com.boogaliwoogali.subnautica.SubnauticaModTest");
            harmony.PatchAll();
        }
    }
}