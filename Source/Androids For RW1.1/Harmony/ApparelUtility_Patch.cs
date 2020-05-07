using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ApparelUtility_Patch

    {
        [HarmonyPatch(typeof(ApparelUtility), "CanWearTogether")]
        public class CanWearTogether_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ThingDef A, ThingDef B, BodyDef body, ref bool __result)
            {
                if (A.defName == "VAE_Headgear_Scarf" && B.defName == "VAE_Headgear_Scarf")
                    __result = false;
            }
        }

        [HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear")]
        public class HasPartsToWear_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ThingDef apparel, ref bool __result)
            {
                try
                {
                    if (!p.IsAndroidTier())
                        return;

                    var councernFeet = apparel.apparel.bodyPartGroups.Contains(DefDatabase<BodyPartGroupDef>.GetNamed("Feet", false));
                    var councernHanbd = apparel.apparel.bodyPartGroups.Contains(DefDatabase<BodyPartGroupDef>.GetNamed("Hands", false));
                    if (!councernHanbd && !councernFeet) return;

                    if (councernFeet)
                    {
                        if (!Enumerable.Any(p.health.hediffSet.hediffs, el => Utils.ExceptionBionicHaveFeet.Contains(el.def.defName))) return;

                        __result = true;
                    }
                    else
                    {
                        if (!Enumerable.Any(p.health.hediffSet.hediffs, el => Utils.ExceptionBionicHaveHand.Contains(el.def.defName))) return;

                        __result = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] ApparelUtility.HasPartsToWear " + ex.Message + " " + ex.StackTrace);
                }
            }
        }
    }
}