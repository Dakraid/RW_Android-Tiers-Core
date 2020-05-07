using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class IngestionOutcomeDoer_GiveTwoHediffs : IngestionOutcomeDoer
    {
        private bool divideByBodySize;

        public HediffDef hediffDef_Android;

        public HediffDef hediffDef_Human;

        public float severity = -1f;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            if (pawn.IsAndroid() == false)
            {
                var hediff = HediffMaker.MakeHediff(hediffDef_Human, pawn);
                float num;
                if (severity > 0f)
                    num = severity;
                else
                    num = hediffDef_Human.initialSeverity;
                if (divideByBodySize) num /= pawn.BodySize;
                hediff.Severity = num;
                pawn.health.AddHediff(hediff, null, null);
            }
            else
            {
                var hediff = HediffMaker.MakeHediff(hediffDef_Android, pawn);
                float num;
                if (severity > 0f)
                    num = severity;
                else
                    num = hediffDef_Android.initialSeverity;
                if (divideByBodySize) num /= pawn.BodySize;
                hediff.Severity = num;
                pawn.health.AddHediff(hediff, null, null);
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && chance >= 1f)
                foreach (var s in hediffDef_Human.SpecialDisplayStats(StatRequest.ForEmpty()))
                    yield return s;
        }
    }
}