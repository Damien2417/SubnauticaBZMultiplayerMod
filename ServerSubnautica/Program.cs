using Newtonsoft.Json.Linq;
using ServerSubnautica;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

class Program
{
    static readonly object _lock = new object();
    static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
    static byte[] mapBytes;
    static string mapName;

    static void Main(string[] args)
    {
        string[] paths = { AppDomain.CurrentDomain.BaseDirectory, "config.json"};
        string fullPath = Path.Combine(paths);
        mapName = JObject.Parse(File.ReadAllText(fullPath))["MapFolderName"].ToString();
        if (!zipFile(mapName))
        {
            Console.WriteLine("Can't compress world");
            Console.WriteLine("Press a key...");
            Console.ReadKey();
            System.Environment.Exit(1);
        }

        string[] outPath = { AppDomain.CurrentDomain.BaseDirectory, JObject.Parse(File.ReadAllText(fullPath))["MapFolderName"].ToString() + ".zip" };
        string outFullPath = Path.Combine(outPath);
        mapBytes = getFileBytes(outFullPath);

        File.Delete(outFullPath);

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

    public static void firstLoop(NetworkStream stream, int id) {
        int bufferSize = 1024;

        byte[] dataLength = BitConverter.GetBytes(mapBytes.Length);

        stream.Write(dataLength, 0, 4);

        int bytesSent = 0;
        int bytesLeft = mapBytes.Length;

        while (bytesLeft > 0)
        {
            int curDataSize = Math.Min(bufferSize, bytesLeft);

            stream.Write(mapBytes, bytesSent, curDataSize);

            bytesSent += curDataSize;
            bytesLeft -= curDataSize;
        }

        string[] paths = { AppDomain.CurrentDomain.BaseDirectory, mapName, "gameinfo.json" };
        string fullPath = Path.Combine(paths);
        string session = JObject.Parse(File.ReadAllText(fullPath))["session"].ToString();
        string changeSet = JObject.Parse(File.ReadAllText(fullPath))["changeSet"].ToString();
        string gameMode = JObject.Parse(File.ReadAllText(fullPath))["gameMode"].ToString();
        string storyVersion = JObject.Parse(File.ReadAllText(fullPath))["storyVersion"].ToString();

        byte[] test2 = Encoding.ASCII.GetBytes(session + "$" + changeSet + "$" + gameMode + "$" + storyVersion);

        stream.Write(test2, 0, test2.Length);

        byte[] buffer2 = new byte[1024];
        stream.Read(buffer2, 0, buffer2.Length);
        broadcast("NewId:" + id, id);
        string ids = "";
        lock (_lock)
        {

            foreach (var item in list_clients)
            {
                if (item.Key != id)
                {
                    ids += item.Key + "$";
                }

            }
        }
        if (ids.Length > 1)
        {
            specialBroadcast("AllId:" + ids, id);
        }
    }

    public static void handle_clients(object o)
    {
        int id = (int)o;
        TcpClient client;

        lock (_lock) client = list_clients[id];
        NetworkStream stream = client.GetStream();
        firstLoop(stream, id);
        while (true)
        {            
            byte[] buffer = new byte[1024];
            //Array.Clear(buffer, 0, buffer.Length);
            int byte_count;
           
            byte_count = stream.Read(buffer, 0, buffer.Length);
            
            string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
            
            if(data.Contains("Disconnected:"))
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
        broadcast(id+ "Disconnected:", id);
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
    public static bool zipFile(string folderName)
    {
        try
        {
            string[] paths = { AppDomain.CurrentDomain.BaseDirectory, folderName };
            string fullPath = Path.Combine(paths);

            string[] outPath = { AppDomain.CurrentDomain.BaseDirectory, folderName + ".zip" };
            string outFullPath = Path.Combine(outPath);
            string startPath = fullPath;
            string zipPath = outFullPath;

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(startPath, zipPath);
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public static byte[] getFileBytes(string path)
    {
        return File.ReadAllBytes(path);
    }
}