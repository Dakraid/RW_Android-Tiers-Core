using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    [DefOf]
    public static class BodyPartTagDefOf
    {
        public static BodyPartTagDef CPSource;

        public static BodyPartTagDef HVSource;

        public static BodyPartTagDef EVKidney;

        public static BodyPartTagDef EVLiver;

        static BodyPartTagDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartTagDefOf));
        }
    }
}