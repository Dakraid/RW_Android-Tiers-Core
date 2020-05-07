﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Corpse_Patch
    {
        /*
         * PostFix servant à annuler les moods du butchering d'un android de Android Tiers
         */
        [HarmonyPatch(typeof(Corpse), "ButcherProducts")]
        public class ButcherProducts_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn butcher, float efficiency, Corpse __instance, ref IEnumerable<Thing> __result)
            {
                Utils.lastButcheredPawnIsAndroid = false;

                //Si Surrogate T4 butcherisé alors on supprime le IA-Core des produits 
                if (__instance.InnerPawn == null || __instance.InnerPawn.def.defName != Utils.T4 || __instance.InnerPawn.TryGetComp<CompAndroidState>() == null ||
                    __result == null) return;

                var cas = __instance.InnerPawn.TryGetComp<CompAndroidState>();
                if (!cas.isSurrogate) return;

                var res = __result.ToList().Where(r => r.def != null && r.def.defName != "AIPersonaCore").ToList();
                __result = res;
            }
        }


        /*
         * PostFix servant à annuler les moods de voir le corp d'un android de Android Tiers mort ou pourrie
         */
        [HarmonyPatch(typeof(Corpse), "GiveObservedThought")]
        public class GiveObservedThought_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Corpse __instance, ref Thought_Memory __result)
            {
                if (__instance.InnerPawn.RaceProps.Humanlike
                    && Utils.ExceptionAndroidList.Contains(__instance.InnerPawn.def.defName))
                    __result = null;
            }
        }

        /*
         * PostFix servant à annuler la possibilité de manger des androids
         */
        [HarmonyPatch(typeof(Corpse), "get_IngestibleNow")]
        public class IngestibleNow_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Corpse __instance, ref bool __result)
            {
                if (__instance.InnerPawn.RaceProps.Humanlike
                    && Utils.ExceptionAndroidList.Contains(__instance.InnerPawn.def.defName)
                    || Utils.ExceptionAndroidAnimals.Contains(__instance.InnerPawn.def.defName))
                    __result = false;
            }
        }
    }
}