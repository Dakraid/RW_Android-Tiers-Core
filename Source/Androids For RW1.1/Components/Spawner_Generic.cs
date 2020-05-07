using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompAndroidSpawnerGeneric : ThingComp
    {
        public SpawnerCompProperties_GenericSpawner Spawnprops => props as SpawnerCompProperties_GenericSpawner;

        public override void CompTick()
        {
            CheckShouldSpawn();
        }

        private void CheckShouldSpawn()
        {
            if (true)
            {
                Utils.forceGeneratedAndroidToBeDefaultPainted = true;
                Utils.PawnInventoryGeneratorCanHackInvNutritionValue = false;
                SpawnDude();
                Utils.forceGeneratedAndroidToBeDefaultPainted = false;
                Utils.PawnInventoryGeneratorCanHackInvNutritionValue = true;

                parent.Destroy();
            }
        }

        public void SpawnDude()
        {
            Gender gender = default;
            if (Spawnprops.gender != -1)
            {
                if (Spawnprops.gender == 0)
                    gender = Gender.Male;
                else
                    gender = Gender.Female;
            }

            var request = new PawnGenerationRequest(Spawnprops.Pawnkind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, fixedGender: gender);
            var pawn = PawnGenerator.GeneratePawn(request);


            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
        }
    }
}