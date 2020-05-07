using System.Collections.Generic;
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

                var ret = new List<Pawn>();

                foreach (var el in __result)
                    if (Utils.ExceptionAndroidAnimals.Contains(el.def.defName))
                        ret.Add(el);

                __result = ret;
            }
        }
    }
}