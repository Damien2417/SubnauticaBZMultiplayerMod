using ClientSubnautica.ClientManager;
using ClientSubnautica.MultiplayerManager.ReceiveData;
using ClientSubnautica.StartMod;
using System;
using System.Net.Sockets;
using System.Text;

namespace ClientSubnautica
{
    public class SendOnDrop
    {      
        public static void send(Pickupable pickupable)
        {
            NetworkStream ns2 = MainMenuBegin.client.GetStream();

            byte[] msgresponse = Encoding.ASCII.GetBytes("");
            Array.Clear(msgresponse, 0, msgresponse.Length);
            msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SpawnItem") + ":" + pickupable.GetTechName() + ";" + pickupable.gameObject.GetComponent<UniqueGuid>().guid + "/END/");
            // Position envoyé !
            ns2.Write(msgresponse, 0, msgresponse.Length);
        }
    }
}
