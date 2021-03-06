﻿using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Pawn_NeedsTracker_Patch
    {
        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
        public class ShouldHaveNeed_Patch
        {
            [HarmonyPostfix]
            public static void Listener(NeedDef nd, ref bool __result, Pawn ___pawn)
            {
                try
                {
                    var isAndroid = Utils.ExceptionAndroidList.Contains(___pawn.def.defName);


                    if (!isAndroid)
                        return;

                    var advancedAndroids = Utils.ExceptionAndroidListAdvanced.Contains(___pawn.def.defName);

                    if (Utils.ExceptionAndroidListBasic.Contains(___pawn.def.defName)
                        && nd.defName == "Outdoors"
                        || ___pawn.def.defName == "Android1Tier" && nd.defName == "Beauty"
                        || isAndroid && (nd.defName == "Hygiene" || nd.defName == "Bladder" || nd.defName == "DBHThirst")
                        || nd.defName == "Comfort" && (!advancedAndroids || advancedAndroids && Settings.removeComfortNeedForT3T4))
                        __result = false;


                    if (___pawn.def.defName == "M7Mech" && ___pawn.IsSurrogateAndroid() && nd.defName == "Food")
                        __result = true;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn_StoryTracker.ShouldHaveNeed : " + e.Message + " - " + e.StackTrace);
                }
            }
        }


        [HarmonyPatch(typeof(Pawn_NeedsTracker), "NeedsTrackerTick")]
        public class NeedsTrackerTick_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn ___pawn)
            {
                var cso = ___pawn.TryGetComp<CompSurrogateOwner>();
                return cso?.skyCloudHost == null;
            }
        }
    }
}