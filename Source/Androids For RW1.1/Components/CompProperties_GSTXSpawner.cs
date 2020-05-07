using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_GSTXSpawner : CompProperties
    {
        public string GSThing;

        public PawnKindDef Pawnkind;
        public int surrogate = 0;

        public CompProperties_GSTXSpawner()
        {
            compClass = typeof(CompGSTXSpawner);
        }
    }
}