using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubnautica.MultiplayerManager.ReceiveData
{
    class NetworkCMD
    {
        public static Dictionary<string, string> types = new Dictionary<string, string>()
        {
            {"1", "WorldPosition"},
            {"2", "Disconnected"},
            {"3", "SpawnPiece"},
            {"4", "NewId"},
            {"5", "AllId"},
            {"6", "GetTimePassed"}
        };

        public static string Translate(string idCMD)
        {
            return NetworkCMD.types[idCMD];
        }
        public static string getIdCMD(string value)
        {
            return NetworkCMD.types.FirstOrDefault(t => t.Value == value).Key;
        }

    }
}
