using Verse;

namespace MOARANDROIDS
{
    // Token: 0x0200093B RID: 2363
    public class Verb_MechFall : Verb
    {
        // Token: 0x0400210D RID: 8461
        private const int DurationTicks = 450;

        // Token: 0x06003290 RID: 12944 RVA: 0x0017A8E0 File Offset: 0x00178CE0
        protected override bool TryCastShot()
        {
            if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map) return false;
            var mechfall = (MechFall) GenSpawn.Spawn(ThingDefOf.MechFallBeam, currentTarget.Cell, caster.Map);
            mechfall.duration = 450;
            mechfall.instigator = caster;
            mechfall.weaponDef = EquipmentSource == null ? null : EquipmentSource.def;
            mechfall.StartStrike();
            if (EquipmentSource != null && !EquipmentSource.Destroyed) EquipmentSource.Destroy();
            return true;
        }

        // Token: 0x06003291 RID: 12945 RVA: 0x0017A9AC File Offset: 0x00178DAC
        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = false;
            return 2f;
        }
    }
}