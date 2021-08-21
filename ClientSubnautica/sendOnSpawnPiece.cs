using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SubnauticaMod
{
    public class sendOnSpawnPiece
    {      
        public static void send(string techtype)
        {
            if (true)
            {
                NetworkStream ns = SubnauticaMod.ApplyPatches.client.GetStream();

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
