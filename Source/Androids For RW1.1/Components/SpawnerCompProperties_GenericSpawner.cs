using Verse;

namespace MOARANDROIDS
{
    public class SpawnerCompProperties_GenericSpawner : CompProperties
    {
        public int gender;

        public PawnKindDef Pawnkind;

        public SpawnerCompProperties_GenericSpawner()
        {
            compClass = typeof(CompAndroidSpawnerGeneric);
        }
    }
}