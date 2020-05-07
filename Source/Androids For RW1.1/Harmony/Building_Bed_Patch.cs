using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class Building_Bed_Patch

    {
        [HarmonyPatch(typeof(CompAssignableToPawn), "get_AssigningCandidates")]
        public class get_AssigningCandidates
        {
            private static void addInactiveSurrogates(ref List<Pawn> lst, Map map, bool M7)
            {
                foreach (var p in map.mapPawns.AllPawns)
                    if (p.Faction == Faction.OfPlayer)
                    {
                        var cas = p.TryGetComp<CompAndroidState>();

                        if (cas != null && cas.isSurrogate && cas.surrogateController == null && !cas.isOrganic && (!M7 || p.def.defName == Utils.M7)) lst.Add(p);
                    }
            }

            [HarmonyPostfix]
            public static void Listener(ref IEnumerable<Pawn> __result, CompAssignableToPawn __instance)
            {
                var orig = __result;
                try
                {
                    var bed = (Building_Bed) __instance.parent;

                    if (bed.def.defName == "ATPP_AndroidPod")
                    {
                        var lst = new List<Pawn>();
                        foreach (var el in __result)
                            if (el.def.defName != Utils.M7 && el.IsAndroidTier())
                                lst.Add(el);

                        //Si option masquant les surrogates activé alors ajout de ces derniers à la fin
                        if (Settings.hideInactiveSurrogates)
                            addInactiveSurrogates(ref lst, bed.Map, false);

                        __result = lst;
                    }
                    else if (bed.def.defName == "ATPP_AndroidPodMech")
                    {
                        var lst = new List<Pawn>();
                        foreach (var el in __result)
                            if (el.def.defName == Utils.M7)
                                lst.Add(el);
                        //Si option masquant les surrogates activé alors ajout de ces derniers à la fin
                        if (Settings.hideInactiveSurrogates)
                            addInactiveSurrogates(ref lst, bed.Map, false);

                        __result = lst;
                    }
                    else if (bed.def.defName != "SleepingSpot")
                    {
                        var lst = new List<Pawn>();
                        foreach (var el in __result)
                            if (!el.IsAndroidTier())
                                lst.Add(el);
                        __result = lst;
                    }
                }
                catch (Exception e)
                {
                    __result = orig;
                    Log.Message("[ATPP] Building_Bed get_AssigningCandidates" + e.Message + " " + e.StackTrace);
                }
            }
        }


        [HarmonyPatch(typeof(Building_Bed), "GetFloatMenuOptions")]
        public class GetFloatMenuOptions_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn myPawn, Building_Bed __instance, ref IEnumerable<FloatMenuOption> __result)
            {
                if (__instance.def.defName != "ATPP_AndroidPod" && __instance.def.defName != "ATPP_AndroidPodMech" || __instance.Medical ||
                    myPawn.ownership != null && myPawn.ownership.OwnedBed != null && myPawn.ownership.OwnedBed != __instance)
                    return;

                if (__result == null)
                    __result = new List<FloatMenuOption>();

                var failureReason = GetFailureReason(__instance, myPawn);
                if (failureReason != null)
                    __result = __result.AddItem(failureReason);
                else
                    __result = __result.AddItem(new FloatMenuOption("ATPP_ForceReload".Translate(), delegate
                    {
                        //Affectation du pod a myPawn pour eviter le rehet du job
                        myPawn.ownership.ClaimBedIfNonMedical(__instance);

                        var job = new Job(DefDatabase<JobDef>.GetNamed("ATPP_GoReloadBattery"), new LocalTargetInfo(__instance));
                        myPawn.jobs.TryTakeOrderedJob(job);
                    }));
            }

            private static FloatMenuOption GetFailureReason(Building_Bed bed, Pawn myPawn)
            {
                if (!myPawn.CanReach(bed, PathEndMode.InteractionCell, Danger.Some)) return new FloatMenuOption("CannotUseNoPath".Translate(), null);
                if (bed.Spawned && bed.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
                    return new FloatMenuOption("CannotUseSolarFlare".Translate(), null);
                if (!bed.TryGetComp<CompPowerTrader>().PowerOn) return new FloatMenuOption("CannotUseNoPower".Translate(), null);
                if (!Utils.ExceptionAndroidList.Contains(myPawn.def.defName)) return new FloatMenuOption("ATPP_CanOnlyBeUsedByAndroid".Translate(), null);

                var ca = myPawn.TryGetComp<CompAndroidState>();
                if (ca == null || !ca.UseBattery)
                    return new FloatMenuOption("ATPP_CannotUseBecauseNotInBatteryMode".Translate(), null);

                return null;
            }
        }
    }
}