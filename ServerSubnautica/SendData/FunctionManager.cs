using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;

namespace ServerSubnautica
{
    class FunctionManager
    {
        ClientMethod client = new ClientMethod();
        public void WorldPosition(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("WorldPosition") + ":" + param[0] + ";" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + ";" + param[5] + ";" + param[6] +";" + param[7] + "/END/", int.Parse(param[0]));
            Console.WriteLine("id: "+ param[0] + " at position: " + param[1]+";"+param[2]+";"+param[3] + " rotation: "+ param[4] + ";" + param[5] + ";" + param[6] + ";" + param[7]);
        }

        public void SpawnItem(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("SpawnItem") + ":" + param[0] + ";" + param[1] + ";" + param[2] + "/END/", int.Parse(param[0]));
            Console.WriteLine("spawnItem:"+ param[1]);
        }
        
        public void PickupItem(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("PickupItem") + ":" + param[0] + ";" + param[1]+"/END/", int.Parse(param[0]));
            Console.WriteLine("pickupItem:" + param[1]);
        }

        public void SpawnBasePiece(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("SpawnBasePiece") + ":" + param[0] + ";" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + "/END/", int.Parse(param[0]));
            Console.WriteLine("spawnBasePiece:" + param[1]);
        }

        public void timePassed(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("timePassed") + ":" + param[1]+"/END/", int.Parse(param[0]));
        }
        
        public void Disconnected(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("Disconnected") + ":" + param[0]+"/END/", int.Parse(param[0]));
        }

        public void SaveGameRequest(string[] param)
        {
            //client.broadcast(NetworkCMD.getIdCMD("SaveGameRequest") + ":" + param[0] + ":" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + ";" + param[5] + ";" + param[6]+"/END/", int.Parse(param[0]));
            Console.WriteLine("SavingGame, player position: " + ":" + param[0] + ":" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + ";" + param[5] + ";" + param[6]);
        }

    }
}
