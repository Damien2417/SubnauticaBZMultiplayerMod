using ClientSubnautica.MultiplayerManager.ReceiveData;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ServerSubnautica
{
    internal class HandleClient
    {
        // ID of the users (provided by them at their first request)
        string id;
        TcpClient client;
        NetworkStream stream;
        ClientMethod clientAction = new ClientMethod();
        public HandleClient(string id)
        {
            this.id = id;
            lock (Server._lock) this.client = Server.list_clients[id];
            this.stream = this.client.GetStream();
            initialize();          
        } 
        public void initialize()
        {
            int bufferSize = 1024;

            byte[] dataLength = BitConverter.GetBytes(Server.mapBytes.Length);

            stream.Write(dataLength, 0, 4);

            int bytesSent = 0;
            int bytesLeft = Server.mapBytes.Length;

            while (bytesLeft > 0)
            {
                int curDataSize = Math.Min(bufferSize, bytesLeft);

                stream.Write(Server.mapBytes, bytesSent, curDataSize);

                bytesSent += curDataSize;
                bytesLeft -= curDataSize;
            }

            string session = Server.gameInfo["session"].ToString();
            string changeSet = Server.gameInfo["changeSet"].ToString();
            string gameMode = Server.gameInfo["gameMode"].ToString();
            string storyVersion = Server.gameInfo["storyVersion"].ToString();

            byte[] test2 = Encoding.ASCII.GetBytes(session + "$" + changeSet + "$" + gameMode + "$" + storyVersion);

            stream.Write(test2, 0, test2.Length);

            byte[] buffer2 = new byte[1024];
            stream.Read(buffer2, 0, buffer2.Length);
            clientAction.broadcast(NetworkCMD.getIdCMD("NewId") + $":{this.id}:{Server.list_nicknames[this.id]}/END/", this.id);
            Console.WriteLine($"{NetworkCMD.getIdCMD("NewId")}:{this.id}:{Server.list_nicknames[this.id]}/END/");
            string ids = "";
            lock (Server._lock)
            {
                foreach (var item in Server.list_clients)
                {
                    if (item.Key != this.id)
                    {
                        ids += $"{item.Key}&{Server.list_nicknames[item.Key]};";
                    }
                }
            }
            if (ids.Length > 1)
            {
                clientAction.specialBroadcast(NetworkCMD.getIdCMD("AllId") + $":{ids}/END/", this.id);
                lock (Server._lock)
                {
                    Server.list_clients.First().Value.GetStream().Write(Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("GetTimePassed") + "/END/"));
                }
            }
        }
        /// <summary>
        /// Start the player connection, globally.
        /// </summary>
        public void start()
        {
            loop();
            endConnection();
        }

        public void loop()
        {
            while (true)
            {
                // THIS IS THE PART WHER WE READ COMMANDS
                int cont = 1; // I don't understand the point of this but no problem
                byte[] buffer = new byte[1024];
                //Array.Clear(buffer, 0, buffer.Length);
                int byte_count;

                byte_count = this.stream.Read(buffer, 0, buffer.Length);

                string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                if (!data.Contains("/END/"))
                    continue;

                string[] commands = data.Split(new string[] { "/END/" }, StringSplitOptions.None);
                foreach (var command in commands)
                {
                    if (command.Length <= 1)
                        continue;
                    try
                    {
                        string idCMD = command.Split(':')[0];
                        if (idCMD == NetworkCMD.getIdCMD("Disconnected"))
                        {
                            cont = 0;
                            break;
                        }

                        var tempList = command.Substring(command.IndexOf(":") + 1).Split(';').ToList();
                        if (idCMD != NetworkCMD.getIdCMD("Disconnected"))
                            tempList.Insert(0, id.ToString());
                        string[] param = tempList.ToArray();

                        //Redirecting data received to right method
                        clientAction.redirectCall(param, idCMD);
                    }
                    catch (Exception) { }
                }
                if (cont == 0)
                    break;
            }
        }

        /// <summary>
        /// Well... It looks obvious... It disconnect the player.
        /// </summary>
        public void endConnection()
        {
            lock (Server._lock) Server.list_clients.Remove(id);
            Console.WriteLine("Someone disconnected, id: " + id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
            clientAction.redirectCall(new string[] { id.ToString() }, NetworkCMD.getIdCMD("Disconnected"));
        } 
    }
}
