using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnGroupMakerUtility_Patch

    {
        [HarmonyPatch(typeof(PawnGroupMakerUtility), "GeneratePawns")]
        public class GeneratePawns_Patch
        {
            [HarmonyPostfix]
            public static void Listener(PawnGroupMakerParms parms, bool warnOnZeroResults, ref IEnumerable<Pawn> __result)
            {
                try
                {
                    if (!Settings.otherFactionsCanUseSurrogate || Utils.ExceptionBlacklistedFactionNoSurrogate.Contains(parms.faction.def.defName) ||
                        Utils.getRandomMapOfPlayer().gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare) || Settings.androidsAreRare && Rand.Chance(0.95f))
                        return;

                    IEnumerable<Pawn> pawn = __result as Pawn[] ?? __result.ToArray();
                    var nbHumanoids = pawn.Where(e => e.def.race != null && e.def.race.Humanlike).Count(e => e.trader == null && e.TraderKind == null);


                    /*if(e.def.defName == Utils.T1 || e.def.defName == Utils.T2)
                        {
                            Utils.removeAllTraits(e);
                        }*/


                    if (parms.faction.def.techLevel < TechLevel.Industrial || nbHumanoids < 5) return;

                    {
                        var ret = new List<Pawn>();
                        var tmp = pawn.ToList();


                        var other = tmp.Where(x => x.def.race == null || !x.def.race.Humanlike || x.trader != null || x.TraderKind != null).ToList();


                        foreach (var e in other) tmp.Remove(e);


                        var nb = (int) (tmp.Count() * Rand.Range(Settings.percentageOfSurrogateInAnotherFactionGroupMin, Settings.percentageOfSurrogateInAnotherFactionGroup));
                        if (nb <= 0)
                        {
                            if (Settings.percentageOfSurrogateInAnotherFactionGroupMin == 0.0f)
                                return;

                            nb = 1;
                        }


                        while (tmp.Count > nb)
                        {
                            var p = tmp.RandomElement();
                            other.Add(p);
                            tmp.Remove(p);
                        }


                        for (var i = 0; i != nb; i++)
                        {
                            PawnKindDef rpkd = null;

                            if (parms.groupKind == PawnGroupKindDefOf.Peaceful || parms.groupKind == PawnGroupKindDefOf.Trader)
                            {
                                if (Rand.Chance(0.10f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDNeutral[3];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDNeutral[3];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDNeutral[3];
                                }
                                else if (Rand.Chance(0.35f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDNeutral[2];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDNeutral[2];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDNeutral[2];
                                }
                                else if (Rand.Chance(0.55f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsPKDNeutral[1];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDNeutral[1];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDNeutral[1];
                                }
                                else
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDNeutral[0];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDNeutral[0];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDNeutral[0];
                                }
                            }
                            else
                            {
                                if (Rand.Chance(0.10f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDHostile[3];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDHostile[3];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDHostile[3];
                                }
                                else if (Rand.Chance(0.35f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDHostile[2];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDHostile[2];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDHostile[2];
                                }
                                else if (Rand.Chance(0.55f))
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsPKDHostile[1];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDHostile[1];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDHostile[1];
                                }
                                else
                                {
                                    if (!Utils.TXSERIE_LOADED || Rand.Chance(0.5f))
                                        rpkd = Utils.AndroidsPKDHostile[0];
                                    else if (Rand.Chance(0.75f))
                                        rpkd = Utils.AndroidsXISeriePKDHostile[0];
                                    else
                                        rpkd = Utils.AndroidsXSeriePKDHostile[0];
                                }
                            }


                            var surrogate = Utils.generateSurrogate(parms.faction, rpkd, IntVec3.Invalid, null, false, true, parms.tile, true, parms.inhabitants);


                            if (!surrogate.IsAndroidTier())
                            {
                                surrogate.Destroy();
                                ret.Add(tmp[i]);
                                continue;
                            }


                            if (surrogate.inventory != null && surrogate.inventory.innerContainer != null)
                                surrogate.inventory.innerContainer.Clear();

                            surrogate.apparel.DestroyAll();

                            surrogate.equipment.DestroyAllEquipment();


                            var cas = surrogate.TryGetComp<CompAndroidState>();
                            if (cas != null)
                            {
                                cas.externalController = tmp[i];
                                var cso = tmp[i].TryGetComp<CompSurrogateOwner>();
                                cso?.setControlledSurrogate(surrogate, true);
                            }


                            if (tmp[i].equipment != null && surrogate.equipment != null)
                            {
                                /*Pawn_EquipmentTracker pet = tmp[i].equipment;
                                pet.pawn = surrogate;
                                tmp[i].equipment = surrogate.equipment;
                                tmp[i].equipment.pawn = tmp[i];
                                surrogate.equipment = pet;*/
                                surrogate.equipment.DestroyAllEquipment();

                                foreach (var e in tmp[i].equipment.AllEquipmentListForReading.ToList())
                                    try
                                    {
                                        tmp[i].equipment.Remove(e);
                                        if (!(e.def.equipmentType == EquipmentType.Primary && surrogate.equipment.Primary != null))
                                            surrogate.equipment.AddEquipment(e);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Message("[ATPP] PawnGroupMakerUtility.GeneratePawns.transfertEquipment " + ex.Message + " " + ex.StackTrace);
                                    }


                                foreach (var e in surrogate.equipment.AllEquipmentListForReading)
                                    try
                                    {
                                        if (!Utils.CELOADED || e == null || !e.def.IsRangedWeapon) continue;

                                        var ammoUser = Utils.TryGetCompByTypeName(e, "CompAmmoUser", "CombatExtended");
                                        if (ammoUser == null) continue;

                                        var props = Traverse.Create(ammoUser).Property("Props").GetValue();
                                        var magazineSize = Traverse.Create(props).Field("magazineSize").GetValue<int>();
                                        var def = Traverse.Create(ammoUser).Field("selectedAmmo").GetValue<ThingDef>();
                                        if (def != null) Traverse.Create(ammoUser).Method("ResetAmmoCount", def).GetValue();
                                    }
                                    catch (Exception)
                                    {
                                    }
                            }

                            /*foreach(var e in tmp[i].equipment.AllEquipmentListForReading.ToList())
                            {
                                e.
                                surrogate.equipment.AddEquipment(e);
                            }*/


                            if (tmp[i].apparel != null)
                                try
                                {
                                    foreach (var e in tmp[i].apparel.WornApparel.ToList())
                                    {
                                        var path = "";
                                        if (e.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
                                            path = e.def.apparel.wornGraphicPath;
                                        else
                                            path = e.def.apparel.wornGraphicPath + "_" + surrogate.story.bodyType.defName + "_south";

                                        Texture2D appFoundTex = null;

                                        for (var j = LoadedModManager.RunningModsListForReading.Count - 1; j >= 0; j--)
                                        {
                                            var appTex = LoadedModManager.RunningModsListForReading[j].GetContentHolder<Texture2D>().Get(path);
                                            if (appTex == null) continue;

                                            appFoundTex = appTex;
                                            break;
                                        }


                                        if (appFoundTex == null)
                                        {
                                            path = GenFilePaths.ContentPath<Texture2D>() + path;
                                            appFoundTex = Resources.Load<Texture2D>(path);
                                        }


                                        if (appFoundTex == null) continue;

                                        tmp[i].apparel.Remove(e);
                                        surrogate.apparel.Wear(e);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Message("[ATPP] PawnGroupMakerUtility.TransfertApparel " + ex.Message + " " + ex.StackTrace);
                                }


                            if (tmp[i].inventory != null && tmp[i].inventory.innerContainer != null && surrogate.inventory != null && surrogate.inventory.innerContainer != null)


                                try
                                {
                                    tmp[i].inventory.innerContainer.TryTransferAllToContainer(surrogate.inventory.innerContainer);


                                    foreach (var el in surrogate.inventory.innerContainer.ToList().Where(el => el.def.IsDrug))
                                        surrogate.inventory.innerContainer.Remove(el);
                                }
                                catch (Exception ex)
                                {
                                    Log.Message("[ATPP] PawnGroupMakerUtility.GeneratePawns.transfertInventory " + ex.Message + " " + ex.StackTrace);
                                }


                            ret.Add(surrogate);
                        }


                        __result = other.Concat(ret);
                    }
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] PawnGroupMakerUtility.GeneratePawns " + ex.Message + " " + ex.StackTrace);
                }
            }
        }
    }
}