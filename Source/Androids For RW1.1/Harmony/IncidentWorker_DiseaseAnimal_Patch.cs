using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class IncidentWorker_DiseaseAnimal_Patch

    {
        /*
         * PostFix évitant de recevoir une notif de maladie sur des androides
         */
        [HarmonyPatch(typeof(IncidentWorker_DiseaseAnimal), "PotentialVictimCandidates")]
        public class PotentialVictims_Patch
        {
            [HarmonyPostfix]
            public static void Listener(IIncidentTarget target, ref IEnumerable<Pawn> __result)
            {
                if (__result == null)
                    return;

                var ret = __result.Where(el => Utils.ExceptionAndroidAnimals.Contains(el.def.defName)).ToList();

                __result = ret;
            }
        }
    }
}