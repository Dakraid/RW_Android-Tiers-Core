using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompAndroidPod : ThingComp
    {
        public override void CompTick()
        {
            if (!parent.Spawned) return;

            var CGT = Find.TickManager.TicksGame;
            if (CGT % 60 == 0)

                refreshPowerConsumed();
        }

        public float getPowerConsumed()
        {
            var cpt = parent.TryGetComp<CompPowerTrader>();

            if (cpt == null)
                return 0;

            return getCurrentAndroidPowerConsumed() + cpt.Props.basePowerConsumption;
        }


        public void refreshPowerConsumed()
        {
            parent.TryGetComp<CompPowerTrader>().powerOutputInt = -getPowerConsumed();
        }

        public int getCurrentAndroidPowerConsumed()
        {
            var bed = (Building_Bed) parent;

            return bed.CurOccupants.Where(cp => cp != null && Utils.ExceptionAndroidList.Contains(cp.def.defName)).Sum(cp => Utils.getConsumedPowerByAndroid(cp.def.defName));
        }
    }
}