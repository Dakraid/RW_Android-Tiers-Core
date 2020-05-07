using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ThoughtWorker_Greedy_Patch

    {
        [HarmonyPatch(typeof(ThoughtWorker_Greedy), "CurrentStateInternal")]
        public class CurrentStateInternal_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref ThoughtState __result)
            {
                if (Utils.ExceptionAndroidListBasic.Contains(p.def.defName)) __result = ThoughtState.Inactive;
            }
        }
    }
}