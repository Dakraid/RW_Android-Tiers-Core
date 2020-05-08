using Verse;

namespace MOARANDROIDS
{
    public class Verb_MechFall : Verb
    {
        private const int DurationTicks = 450;


        protected override bool TryCastShot()
        {
            if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map) return false;

            var mechfall = (MechFall) GenSpawn.Spawn(ThingDefOf.MechFallBeam, currentTarget.Cell, caster.Map);
            mechfall.duration = 450;
            mechfall.instigator = caster;
            mechfall.weaponDef = EquipmentSource?.def;
            mechfall.StartStrike();
            if (EquipmentSource != null && !EquipmentSource.Destroyed) EquipmentSource.Destroy();
            return true;
        }


        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = false;
            return 2f;
        }
    }
}