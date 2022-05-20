using ClientSubnautica.MultiplayerManager;
using ClientSubnautica.MultiplayerManager.ReceiveData;
using HarmonyLib;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UWE;

namespace ClientSubnautica.StartMod
{

    // patches
    //[HarmonyPatch(typeof(uGUI_MainMenu), "Start")]
    public class MainMenuBegin
    {
        

        class SimpleEnumerator : IEnumerable
        {
            public IEnumerator enumerator;
            public Action prefixAction, postfixAction;
            public Action<object> preItemAction, postItemAction;
            public Func<object, object> itemAction;
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            public IEnumerator GetEnumerator()
            {
                prefixAction();
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    preItemAction(item);
                    //yield return itemAction(item);
                    postItemAction(item);
                    yield return item;
                }
                postfixAction();
            }
        }

        static void Postfix(ref IEnumerator __result, uGUI_MainMenu __instance)
        {
            Action prefixAction = () => {
                

                //AccessTools.Method(typeof(uGUI_MainMenu), "StartMostRecentSaveOrNewGame").Invoke(__instance, new object[] { }); 
            };
            Action postfixAction = () => { };
            Action<object> preItemAction = (item) => { };
            Action<object> postItemAction = (item) => { };
            Func<object, object> itemAction = (item) =>
            {
                var newItem = item + "+";

                return newItem;
            };
            var myEnumerator = new SimpleEnumerator()
            {
                enumerator = __result,
                prefixAction = prefixAction,
                postfixAction = postfixAction,
                preItemAction = preItemAction,
                postItemAction = postItemAction,
                itemAction = itemAction
            };
            __result = myEnumerator.GetEnumerator();
        }



        
    }
}