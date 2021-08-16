using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static readonly object _lock = new object();
    static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
    static string ipAdress= "192.168.0.83";
    static void Main(string[] args)
    {
        int count = 1;
        IPAddress host = IPAddress.Parse(ipAdress);
        TcpListener ServerSocket = new TcpListener(host, 5000);
        ServerSocket.Start();
        Console.WriteLine("Listening on 192.168.0.83:5000");

        while (true)
        {
            TcpClient client = ServerSocket.AcceptTcpClient();
            lock (_lock) list_clients.Add(count, client);
            Console.WriteLine("Someone connected, id: "+count);

            Thread receiveThread = new Thread(handle_clients);
            receiveThread.Start(count);    
            count++;
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

            int byte_count = stream.Read(buffer, 0, buffer.Length);

            //Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, byte_count));

            if (byte_count == 0)
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
            
            string[] data2 = data.Split(new string[] { "WORLDPOSITION" }, StringSplitOptions.None);
            foreach (var item in data2){
                if (item.Length > 1){
                    broadcast(id + "WORLDPOSITION" + item, id);
                    Console.WriteLine(id + "WORLDPOSITION" + item);
                }
                
            }
            
            
            //Console.WriteLine(data+" from id "+id);
        }

        lock (_lock) list_clients.Remove(id);
        Console.WriteLine("Someone deconnected, id: "+id);
        client.Client.Shutdown(SocketShutdown.Both);
        client.Close();
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
}