using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace ClientSubnautica
{
	// Token: 0x0200000E RID: 14
	[Menu("Multiplayer", SaveOn =MenuAttribute.SaveEvents.None)]
	public class Config : ConfigFile
	{
        public string ipAddress;
        public string port;

    }
}
