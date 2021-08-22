using System;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace ClientSubnautica
{
	// Token: 0x0200000E RID: 14
	[Menu("Multiplayer", SaveOn =MenuAttribute.SaveEvents.None)]
	public class Config : ConfigFile
	{
        [Toggle("Enable multiplayer"), OnChange(nameof(MyCheckboxToggleEvent))]
        public bool ToggleValue;

        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {
            ApplyPatches.startMultiplayer = e.Value;
        }

    }
}
