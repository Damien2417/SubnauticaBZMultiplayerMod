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
        public static string modFolder;
        public static string id;
        public static JObject configFile;

        [QModPatch]
        public static void Patch()
        {
            location = AppDomain.CurrentDomain.BaseDirectory;
            modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Loading the user configs.
            configFile = LoadParam(Path.Combine(modFolder, "player.json"));
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
        public static JObject LoadParam(string path)
        {
            if (File.Exists(path))
            {
                return JObject.Parse(File.ReadAllText(path));
            }
            else if (path.EndsWith("player.json"))
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
            else throw new Exception("The file you're trying to access does not exist, and has no default value.");
        }
        public static string GenerateID()
        {
            var tid = Process.GetCurrentProcess().Id.ToString() + ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            return tid;
        }
    }
}