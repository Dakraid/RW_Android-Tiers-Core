using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000262 RID: 610
    public class CompProperties_HeatSensitive : CompProperties
    {
        public float hot1 = 20;
        public float hot2 = 30;
        public float hot3 = 35;

        public SoundDef hotSoundDef;

        // Token: 0x06000AD0 RID: 2768 RVA: 0x0005640A File Offset: 0x0005480A
        public CompProperties_HeatSensitive()
        {
            compClass = typeof(CompHeatSensitive);
        }
    }
}