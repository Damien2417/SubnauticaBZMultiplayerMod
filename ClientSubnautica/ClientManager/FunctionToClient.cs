using ClientSubnautica.MultiplayerManager;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering;

namespace ClientSubnautica.ClientManager
{
    class FunctionToClient
    {
        public static void addPlayer(int id)
        {
            var pos = new Vector3((float)-294.3636, (float)17.02644, (float)252.9224);
            GameObject body = GameObject.Find("player_view_female");

            body.GetComponentInParent<Player>().staticHead.shadowCastingMode = ShadowCastingMode.On;
            lock (RedirectData.m_lockPlayers)
            {
                RedirectData.players.TryAdd(id, UnityEngine.Object.Instantiate<GameObject>(body, pos, Quaternion.identity));
            }
            body.GetComponentInParent<Player>().staticHead.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

            //GameObject.Destroy(players[id].GetComponent<Animator>());

            //posLastLoop.TryAdd(id, "0");
            //lastPos.TryAdd(id, "0");
        }

        public static void setPosPlayer(int id, string data)
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
                lock (RedirectData.m_lockPlayers)
                {
                    RedirectData.players[id].transform.position = new Vector3(x2, y2, z2);
                }
                //posLastLoop[id] = lastPos[id];

                //}                   
            }
            catch (Exception e)
            {

                UnityEngine.Debug.Log("It seem that you can't set the position of other players.\n" + e);
            }
        }
    }
}
