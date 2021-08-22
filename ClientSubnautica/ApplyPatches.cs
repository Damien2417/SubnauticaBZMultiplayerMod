using HarmonyLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UWE;

namespace ClientSubnautica
{
    internal class ApplyPatches
    {
        // patches      
        public static List<String> messages = new List<String>();
        public static ConcurrentDictionary<int, GameObject> players = new ConcurrentDictionary<int, GameObject>();
        //public static ConcurrentDictionary<int, string> lastPos = new ConcurrentDictionary<int, string>();
        //public static ConcurrentDictionary<int, string> posLastLoop = new ConcurrentDictionary<int, string>();

        [HarmonyPatch(typeof(Player),"Update")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (MainMenuBegin.threadStarted)
                {
                    manageReceivedData();                    
                }                   
            }
        }
            public static void addPlayer(int id)
            {
                var pos = new Vector3((float)-294.3636, (float)17.02644, (float)252.9224);
                GameObject body = GameObject.Find("player_view_female");
                
                body.GetComponentInParent<Player>().staticHead.shadowCastingMode = ShadowCastingMode.On;
                players.TryAdd(id, UnityEngine.Object.Instantiate<GameObject>(body, pos, Quaternion.identity));
                body.GetComponentInParent<Player>().staticHead.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                //GameObject.Destroy(players[id].GetComponent<Animator>());
                
                //posLastLoop.TryAdd(id, "0");
                //lastPos.TryAdd(id, "0");
            }

            public static void setPosPlayer(int id,string data)
            {
                string pos;
                try
                {               
                    //if (lastPos[id] != posLastLoop[id])
                    //{
                    pos = data.Split('(')[1];
                    string x = pos.Split(';')[0];
                    string y = pos.Split(';')[1];
                    string z = pos.Split(';')[2];
                    z = z.Substring(0, z.LastIndexOf(")"));

                    float x2 = float.Parse(x.Replace(",", "."), CultureInfo.InvariantCulture);
                    float y2 = float.Parse(y.Replace(",", "."), CultureInfo.InvariantCulture);
                    float z2 = float.Parse(z.Replace(",", "."), CultureInfo.InvariantCulture);
                    players[id].transform.position = new Vector3(x2, y2, z2);

                //posLastLoop[id] = lastPos[id];

                //}                   
                }
                catch (Exception e)
                {

                    UnityEngine.Debug.Log("It seem that you can't set the position of other players.\n" + e);
                }

                
            }

            public static void manageReceivedData()
            {
                lock (messages)
                {
                    foreach (var item in messages)
                    {
                        if (item.Contains(":"))
                        {
                            string[] param = item.Split(':');
                            string id = Char.ToString(param[0][0]);
                            string meth = param[0];
                            //remove id in first position to trigger right method
                            if (meth.Contains("WorldPosition")| meth.Contains("Disconnected") | meth.Contains("SpawnPiece"))
                                meth=meth.Remove(0, 1);

                            Type type = typeof(MethodResponse);
                            MethodInfo method = type.GetMethod(meth);
                            MethodResponse c = new MethodResponse();
                            method.Invoke(c, new System.Object[] { id, param[1] });
                        }

                    }
                    messages.Clear();
                }
            }
        }
    }

