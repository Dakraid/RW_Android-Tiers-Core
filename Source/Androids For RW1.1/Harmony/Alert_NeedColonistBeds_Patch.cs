using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Alert_NeedColonistBeds_Patch
    {
        [HarmonyPatch(typeof(Alert_NeedColonistBeds), "NeedColonistBeds")]
        public class NeedColonistBeds_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Map map, ref bool __result)
            {
                try
                {
                    if (!map.IsPlayerHome) return false;
                    var num = 0;
                    var num2 = 0;
                    var allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
                    for (var i = 0; i < allBuildingsColonist.Count; i++)
                    {
                        var building_Bed = allBuildingsColonist[i] as Building_Bed;
                        if (building_Bed != null && !building_Bed.ForPrisoners && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
                        {
                            if (building_Bed.SleepingSlotsCount == 1)
                                num++;
                            else
                                num2++;
                        }
                    }

                    var num3 = 0;
                    var num4 = 0;
                    foreach (var current in map.mapPawns.FreeColonistsSpawned)
                    {
                        //On ignore les androis dans la comptabilisation
                        if (Utils.ExceptionAndroidList.Contains(current.def.defName))
                            continue;

                        var pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(current, false);
                        if (pawn == null || !pawn.Spawned || pawn.Map != current.Map || pawn.Faction != Faction.OfPlayer || pawn.HostFaction != null)
                            num3++;
                        else
                            num4++;
                    }

                    if (num4 % 2 != 0)
                    {
                        //Log.ErrorOnce("partneredCols % 2 != 0", 743211, false);
                    }

                    for (var j = 0; j < num4 / 2; j++)
                        if (num2 > 0)
                            num2--;
                        else
                            num -= 2;
                    for (var k = 0; k < num3; k++)
                        if (num2 > 0)
                            num2--;
                        else
                            num--;
                    __result = num < 0 || num2 < 0;

                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] ALert_NeedColonistBeds.NeedColonistBeds :" + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}