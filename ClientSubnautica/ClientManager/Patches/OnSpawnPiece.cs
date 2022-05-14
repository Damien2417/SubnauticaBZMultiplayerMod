using HarmonyLib;
using UnityEngine;

namespace ClientSubnautica
{
    internal class OnSpawnPiece
    {
        [HarmonyPatch(typeof(CraftingAnalytics), "OnConstruct")]
        internal static class Patches
        {
            [HarmonyPostfix]
            public static void Postfix(TechType techType, Vector3 position)
            {
                SendOnSpawnPiece.send(techType.ToString(), position.x.ToString(), position.y.ToString(), position.z.ToString());
            }

            /*public static void Postfix(Base.Piece piece, Int3 cell, Vector3 position, Quaternion rotation, Base.Direction? faceDirection = null, BaseDeconstructable sourceBaseDeconstructable = null)
            {
                Base.Piece enumValue = (Base.Piece)piece;

                sendOnSpawnPiece.send(enumValue.ToString(), cell,position,rotation);
            }*/
        }
    }
}
