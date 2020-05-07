using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000C7D RID: 3197
    public class HediffGiver_BirthdayMechanical : HediffGiver
    {
        // Token: 0x04002E99 RID: 11929
        private static readonly List<Hediff> addedHediffs = new List<Hediff>();

        // Token: 0x04002E97 RID: 11927
        public SimpleCurve ageFractionChanceCurve;

        // Token: 0x04002E98 RID: 11928
        public float averageSeverityPerDayBeforeGeneration;

        // Token: 0x06004370 RID: 17264 RVA: 0x001E87A0 File Offset: 0x001E6BA0
        public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
        {
            addedHediffs.Clear();
            if (!TryApply(pawn, addedHediffs)) return;

            if (averageSeverityPerDayBeforeGeneration != 0f)
            {
                var num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
                if (num < 0f)
                {
                    Log.Error(string.Concat("daysPassed < 0, pawn=", pawn, ", gotAtAge=", gotAtAge));
                    return;
                }

                foreach (var hediff in addedHediffs)
                    SimulateSeverityChange(pawn, hediff, num, tryNotToKillPawn);
            }

            addedHediffs.Clear();
        }

        // Token: 0x06004371 RID: 17265 RVA: 0x001E8860 File Offset: 0x001E6C60
        private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
        {
            var num = averageSeverityPerDayBeforeGeneration * daysPassed;
            num *= Rand.Range(0.5f, 1.4f);
            num += hediff.def.initialSeverity;
            if (tryNotToKillPawn) AvoidLifeThreateningStages(ref num, hediff.def.stages);
            hediff.Severity = num;
            pawn.health.Notify_HediffChanged(hediff);
        }

        // Token: 0x06004372 RID: 17266 RVA: 0x001E88C4 File Offset: 0x001E6CC4
        private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
        {
            if (stages.NullOrEmpty()) return;

            var num = -1;
            for (var i = 0; i < stages.Count; i++)
                if (stages[i].lifeThreatening)
                {
                    num = i;
                    break;
                }

            if (num >= 0) severity = num == 0 ? Mathf.Min(severity, stages[num].minSeverity) : Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
        }

        // Token: 0x06004373 RID: 17267 RVA: 0x001E8960 File Offset: 0x001E6D60
        public float DebugChanceToHaveAtAge(Pawn pawn, int age)
        {
            var num = 1f;
            for (var i = 1; i <= age; i++)
            {
                var x = i / pawn.RaceProps.lifeExpectancy;
                num *= 1f - ageFractionChanceCurve.Evaluate(x);
            }

            return 1f - num;
        }

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            //Remove any disease from affecting.
            if (!hediff.def.makesSickThought && hediff.def != RimWorld.HediffDefOf.FoodPoisoning && hediff.def != RimWorld.HediffDefOf.CryptosleepSickness) return true;

            pawn.health.RemoveHediff(hediff);
            return false;
        }
    }
}