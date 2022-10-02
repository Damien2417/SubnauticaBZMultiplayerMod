using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ServerSubnautica
{
    internal class ClientMethod
    {
        /// <summary>
        /// Sends a signal to user
        /// </summary>
        /// <param name="data">Data to sned to user</param>
        /// <param name="id">ID of the user who will receive</param>
        public void broadcast(string data, string id)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //Console.WriteLine("Sending data :"+data);
            lock (Server._lock)
            {
                foreach (var c in Server.list_clients)
                {
                    if (c.Key != id)
                    {
                        //Console.WriteLine("Sending position to id "+id);
                        NetworkStream stream = c.Value.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        public void specialBroadcast(string data, string id)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            lock (Server._lock)
            {
                foreach (var c in Server.list_clients)
                {
                    if (c.Key == id)
                    {
                        //Console.WriteLine("Sending position to id "+id);
                        NetworkStream stream = c.Value.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        public void redirectCall(string[] param, string id)
        {
            try
            {
                Type type = typeof(FunctionManager);
                MethodInfo method = type.GetMethod(NetworkCMD.Translate(id));
                FunctionManager c = new FunctionManager();
                method.Invoke(c, new System.Object[] { param });
            }
            catch (Exception) { }
        }
    }
}
