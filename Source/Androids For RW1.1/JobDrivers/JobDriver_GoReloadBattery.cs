using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class JobDriver_GoReloadBattery : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Downed)
                return false;

            pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (TargetThingA is Building_Bed)
            {
                yield return Toils_Bed.GotoBed(TargetIndex.A);

                yield return Toils_LayDownPower.LayDown(TargetIndex.A, true, false, false);
            }
            else
            {
                var gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
                var nothing = new Toil();
                yield return gotoCell;

                var setSkin = new Toil {initAction = delegate { pawn.Rotation = Rot4.South; }};
                yield return setSkin;
                yield return nothing;
                yield return Toils_General.Wait(50);
                yield return Toils_Jump.JumpIf(nothing, () => pawn.needs.food.CurLevelPercentage < 1.0f
                                                              && !job.targetB.ThingDestroyed && !((Building) job.targetB).IsBrokenDown()
                                                              && ((Building) job.targetB).TryGetComp<CompPowerTrader>().PowerOn);
            }
        }
    }
}