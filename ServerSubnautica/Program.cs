using Newtonsoft.Json.Linq;
using ServerSubnautica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

class Program
{
    static readonly object _lock = new object();
    static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();



    static void Main(string[] args)
    {
        string[] paths = { AppDomain.CurrentDomain.BaseDirectory, "config.json"};
        string fullPath = Path.Combine(paths);

        
        string ipAddress = JObject.Parse(File.ReadAllText(fullPath))["ipAddress"].ToString();
        int port = int.Parse(JObject.Parse(File.ReadAllText(fullPath))["port"].ToString());
        int count = 1;
        IPAddress host = IPAddress.Parse(ipAddress);
        TcpListener ServerSocket = new TcpListener(host, port);
        ServerSocket.Start();
        Console.WriteLine("Listening on "+ ipAddress + ":"+port);

        while (true)
        {
            TcpClient client = ServerSocket.AcceptTcpClient();
            lock (_lock) list_clients.Add(count, client);
            Console.WriteLine("Someone connected, id: "+count);

            Thread receiveThread = new Thread(handle_clients);
            receiveThread.Start(count);    
            count++;
            Thread.Sleep(16);
        }
    }

    public static void handle_clients(object o)
    {
        int id = (int)o;
        bool test = true;
        TcpClient client;

        lock (_lock) client = list_clients[id];
        NetworkStream stream = client.GetStream();
        while (true)
        {            
            byte[] buffer = new byte[1024];
            //Array.Clear(buffer, 0, buffer.Length);
            int byte_count=0;
            try
            {
                byte_count = stream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                break;
            }

            if (byte_count < 1)
            {
                break;
            }
            if (test)
            {
                broadcast("NEWID:" + id,id);
                string ids = "";
                lock (_lock) {
                   
                    foreach (var item in list_clients)
                    {
                        if (item.Key != id)
                        {
                            ids += item.Key + "$";
                        }
                        
                    }
                }
                if (ids.Length>1)
                {
                    specialBroadcast("ALLID:" +ids,id);
                }
                test = false;
            }

            string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
            
            if(data.Contains("DISCONNECTED"))
            {
                break;
            }

            //Redirecting data received to right method
            redirectCall(data,id);

            Thread.Sleep(8);
        }

        lock (_lock) list_clients.Remove(id);
        Console.WriteLine("Someone deconnected, id: "+id);
        client.Client.Shutdown(SocketShutdown.Both);
        client.Close();
        broadcast(id+"DISCONNECTED", id);
    }

    public static void broadcast(string data, int id)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(data); 
        //Console.WriteLine("Sending data :"+data);
        lock (_lock)
        {
            foreach (var c in list_clients)
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

    public static void specialBroadcast(string data, int id)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        //Console.WriteLine("Sending data :" + data);
        lock (_lock)
        {
            foreach (var c in list_clients)
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
    public static void redirectCall(string data, int id)
    {
        string[] data2 = data.Split(new string[] { "/END/" }, StringSplitOptions.None);
        foreach (var item in data2)
        {
            if (item.Contains(':'))
            {
                string[] param = item.Split(':');
                Type type = typeof(MethodResponse);
                MethodInfo method = type.GetMethod(param[0]);
                MethodResponse c = new MethodResponse();
                method.Invoke(c, new Object[] { id.ToString(), param[1] });
            }

        }
    }
}