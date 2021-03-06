﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class WorkGiver_DoBill_Patch

    {
        [HarmonyPatch(typeof(WorkGiver_DoBill), "AddEveryMedicineToRelevantThings")]
        public class AddEveryMedicineToRelevantThings_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, Thing billGiver, List<Thing> relevantThings, Predicate<Thing> baseValidator, Map map)
            {
                try
                {
                    if (!(billGiver is Pawn patient)) return;

                    if (patient.IsAndroidTier() || patient.IsCyberAnimal())
                        foreach (var el in relevantThings.ToList().Where(el => !Utils.ExceptionNanoKits.Contains(el.def.defName)))
                            relevantThings.Remove(el);

                    else
                        foreach (var el in relevantThings.ToList().Where(el => Utils.ExceptionNanoKits.Contains(el.def.defName)))
                            relevantThings.Remove(el);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] WorkGiver_DoBill.AddEveryMedicineToRelevantThings " + e.Message + " " + e.StackTrace);
                }
            }
        }


        /*
         * Allow crafters to do doctor jobs (for androids)
         */
        [HarmonyPatch(typeof(WorkGiver_DoBill), "JobOnThing")]
        public class JobOnThing_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, Thing thing, bool forced, ref Job __result, WorkGiver_DoBill __instance)
            {
                try
                {
                    if (!Settings.androidsCanOnlyBeHealedByCrafter)
                    {
                        if (__instance.def.workType == Utils.WorkTypeDefSmithing && Utils.CrafterDoctorJob.Contains(__instance.def))
                            __result = null;

                        return;
                    }


                    if (__instance.def.workType == WorkTypeDefOf.Doctor)
                    {
                        if (thing is Pawn && ((Pawn) thing).IsAndroidTier())
                            __result = null;
                    }
                    else
                    {
                        if (!Utils.CrafterDoctorJob.Contains(__instance.def)) return;

                        if (thing is Pawn && ((Pawn) thing).IsAndroidTier())
                        {
                            var cso = pawn.TryGetComp<CompSurrogateOwner>();

                            if (cso == null || !cso.repairAndroids)
                                __result = null;
                        }
                        else
                        {
                            __result = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] WorkGiver_DoBill.JobOnThing " + e.Message + " " + e.StackTrace);
                }
            }
        }
    }
}