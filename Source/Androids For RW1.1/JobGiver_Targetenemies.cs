using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class JobGiver_TargetEnemiesSwarm : ThinkNode_JobGiver
    {
        private const float WaitChance = 0f;

        private const int WaitTicks = 0;

        private const int MinMeleeChaseTicks = 999999;

        private const int MaxMeleeChaseTicks = 999999;

        private const int WanderOutsideDoorRegions = 9;


        protected override Job TryGiveJob(Pawn pawn)
        {
            var flag = pawn.TryGetAttackVerb(null) == null;
            var flag2 = flag;
            Job result;
            if (flag2)
            {
                result = null;
            }
            else
            {
                var pawn2 = FindPawnTarget(pawn);
                var flag3 = pawn2 != null;
                var flag4 = flag3;
                result = flag4 ? MeleeAttackJob(pawn, pawn2) : null;
            }

            return result;
        }

        private Job MeleeAttackJob(Pawn pawn, Thing target)
        {
            return new Job(JobDefOf.AttackMelee, target)
            {
                maxNumMeleeAttacks = 999,
                expiryInterval = 999999,
                attackDoorIfTargetLost = true
            };
        }

        private Pawn FindPawnTarget(Pawn pawn)
        {
            return (Pawn) AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, x => x is Pawn, 0f, 100f, default, float.MaxValue, true);
        }
    }
}