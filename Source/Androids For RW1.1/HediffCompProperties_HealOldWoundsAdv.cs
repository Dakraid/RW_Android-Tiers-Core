using Verse;

namespace MOARANDROIDS
{
    public class HediffCompProperties_RegenWoundsAdv : HediffCompProperties
    {
        public int Delay;

        public float HealingAmount;

        public HediffCompProperties_RegenWoundsAdv()
        {
            compClass = typeof(HediffComp_RegenWoundsAdv);
        }
    }
}