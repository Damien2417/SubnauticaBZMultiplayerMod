using System;
using UnityEngine;
using UWE;

namespace ClientSubnautica
{
    internal class triggerOnSpawnPiece
    {
        //[HarmonyPatch(typeof(CraftingAnalytics), "OnConstruct")]
        internal static class Patches
        {
            //[HarmonyPostfix]
            public static void Postfix(TechType techType, Vector3 position)
            {

                /*GameObject test;
                CoroutineHost.StartCoroutine(Enumerable.SetupNewGameObject((TechType)Enum.Parse(typeof(TechType), techType.ToString()), returnValue =>
                {
                    test = returnValue;
                    test.transform.position = Player.main.transform.position;

                }));*/
                //SendOnSpawnPiece.send(techType.ToString());
            }

            /*public static void Postfix(Base.Piece piece, Int3 cell, Vector3 position, Quaternion rotation, Base.Direction? faceDirection = null, BaseDeconstructable sourceBaseDeconstructable = null)
            {
                Base.Piece enumValue = (Base.Piece)piece;

                sendOnSpawnPiece.send(enumValue.ToString(), cell,position,rotation);
            }*/
        }
    }
}
