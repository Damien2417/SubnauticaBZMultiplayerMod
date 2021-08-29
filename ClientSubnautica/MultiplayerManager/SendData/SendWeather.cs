using ClientSubnautica.StartMod;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    class SendWeather
    {
        public static void send(string id)
        {
            NetworkStream ns = MainMenuBegin.client.GetStream();
            byte[] msgresponse;
            msgresponse = Encoding.ASCII.GetBytes("setWeather:" + id+"/END/");
            ns.Write(msgresponse, 0, msgresponse.Length);
            //ns.Close();
        }
    }
}
