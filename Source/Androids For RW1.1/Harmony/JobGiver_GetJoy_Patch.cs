using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

// TODO: Look into performance issues
namespace MOARANDROIDS
{
    internal class JobGiver_GetJoy_Patch

    {
        /*
         * ON Force les T1 et T2 a NE PAS FAIRE d'activité de joie
         */
        [HarmonyPatch(typeof(JobGiver_GetJoy), "TryGiveJob")]
        public class GetPriority_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ref Job __result)
            {
                if (pawn.IsBasicAndroidTier()) __result = null;
            }
        }
    }
}