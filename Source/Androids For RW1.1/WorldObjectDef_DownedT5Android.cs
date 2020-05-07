using RimWorld;
using RimWorld.Planet;

namespace MOARANDROIDS
{
    public class WorldObjectCompProperties_DownedT5Android : WorldObjectCompProperties
    {
        public WorldObjectCompProperties_DownedT5Android()
        {
            compClass = typeof(DownedT5AndroidComp);
            compClass = typeof(TimedForcedExit);
        }
    }
}