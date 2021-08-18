using System;
using UnityEngine;

namespace CreaturePetMod_BZ
{
	// Token: 0x02000009 RID: 9
	internal class PetUtils
	{
		// Token: 0x0600001A RID: 26 RVA: 0x0000290A File Offset: 0x00000B0A
		internal static bool IsCreaturePet(Creature creaturePet)
		{
			return creaturePet.GetComponentInParent<CreaturePet>();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002917 File Offset: 0x00000B17
		internal static string GetCreaturePrefabId(Creature creaturePet)
		{
			return creaturePet.GetComponent<PrefabIdentifier>().Id;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002924 File Offset: 0x00000B24
		internal static bool IsGameGameObjectPet(GameObject gameObject)
		{
			return gameObject.GetComponentInParent<CreaturePet>();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002934 File Offset: 0x00000B34
		internal static PetDetails IsPrefabIdInHashSet(string prefabid)
		{
			foreach (PetDetails petDetails in QMod.PetDetailsHashSet)
			{
				if (petDetails.PrefabId == prefabid)
				{
					return petDetails;
				}
			}
			return null;
		}
	}
}
