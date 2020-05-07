using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class TimeinbedformedicalreasonsAndroid : RecordWorker
    {
        public override bool ShouldMeasureTimeNow(Pawn pawn)
        {
            return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || HealthAIUtility.ShouldSeekMedicalRest(pawn) || pawn.CurJob.restUntilHealed);
        }
    }
}