using Verse;

//Extension method, in RimWorld namespace
namespace RimWorld
{
    //Make sure I bind to the Android fleshDef as early as possible AFTER they are loaded
    [StaticConstructorOnStartup]
    public static class PawnExt
    {
        public static readonly FleshTypeDef androidFlesh;

        public static readonly FleshTypeDef mechFlesh;

        static PawnExt()
        {
            androidFlesh = DefDatabase<FleshTypeDef>.GetNamed("Android");
            mechFlesh = DefDatabase<FleshTypeDef>.GetNamed("MechanisedInfantry");
        }

        public static bool IsAndroid(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == androidFlesh || pawn.RaceProps.FleshType == mechFlesh;
        }

        public static bool IsNotAndroid(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType != androidFlesh && pawn.RaceProps.FleshType != mechFlesh;
        }
    }
}