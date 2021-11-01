using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientSubnautica.MultiplayerManager.ReceiveData
{
    class ReceiveDataFromServer
    {
        //Receive data from server
        internal static void start(TcpClient client2)
        {
            NetworkStream ns2 = client2.GetStream();
            try
            {
                byte[] receivedBytes = new byte[1024];
                int byte_count;

                while ((byte_count = ns2.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                    lock (RedirectData.m_lockRequests)
                    {
                        RedirectData.receivedRequestsQueue.Add(message);
                    }
                }
                ns2.Close();
            }
            catch
            {
                client2.Client.Shutdown(SocketShutdown.Send);
                ns2.Close();
                client2.Close();
            }
        }
    }
}
