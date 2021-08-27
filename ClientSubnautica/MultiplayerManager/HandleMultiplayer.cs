using System.Collections.Concurrent;
using UnityEngine;

namespace ClientSubnautica.MultiplayerManager
{
    internal class HandleMultiplayer
    {
        public static ConcurrentDictionary<int, GameObject> players { get; set; }
        public static ConcurrentDictionary<int, string> lastPos { get; set; }
        public static ConcurrentDictionary<int, string> posLastLoop { get; set; }
        public static GameObject[] playerBodies { get; set; }
    }
}