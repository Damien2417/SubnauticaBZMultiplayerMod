using SubnauticaModTest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientSubnautica
{
    internal class StartMultiplayer
    {
        public static ConcurrentDictionary<int, GameObject> players { get; set; }
        public static ConcurrentDictionary<int, string> lastPos { get; set; }
        public static ConcurrentDictionary<int, string> posLastLoop { get; set; }
        public static GameObject[] playerBodies { get; set; }


        public static TcpClient main()
        {
            
            ErrorMessage.AddMessage("Searching server...");
            IPAddress ip = IPAddress.Parse("192.168.0.83");
            int port = 5000;
            TcpClient client = new TcpClient();
            while (true)
            {
                try
                {
                    client.Connect(ip, port);
                    break;
                }
                catch { }
                Thread.Sleep(500);
            }
            ErrorMessage.AddMessage("Connected on " + ip + ":" + port + " !");
            return client;
        }

        //Receive data from server
        internal static void ReceiveData(TcpClient client2)
        {
            NetworkStream ns2 = client2.GetStream();
            try
            {
                byte[] receivedBytes = new byte[1024];
                int byte_count;



                
                while ((byte_count = ns2.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(receivedBytes, 0, byte_count);
                    ApplyPatches.messages.Add(message);                  
                    Thread.Sleep(16);
                }
                ns2.Close();
            }
            catch
            {
                client2.Client.Shutdown(SocketShutdown.Send);
                ns2.Close();
                client2.Close();
            }
        }

        //Send data to server
        public static void SendData(TcpClient client2)
        {
            NetworkStream ns2 = client2.GetStream();
            try
            {
                string data = null;

                string pos;
                string x = "";
                string y = "";
                string z = "";
                while (true)
                {
                    string rotx = Player.main.transform.localRotation.eulerAngles.x.ToString();
                    string roty = Player.main.transform.localRotation.eulerAngles.y.ToString();
                    string rotz = Player.main.transform.localRotation.eulerAngles.z.ToString();
                    if (Player.main.transform.position.x.ToString() != x | Player.main.transform.position.y.ToString() != y | Player.main.transform.position.z.ToString() != z)
                    {
                        //UnityEngine.Debug.Log(""+Player.main.rigidBody.rotation.eulerAngles.y);

                        byte[] msgresponse = Encoding.ASCII.GetBytes("");
                        Array.Clear(msgresponse, 0, msgresponse.Length);

                        msgresponse = Encoding.ASCII.GetBytes("WORLDPOSITION" + "(" + Player.main.transform.position.x + ";" + Player.main.transform.position.y + ";" + Player.main.transform.position.z +/*";"+rotx+";"+roty+";"+rotz+*/ ")");


                        // Position envoyé !
                        ns2.Write(msgresponse, 0, msgresponse.Length);

                        data = Encoding.ASCII.GetString(msgresponse, 0, msgresponse.Length);
                        pos = data.Split('(')[1];
                        x = pos.Split(';')[0];
                        y = pos.Split(';')[1];
                        z = pos.Split(';')[2];
                        z = z.Remove(z.Length - 1);
                    }
                    Thread.Sleep(16);
                }
            }
            catch
            {
                byte[] test = Encoding.ASCII.GetBytes("DISCONNECTED");

                ns2.Write(test, 0, test.Length);
                //client2.Client.Shutdown(SocketShutdown.Send);
                ns2.Close();
                client2.Close();
            }
        }      
    }
}