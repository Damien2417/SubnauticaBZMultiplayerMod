using ClientSubnautica;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

namespace SubnauticaModTest
{
    internal class ApplyPatches
    {
        // patches
        public static bool startMultiplayer = false;
        public static bool startedMultiplayer = false;
        public static bool threadStarted = false;
        public static List<String> messages = new List<String>();
        //public static ConcurrentDictionary<int, string> messagesDic { get; set; }
        public static ConcurrentDictionary<int, GameObject> players = new ConcurrentDictionary<int, GameObject>();
        public static ConcurrentDictionary<int, string> lastPos = new ConcurrentDictionary<int, string>();
        public static ConcurrentDictionary<int, string> posLastLoop = new ConcurrentDictionary<int, string>();

        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal static class Patches
        {

            

            [HarmonyPostfix]
            public static void Postfix()
            {
                
                if (startMultiplayer)
                {
                    //Thread sender
                    TcpClient client=new TcpClient();
                    client = StartMultiplayer.main();
                    bool isconnected = client.Connected;
                    startMultiplayer = !isconnected;
                    startedMultiplayer = isconnected;
                    NetworkStream ns = client.GetStream();
                    if (!threadStarted)
                    {
                        //Thread receiver
                        Thread threadReceiver = new Thread(o => StartMultiplayer.ReceiveData((TcpClient)o));
                        threadReceiver.Start(client);

                        //Thread sender
                        Thread threadSender = new Thread(o => StartMultiplayer.SendData((TcpClient)o));
                        threadSender.Start(client);

                        //Thread position
                        //Thread threadPosition = new Thread(o => StartMultiplayer.setPosPlayer((TcpClient)o));
                        //threadPosition.Start(client);
                        threadStarted = true;
                    }
                }

                if (threadStarted)
                {
                    lock (messages)
                    {
                        foreach (var message in messages)
                        {
                            if (message.Contains("NEWID:"))
                            {
                                //UnityEngine.Debug.Log("Ajout joueur, id: " + int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                                addPlayer(int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                                //ErrorMessage.AddMessage("Player "+ message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]+" joined !");
                            }
                            else if (message.Contains("WORLDPOSITION"))
                            {
                                int id = int.Parse(message.Split(new string[] { "WORLDPOSITION" }, StringSplitOptions.None)[0]);
                                string pos = message.Split(new string[] { "WORLDPOSITION" }, StringSplitOptions.None)[1];

                                lastPos[id] = pos;
                                setPosPlayer(id, pos);
                            }
                            else if (message.Contains("ALLID:"))
                            {
                                //UnityEngine.Debug.Log("Liste joueurs");
                                string ids = message.Split(new string[] { "ALLID:" }, StringSplitOptions.None)[1];
                                string[] idArray = ids.Split('$');
                                if (idArray.Length > 1)
                                {
                                    foreach (var id in idArray)
                                    {
                                        if (id.Length > 0)
                                            addPlayer(int.Parse(id));
                                    }

                                    //UnityEngine.Debug.Log("Liste ajouté");
                                }
                            }
                            else if (message.Contains("DISCONNECTED"))
                            {
                                try
                                {

                                    string id = message.Split(new string[] { "DISCONNECTED" }, StringSplitOptions.None)[0];
                                    GameObject val;
                                    string val2;
                                    string val3;
                                    GameObject.Destroy(players[int.Parse(id)]);

                                    players.TryRemove(int.Parse(id), out val);
                                    posLastLoop.TryRemove(int.Parse(id), out val2);
                                    lastPos.TryRemove(int.Parse(id), out val3);
                                    //ErrorMessage.AddMessage("Player "+id+" disconnected.");
                                }
                                catch
                                { }
                            }                         
                        }
                        messages.Clear();
                    }
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

                GameObject.Destroy(players[id].GetComponent<Animator>());
                
                posLastLoop.TryAdd(id, "0");
                lastPos.TryAdd(id, "0");
            }

            public static void setPosPlayer(int id,string data)
            {

                string pos;
                string x = "";
                string y = "";
                string z = "";
                string rotx = "";
                string roty = "";
                string rotz = "";


            try
                {

                    //UnityEngine.Debug.Log("iciii "+item.Key+" "+item.Value);
                    //UnityEngine.Debug.Log("comparé au num " + item.Key + " valeur " + localPosLastLoop[item.Key]);
                    if (lastPos[id] != posLastLoop[id])
                    {
                        pos = lastPos[id].Split('(')[1];
                        x = pos.Split(';')[0];
                        y = pos.Split(';')[1];
                        z = pos.Split(';')[2];
                        z = z.Substring(0, z.LastIndexOf(")"));
                    /*rotx= pos.Split(';')[3];
                    roty = pos.Split(';')[4];
                    rotz = pos.Split(';')[5];
                    rotz = rotz.Substring(0, z.LastIndexOf(")"));*/


                    float x2 = float.Parse(x.Replace(",", "."), CultureInfo.InvariantCulture);
                        float y2 = float.Parse(y.Replace(",", "."), CultureInfo.InvariantCulture);
                        float z2 = float.Parse(z.Replace(",", "."), CultureInfo.InvariantCulture);

                        /*float x3 = float.Parse(rotx.Replace(",", "."), CultureInfo.InvariantCulture);
                        float y3 = float.Parse(roty.Replace(",", "."), CultureInfo.InvariantCulture);
                        float z3 = float.Parse(rotz.Replace(",", "."), CultureInfo.InvariantCulture);*/

                        players[id].transform.position = new Vector3(x2, y2, z2);

                        posLastLoop[id] = lastPos[id];

                        //players[id].transform.eulerAngles = new Vector3(x3, y3, z3);

                    }
                }
                catch (Exception e)
                {

                    UnityEngine.Debug.Log("It seem that you can't set the position of other players.\n" + e);
                }

                
            }
        }
    }

