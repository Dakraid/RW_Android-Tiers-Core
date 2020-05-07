using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class TendUtility_Patch

    {
        /*
         * Set correct stats for crafter healing androids
         */
        [HarmonyPatch(typeof(TendUtility), "CalculateBaseTendQuality")]
        [HarmonyPatch(new[] {typeof(Pawn), typeof(Pawn), typeof(float), typeof(float)}, new[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal})]
        public class CalculateBaseTendQuality_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn doctor, Pawn patient, float medicinePotency, float medicineQualityMax, ref float __result)
            {
                if (!Settings.androidsCanOnlyBeHealedByCrafter || !patient.IsAndroidTier())
                    return true;

                try
                {
                    var num = doctor?.GetStatValue(Utils.statDefAndroidTending) ?? 0.75f;
                    num *= medicinePotency;
                    var building_Bed = patient?.CurrentBed();
                    if (building_Bed != null) num += building_Bed.GetStatValue(StatDefOf.MedicalTendQualityOffset);
                    if (doctor == patient && doctor != null) num *= 0.7f;
                    __result = Mathf.Clamp(num, 0f, medicineQualityMax);
                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] TendUtility.CalculateBaseTendQuality " + e.Message + " " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}