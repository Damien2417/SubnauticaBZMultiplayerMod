using ClientSubnautica.StartMod;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica
{
    public class SendOnSpawnPiece
    {      
        public static void send(string techtype)
        {
            if (true)
            {
                NetworkStream ns = MainMenuBegin.client.GetStream();

                byte[] msgresponse;

                msgresponse = Encoding.ASCII.GetBytes("SpawnPiece:" + techtype+ "/END/");

                ErrorMessage.AddMessage("sending "+techtype);
                // Position envoyé !
                ns.Write(msgresponse, 0, msgresponse.Length);
                //ns.Close();
            }
        }
    }
}
