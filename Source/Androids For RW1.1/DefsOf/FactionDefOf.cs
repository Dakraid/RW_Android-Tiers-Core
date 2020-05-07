using RimWorld;

namespace MOARANDROIDS
{
    [DefOf]
    public static class FactionDefOf
    {
        public static FactionDef AndroidFriendliesAtlas;

        public static FactionDef PlayerColonyAndroid;

        static FactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
        }
    }
}