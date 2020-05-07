using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnDiedOrDownedThoughtsUtility_Patch
    {
        /*
         * PreFix évitant debuff en cas de mort d'un T1 ou T2
         */
        [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike")]
        public class AppendThoughts_ForHumanlike_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(ref Pawn victim)
            {
                //Si android avec peu de logique alors tous le monde se fout de sa mort OU si Surrogate
                return !Utils.ExceptionAndroidListBasic.Contains(victim.def.defName) && !victim.IsSurrogateAndroid() && !victim.IsBlankAndroid();
            }
        }
    }
}