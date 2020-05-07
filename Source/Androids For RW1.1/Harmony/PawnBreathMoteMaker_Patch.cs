using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnBreathMoteMaker_Patch

    {
        [HarmonyPatch(typeof(PawnBreathMoteMaker), "TryMakeBreathMote")]
        public class TryMakeBreathMote_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn ___pawn)
            {
                return !___pawn.IsAndroidTier();
            }
        }
    }
}