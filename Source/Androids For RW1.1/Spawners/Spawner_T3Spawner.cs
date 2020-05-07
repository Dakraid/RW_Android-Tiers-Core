using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompAndroidSpawner3T : ThingComp
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
                PawnKindDefOf.AndroidT3Colonist
            }.RandomElement();
            var request = new PawnGenerationRequest(pawnKindDef, Faction.OfPlayer);
            var pawn = PawnGenerator.GeneratePawn(request);

            //TODO: Implement, make work, test.
            //Pawn originalCloned = parent.TryGetComp<ThingyHolderThatsHoldingAClonedPawn>();
            //pawn.story = originalCloned.story;

            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
        }
    }
}