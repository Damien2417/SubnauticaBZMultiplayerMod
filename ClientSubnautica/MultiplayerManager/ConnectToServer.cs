using System.Net;
using System.Net.Sockets;

namespace ClientSubnautica.MultiplayerManager
{
    class ConnectToServer
    {
        //Connect to server
        public static TcpClient start()
        {
            ErrorMessage.AddMessage("Searching server...");
            IPAddress ip = IPAddress.Parse(MainPatcher.Config.ipAddress);
            int port = int.Parse(MainPatcher.Config.port);
            TcpClient client = new TcpClient();

            client.Connect(ip, port);
            ErrorMessage.AddMessage("Connected on " + ip + ":" + port + " !");
            return client;
        }
    }
}
