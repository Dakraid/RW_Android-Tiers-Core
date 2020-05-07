using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace MOARANDROIDS
{
    public class HediffComp_RegenWoundsAdv : HediffComp
    {
        [CompilerGenerated] private static Func<Hediff, bool> stuff;

        private int ticksToHeal;

        public HediffCompProperties_RegenWoundsAdv HealingProps => props as HediffCompProperties_RegenWoundsAdv;

        public override void CompPostMake()
        {
            base.CompPostMake();
            ResetTicksToHeal();
        }

        private void ResetTicksToHeal()
        {
            ticksToHeal = Rand.Range(HealingProps.Delay, HealingProps.Delay + 1) * 50;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            ticksToHeal--;
            if (ticksToHeal > 0) return;
            
            TryHealRandomWound();
            ResetTicksToHeal();
        }

        private void TryHealRandomWound()
        {
            IEnumerable<Hediff> hediffs = Pawn.health.hediffSet.hediffs;
            if (stuff == null) stuff = HediffUtility.IsTended;
            if (!hediffs.Where(stuff).TryRandomElement(out var hediff)) return;

            if (hediff.def != RimWorld.HediffDefOf.WoundInfection || hediff.def.makesSickThought) hediff.Severity -= HealingProps.HealingAmount;
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksToHeal, "ticksToHeal");
        }

        public override string CompDebugString()
        {
            return "ticksToHeal: " + ticksToHeal;
        }
    }
}