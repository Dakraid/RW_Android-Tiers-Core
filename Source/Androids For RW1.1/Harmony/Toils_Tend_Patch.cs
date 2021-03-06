﻿using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class Toils_Tend_Patch

    {
        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(Toils_Tend), "FinalizeTend")]
        public class FinalizeTend_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn patient, ref Toil __result)
            {
                try
                {
                    if (!Settings.androidsCanOnlyBeHealedByCrafter || !patient.IsAndroidTier() && !patient.IsCyberAnimal()) return true;

                    var toil = new Toil();
                    toil.initAction = delegate
                    {
                        var actor = toil.actor;
                        var medicine = (Medicine) actor.CurJob.targetB.Thing;
                        var num = !patient.RaceProps.Animal ? 500f : 175f;
                        var num2 = medicine?.def.MedicineTendXpGainFactor ?? 0.5f;
                        actor.skills.Learn(SkillDefOf.Crafting, num * num2);
                        TendUtility.DoTend(actor, patient, medicine);
                        if (medicine != null && medicine.Destroyed) actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
                        if (toil.actor.CurJob.endAfterTendedOnce) actor.jobs.EndCurrentJob(JobCondition.Succeeded);
                    };
                    toil.defaultCompleteMode = ToilCompleteMode.Instant;
                    __result = toil;

                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Toils_Tend.FinalizeTend " + e.Message + " " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}