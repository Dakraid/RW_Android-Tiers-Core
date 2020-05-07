using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ThoughtUtility_Patch

    {
        [HarmonyPatch(typeof(ThoughtUtility), "GiveThoughtsForPawnExecuted")]
        public class GiveThoughtsForPawnExecuted_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn victim, PawnExecutionKind kind)
            {
                return !victim.IsBasicAndroidTier();
            }
        }

        [HarmonyPatch(typeof(ThoughtUtility), "GiveThoughtsForPawnOrganHarvested")]
        public class GiveThoughtsForPawnOrganHarvested_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn victim)
            {
                return !victim.IsBasicAndroidTier();
            }
        }


        [HarmonyPatch(typeof(ThoughtUtility), "CanGetThought")]
        public class CanGetThought_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ThoughtDef def, ref bool __result)
            {
                if (!pawn.IsBasicAndroidTier()) return;

                if (def == ThoughtDefOf.DeadMansApparel || def == ThoughtDefOf.HumanLeatherApparelSad)
                    __result = false;
                if (def == ThoughtDefOf.HumanLeatherApparelHappy)
                    __result = true;
            }
        }
    }
}