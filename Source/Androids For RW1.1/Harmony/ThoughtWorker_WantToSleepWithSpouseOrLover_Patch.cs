using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ThoughtWorker_WantToSleepWithSpouseOrLover_Patch

    {
        [HarmonyPatch(typeof(ThoughtWorker_WantToSleepWithSpouseOrLover), "CurrentStateInternal")]
        public class CurrentStateInternal_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref ThoughtState __result)
            {
                try
                {
                    if (!__result.Active)
                        return;

                    var cso = p.TryGetComp<CompSurrogateOwner>();

                    var otherPawn = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(p, false).otherPawn;
                    CompSurrogateOwner cso2 = null;
                    if (otherPawn != null)
                        cso2 = otherPawn.TryGetComp<CompSurrogateOwner>();

                    if (p.IsAndroidTier() || p.IsSurrogateAndroid() || cso != null && cso.skyCloudHost != null || otherPawn != null &&
                        (otherPawn.IsAndroidTier() || otherPawn.IsSurrogateAndroid() || cso2 != null && cso2.skyCloudHost != null))
                        __result = false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATTP] ThoughtWorker_WantToSleepWithSpouseOrLover.CurrentStateInternal " + e.Message + " " + e.StackTrace);
                }
            }
        }
    }
}