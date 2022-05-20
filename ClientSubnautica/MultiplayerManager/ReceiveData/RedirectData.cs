using ClientSubnautica.MultiplayerManager.ReceiveData;
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
            if (InitializeConnection.threadStarted)
            {
                lock (m_lockRequests)
                {
                    foreach (var item in receivedRequestsQueue)
                    {
                        if (item.Contains("/END/"))
                        {
                            string[] commands= item.Split(new string[] { "/END/" }, StringSplitOptions.None);
                            foreach (var command in commands)
                            {
                                try
                                {
                                    if (command.Length > 1)
                                    {
                                        string idCMD = command.Split(':')[0];
                                        string[] param;
                                        param = command.Substring(command.IndexOf(":") + 1).Split(';');
                                        Type type = typeof(FunctionManager);
                                        MethodInfo method = type.GetMethod(NetworkCMD.Translate(idCMD));
                                        FunctionManager c = new FunctionManager();
                                        method.Invoke(c, new System.Object[] { param });
                                    }
                                }
                                catch (Exception e) {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                    receivedRequestsQueue.Clear();
                }
            }
        }
    }
}
