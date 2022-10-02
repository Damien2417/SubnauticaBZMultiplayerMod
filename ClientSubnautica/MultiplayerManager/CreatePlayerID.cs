using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubnautica.MultiplayerManager
{
    internal class CreatePlayerID
    {
        public static string GenerateID()
        {
            var tid = Process.GetCurrentProcess().Id.ToString() + DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            return tid;
        }
    }
}
