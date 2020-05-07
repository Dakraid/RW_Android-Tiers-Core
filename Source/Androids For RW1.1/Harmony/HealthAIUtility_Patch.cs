using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class HealthAIUtility_Patch
    {
        /*
         * Permet de forcer les surrogates downed d'etres entreposés dans leurs pods dédiés
         */
        [HarmonyPatch(typeof(HealthAIUtility), "ShouldSeekMedicalRest")]
        public class ShouldSeekMedicalRest_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ref bool __result)
            {
                try
                {
                    if (pawn.Faction != Faction.OfPlayer) return;

                    var cas = pawn.TryGetComp<CompAndroidState>();
                    if (cas != null && pawn.health != null && pawn.health.summaryHealth.SummaryHealthPercent >= 0.80f && cas.isSurrogate && cas.surrogateController == null && pawn.ownership?.OwnedBed != null) //&& ReachabilityUtility.CanReach(pawn, pawn.ownership.OwnedBed, PathEndMode.OnCell, Danger.Deadly))
                        __result = false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] HealthAIUtility.ShouldSeekMedicalRest " + e.Message + " " + e.StackTrace);
                }
            }
        }


        [HarmonyPatch(typeof(HealthAIUtility), "FindBestMedicine")]
        public class FindBestMedicine_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn healer, Pawn patient, ref Thing __result)
            {
                try
                {
                    //On ne soccupe que des patient étant des androids
                    /*if (!)
                        return true;*/


                    if (Settings.androidsCanUseOrganicMedicine)
                        return;

                    var patientIsAndroid = Utils.ExceptionAndroidList.Contains(patient.def.defName) || patient.IsCyberAnimal();

                    if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
                    {
                        __result = null;
                        return;
                    }

                    if (Medicine.GetMedicineCountToFullyHeal(patient) <= 0)
                    {
                        __result = null;
                        return;
                    }

                    Predicate<Thing> predicate;

                    //COmpatibilité avec pharmacist, le medoc renvoyé doit avoir une quantitée de soin inferieur ou egal à celui renvoyé par les appels précédents
                    float medicalPotency = 0;
                    if (__result != null) medicalPotency = __result.def.GetStatValueAbstract(StatDefOf.MedicalPotency);

                    if (patientIsAndroid)
                        predicate = m => Utils.ExceptionNanoKits.Contains(m.def.defName) && m.def.GetStatValueAbstract(StatDefOf.MedicalPotency) <= medicalPotency &&
                                         !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 10, 1);
                    else
                        predicate = m => !Utils.ExceptionNanoKits.Contains(m.def.defName) && m.def.GetStatValueAbstract(StatDefOf.MedicalPotency) <= medicalPotency &&
                                         !m.IsForbidden(healer) && !m.IsForbidden(healer) && patient.playerSettings.medCare.AllowsMedicine(m.def) && healer.CanReserve(m, 10, 1);

                    Func<Thing, float> priorityGetter = t => t.def.GetStatValueAbstract(StatDefOf.MedicalPotency);

                    var position = patient.Position;
                    var map = patient.Map;
                    var searchSet = patient.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
                    var peMode = PathEndMode.ClosestTouch;
                    var traverseParams = TraverseParms.For(healer);
                    var validator = predicate;
                    __result = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, priorityGetter);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] HealthAIUtility.FindBestMedicine(Error) : " + e.Message + " - " + e.StackTrace);
                }
            }
        }
    }
}