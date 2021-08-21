using System;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using SubnauticaModTest;
using UnityEngine;

namespace SubnauticaMod
{
	// Token: 0x0200000E RID: 14
	[Menu("Multiplayer", SaveOn =MenuAttribute.SaveEvents.None)]
	public class Config : ConfigFile
	{
        [Toggle("Enable multiplayer"), OnChange(nameof(MyCheckboxToggleEvent))]
        public bool ToggleValue;

        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {
            SubnauticaMod.ApplyPatches.startMultiplayer = e.Value;
        }

    }
}
