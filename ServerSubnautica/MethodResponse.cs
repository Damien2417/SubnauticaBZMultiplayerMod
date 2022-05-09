using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Linq;
using System.Text;

namespace ServerSubnautica
{
    class MethodResponse
    {
        public void WorldPosition(string[] param)
        {
            Program.broadcast(NetworkCMD.getIdCMD("WorldPosition") + ":" + param[0] + ";" + param[1] + ";" + param[2] + ";" + param[3] + "/END/", int.Parse(param[0]));
            Console.WriteLine("id: "+ param[0] + " at position: " + param[1]+";"+param[2]+";"+param[3]);
        }

        public void SpawnPiece(string[] param)
        {
            Program.broadcast(NetworkCMD.getIdCMD("SpawnPiece") + ":" + param[0] + ";" + param[1] + ";" + param[2] + "/END/", int.Parse(param[0]));
            Console.WriteLine("spawnPiece:"+ param[1]);
        }
        
        public void PickupPiece(string[] param)
        {
            Program.broadcast(NetworkCMD.getIdCMD("PickupPiece") + ":" + param[0] + ";" + param[1]+"/END/", int.Parse(param[0]));
            Console.WriteLine("pickupPiece:"+ param[1]);
        }


        public void timePassed(string[] param)
        {
            Program.broadcast(NetworkCMD.getIdCMD("timePassed") + ":" + param[1]+"/END/", int.Parse(param[0]));
        }
        
        public void Disconnected(string[] param)
        {
            Program.broadcast(NetworkCMD.getIdCMD("Disconnected") + ":" + param[0]+"/END/", int.Parse(param[0]));
        }

    }
}
