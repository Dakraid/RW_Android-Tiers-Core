﻿using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompUseEffect_HealFrameworkSystem : CompUseEffect
    {
        public static void Apply(Pawn user)
        {
            var cas = user.TryGetComp<CompAndroidState>();
            if (cas == null) return;

            var CGT = Find.TickManager.TicksGame;
            cas.frameworkNaniteEffectGTStart = CGT;
            cas.frameworkNaniteEffectGTEnd = CGT + Rand.Range(Settings.minHoursNaniteFramework, Settings.maxHoursNaniteFramework) * 2500;
        }

        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);

            Apply(user);
        }

        public override bool CanBeUsedBy(Pawn p, out string failReason)
        {
            if (!Utils.ExceptionAndroidList.Contains(p.def.defName))
            {
                failReason = "ATPP_CanOnlyBeUsedByAndroid".Translate();
                return false;
            }

            var cas = p.TryGetComp<CompAndroidState>();
            if (cas == null || cas.frameworkNaniteEffectGTEnd == -1) return base.CanBeUsedBy(p, out failReason);

            failReason = "";
            return false;
        }
    }
}