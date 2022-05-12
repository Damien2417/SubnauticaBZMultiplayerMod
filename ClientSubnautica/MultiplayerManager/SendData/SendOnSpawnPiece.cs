using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica
{
    public class SendOnSpawnPiece
    {      
        public static void send(string techtype, string x, string y, string z)
        {
            NetworkStream ns = MainMenuBegin.client.GetStream();

            byte[] msgresponse;
            
            msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SpawnBasePiece") + ":" + techtype + ";" + x + ";" + y + ";" + z +"/END/");

            // Position envoyé !
            ns.Write(msgresponse, 0, msgresponse.Length);
            //ns.Close();
            
        }
    }
}
