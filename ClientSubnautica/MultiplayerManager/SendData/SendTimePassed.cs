using ClientSubnautica.StartMod;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    class SendTimePassed
    {
        public static void send()
        {
            NetworkStream ns = MainMenuBegin.client.GetStream();
            byte[] msgresponse;
            msgresponse = Encoding.ASCII.GetBytes("timePassed:"+DayNightCycle.main.timePassedAsFloat.ToString());
            ns.Write(msgresponse, 0, msgresponse.Length);
            //ns.Close();
        }
    }
}
