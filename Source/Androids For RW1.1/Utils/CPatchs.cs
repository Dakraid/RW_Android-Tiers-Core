﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    public static class CPaths
    {
        public static Need_DummyRest dummyRest;


        public static bool HospitalityPatchInsideFindBedFor;

        private static string TargetGSequencer;

        public static bool PrisonLabor_WorkTimePrefix(Pawn pawn, ref bool __result)
        {
            if (pawn == null || !pawn.IsAndroidTier()) return true;

            if (pawn.timetable == null)
            {
                __result = true;
            }
            else if (pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Work)
            {
                __result = true;
            }
            else if (pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Anything)
            {
                if (HealthAIUtility.ShouldSeekMedicalRest(pawn) ||
                    pawn.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious) ||
                    pawn.needs.food.CurCategory > HungerCategory.Hungry)
                    __result = false;
                else
                    __result = true;
            }
            else
            {
                __result = false;
            }

            return false;
        }

        public static bool PrisonLabor_GetChangePointsPrefix(ref bool __result, Pawn ___pawn)
        {
            if (___pawn.IsAndroidTier()) ___pawn.needs.rest = dummyRest;
            return true;
        }

        public static void PrisonLabor_GetChangePointsPostfix(ref bool __result, Pawn ___pawn)
        {
            if (___pawn.IsAndroidTier()) ___pawn.needs.rest = null;
        }


        public static void SaveOurShip2_hasSpaceSuit(Pawn thePawn, ref bool __result)
        {
            if (thePawn.IsAndroidTier()) __result = true;
        }

        public static void Hopistality_FindBedForPrefix(Pawn guest)
        {
            HospitalityPatchInsideFindBedFor = true;
        }

        public static void Hopistality_FindBedForPostfix(Pawn guest)
        {
            HospitalityPatchInsideFindBedFor = false;
        }


        public static void RimworldVanilla_PawnApparelGeneratorPossibleApparelSetPairOverlapsAnything(ThingStuffPair pair, ref bool __result, ThingDef ___raceDef,
            List<ThingStuffPair> ___aps)
        {
            try
            {
                if (pair.thing.defName != "VAE_Headgear_Scarf" && !Utils.ExceptionAndroidList.Contains(___raceDef.defName)) return;

                if (Enumerable.Any(___aps, el => el.thing.defName == "VAE_Headgear_Scarf"))
                    __result = true;
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] PawnApparelGeneratorPossibleApparelSetPairOverlapsAnything " + e.Message + " " + e.StackTrace);
            }
        }


        public static void PowerPP_CompLocalWirelessPowerEmitter_CompInspectStringExtra(ThingComp __instance, ref string __result)
        {
            if (!(__instance.parent is Building))
                return;

            var build = (Building) __instance.parent;

            var nbConn = 0;
            if (Utils.GCATPP.listerLWPNAndroid.ContainsKey(build))
                nbConn = Utils.GCATPP.listerLWPNAndroid[build].Count();

            if (__instance.parent.def.defName == "ARKPPP_LocalWirelessPortablePowerEmitter")
                __result = "ATPP_LWPNNbConnectedAndroid".Translate(nbConn) + "/" + Settings.maxAndroidByPortableLWPN + "\n" + __result;
            else
                __result = "ATPP_LWPNNbConnectedAndroid".Translate(nbConn) + "\n" + __result;
        }

        public static bool QEE_BuildingPawnVatGrower_TryMakeClonePrefix(Building __instance)
        {
            try
            {
                var BUID = __instance.GetUniqueLoadID();

                QEE_CheckInitData(__instance);

                var to = (ThingOwner) Traverse.Create(__instance).Field("innerContainer").GetValue();

                var genome = to.FirstOrDefault(thing => thing.GetType().Name == "GenomeSequence");
                if (genome != null && Utils.ExceptionQEEGS.Contains(genome.def.defName))
                {
                    var bodyType = BodyTypeDefOf.Female;
                    if (genome.def.defName == "ATPP_GS_TX2KMale" || genome.def.defName == "ATPP_GS_TX3Male" || genome.def.defName == "ATPP_GS_TX4Male")
                        bodyType = BodyTypeDefOf.Male;

                    var bas = Traverse.Create(genome);
                    bas.Field("hairColor").SetValue(Utils.getHairColor(Utils.GCATPP.QEEAndroidHairColor[BUID]));
                    bas.Field("skinColor").SetValue(Utils.getSkinColor(Utils.GCATPP.QEESkinColor[BUID]));
                    bas.Field("hair").SetValue(DefDatabase<HairDef>.GetNamed(Utils.GCATPP.QEEAndroidHair[BUID], false));
                    bas.Field("bodyType").SetValue(bodyType);
                }
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] QEE_BuildingPawnVatGrower_TryMakeClonePrefix " + e.Message + " " + e.StackTrace);
            }

            return true;
        }

        public static void QEE_BuildingPawnVatGrower_TryMakeClonePostfix(Building __instance, ref bool __result)
        {
            try
            {
                var BUID = __instance.GetUniqueLoadID();


                if (__result)
                {
                    var to = (ThingOwner) Traverse.Create(__instance).Field("innerContainer").GetValue();

                    Thing genome = null;

                    if (to != null)
                        genome = to.FirstOrDefault(thing => thing != null && thing.GetType().Name == "GenomeSequence");

                    if (genome != null && Utils.ExceptionQEEGS.Contains(genome.def.defName))
                    {
                        Utils.GCATPP.VatGrowerLastPawnInProgress[BUID] = (Pawn) Traverse.Create(__instance).Field("pawnBeingGrown").GetValue();
                        Utils.GCATPP.VatGrowerLastPawnIsTX[BUID] = true;
                    }
                    else
                    {
                        Utils.GCATPP.VatGrowerLastPawnInProgress[BUID] = null;
                        Utils.GCATPP.VatGrowerLastPawnIsTX[BUID] = false;
                    }
                }
                else
                {
                    Utils.GCATPP.VatGrowerLastPawnInProgress[BUID] = null;
                    Utils.GCATPP.VatGrowerLastPawnIsTX[BUID] = false;
                }
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] QEE_BuildingPawnVatGrower_TryMakeClonePostfix " + e.Message + " " + e.StackTrace);
            }
        }

        private static void QEE_CheckInitData(Building __instance)
        {
            var BUID = __instance.GetUniqueLoadID();

            if (!Utils.GCATPP.QEEAndroidHair.ContainsKey(BUID) || DefDatabase<HairDef>.GetNamed(Utils.GCATPP.QEEAndroidHair[BUID], false) == null)
                Utils.GCATPP.QEEAndroidHair[BUID] = DefDatabase<HairDef>.AllDefs.First().defName;
            if (!Utils.GCATPP.QEEAndroidHairColor.ContainsKey(BUID)) Utils.GCATPP.QEEAndroidHairColor[BUID] = "black";
            if (!Utils.GCATPP.QEESkinColor.ContainsKey(BUID)) Utils.GCATPP.QEESkinColor[BUID] = "fair";
        }

        public static void QEE_BuildingPawnVatGrower_GetGizmosPostfix(ref IEnumerable<Gizmo> __result, Building __instance)
        {
            try
            {
                var status = (int) Traverse.Create(__instance).Field("status").GetValue();

                if (status != 0)
                    return;

                var ret = new List<Gizmo>();


                var BUID = __instance.GetUniqueLoadID();

                QEE_CheckInitData(__instance);


                var ch = DefDatabase<HairDef>.GetNamed(Utils.GCATPP.QEEAndroidHair[BUID], false);
                var hairColor = Utils.getHairColor(Utils.GCATPP.QEEAndroidHairColor[BUID]);
                var baseMat = GraphicDatabase.Get<Graphic_Multi>(ch.texPath, ShaderDatabase.Cutout, Vector2.one, hairColor).MatAt(Rot4.South);

                Gizmo b = new Command_Action
                {
                    defaultLabel = ch.LabelCap,
                    defaultDesc = "",
                    icon = (Texture2D) baseMat.mainTexture,
                    action = delegate
                    {
                        var opts = DefDatabase<HairDef>.AllDefs.Select(h => new FloatMenuOption(h.LabelCap, delegate { Utils.GCATPP.QEEAndroidHair[BUID] = h.defName; })).ToList();
                        opts.SortBy(x => x.Label);

                        var floatMenuMap = new FloatMenu(opts, "");
                        Find.WindowStack.Add(floatMenuMap);
                    }
                };

                ret.Add(b);

                b = new Command_Action
                {
                    defaultLabel = "ATPP_HairColor".Translate() + " : " + ("ATPP_HairColor" + Utils.GCATPP.QEEAndroidHairColor[BUID].CapitalizeFirst()).Translate(),
                    defaultDesc = "",
                    icon = Tex.ColorPicker,
                    action = delegate
                    {
                        var opts = Utils.ExceptionHairColors.Select(h =>
                            new FloatMenuOption(("ATPP_HairColor" + h).Translate(), delegate { Utils.GCATPP.QEEAndroidHairColor[BUID] = h.ToLower(); })).ToList();

                        opts.SortBy(x => x.Label);

                        var floatMenuMap = new FloatMenu(opts, "");
                        Find.WindowStack.Add(floatMenuMap);
                    }
                };

                ret.Add(b);

                b = new Command_Action
                {
                    defaultLabel = "ATPP_SkinColor".Translate() + " : " + ("ATPP_SkinColor" + Utils.GCATPP.QEESkinColor[BUID].CapitalizeFirst()).Translate(),
                    defaultDesc = "",
                    icon = Tex.ColorPicker,
                    action = delegate
                    {
                        var opts = Utils.ExceptionSkinColors
                            .Select(h => new FloatMenuOption(("ATPP_SkinColor" + h).Translate(), delegate { Utils.GCATPP.QEESkinColor[BUID] = h.ToLower(); })).ToList();

                        opts.SortBy(x => x.Label);

                        var floatMenuMap = new FloatMenu(opts, "");
                        Find.WindowStack.Add(floatMenuMap);
                    }
                };

                ret.Add(b);

                if (__result != null)
                    __result = __result.Concat(ret);
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] QEE_BuildingPawnVatGrower_GetGizmosPostfix " + e.Message + " " + e.StackTrace);
            }
        }


        public static bool QEE_BUildingPawnVatGrower_TryExtractProductPrefix(Building __instance, Pawn actor)
        {
            var to = (ThingOwner) Traverse.Create(__instance).Field("innerContainer").GetValue();

            var genome = to.FirstOrDefault(thing => thing.GetType().Name == "GenomeSequence");
            TargetGSequencer = genome?.GetUniqueLoadID();

            return true;
        }

        public static void QEE_BUildingPawnVatGrower_TryExtractProductPostfix(Building __instance, Pawn actor, bool __result)
        {
            var BUID = __instance.GetUniqueLoadID();

            if (TargetGSequencer == null || !__result) return;

            var target = __instance.Map.listerThings.AllThings.FirstOrDefault(el => el != null && el.GetUniqueLoadID() == TargetGSequencer);


            if (target == null) return;

            var source = (string) Traverse.Create(target).Field("sourceName").GetValue();

            if (source.ToLower().Contains("(surrogate)"))
            {
                var cas = Utils.GCATPP.VatGrowerLastPawnInProgress[BUID].TryGetComp<CompAndroidState>();
                if (cas != null)
                {
                    Utils.initBodyAsSurrogate(Utils.GCATPP.VatGrowerLastPawnInProgress[BUID]);
                    cas.initAsSurrogate();
                    Utils.setSurrogateName(Utils.GCATPP.VatGrowerLastPawnInProgress[BUID]);
                }
            }

            target.Destroy();
        }

        public static void QEE_Building_GrowerBase_get_CraftingProgressPercentPostfix(Building __instance, ref float __result)
        {
            try
            {
                var BUID = __instance.GetUniqueLoadID();


                if (Utils.GCATPP.VatGrowerLastPawnIsTX.ContainsKey(BUID) && Utils.GCATPP.VatGrowerLastPawnIsTX[BUID] && !Utils.GCATPP.VatGrowerLastPawnInProgress.ContainsKey(BUID))
                    Utils.GCATPP.VatGrowerLastPawnInProgress[BUID] = (Pawn) Traverse.Create(__instance).Field("pawnBeingGrown").GetValue();


                if (!Utils.GCATPP.VatGrowerLastPawnInProgress.ContainsKey(BUID) || Utils.GCATPP.VatGrowerLastPawnInProgress[BUID] == null) return;

                var cas = Utils.GCATPP.VatGrowerLastPawnInProgress[BUID].TryGetComp<CompAndroidState>();
                if (cas != null)
                {
                    if (cas.forcedDamageLevel != 2 && __result <= 0.45f)
                    {
                        Traverse.Create(__instance).Field("renderTexture").SetValue(null);
                        cas.isAndroidWithSkin = true;
                        cas.forcedDamageLevel = 2;
                        cas.checkTXWithSkinFacialTextureUpdate();
                    }
                    else if (cas.forcedDamageLevel != 1 && __result > 0.45f && __result <= 0.85f)
                    {
                        Traverse.Create(__instance).Field("renderTexture").SetValue(null);
                        cas.isAndroidWithSkin = true;
                        cas.forcedDamageLevel = 1;
                        cas.checkTXWithSkinFacialTextureUpdate();
                    }
                    else if (cas.forcedDamageLevel != -2 && __result > 0.85f)
                    {
                        Traverse.Create(__instance).Field("renderTexture").SetValue(null);
                        cas.isAndroidWithSkin = true;
                        cas.forcedDamageLevel = -2;
                        cas.checkTXWithSkinFacialTextureUpdate();
                    }
                }

                __result = 1.0f;
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] QEE_Building_GrowerBase_get_CraftingProgressPercentPostfix " + e.Message + " " + e.StackTrace);
            }
        }
    }
}