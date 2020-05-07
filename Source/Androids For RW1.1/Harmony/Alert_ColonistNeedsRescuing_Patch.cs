using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Alert_ColonistNeedsRescuing_Patch
    {
        [HarmonyPatch(typeof(Alert_ColonistNeedsRescuing), "NeedsRescue")]
        public class ColonistsNeedingRescue_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref bool __result)
            {
                __result = __result && !p.IsSurrogateAndroid();
            }
        }
    }
}