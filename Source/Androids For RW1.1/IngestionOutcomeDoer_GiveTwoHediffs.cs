using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class IngestionOutcomeDoer_GiveTwoHediffs : IngestionOutcomeDoer
    {
        private readonly bool divideByBodySize = false;

        public HediffDef hediffDef_Android = null;

        public HediffDef hediffDef_Human = null;

        public float severity = -1f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if (pawn.IsAndroid() == false)
            {
                var hediff = HediffMaker.MakeHediff(hediffDef_Human, pawn);
                var num = severity > 0f ? severity : hediffDef_Human.initialSeverity;
                if (divideByBodySize) num /= pawn.BodySize;
                hediff.Severity = num;
                pawn.health.AddHediff(hediff, null, null);
            }
            else
            {
                var hediff = HediffMaker.MakeHediff(hediffDef_Android, pawn);
                var num = severity > 0f ? severity : hediffDef_Android.initialSeverity;
                if (divideByBodySize) num /= pawn.BodySize;
                hediff.Severity = num;
                pawn.health.AddHediff(hediff, null, null);
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (!parentDef.IsDrug || !(chance >= 1f)) yield break;

            foreach (var s in hediffDef_Human.SpecialDisplayStats(StatRequest.ForEmpty()))
                yield return s;
        }
    }
}