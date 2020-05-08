using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnGenerator_Patch

    {
        [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn")]
        [HarmonyPatch(new[] {typeof(PawnGenerationRequest)}, new[] {ArgumentType.Normal})]
        public class GeneratePawn_Patch
        {
            [HarmonyPostfix]
            public static void Listener(PawnGenerationRequest request, ref Pawn __result)
            {
                try
                {
                    var isAndroidTier = __result.IsAndroidTier();


                    if (!(request.Context == PawnGenerationContext.PlayerStarter && Utils.ExceptionPlayerStartingAndroidPawnKindList.Contains(request.KindDef.defName))
                    )
                        if (Settings.androidsAreRare
                            && __result.IsAndroidTier()
                            && (Current.ProgramState == ProgramState.Entry || Current.ProgramState == ProgramState.Playing && request.Faction != Faction.OfPlayer)
                            && Rand.Chance(0.95f))
                        {
                            var r = new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, request.Faction, request.Context, request.Tile, request.ForceGenerateNewPawn,
                                request.Newborn,
                                request.AllowDead, request.AllowDowned, request.CanGeneratePawnRelations, request.MustBeCapableOfViolence, request.ColonistRelationChanceFactor,
                                request.ForceAddFreeWarmLayerIfNeeded, request.AllowGay, request.AllowFood, request.AllowAddictions, request.Inhabitant,
                                request.CertainlyBeenInCryptosleep,
                                request.ForceRedressWorldPawnIfFormerColonist, request.WorldPawnFactionDoesntMatter, request.BiocodeWeaponChance,
                                request.ExtraPawnForExtraRelationChance,
                                request.RelationWithExtraPawnChanceFactor, request.ValidatorPreGear, request.ValidatorPostGear, request.ForcedTraits, request.ProhibitedTraits,
                                request.MinChanceToRedressWorldPawn, request.FixedBiologicalAge, request.FixedChronologicalAge, request.FixedGender, request.FixedMelanin,
                                request.FixedLastName, request.FixedBirthName, request.FixedTitle);

                            __result = PawnGenerator.GeneratePawn(r);
                        }


                    if (isAndroidTier)
                    {
                        if (__result.gender == Gender.Male)
                        {
                            var bd = DefDatabase<BodyTypeDef>.GetNamed("Male", false);
                            if (bd != null)
                                __result.story.bodyType = bd;
                        }
                        else
                        {
                            var bd = DefDatabase<BodyTypeDef>.GetNamed("Female", false);
                            if (bd != null)
                                __result.story.bodyType = bd;
                        }


                        var isAndroidWithSkin = Utils.ExceptionAndroidWithSkinList.Contains(__result.def.defName);

                        if (isAndroidWithSkin)
                        {
                            Utils.changeHARCrownType(__result, "Average_Normal");

                            if (Utils.RIMMSQOL_LOADED && Utils.lastResolveAllGraphicsHeadGraphicPath != null)
                            {
                                __result.story.GetType().GetField("headGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance)
                                    .SetValue(__result.story, Utils.lastResolveAllGraphicsHeadGraphicPath);
                                Utils.lastResolveAllGraphicsHeadGraphicPath = null;
                            }
                        }

                        Utils.removeMindBlacklistedTrait(__result);

                        if (!isAndroidWithSkin && Rand.Chance(Settings.chanceGeneratedAndroidCanBePaintedOrRust))
                        {
                            var cas = __result.TryGetComp<CompAndroidState>();
                            if (cas != null)
                            {
                                if (Utils.forceGeneratedAndroidToBeDefaultPainted)
                                {
                                    cas.paintingIsRusted = false;
                                    cas.paintingRustGT = Rand.Range(Settings.minDaysAndroidPaintingCanRust, Settings.maxDaysAndroidPaintingCanRust) * 60000;
                                    cas.customColor = (int) AndroidPaintColor.Default;
                                }
                                else
                                {
                                    if (Settings.androidsCanRust && Rand.Chance(0.35f))
                                        cas.setRusted();
                                    else
                                        cas.customColor = Rand.Range((int) AndroidPaintColor.Black, (int) AndroidPaintColor.Khaki + 1);
                                }
                            }
                        }
                    }


                    if (Settings.preventM7T5AppearingInCharacterScreen && Current.ProgramState == ProgramState.Entry)
                        if (__result.def.defName == Utils.M7 || __result.def.defName == Utils.T5)
                        {
                            var r = new PawnGenerationRequest(Utils.AndroidsPKDNeutral.RandomElement(), request.Faction, request.Context, request.Tile,
                                request.ForceGenerateNewPawn, request.Newborn,
                                request.AllowDead, request.AllowDowned, request.CanGeneratePawnRelations, request.MustBeCapableOfViolence, request.ColonistRelationChanceFactor,
                                request.ForceAddFreeWarmLayerIfNeeded, request.AllowGay, request.AllowFood, request.AllowAddictions, request.Inhabitant,
                                request.CertainlyBeenInCryptosleep,
                                request.ForceRedressWorldPawnIfFormerColonist, request.WorldPawnFactionDoesntMatter, request.BiocodeWeaponChance,
                                request.ExtraPawnForExtraRelationChance,
                                request.RelationWithExtraPawnChanceFactor, request.ValidatorPreGear, request.ValidatorPostGear, request.ForcedTraits, request.ProhibitedTraits,
                                request.MinChanceToRedressWorldPawn, request.FixedBiologicalAge, request.FixedChronologicalAge, request.FixedGender, request.FixedMelanin,
                                request.FixedLastName, request.FixedBirthName, request.FixedTitle);

                            __result = PawnGenerator.GeneratePawn(r);
                        }

                    if (!Settings.notRemoveAllSkillPassionsForBasicAndroids)

                        if (__result.IsBasicAndroidTier() && __result.def.defName != "M7Mech" && __result.skills != null && __result.skills.skills != null)
                            foreach (var sr in __result.skills.skills)
                                sr.passion = Passion.None;
                    if (!Settings.notRemoveAllTraitsFromT1T2)

                        if (__result.IsBasicAndroidTier() && __result.def.defName != "M7Mech")
                            Utils.removeAllTraits(__result);


                    if (Utils.ANDROIDTIERSGYNOID_LOADED
                        && isAndroidTier
                        && (__result.def.defName == Utils.T1 || __result.def.defName == Utils.T2 || __result.def.defName == Utils.T3 || __result.def.defName == Utils.T4)
                        && Current.ProgramState == ProgramState.Playing && __result.Faction == Faction.OfPlayer)
                    {
                        if (Rand.Chance(Settings.percentageChanceMaleAndroidModel))
                        {
                            __result.gender = Gender.Male;
                            __result.story.bodyType = BodyTypeDefOf.Male;
                        }
                        else
                        {
                            __result.gender = Gender.Female;
                            __result.story.bodyType = BodyTypeDefOf.Female;
                        }
                    }


                    if (Current.ProgramState == ProgramState.Playing && !Settings.basicAndroidsRandomSKills && __result.Faction == Faction.OfPlayer)
                    {
                        SkillRecord sr = null;

                        switch (__result.def.defName)
                        {
                            case Utils.T1:
                            {
                                sr = __result.skills.GetSkill(SkillDefOf.Animals);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Animals;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Artistic);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Artistic;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Construction);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Construction;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Cooking);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Cooking;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Crafting);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Crafting;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Intellectual);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Intellectual;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Medicine);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Medical;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Melee);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Melee;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Mining);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Mining;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Plants);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Plants;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Shooting);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Shoot;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Social);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT1Social;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                break;
                            }
                            case Utils.T2:
                            {
                                sr = __result.skills.GetSkill(SkillDefOf.Animals);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Animals;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Artistic);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Artistic;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Construction);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Construction;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Cooking);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Cooking;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Crafting);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Crafting;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Intellectual);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Intellectual;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Medicine);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Medical;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Melee);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Melee;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Mining);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Mining;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Plants);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Plants;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Shooting);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Shoot;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                sr = __result.skills.GetSkill(SkillDefOf.Social);
                                if (sr != null)
                                {
                                    sr.levelInt = Settings.defaultSkillT2Social;
                                    sr.xpSinceLastLevel = 0;
                                    sr.xpSinceMidnight = 0;
                                }

                                break;
                            }
                        }
                    }


                    if (__result.def.defName != Utils.TX3 && __result.def.defName != Utils.TX4) return;

                    Utils.changeHARCrownType(__result, "Average_Normal");

                    __result.Drawer.renderer.graphics.ResolveAllGraphics();
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] PawnGenerator.GeneratePawn " + ex.Message + " " + ex.StackTrace);
                }
            }
        }
    }
}