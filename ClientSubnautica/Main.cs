using ClientSubnautica.MultiplayerManager;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ClientSubnautica
{
    [QModCore]
    public static class MainPatcher
    {
        public static string location;
        public static string id;
        public static JObject configFile;

        [QModPatch]
        public static void Patch()
        {
            location = AppDomain.CurrentDomain.BaseDirectory;
            configFile = loadParam(Path.Combine(location, "config.json"));
            string playerID = configFile["playerID"].ToString();
            id = configFile["playerID"].ToString();
            string username = configFile["nickname"].ToString();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = "dam_" + executingAssembly.GetName().Name;
            new Harmony(text).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, playerID + " - Username: " + username);
        }
        public static JObject loadParam(string path)
        {
            if (File.Exists(path))
            {
                return JObject.Parse(File.ReadAllText(path));
            }
            else
            {
                var id = CreatePlayerID.GenerateID();
                File.WriteAllText(path,
@"{
    ""WARNING"": ""DO NOT CHANGE OR DELETE THE ID OR YOU WILL LOSE ALL YOUR PROGRESSIONS ON EVERY GAMES"",
    ""playerID"": """ + id + @""",
    ""nickname"": ""Player" + id + @"""
}");
                return JObject.Parse(File.ReadAllText(path));
            }
        }
    }
}