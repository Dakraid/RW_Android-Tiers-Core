﻿using HarmonyLib;
using Verse;

namespace MOARANDROIDS
{
    internal class ThingDef_Patch

    {
        /*
         * PostFix permettant Si androide surrogate on va virer la possibilité d'ajouter des VX chips
         */
        [HarmonyPatch(typeof(RecipeDef), "get_AvailableNow")]
        public class get_AvailableNow_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref bool __result, RecipeDef __instance)
            {
                switch (Utils.curSelPatientDrawMedOperationsTab)
                {
                    case null:
                        return;
                    case Pawn _:
                    {
                        var cas = Utils.curSelPatientDrawMedOperationsTab.TryGetComp<CompAndroidState>();

                        if (cas == null) return;

                        if (cas.isSurrogate)
                        {
                            if (Utils.ExceptionVXNeuralChipSurgery.Contains(__instance.defName))
                                __result = false;
                        }
                        else
                        {
                            if (Utils.ExceptionArtificialBrainsSurgery.Contains(__instance.defName))
                                __result = false;
                        }

                        break;
                    }
                }
            }
        }
    }
}