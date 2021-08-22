using System;

namespace ServerSubnautica
{
    class MethodResponse
    {
        public void WorldPosition(string id,string data)
        {
            Program.broadcast(id + "WorldPosition:" + data, int.Parse(id));
            Console.WriteLine(id + "WorldPosition:" + data);
        }

        public void SpawnPiece(string id, string data)
        {
            Program.broadcast(id + "SpawnPiece:" + data, int.Parse(id));
            Console.WriteLine(id + "SpawnPiece:" + data);
        }

    }
}
