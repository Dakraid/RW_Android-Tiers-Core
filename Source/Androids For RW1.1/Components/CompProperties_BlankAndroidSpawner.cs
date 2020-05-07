using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_BlankAndroidSpawner : CompProperties
    {
        public PawnKindDef Pawnkind;

        public CompProperties_BlankAndroidSpawner()
        {
            compClass = typeof(CompBlankAndroidSpawner);
        }
    }
}