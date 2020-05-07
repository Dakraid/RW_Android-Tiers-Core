using HarmonyLib;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class MentalState_Patch

    {
        /*
         * PostFix servant a desactivé les moods liés a la joie pour les T1 et T2
         */
        [HarmonyPatch(typeof(MentalState), "PostEnd")]
        public class PostEnd_Patch
        {
            [HarmonyPostfix]
            public static void Listener(MentalState __instance)
            {
                if (!__instance.pawn.IsSurrogateAndroid()) return;

                var csm = __instance.pawn.TryGetComp<CompSkyMind>();
                if (csm == null)
                    return;

                if (csm.Infected != 4) return;

                csm.Infected = -1;
                var he = __instance.pawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
                if (he == null) __instance.pawn.health.AddHediff(Utils.hediffNoHost);
            }
        }

        /*
         * PostFix servant a deconencté de ses activitées un mind ayant un mentalbreak et initié un timeout de fin de break
         */
        [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState")]
        public class TryStartMentalState_Patch
        {
            [HarmonyPostfix]
            public static void Listener(MentalStateDef stateDef, string reason, bool forceWake, bool causedByMood, Pawn otherPawn, bool transitionSilently, Pawn ___pawn,
                MentalStateHandler __instance, ref bool __result)
            {
                if (!__result || !___pawn.IsSurrogateAndroid()) return;

                var cas = ___pawn.TryGetComp<CompAndroidState>();
                var cso = cas?.surrogateController?.TryGetComp<CompSurrogateOwner>();
                var csc = cso?.skyCloudHost?.TryGetComp<CompSkyCloudCore>();

                //Ajout a une liste de minds boudant avec timeout
                csc?.setMentalBreak(cas.surrogateController);
            }
        }
    }
}