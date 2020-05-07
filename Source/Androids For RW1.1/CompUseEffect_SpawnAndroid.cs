using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000024 RID: 36
    public class CompUseEffect_SpawnAndroid : CompUseEffect
    {
        // Token: 0x1700000E RID: 14
        // (get) Token: 0x0600007D RID: 125 RVA: 0x000054BB File Offset: 0x000036BB
        public override float OrderPriority => 1000f;

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600007E RID: 126 RVA: 0x000054C4 File Offset: 0x000036C4
        public CompProperties_SpawnPawn SpawnerProps => props as CompProperties_SpawnPawn;

        // Token: 0x0600007F RID: 127 RVA: 0x000054E4 File Offset: 0x000036E4
        public virtual void DoSpawn(Pawn usedBy)
        {
            var pawn = PawnGenerator.GeneratePawn(SpawnerProps.pawnKind, Faction.OfPlayer);
            var flag = pawn != null;
            if (flag)
            {
                GenPlace.TryPlaceThing(pawn, parent.Position, parent.Map, ThingPlaceMode.Near);
                var sendMessage = SpawnerProps.sendMessage;
                if (sendMessage) Messages.Message("AndroidSpawnedt".Translate(pawn.Name.ToStringFull), MessageTypeDefOf.NeutralEvent);
            }
        }

        // Token: 0x06000080 RID: 128 RVA: 0x00005574 File Offset: 0x00003774
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            for (var i = 0; i < SpawnerProps.amount; i++) DoSpawn(usedBy);
        }

        // Token: 0x06000081 RID: 129 RVA: 0x000055AC File Offset: 0x000037AC
    }
}