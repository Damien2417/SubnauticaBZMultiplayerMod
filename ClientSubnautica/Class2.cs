using System;
using System.Threading;
using HarmonyLib;
using UnityEngine;

namespace CreaturePetMod_BZ
{
	// Token: 0x0200000A RID: 10
	internal class SpawnInstigator
	{
		public static bool startMultiplayer = false;
		public static bool startedMultiplayer = false;
		// Token: 0x02000016 RID: 22
		//[HarmonyPatch(typeof(Player))]
		//[HarmonyPatch("Update")]
		internal class PetSpawner
		{
			// Token: 0x06000037 RID: 55 RVA: 0x00002FFB File Offset: 0x000011FB
			//[HarmonyPostfix]
			public static void OnSpawn()
			{
				if (startMultiplayer)
				{
					if (!startedMultiplayer)
					{
						ErrorMessage.AddMessage("go");
						CreaturePetMod_BZ.PetSpawner.SpawnCreaturePet();
						startedMultiplayer = true;
					}
				}
			}
		}
	}
}
