using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    class SendTimePassed
    {
        public static void send()
        {
            NetworkStream ns = StartMultiplayer.client.GetStream();
            byte[] msgresponse;
            msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("timePassed") + ":" + DayNightCycle.main.timePassedAsFloat.ToString()+"/END/");
            ns.Write(msgresponse, 0, msgresponse.Length);
            //ns.Close();
        }
    }
}
