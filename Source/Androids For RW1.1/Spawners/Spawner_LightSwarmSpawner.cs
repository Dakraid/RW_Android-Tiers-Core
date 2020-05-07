using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompAndroidSpawnerLightSwarm : ThingComp
    {
        public override void CompTick()
        {
            CheckShouldSpawn();
        }

        private void CheckShouldSpawn()
        {
            SpawnDude();
            parent.Destroy();
        }

        public void SpawnDude()
        {
            var pawnKindDef = new List<PawnKindDef>
            {
                PawnKindDefOf.MicroScyther
            }.RandomElement();
            var request = new PawnGenerationRequest(pawnKindDef, Faction.OfPlayer);
            var pawn = PawnGenerator.GeneratePawn(request);
            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
            pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterNotColony, null, true);

            var hediff = HediffMaker.MakeHediff(HediffDefOf.BatteryChargeMech, pawn);
            hediff.Severity = 0.5f;
            pawn.health.AddHediff(hediff, null, null);
        }
    }
}