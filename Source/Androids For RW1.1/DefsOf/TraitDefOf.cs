using RimWorld;

namespace MOARANDROIDS
{
    [DefOf]
    public static class TraitDefOf
    {
        public static TraitDef FeelingsTowardHumanity;

        public static TraitDef Transhumanist;

        static TraitDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TraitDefOf));
        }
    }
}