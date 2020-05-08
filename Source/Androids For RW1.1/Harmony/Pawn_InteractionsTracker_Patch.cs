using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Pawn_InteractionsTracker_Patch

    {
        /*
         * PRIMARY remove androids to be have romance
         */
        [HarmonyPatch(typeof(Pawn_InteractionsTracker), "TryInteractWith")]
        public class TryInteractWith_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn recipient, InteractionDef intDef, Pawn ___pawn, ref bool __result)
            {
                if (!___pawn.IsBasicAndroidTier() && !recipient.IsBasicAndroidTier() || !Utils.IgnoredInteractionsByBasicAndroids.Contains(intDef.defName)) return true;

                __result = false;
                return false;
            }
        }
    }
}