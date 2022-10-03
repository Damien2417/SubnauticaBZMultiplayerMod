using ClientSubnautica.ClientManager;
using HarmonyLib;
using System;
using System.Globalization;
using UnityEngine;
using UWE;

namespace ClientSubnautica.MultiplayerManager
{
    class FunctionManager
    {
        public void WorldPosition(string[] param)
        {
            FunctionToClient.setPosPlayer(param);
        }

        public void NewId(string[] param)
        {
            MainPatcher.player_list.Add(param[0], param[1]);
            FunctionToClient.addPlayer(param[0], param[1]);
            ErrorMessage.AddMessage("Player " + param[0] + " joined !");
        }
        public void AllId(string[] param)
        {
            foreach (string playerData in param)
            {
                if (playerData.Length > 0)
                {
                    var entries = playerData.Split('&');
                    FunctionToClient.addPlayer(entries[0], entries[1]);
                }
            } 
        }
        public void Disconnected(string[] param)
        {
            GameObject val;
            //string val2;
            //string val3;
            lock (RedirectData.m_lockPlayers)
            {
                GameObject.Destroy(RedirectData.players[param[0]]);

                RedirectData.players.TryRemove(param[0], out val);
                MainPatcher.player_list.Remove(param[0]);
            }
            //ApplyPatches.posLastLoop.TryRemove(int.Parse(id), out val2);
            //ApplyPatches.lastPos.TryRemove(int.Parse(id), out val3);
            ErrorMessage.AddMessage("Player " + param[0] + " disconnected.");
        }

        public void SpawnItem(string[] param)
        {
            CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType), param[1]), RedirectData.players[param[0]].transform.position, param[2], returnValue =>
            {
            }));

        }

        public void PickupItem(string[] param)
        {
            GameObject[] firstList = GameObject.FindObjectsOfType<GameObject>();
            foreach (var item in firstList)
            {
                if(item.GetComponent<UniqueGuid>() != null)
                {
                    if (item.GetComponent<UniqueGuid>().guid == param[1])
                    {
                        UnityEngine.Object.Destroy(item);
                        break;
                    }
                }
            }

        }

        public void SpawnBasePiece(string[] param)
        {
            float x = float.Parse(param[2].Replace(",", "."), CultureInfo.InvariantCulture);
            float y = float.Parse(param[3].Replace(",", "."), CultureInfo.InvariantCulture);
            float z = float.Parse(param[4].Replace(",", "."), CultureInfo.InvariantCulture);

            CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType), param[1]), new Vector3(x,y,z), "randomguid", returnValue =>
            {
            }));

        }

        public void askTimePassed(string[] param)
        {
            SendData.SendTimePassed.send();
        }

        public void timePassed(string[] param)
        {
            bool flag = DayNightCycle.main.IsDay();
            DayNightCycle.main.SetTimePassed(float.Parse(param[1]));
            /*float lightScalar = DayNightCycle.main.GetLightScalar();
            float value = Mathf.GammaToLinearSpace(DayNightCycle.main.GetLocalLightScalar());
            Shader.SetGlobalFloat(ShaderPropertyID._UweLightScalar, lightScalar);
            Shader.SetGlobalColor(ShaderPropertyID._AtmoColor, DayNightCycle.main.atmosphereColor.Evaluate(lightScalar));
            //Shader.SetGlobalFloat(ShaderPropertyID._UweAtmoLightFade, DayNightCycle.main.sunlight.fade);
            Shader.SetGlobalFloat(ShaderPropertyID._UweLocalLightScalar, value);
            DayNightCycle.main.StopSkipTimeMode();*/

            AccessTools.Method(typeof(DayNightCycle), "UpdateAtmosphere").Invoke(DayNightCycle.main, new object[] { });

            if (!flag)
            {
                DayNightCycle.main.dayNightCycleChangedEvent.Trigger(true);
            }
        }

        public void weather(string[] param)
        {
        }
    }
}
