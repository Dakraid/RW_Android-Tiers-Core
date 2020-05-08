using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class Building_ReloadStation : Building
    {
        private FloatMenuOption GetFailureReason(Pawn myPawn)
        {
            if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some)) return new FloatMenuOption("CannotUseNoPath".Translate(), null);
            if (Spawned && Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare)) return new FloatMenuOption("CannotUseSolarFlare".Translate(), null);
            if (!this.TryGetComp<CompPowerTrader>().PowerOn) return new FloatMenuOption("CannotUseNoPower".Translate(), null);
            if (!Utils.ExceptionAndroidList.Contains(myPawn.def.defName)) return new FloatMenuOption("ATPP_CanOnlyBeUsedByAndroid".Translate(), null);

            var ca = myPawn.TryGetComp<CompAndroidState>();
            if (ca == null || !ca.UseBattery)
                return new FloatMenuOption("ATPP_CannotUseBecauseNotInBatteryMode".Translate(), null);


            var rs = this.TryGetComp<CompReloadStation>();
            var nb = rs.getNbAndroidReloading(true);

            return nb >= 8 ? new FloatMenuOption("ATPP_CannotUseEveryPlaceUsed".Translate(), null) : null;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            var failureReason = GetFailureReason(myPawn);
            if (failureReason != null)
                yield return failureReason;
            else
                yield return new FloatMenuOption("ATPP_ForceReload".Translate(), delegate
                {
                    var rs = this.TryGetComp<CompReloadStation>();

                    var job = new Job(DefDatabase<JobDef>.GetNamed("ATPP_GoReloadBattery"), new LocalTargetInfo(rs.getFreeReloadPlacePos(myPawn)), new LocalTargetInfo(this));
                    myPawn.jobs.TryTakeOrderedJob(job);
                });
        }
    }
}