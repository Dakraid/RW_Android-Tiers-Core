using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    // Token: 0x0200002A RID: 42
    public class JobGiver_TargetEnemiesSwarm : ThinkNode_JobGiver
    {
        private const float WaitChance = 0f;

        private const int WaitTicks = 0;

        private const int MinMeleeChaseTicks = 999999;

        private const int MaxMeleeChaseTicks = 999999;

        private const int WanderOutsideDoorRegions = 9;

        // Token: 0x06000082 RID: 130 RVA: 0x00004484 File Offset: 0x00002684
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
                if (flag4)
                {
                    result = MeleeAttackJob(pawn, pawn2);
                }
                else
                {
                    var flag5 = pawn2 != null;
                    var flag6 = flag5;
                    if (flag6)
                        using (var pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.None, TraverseMode.PassDoors)))
                        {
                            var flag7 = !pawnPath.Found;
                            var flag8 = flag7;
                            if (flag8) return null;
                            var flag9 = !pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out var intVec);
                            var flag10 = flag9;
                            if (flag10)
                            {
                                Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
                                return null;
                            }
                        }

                    result = null;
                }
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