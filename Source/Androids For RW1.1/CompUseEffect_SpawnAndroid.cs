using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompUseEffect_SpawnAndroid : CompUseEffect
    {
        public override float OrderPriority => 1000f;


        public CompProperties_SpawnPawn SpawnerProps => props as CompProperties_SpawnPawn;


        public virtual void DoSpawn(Pawn usedBy)
        {
            var pawn = PawnGenerator.GeneratePawn(SpawnerProps.pawnKind, Faction.OfPlayer);
            var flag = pawn != null;
            if (!flag) return;

            GenPlace.TryPlaceThing(pawn, parent.Position, parent.Map, ThingPlaceMode.Near);
            var sendMessage = SpawnerProps.sendMessage;
            if (sendMessage) Messages.Message("AndroidSpawnedt".Translate(pawn.Name.ToStringFull), MessageTypeDefOf.NeutralEvent);
        }


        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            for (var i = 0; i < SpawnerProps.amount; i++) DoSpawn(usedBy);
        }
    }
}