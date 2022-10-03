using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    class SendMyID
    {
        public static void start(TcpClient client2)
        {
            // Get the datastream of the player
            NetworkStream ns2 = client2.GetStream();
            var msgresponse = Encoding.ASCII.GetBytes($"{NetworkCMD.getIdCMD("SendingID")}:{MainPatcher.id}:{MainPatcher.username}/END/"); // Create a message containing player ID
            ns2.Write(msgresponse, 0, msgresponse.Length); // Sends the message.
        }
    }
}
