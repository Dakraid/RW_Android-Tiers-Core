using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class InspirationWorker_Patch

    {
        /*
         * PostFix évitant au surrogate d'avoir des inspirations
         */
        [HarmonyPatch(typeof(InspirationWorker), "InspirationCanOccur")]
        public class InspirationCanOccur_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, ref bool __result)
            {
                if (pawn.IsSurrogateAndroid())
                    __result = false;
            }
        }
    }
}