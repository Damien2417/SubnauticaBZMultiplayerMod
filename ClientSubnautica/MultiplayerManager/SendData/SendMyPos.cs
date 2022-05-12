using ClientSubnautica.MultiplayerManager.ReceiveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    class SendMyPos
    {
        //Send data to server
        public static void start(TcpClient client2)
        {
            NetworkStream ns2 = client2.GetStream();
            try
            {
                string x = "";
                string y = "";
                string z = "";
                string rotx = "";
                string roty = "";
                string rotz = "";
                string rotw = "";
                string rotxTemp = "";
                string rotyTemp = "";
                string rotzTemp = "";
                string rotwTemp = "";
                while (true)
                {
                    try
                    {
                        rotxTemp = MainCameraControl.main.viewModel.transform.rotation.x.ToString();
                        rotyTemp = MainCameraControl.main.viewModel.transform.rotation.y.ToString();
                        rotzTemp = MainCameraControl.main.viewModel.transform.rotation.z.ToString();
                        rotwTemp = MainCameraControl.main.viewModel.transform.rotation.w.ToString();
                    }
                    catch
                    { }

                    if (Player.main.transform.position.x.ToString() != x | Player.main.transform.position.y.ToString() != y | Player.main.transform.position.z.ToString() != z | rotxTemp != rotx | rotyTemp != roty | rotzTemp != rotz | rotwTemp != rotw)
                    {
                        byte[] msgresponse = Encoding.ASCII.GetBytes("");
                        Array.Clear(msgresponse, 0, msgresponse.Length);

                        msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("WorldPosition") +":" + Player.main.transform.position.x + ";" + Player.main.transform.position.y + ";" + Player.main.transform.position.z +";"+rotx+";"+roty+";"+rotz+";"+rotw+ "/END/");

                        // Position envoyé !
                        ns2.Write(msgresponse, 0, msgresponse.Length);
                        x = Player.main.transform.position.x.ToString();
                        y = Player.main.transform.position.y.ToString();
                        z = Player.main.transform.position.z.ToString();

                        rotx = rotxTemp;
                        roty = rotyTemp;
                        rotz = rotzTemp;
                        rotw = rotwTemp;
                    }
                    Thread.Sleep(10);
                }
            }
            catch
            {
                byte[] test = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("Disconnected") + ":/END/");

                ns2.Write(test, 0, test.Length);
                client2.Client.Shutdown(SocketShutdown.Send);
                ns2.Close();
                client2.Close();
            }
        }
    }
}
