using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public static class Toils_LayDownPower
    {
        private const int TicksBetweenSleepZs = 100;

        public const float GroundRestEffectiveness = 0.8f;

        private const int GetUpOrStartJobWhileInBedCheckInterval = 211;

        public static Toil LayDown(TargetIndex bedOrRestSpotIndex, bool hasBed, bool lookForOtherJobs, bool canSleep = true, bool gainRestAndHealth = true)
        {
            var layDown = new Toil();
            layDown.initAction = delegate
            {
                var actor = layDown.actor;
                actor.pather.StopDead();
                var curDriver = actor.jobs.curDriver;
                if (hasBed)
                {
                    var t = (Building_Bed) actor.CurJob.GetTarget(bedOrRestSpotIndex).Thing;
                    if (!t.OccupiedRect().Contains(actor.Position))
                    {
                        Log.Error("Can't start LayDown toil because pawn is not in the bed. pawn=" + actor);
                        actor.jobs.EndCurrentJob(JobCondition.Errored);
                        return;
                    }

                    actor.jobs.posture = PawnPosture.LayingInBed;
                }
                else
                {
                    actor.jobs.posture = PawnPosture.LayingOnGroundNormal;
                }

                curDriver.asleep = false;
                if (actor.mindState.applyBedThoughtsTick == 0)
                {
                    actor.mindState.applyBedThoughtsTick = Find.TickManager.TicksGame + Rand.Range(2500, 10000);
                    actor.mindState.applyBedThoughtsOnLeave = false;
                }

                if (actor.ownership != null && actor.CurrentBed() != actor.ownership.OwnedBed) ThoughtUtility.RemovePositiveBedroomThoughts(actor);
            };

            layDown.tickAction = delegate
            {
                var actor = layDown.actor;
                var curJob = actor.CurJob;
                var curDriver = actor.jobs.curDriver;
                var building_Bed = (Building_Bed) curJob.GetTarget(bedOrRestSpotIndex).Thing;
                actor.GainComfortFromCellIfPossible();

                if (actor.IsHashIntervalTick(100) && !actor.Position.Fogged(actor.Map))
                {
                    if (curDriver.asleep) MoteMaker.ThrowMetaIcon(actor.Position, actor.Map, ThingDefOf.Mote_SleepZ);
                    if (gainRestAndHealth && actor.health.hediffSet.GetNaturallyHealingInjuredParts().Any())
                        MoteMaker.ThrowMetaIcon(actor.Position, actor.Map, ThingDefOf.Mote_HealingCross);
                }

                if (actor.ownership != null && building_Bed != null && !building_Bed.Medical && !building_Bed.OwnersForReading.Contains(actor))
                {
                    if (actor.Downed) actor.Position = CellFinder.RandomClosewalkCellNear(actor.Position, actor.Map, 1);
                    actor.jobs.EndCurrentJob(JobCondition.Incompletable);
                    return;
                }

                if (lookForOtherJobs && actor.IsHashIntervalTick(211))
                {
                    actor.jobs.CheckForJobOverride();
                    return;
                }


                if (building_Bed != null && (actor.needs.food.CurLevelPercentage >= 1.0f
                                             || building_Bed.Destroyed || building_Bed.IsBrokenDown()
                                             || !building_Bed.TryGetComp<CompPowerTrader>().PowerOn))
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded);
            };
            layDown.defaultCompleteMode = ToilCompleteMode.Never;
            if (hasBed) layDown.FailOnBedNoLongerUsable(bedOrRestSpotIndex);
            layDown.AddFinishAction(delegate
            {
                var actor = layDown.actor;
                var curDriver = actor.jobs.curDriver;
                curDriver.asleep = false;
            });
            return layDown;
        }
    }
}