using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal static class Building_Patch
    {
        [HarmonyPatch(typeof(Building), "ClaimableBy")]
        public class ClaimableBy
        {
            [HarmonyPostfix]
            public static void Replacement(Building __instance, ref bool __result, Faction by)
            {
                //Added check if building is virused or hacked => no claim possible
                if (__instance.TryGetComp<CompSkyMind>() != null && __instance.TryGetComp<CompSkyMind>().Infected != -1) __result = false;
            }
        }

        [HarmonyPatch(typeof(Building), "GetGizmos")]
        public class GetGizmos_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Building __instance, ref IEnumerable<Gizmo> __result)
            {
                //Si PawnVatGrower alors ajout des bouttons de selection de TXI a placer
                if (__instance.def.defName == "Building_PawnVatGrower")
                {
                }
            }
        }
    }
}