using ClientSubnautica.MultiplayerManager;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Reflection;

namespace ClientSubnautica
{
    [QModCore]
    public static class MainPatcher
    {
        public static string location;
        [QModPatch]
        public static void Patch()
        {
            location = AppDomain.CurrentDomain.BaseDirectory;
            var playerID = CreatePlayerID.GenerateID();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = "dam_" + executingAssembly.GetName().Name;
            new Harmony(text).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, playerID);
        }
    }
}