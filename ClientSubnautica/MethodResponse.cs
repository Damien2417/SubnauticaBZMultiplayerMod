using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientSubnautica
{
    class MethodResponse
    {
        public void WorldPosition(string id, string data)
        {
            ErrorMessage.AddMessage(data);
            ApplyPatches.lastPos[int.Parse(id)] = data;
            ApplyPatches.setPosPlayer(int.Parse(id), data);
        }

        public void NewId(string id, string data)
        {
            ApplyPatches.addPlayer(int.Parse(data));
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
                        ApplyPatches.addPlayer(int.Parse(id2));
                }
            }
        }
        public void Disconnected(string id, string data)
        {
            GameObject val;
            string val2;
            string val3;
            GameObject.Destroy(ApplyPatches.players[int.Parse(id)]);

            ApplyPatches.players.TryRemove(int.Parse(id), out val);
            ApplyPatches.posLastLoop.TryRemove(int.Parse(id), out val2);
            ApplyPatches.lastPos.TryRemove(int.Parse(id), out val3);
            ErrorMessage.AddMessage("Player " + id + " disconnected.");
        }
    }
}
