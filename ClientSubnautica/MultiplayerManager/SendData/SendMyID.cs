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
            NetworkStream ns2 = client2.GetStream();
            var msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SendingID") + ":" + MainPatcher.id + ":/END/");
            ns2.Write(msgresponse, 0, msgresponse.Length);
        }
    }
}
