using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ThoughtWorker_PsychicEmanatorSoothe_Patch

    {
        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(ThoughtWorker_PsychicEmanatorSoothe), "CurrentStateInternal")]
        public class CurrentStateInternal_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref ThoughtState __result)
            {
                if (p.IsBasicAndroidTier() || Utils.pawnCurrentlyControlRemoteSurrogate(p)) __result = ThoughtState.Inactive;
            }
        }
    }
}