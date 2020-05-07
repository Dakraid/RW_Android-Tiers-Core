using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public static class DownedT5Utility
    {
        private const float RelationWithColonistWeight = 0.8f;

        private const float ChanceToRedressWorldPawn = 0f;

        public static Pawn GenerateT5(int tile)
        {
            var AndroidT5Colonist = PawnKindDefOf.AndroidT5Colonist;
            var ofplayer = Faction.OfAncients;
            var request = new PawnGenerationRequest(AndroidT5Colonist, ofplayer, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 20f, true, true,
                true, false, false, false, false, false, 0f, null, 0f);
            var pawn = PawnGenerator.GeneratePawn(request);
            HealthUtility.DamageUntilDowned(pawn);
            var hediff = HediffMaker.MakeHediff(HediffDefOf.RebootingSequenceAT, pawn);
            hediff.Severity = 1f;
            pawn.health.AddHediff(hediff, null, null);
            return pawn;
        }
    }
}