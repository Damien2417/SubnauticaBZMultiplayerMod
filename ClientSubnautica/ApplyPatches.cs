using HarmonyLib;
using ClientSubnautica;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UWE;

namespace ClientSubnautica
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
        public static TcpClient client = new TcpClient();

        [HarmonyPatch(typeof(Player),"Update")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (startMultiplayer)
                {
                    //Thread sender                    
                    client = StartMultiplayer.startServer();
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

                        threadStarted = true;

                        //CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(techType, true);
                        /*GameObject test;
                        CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject(TechType.BaseObservatory, returnValue =>
                        {
                            test = returnValue;
                            test.transform.position = Player.main.transform.position;

                        }));*/
                    }
                }

                if (threadStarted)
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

            public static void manageReceivedData()
            {
                lock (messages)
                {
                    foreach (var message in messages)
                    {
                        if (message.Contains("NEWID:"))
                        {
                            //UnityEngine.Debug.Log("Ajout joueur, id: " + int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                            addPlayer(int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                            ErrorMessage.AddMessage("Player " + message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1] + " joined !");
                        }
                        else if (message.Contains("WorldPosition:"))
                        {
                            int id = int.Parse(message.Split(new string[] { "WorldPosition:" }, StringSplitOptions.None)[0]);
                            string pos = message.Split(new string[] { "WorldPosition:" }, StringSplitOptions.None)[1];

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
                                ErrorMessage.AddMessage("Player " + id + " disconnected.");
                            }
                            catch
                            { }
                        }
                        else if (message.Contains("SpawnPiece:"))
                        {
                            string data = message.Split(new string[] { "SpawnPiece:" }, StringSplitOptions.None)[1];
                            data= data.Split(new string[] { "/END/" }, StringSplitOptions.None)[0];
                            ErrorMessage.AddMessage("ajout prefab " + data);
                        //SubnauticaModTest.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType));
                            
                            //CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync((TechType)Enum.Parse(typeof(TechType), data), true);
                            //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(request.GetResult());
                            //Builder.Begin((TechType)Enum.Parse(typeof(TechType), data), request.GetResult());
                           
                            //Builder.BeginAsync((TechType)Enum.Parse(typeof(TechType), data));
                        /*

                        string[] arr = data.Split('$');
                        Base.Piece piece = (Base.Piece)Enum.Parse(typeof(Base.Piece), arr[0], true);
                        Int3 cell = Int3.Parse(arr[1]);

                        if (piece == Base.Piece.Invalid)
                        {
                            break;
                        }
                        Base baseObj = new Base();
                        Transform transform = baseObj.GetCellObject(cell);
                        if (transform == null)
                        {
                            transform = baseObj.CreateCellObject(cell);
                        }
                        Base.PieceDef pieceDef = Base.pieces[(int)piece];
                        GameObject gameObject = this.InstantiateOrReuse(pieceDef.prefab.gameObject, transform, position, rotation, cell);
                        if (faceDirection != null && piece == Base.Piece.CorridorBulkhead)
                        {
                            foreach (BaseWaterTransition baseWaterTransition in gameObject.GetComponentsInChildren<BaseWaterTransition>())
                            {
                                baseWaterTransition.face.cell = cell;
                                baseWaterTransition.face.direction = faceDirection.Value;
                            }
                        }
                        gameObject.SetActive(true);
                        gameObject.BroadcastMessage("OnAddedToBase", this, SendMessageOptions.DontRequireReceiver);
                        if (sourceBaseDeconstructable != null)
                        {
                            foreach (ConstructableBounds constructableBounds in gameObject.transform.GetComponentsInChildren<ConstructableBounds>())
                            {
                                sourceBaseDeconstructable.basePiecesBounds.Add(constructableBounds.bounds);
                            }
                        }
                        return gameObject.transform*/
                    }
                }
                    messages.Clear();
                }
            }
        }
    }

