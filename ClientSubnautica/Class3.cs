using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UWE;
using HarmonyLib;

namespace CreaturePetMod_BZ
{
	// Token: 0x02000006 RID: 6
	internal static class PetSpawner
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000020F4 File Offset: 0x000002F4
		internal static void SpawnCreaturePet()
		{			
			GameObject petCreatureGameObject;

			petCreatureGameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("player_view_female"), new Vector3((float)-294.3636, (float)17.02644, (float)252.9224), Quaternion.identity);
		}

		
	}
}
