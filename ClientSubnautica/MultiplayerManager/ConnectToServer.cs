using ClientSubnautica.MultiplayerManager.SendData;
using System.Net;
using System.Net.Sockets;

namespace ClientSubnautica.MultiplayerManager
{
    class ConnectToServer
    {
        //Connect to server
        public static TcpClient start(string ip)
        {
            string[] ipArray = ip.Split(':');

            IPAddress ipDest = IPAddress.Parse(ipArray[0]);
            int port = int.Parse(ipArray[1]);
            TcpClient client = new TcpClient();
            SendMyID.start(client);

            client.Connect(ipDest, port);
            return client;
        }
    }
}
