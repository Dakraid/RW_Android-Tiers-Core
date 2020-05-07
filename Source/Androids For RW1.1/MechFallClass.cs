using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x0200065F RID: 1631
    public class MechFall : OrbitalStrike
    {
        // Token: 0x0600216E RID: 8558 RVA: 0x000FB115 File Offset: 0x000F9515
        public override void StartStrike()
        {
            base.StartStrike();
            MechFallMoteMaker.MakeMechFallMote(Position, Map);
        }

        // Token: 0x0600216F RID: 8559 RVA: 0x000FB130 File Offset: 0x000F9530
        public override void Tick()
        {
            if (TicksPassed >= duration) SpawnDude();
            if (TicksPassed == duration - 5) CreateExplosion();
            if (TicksPassed == duration - 15) CreateExplosion();
            if (TicksPassed == duration - 160) CreatePod();
            base.Tick();
            if (Destroyed) return;
        }

        private void CreateExplosion()
        {
            var instigator = this.instigator;
            var def = this.def;
            var weaponDef = this.weaponDef;
            GenExplosion.DoExplosion(Position, Map, 1, DamageDefOf.Bomb, instigator, -1, -1f, null, weaponDef, def);
            GenExplosion.DoExplosion(Position, Map, 1, DamageDefOf.Bomb, instigator, -1, -1f, null, weaponDef, def);
        }

        private void CreatePod()
        {
            var info = new ActiveDropPodInfo
            {
                openDelay = 10,
                leaveSlag = true
            };
            DropPodUtility.MakeDropPodAt(Position, Map, info);
        }

        public void SpawnDude()
        {
            var request = new PawnGenerationRequest(PawnKindDefOf.M7MechPawn, Faction.OfPlayer);
            var pawn = PawnGenerator.GeneratePawn(request);
            FilthMaker.TryMakeFilth(Position, Map, RimWorld.ThingDefOf.Filth_RubbleBuilding, 30);

            GenSpawn.Spawn(pawn, Position, Map);
        }
    }
}