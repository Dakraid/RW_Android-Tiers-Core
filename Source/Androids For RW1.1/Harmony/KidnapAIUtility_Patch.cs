﻿using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    internal class KidnapAIUtility_Patch

    {
        [HarmonyPatch(typeof(KidnapAIUtility), "TryFindGoodKidnapVictim")]
        public class TryFindGoodKidnapVictim_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref bool __result, Pawn kidnapper, float maxDist, ref Pawn victim, List<Thing> disallowed = null)
            {
                if (!__result || !victim.IsSurrogateAndroid()) return;

                var csm = victim.TryGetComp<CompSkyMind>();

                if (csm == null || csm.hacked != 3 || csm.hackOrigFaction != kidnapper.Faction) return;

                if (!kidnapper.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) ||
                    !kidnapper.Map.reachability.CanReachMapEdge(kidnapper.Position, TraverseParms.For(kidnapper, Danger.Some)))
                {
                    victim = null;
                    __result = false;
                }

                bool Validator(Thing t)
                {
                    return t is Pawn pawn && pawn.RaceProps.Humanlike && pawn.Downed && pawn.Faction == Faction.OfPlayer &&
                           !(pawn.IsSurrogateAndroid() && pawn.TryGetComp<CompAndroidState>() != null && pawn.TryGetComp<CompSkyMind>().hacked == 3 &&
                             pawn.TryGetComp<CompSkyMind>().hackOrigFaction == kidnapper.Faction) && pawn.Faction.HostileTo(kidnapper.Faction) && kidnapper.CanReserve(pawn) &&
                           (disallowed == null || !disallowed.Contains(pawn));
                }

                victim = (Pawn) GenClosest.ClosestThingReachable(kidnapper.Position, kidnapper.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell,
                    TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some), maxDist, Validator);
                __result = victim != null;
            }
        }
    }
}