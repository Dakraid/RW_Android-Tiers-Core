﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class WorkGiver_FeedPatient_Patch

    {
        /*
         * Allow crafters to do doctor jobs (for androids)
         */
        [HarmonyPatch(typeof(WorkGiver_FeedPatient), "HasJobOnThing")]
        public class HasJobOnThing_Patch
        {
            [HarmonyPostfix]
            public static void ListenerPostfix(Pawn pawn, Thing t, bool forced, ref bool __result, WorkGiver_FeedPatient __instance)
            {
                Utils.genericPostFixExtraCrafterDoctorJobs(pawn, t, forced, ref __result, __instance);
            }

            [HarmonyPrefix]
            public static bool ListenerPrefix(Pawn pawn, Thing t, bool forced, ref bool __result, WorkGiver_FeedPatient __instance)
            {
                if (!(t is Pawn pawn2) || !pawn2.IsAndroidTier()) return true;

                var cas = pawn2.TryGetComp<CompAndroidState>();
                if (cas == null) return true;

                if (!cas.connectedLWPNActive || cas.connectedLWPN == null) return true;

                __result = false;
                return false;
            }
        }
    }
}