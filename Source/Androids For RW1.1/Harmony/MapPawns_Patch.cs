using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal static class MapPawns_Patch
    {
        [HarmonyPatch(typeof(MapPawns), "get_AnyPawnBlockingMapRemoval")]
        public class MapPawns_get_AnyPawnBlockingMapRemoval
        {
            [HarmonyPostfix]
            public static void Listener(MapPawns __instance, ref bool __result, List<Pawn> ___pawnsSpawned)
            {
                if (__result) return;

                if (!(from pawn in ___pawnsSpawned
                    where pawn != null
                    let cas = pawn.TryGetComp<CompAndroidState>()
                    where !pawn.Dead && pawn.Faction != null && pawn.Faction.IsPlayer && cas != null && cas.isSurrogate && cas.externalController == null
                    select pawn).Any()) return;

                __result = true;
            }
        }

        /*
         * Prefix permetant de jerter en fonction de la config les surrogates des listings
         */
        [HarmonyPatch(typeof(MapPawns), "get_FreeColonists")]
        public class get_FreeColonists_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(ref List<Pawn> __result, MapPawns __instance, Dictionary<Faction, List<Pawn>> ___freeHumanlikesOfFactionResult)
            {
                try
                {
                    if (!Settings.hideInactiveSurrogates)
                        return true;

                    if (!___freeHumanlikesOfFactionResult.TryGetValue(Faction.OfPlayer, out var list))
                    {
                        list = new List<Pawn>();
                        ___freeHumanlikesOfFactionResult.Add(Faction.OfPlayer, list);
                    }

                    list.Clear();
                    var allPawns = __instance.AllPawns;
                    foreach (var pawn in allPawns)
                        if (pawn.Faction == Faction.OfPlayer && pawn.HostFaction == null && pawn.RaceProps.Humanlike &&
                            !pawn.IsSurrogateAndroid(false, true))
                            list.Add(pawn);

                    __result = list;

                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] MapPawns.get_FreeColonists " + e.Message + " " + e.StackTrace);

                    return true;
                }
            }
        }
    }
}