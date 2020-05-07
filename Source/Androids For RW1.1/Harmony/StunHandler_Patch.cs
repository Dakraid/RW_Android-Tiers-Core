using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class StunHandler_Patch

    {
        /*
         * Allow android tiers to be affected by EMP
         */
        [HarmonyPatch(new[] {typeof(DamageInfo), typeof(bool)})]
        [HarmonyPatch(typeof(StunHandler), "Notify_DamageApplied")]
        public class Notify_DamageApplied_Patch
        {
            [HarmonyPrefix]
            public static void Listener(DamageInfo dinfo, ref bool affectedByEMP, Thing ___parent)
            {
                if (___parent is Pawn)
                {
                    var pawn = (Pawn) ___parent;
                    if (Utils.ExceptionAndroidWithoutSkinList.Contains(pawn.def.defName) || Utils.ExceptionAndroidAnimals.Contains(pawn.def.defName)) affectedByEMP = true;
                }
            }
        }
    }
}