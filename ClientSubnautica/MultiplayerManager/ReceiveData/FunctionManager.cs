using ClientSubnautica.ClientManager;
using System;
using UnityEngine;
using UWE;

namespace ClientSubnautica.MultiplayerManager
{
    class FunctionManager
    {
        public void WorldPosition(string id, string data)
        {
            //ApplyPatches.lastPos[int.Parse(id)] = data;
            FunctionToClient.setPosPlayer(int.Parse(id), data);
        }

        public void NewId(string id, string data)
        {
            UnityEngine.Debug.Log(data);
            FunctionToClient.addPlayer(int.Parse(data));
            ErrorMessage.AddMessage("Player " + data + " joined !");
        }
        public void AllId(string id, string data)
        {
            string[] idArray = data.Split('$');
            if (idArray.Length > 1)
            {
                foreach (var id2 in idArray)
                {
                    if (id2.Length > 0)
                        FunctionToClient.addPlayer(int.Parse(id2));
                }
            }
        }
        public void Disconnected(string id, string data)
        {
            GameObject val;
            //string val2;
            //string val3;
            lock (RedirectData.m_lockPlayers)
            {
                GameObject.Destroy(RedirectData.players[int.Parse(id)]);

                RedirectData.players.TryRemove(int.Parse(id), out val);
            }
            //ApplyPatches.posLastLoop.TryRemove(int.Parse(id), out val2);
            //ApplyPatches.lastPos.TryRemove(int.Parse(id), out val3);
            ErrorMessage.AddMessage("Player " + id + " disconnected.");
        }

        public void SpawnPiece(string id, string data)
        {
            GameObject test;
            CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType), data), returnValue =>
            {
                test = returnValue;
                test.transform.position = RedirectData.players[int.Parse(id)].transform.position;
            }));

        }
    }
}
