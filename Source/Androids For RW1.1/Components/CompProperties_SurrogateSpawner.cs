using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_SurrogateSpawner : CompProperties
    {
        public int gender;

        public PawnKindDef Pawnkind;

        public CompProperties_SurrogateSpawner()
        {
            compClass = typeof(CompSurrogateSpawner);
        }
    }
}