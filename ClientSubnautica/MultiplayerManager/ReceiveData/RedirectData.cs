using ClientSubnautica.StartMod;
using HarmonyLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ClientSubnautica.MultiplayerManager
{
    [HarmonyPatch(typeof(Player), "Update")]
    static class RedirectData
    {
        public static List<String> receivedRequestsQueue = new List<String>();
        public static ConcurrentDictionary<int, GameObject> players = new ConcurrentDictionary<int, GameObject>();
        public static object m_lockRequests = new object();
        public static object m_lockPlayers = new object();

        [HarmonyPostfix]
        public static void redirectOnFunctionManager()
        {
            if (MainMenuBegin.threadStarted)
            {
                lock (m_lockRequests)
                {
                    foreach (var item in receivedRequestsQueue)
                    {
                        if (item.Contains(":"))
                        {
                            string[] param = item.Split(':');
                            string id = Char.ToString(param[0][0]);
                            string meth = param[0];
                            //remove id in first position to trigger right method
                            if (meth.Contains("WorldPosition") | meth.Contains("Disconnected") | meth.Contains("SpawnPiece"))
                                meth = meth.Remove(0, 1);

                            Type type = typeof(FunctionManager);
                            MethodInfo method = type.GetMethod(meth);
                            FunctionManager c = new FunctionManager();
                            method.Invoke(c, new System.Object[] { id, param[1] });
                        }
                    }
                    receivedRequestsQueue.Clear();
                }
            }
        }
    }
}
