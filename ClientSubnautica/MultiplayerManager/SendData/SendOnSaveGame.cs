using System;
using System.Net.Sockets;
using System.Text;
using ClientSubnautica.MultiplayerManager.ReceiveData;
using UnityEngine;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    public class SendOnSaveGame
    {
        public static void SendPosition(Vector3 position)
        {
            bool saved = SendData(position);
        }

        private static bool SendData(Vector3 position)
        {
            NetworkStream ns2 = InitializeConnection.client.GetStream();
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
                        Quaternion roTemp = MainCameraControl.main.viewModel.transform.rotation;
                        rotxTemp = roTemp.x.ToString();
                        rotyTemp = roTemp.y.ToString();
                        rotzTemp = roTemp.z.ToString();
                        rotwTemp = roTemp.w.ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    if (position.x.ToString() != x |
                        position.y.ToString() != y |
                        position.z.ToString() != z |
                        rotxTemp != rotx | rotyTemp != roty |
                        rotzTemp != rotz | rotwTemp != rotw)
                    {
                        rotx = rotxTemp;
                        roty = rotyTemp;
                        rotz = rotzTemp;
                        rotw = rotwTemp;
                        byte[] msgresponse = Encoding.ASCII.GetBytes("");
                        Array.Clear(msgresponse, 0, msgresponse.Length);

                        msgresponse = Encoding.ASCII.GetBytes(NetworkCMD.getIdCMD("SaveGameRequest") + ":" +
                                                              position.x + ";" +
                                                              position.y + ";" +
                                                              position.z + ";" + rotx + ";" +
                                                              roty + ";" + rotz + ";" + rotw + "/END/");

                        ns2.Write(msgresponse, 0, msgresponse.Length);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}