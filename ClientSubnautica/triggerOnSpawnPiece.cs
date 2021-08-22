using HarmonyLib;
using UnityEngine;

namespace ClientSubnautica
{
    internal class triggerOnSpawnPiece
    {
        //[HarmonyPatch(typeof(Builder), "Begin")]
        internal static class Patches
        {
            //[HarmonyPrefix]
            public static void Prefix(TechType techType)
            {
                ErrorMessage.AddMessage("envoi "+ techType.ToString());
                sendOnSpawnPiece.send(techType.ToString());
            }

            /*public static void Postfix(Base.Piece piece, Int3 cell, Vector3 position, Quaternion rotation, Base.Direction? faceDirection = null, BaseDeconstructable sourceBaseDeconstructable = null)
            {
                Base.Piece enumValue = (Base.Piece)piece;

                sendOnSpawnPiece.send(enumValue.ToString(), cell,position,rotation);
            }*/
        }
    }
}
