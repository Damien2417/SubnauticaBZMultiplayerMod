using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UWE;

namespace ClientSubnautica
{
    class LoadMap
    {
        public static IEnumerator loadMap(uGUI_MainMenu __instance, string saveGame, string session, string changeSet, GameMode gameMode, string storyVersion, System.Action<GameObject> callback = null)
        {
            GameObject a = new GameObject();
            yield return CoroutineHost.StartCoroutine(__instance.LoadGameAsync(saveGame, session,int.Parse(changeSet), gameMode, int.Parse(storyVersion)));
            if (callback != null) { callback.Invoke(a); }
        }
    }
}
