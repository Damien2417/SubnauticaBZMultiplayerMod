using ClientSubnautica.MultiplayerManager;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

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
            // Loading the user configs.
            configFile = loadParam(Path.Combine(location, "config.json"));
            string playerID = configFile["playerID"].ToString();
            id = configFile["playerID"].ToString();
            string username = configFile["nickname"].ToString();

            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string text = "dam_" + executingAssembly.GetName().Name;
            new Harmony(text).PatchAll(executingAssembly);

            Logger.Log(Logger.Level.Info, playerID + " - Username: " + username);
        }
        /// <summary>
        /// Loads a JSON file and parse it, if none is found, one is created. NOT UNIVERSAL.
        /// </summary>
        /// <param name="path">Path to the file (including the file)</param>
        /// <returns>A parsed JSON object.</returns>
        public static JObject loadParam(string path)
        {
            if (File.Exists(path))
            {
                return JObject.Parse(File.ReadAllText(path));
            }
            else
            {
                var id = GenerateID();
                File.WriteAllText(path,
@"{
    ""WARNING"": ""DO NOT CHANGE OR DELETE THE ID OR YOU WILL LOSE ALL YOUR PROGRESSIONS ON EVERY GAMES"",
    ""playerID"": """ + id + @""",
    ""nickname"": ""Player" + id + @"""
}");
                return JObject.Parse(File.ReadAllText(path));
            }
        }
        public static string GenerateID()
        {
            var tid = Process.GetCurrentProcess().Id.ToString() + ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            return tid;
        }
    }
}