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
            {"3", "SpawnItem"},
            {"4", "NewId"},
            {"5", "AllId"},
            {"6", "GetTimePassed"},
            {"7", "PickupItem"},
            {"8", "SpawnBasePiece"},
            {"9", "RecievingID"}
        };

        public static string Translate(string idCMD)
        {
            try
            {
                return NetworkCMD.types[idCMD];
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string getIdCMD(string value)
        {
            return NetworkCMD.types.FirstOrDefault(t => t.Value == value).Key;
        }

    }
}
