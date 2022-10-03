using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Linq;
using System.Text;

namespace ServerSubnautica
{
    class FunctionManager
    {
        ClientMethod client = new ClientMethod();
        public void WorldPosition(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("WorldPosition") + ":" + param[0] + ";" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + ";" + param[5] + ";" + param[6] +";" + param[7] + "/END/", param[0]);
            //Console.WriteLine("id: "+ param[0] + " at position: " + param[1]+";"+param[2]+";"+param[3] + " rotation: "+ param[4] + ";" + param[5] + ";" + param[6] + ";" + param[7]);
        }

        public void SpawnItem(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("SpawnItem") + ":" + param[0] + ";" + param[1] + ";" + param[2] + "/END/", param[0]);
            Console.WriteLine("spawnItem:"+ param[1]);
        }
        
        public void PickupItem(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("PickupItem") + ":" + param[0] + ";" + param[1]+"/END/", param[0]);
            Console.WriteLine("pickupItem:" + param[1]);
        }

        public void SpawnBasePiece(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("SpawnBasePiece") + ":" + param[0] + ";" + param[1] + ";" + param[2] + ";" + param[3] + ";" + param[4] + "/END/", param[0]);
            Console.WriteLine("spawnBasePiece:" + param[1]);
        }

        public void timePassed(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("timePassed") + ":" + param[1]+"/END/", param[0]);
        }
        
        public void Disconnected(string[] param)
        {
            Server.list_clients.Remove(param[0]);
            Server.list_nicknames.Remove(param[0]);
            client.broadcast(NetworkCMD.getIdCMD("Disconnected") + ":" + param[0] + "/END/", param[0]);
        }

        /// <summary>
        /// This is unused, it is just here to ornate, and if one day we need it, well here it is.
        /// </summary>
        /// <param name="param">Command contents.</param>
        public void ReceivingID(string[] param)
        {
            client.broadcast(NetworkCMD.getIdCMD("ReceivingID") + ":" + param[0] + "/END/", param[0]);
            Console.WriteLine("ID Received:" + param[0]);
        }

        public void AllId(string[] param)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string player in Server.list_nicknames.Keys)
            {
                if (player != param[0])
                    continue;
                sb.Append($":{player}&{Server.list_nicknames[player]}");
            }
            client.broadcast(NetworkCMD.getIdCMD("AllId") + $"{sb}/END/", param[0]);
            Console.WriteLine($"{param[0]} requested every IDs of the game. (returned '{sb}')");
        }

    }
}
