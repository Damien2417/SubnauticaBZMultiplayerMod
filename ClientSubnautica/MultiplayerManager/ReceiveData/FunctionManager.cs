using ClientSubnautica.ClientManager;
using HarmonyLib;
using System;
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
            FunctionToClient.addPlayer(int.Parse(param[0]));
            ErrorMessage.AddMessage("Player " + param[0] + " joined !");
        }
        public void AllId(string[] param)
        {
            foreach (var id in param)
            {
                if (id.Length > 0)
                {
                    FunctionToClient.addPlayer(int.Parse(id));
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
                GameObject.Destroy(RedirectData.players[int.Parse(param[0])]);

                RedirectData.players.TryRemove(int.Parse(param[0]), out val);
            }
            //ApplyPatches.posLastLoop.TryRemove(int.Parse(id), out val2);
            //ApplyPatches.lastPos.TryRemove(int.Parse(id), out val3);
            ErrorMessage.AddMessage("Player " + param[0] + " disconnected.");
        }

        public void SpawnPiece(string[] param)
        {
            GameObject test;
            CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType), param[1]), returnValue =>
            {
                test = returnValue;
                test.transform.position = RedirectData.players[int.Parse(param[0])].transform.position;
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
