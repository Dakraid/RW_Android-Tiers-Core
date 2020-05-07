using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnWoundDrawer_Patch

    {
        /*
         * Les androide avec degration visuel de la peau n'ont pas de particules wounds mappées
         */
        [HarmonyPatch(typeof(PawnWoundDrawer), "RenderOverBody")]
        public class RenderOverBody_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Vector3 drawLoc, Mesh bodyMesh, Quaternion quat, bool forPortrait, Pawn ___pawn)
            {
                return !Utils.ExceptionAndroidWithSkinList.Contains(___pawn.def.defName);
            }
        }
    }
}