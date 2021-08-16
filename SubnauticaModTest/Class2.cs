using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UWE;

namespace SubnauticaModTest
{

	// patches

	[HarmonyPatch(typeof(MainGameController), "StartGame")]
	public class Patches
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

		static void Postfix(ref IEnumerator __result)
		{
			Action prefixAction = () => { };
            Action postfixAction = () =>
            {


                //Thread sender
                Thread go = new Thread(main);
                go.Start();

                void main()
                {

                    IPAddress ip = IPAddress.Parse("192.168.0.83");
                    int port = 5000;
                    TcpClient client = new TcpClient();
                    while (true)
                    {
                        try
                        {
                            UnityEngine.Debug.Log("Searching server...");
                            client.Connect(ip, port);
                            break;
                        }
                        catch { }
                    }

                    UnityEngine.Debug.Log("Server found !");
                    NetworkStream ns = client.GetStream();

                    ConcurrentDictionary<int, GameObject> players = new ConcurrentDictionary<int, GameObject>();
                    ConcurrentDictionary<int, string> lastPos = new ConcurrentDictionary<int, string>();
                    ConcurrentDictionary<int, string> posLastLoop = new ConcurrentDictionary<int, string>();

                    //Thread receiver
                    Thread threadReceiver = new Thread(o => ReceiveData((TcpClient)o));
                    threadReceiver.Start(client);

                    //Thread sender
                    Thread threadSender = new Thread(o => SendData((TcpClient)o));
                    threadSender.Start(client);

                    //Thread position
                    Thread threadPosition = new Thread(o => setPosPlayer((TcpClient)o));
                    threadPosition.Start(client);

                    string s;
                    while (true)
                    {
                        //byte[] buffer = Encoding.ASCII.GetBytes(s);
                        //ns.Write(buffer, 0, buffer.Length);
                    }

                    client.Client.Shutdown(SocketShutdown.Send);
                    threadReceiver.Join();
                    threadSender.Join();
                    threadPosition.Join();
                    client.Close();

                    void ReceiveData(TcpClient client2)
                    {
                        NetworkStream ns2 = client2.GetStream();
                        byte[] receivedBytes = new byte[1024];
                        int byte_count;

                        //UnityEngine.Debug.Log("Attente de data du serveur");
                        while ((byte_count = ns2.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                        {
                            //UnityEngine.Debug.Log("Data recu");
                            //SET POS PLAYERS
                            string message = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);

                            if (message.Contains("NEWID:"))
                            {
                                //UnityEngine.Debug.Log("Ajout joueur, id: " + int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                                addPlayer(int.Parse(message.Split(new string[] { "NEWID:" }, StringSplitOptions.None)[1]));
                                //UnityEngine.Debug.Log("Joueur ajouté!");
                            }
                            else if (message.Contains("WORLDPOSITION"))
                            {
                                int id = int.Parse(message.Split(new string[] { "WORLDPOSITION" }, StringSplitOptions.None)[0]);
                                string pos = message.Split(new string[] { "WORLDPOSITION" }, StringSplitOptions.None)[1];

                                lastPos[id] = pos;
                                //UnityEngine.Debug.Log("var maj!" + lastPos[id]);

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
                                        {
                                            addPlayer(int.Parse(id));
                                        }
                                    }
                                    //UnityEngine.Debug.Log("Liste ajouté");
                                }
                            }
                        }
                        ns2.Close();
                    }

                    void SendData(TcpClient client2)
                    {
                        NetworkStream ns2 = client2.GetStream();
                        
                        string data = null;

                        string pos;
                        string x = "";
                        string y = "";
                        string z = "";

                        while (true)
                        {
                            if (Player.main.transform.position.x.ToString() != x | Player.main.transform.position.y.ToString() != y | Player.main.transform.position.z.ToString() != z)
                            {
                                byte[] msgresponse = Encoding.ASCII.GetBytes("");
                                Array.Clear(msgresponse, 0, msgresponse.Length);

                                msgresponse = Encoding.ASCII.GetBytes("WORLDPOSITION" + "(" + Player.main.transform.position.x + ";" + Player.main.transform.position.y + ";" + Player.main.transform.position.z + ")");

                                // Position envoyé !
                                ns2.Write(msgresponse, 0, msgresponse.Length);

                                data = Encoding.ASCII.GetString(msgresponse, 0, msgresponse.Length);
                                pos = data.Split('(')[1];
                                x = pos.Split(';')[0];
                                y = pos.Split(';')[1];
                                z = pos.Split(';')[2];
                                z = z.Remove(z.Length - 1);
                            }
                        }
                    }

                    void addPlayer(int id)
                    {
                        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        var pos = new Vector3((float)-294.3636, (float)17.02644, (float)252.9224);

                        //CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject(TechType.DiveSuit));
                        GameObject test = new GameObject();
                        CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject(TechType.Workbench, returnValue =>
                        {
                            test = returnValue;
                            test.transform.position = pos;
                            players.TryAdd(id, test);
                            lastPos.TryAdd(id, "0");
                            posLastLoop.TryAdd(id, "0");
                        }));
                       

                                             

                        
                    }

                    void setPosPlayer(TcpClient client2)
                    {
                        string pos;
                        string x = "";
                        string y = "";
                        string z = "";
                        while (true)
                        {
                            foreach (var item in lastPos)
                            {
                                try
                                {

                                    //UnityEngine.Debug.Log("iciii "+item.Key+" "+item.Value);
                                    //UnityEngine.Debug.Log("comparé au num " + item.Key + " valeur " + localPosLastLoop[item.Key]);

                                    //UnityEngine.Debug.Log("item val:"+ item.Value+" localpos:"+ posLastLoop[item.Key]);
                                    if (item.Value != posLastLoop[item.Key])
                                    {
                                        //UnityEngine.Debug.Log("maj position du joueur");
                                        pos = item.Value.Split('(')[1];
                                        x = pos.Split(';')[0];
                                        y = pos.Split(';')[1];
                                        z = pos.Split(';')[2];
                                        z = z.Remove(z.Length - 1);
                                        players[item.Key].transform.position = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
                                        posLastLoop[item.Key] = lastPos[item.Key];
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            };
			Action<object> preItemAction = (item) => {  };
			Action<object> postItemAction = (item) => {};
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

