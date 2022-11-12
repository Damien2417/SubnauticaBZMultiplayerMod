using Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubnautica.MultiplayerManager.SendData
{
    internal class SendStoryGoal
    {
        // Send current story goal to server
        public static void start(TcpClient client)
        {
            NetworkStream ns2 = client.GetStream();
            try
            {
                var alanDlGoal = StoryGoalManager.main.alanDownloadedGoal;
                var alanTransferGoal = StoryGoalManager.main.alanTransferedGoal;
                var StoryProgress = new StoryGoalProgressionData().progression;
            } catch (Exception e)
            {
                ErrorMessage.AddError(e.Message);
            }
        }
    }
}
