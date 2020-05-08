using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_HeatSensitive : CompProperties
    {
        public float hot1 = 20;
        public float hot2 = 30;
        public float hot3 = 35;

        public SoundDef hotSoundDef;


        public CompProperties_HeatSensitive()
        {
            compClass = typeof(CompHeatSensitive);
        }
    }
}