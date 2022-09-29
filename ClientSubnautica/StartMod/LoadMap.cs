using System.Collections;
using UnityEngine;
using UWE;

namespace ClientSubnautica.StartMod
{
    class LoadMap
    {
        public static IEnumerator loadMap(uGUI_MainMenu __instance, string saveGame, string session, string changeSet, GameModePresetId gameMode, GameOptions options, string storyVersion, System.Action<GameObject> callback = null)
        {
            GameObject a = new GameObject();
            yield return CoroutineHost.StartCoroutine(__instance.LoadGameAsync(saveGame, session,int.Parse(changeSet), gameMode, options,int.Parse(storyVersion)));
            if (callback != null) { callback.Invoke(a); }
        }
    }
}
