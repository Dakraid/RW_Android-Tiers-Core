using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class ThinkNode_ConditionalMustKeepLyingDownM7Surrogate : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.CurJob == null || !pawn.GetPosture().Laying() || pawn.def.defName == "M7Mech" && !pawn.IsSurrogateAndroid()) return false;

            if (pawn.Downed) return true;
            if (RestUtility.DisturbancePreventsLyingDown(pawn)) return false;

            if (pawn.CurJob.restUntilHealed && HealthAIUtility.ShouldSeekMedicalRest(pawn)) return true;
            if (!pawn.jobs.curDriver.asleep) return false;

            return pawn.CurJob.playerForced || !RestUtility.TimetablePreventsLayDown(pawn);
        }
    }
}