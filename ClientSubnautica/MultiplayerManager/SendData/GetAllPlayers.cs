using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    internal class GetAllPlayers
    {
        public static void start(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            var msgResponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("AllId") + $":{MainPatcher.id}/END/");
            stream.Write(msgResponse, 0, msgResponse.Length);
        }
    }
}
