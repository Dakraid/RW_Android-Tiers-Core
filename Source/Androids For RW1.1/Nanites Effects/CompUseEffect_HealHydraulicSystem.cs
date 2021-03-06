﻿using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompUseEffect_HealHydraulicSystem : CompUseEffect
    {
        public static void Apply(Pawn user)
        {
            var nb = 0;
            var chance = false;

            if (!Rand.Chance(Settings.percentageNanitesFail))
            {
                nb = user.health.hediffSet.hediffs.RemoveAll(h => Utils.AndroidOldAgeHediffHydraulic.Contains(h.def.defName));
                if (nb > 0) Utils.refreshHediff(user);
                chance = true;
            }

            if (nb == 0)
                Messages.Message(chance ? "ATPP_NoBrokenStuffFound".Translate(user.LabelShort) : "ATPP_BrokenStuffRepairFailed".Translate(user.LabelShort), user,
                    MessageTypeDefOf.NegativeEvent);
            else
                Messages.Message("ATPP_BrokenHydraulicSystemRepaired".Translate(user.LabelShort), user, MessageTypeDefOf.PositiveEvent);
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

            if (p.haveAndroidOldAgeHediff(Utils.AndroidOldAgeHediffHydraulic)) return base.CanBeUsedBy(p, out failReason);

            failReason = "ATPP_CannotBeUsedBecauseNoOldAgeIssues".Translate();
            return false;
        }
    }
}