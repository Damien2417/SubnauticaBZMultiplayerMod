using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica
{
    public class sendOnSpawnPiece
    {      
        public static void send(string techtype)
        {
            if (true)
            {
                NetworkStream ns = StartMultiplayer.client.GetStream();

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
