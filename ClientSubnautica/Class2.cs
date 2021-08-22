using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientSubnautica
{
    // patches

    /*[HarmonyPatch(typeof(uGUI_MainMenu), "Start")]
    public class Patches
    {
        static public GameObject GOTarget => Target as GameObject;
        static public object Target { get; set; }
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

        static void Postfix(ref IEnumerator __result)
        {
            Action prefixAction = () => { };
            Action postfixAction = () =>
            {
                __result.StartMostRecentSaveOrNewGame();
            };
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
    }*/
}
