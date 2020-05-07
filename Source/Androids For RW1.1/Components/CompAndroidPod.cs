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
                //Rafraichissement qt de courant consommé
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
            var ret = 0;

            var bed = (Building_Bed) parent;

            foreach (var cp in bed.CurOccupants)
                //Il sagit d'un android 
                if (cp != null && Utils.ExceptionAndroidList.Contains(cp.def.defName))
                    ret += Utils.getConsumedPowerByAndroid(cp.def.defName);
            return ret;
        }
    }
}