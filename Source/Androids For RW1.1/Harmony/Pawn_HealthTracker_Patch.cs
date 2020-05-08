using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace MOARANDROIDS
{
    internal class Pawn_HealthTracker_Patch
    {
        [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff")]
        [HarmonyPatch(new[] {typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult)})]
        public class AddHediff_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref Pawn ___pawn, ref Hediff hediff, BodyPartRecord part)
            {
                try
                {
                    if (hediff.def.defName != "ATPP_HediffVX0Chip" || ___pawn.Faction != Faction.OfPlayer && !___pawn.IsPrisoner) return;

                    var cas = ___pawn.TryGetComp<CompAndroidState>();
                    if (cas == null || cas.isSurrogate)
                        return;

                    if (___pawn.Faction.IsPlayer && !Utils.preventVX0Thought)
                    {
                        PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(___pawn, null, PawnDiedOrDownedThoughtsKind.Died);

                        var spouse = ___pawn.GetSpouse();
                        if (spouse != null && !spouse.Dead && spouse.needs.mood != null)
                        {
                            var memories = spouse.needs.mood.thoughts.memories;
                            memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                            memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
                        }

                        Traverse.Create(___pawn.relations).Method("AffectBondedAnimalsOnMyDeath").GetValue();

                        ___pawn.health.NotifyPlayerOfKilled(null, null, null);
                    }
                    else
                    {
                        if (!___pawn.Faction.IsPlayer)
                            ___pawn.SetFaction(Faction.OfPlayer);
                    }

                    cas.initAsSurrogate();


                    ___pawn.skills = new Pawn_SkillTracker(___pawn);
                    ___pawn.needs = new Pawn_NeedsTracker(___pawn);


                    ___pawn.relations = new Pawn_RelationsTracker(___pawn);


                    var td = DefDatabase<TraitDef>.GetNamed("SimpleMindedAndroid", false);
                    Trait t = null;
                    if (td != null)
                        t = new Trait(td);

                    ___pawn.story.traits.allTraits.Clear();
                    if (t != null)
                        ___pawn.story.traits.allTraits.Add(t);
                    Utils.notifTraitsChanged(___pawn);

                    if (!___pawn.IsAndroidTier())
                    {
                        if (!Settings.keepPuppetBackstory && ___pawn.story.childhood != null)
                        {
                            BackstoryDatabase.TryGetWithIdentifier("MercenaryRecruit", out var bs);
                            if (bs != null)
                                ___pawn.story.childhood = bs;
                        }

                        ___pawn.story.adulthood = null;
                    }


                    Utils.ResetCachedIncapableOf(___pawn);


                    ___pawn.Name = new NameTriple("", "S" + 0 + "-" + Utils.GCATPP.getNextSXID(0), "");
                    Utils.GCATPP.incNextSXID(0);

                    if (!Utils.preventVX0Thought)

                        foreach (var current in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
                            current.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(Utils.thoughtDefVX0Puppet, 0));

                    if (___pawn.IsPrisoner) ___pawn.guest?.SetGuestStatus(Faction.OfPlayer);

                    if (___pawn.workSettings != null) return;

                    ___pawn.workSettings = new Pawn_WorkSettings(___pawn);
                    ___pawn.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] Pawn_HealthTracker.AddHediffPostfix " + ex.Message + " " + ex.StackTrace);
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff")]
        [HarmonyPatch(new[] {typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult)})]
        public class AddHediff_PatchPrefix
        {
            [HarmonyPrefix]
            public static bool Listener(ref Pawn ___pawn, ref Hediff hediff, BodyPartRecord part)
            {
                try
                {
                    if (___pawn.IsAndroidTier())
                    {
                        if (Utils.BlacklistAndroidHediff.Contains(hediff.def.defName))
                            return false;
                    }
                    else
                    {
                        if (part == null && Utils.ExceptionAndroidOnlyHediffs.Contains(hediff.def.defName)) return false;
                    }


                    if (!Utils.ExceptionNeuralChip.Contains(hediff.def.defName)) return true;

                    var cas = ___pawn.TryGetComp<CompAndroidState>();


                    if (cas != null && cas.isSurrogate)
                    {
                        IntVec3 pos1;
                        Map map1;
                        if (Utils.lastInstallImplantBillDoer != null && Utils.lastInstallImplantBillDoer.Map == ___pawn.Map)
                        {
                            pos1 = Utils.lastInstallImplantBillDoer.Position;
                            map1 = Utils.lastInstallImplantBillDoer.Map;
                        }
                        else
                        {
                            pos1 = ___pawn.Position;
                            map1 = ___pawn.Map;
                        }

                        GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos1, map1);
                        return false;
                    }

                    var he = ___pawn.HaveNotStackableVXChip();

                    if (he == null) return true;

                    ___pawn.health.RemoveHediff(he);

                    IntVec3 pos;
                    Map map;
                    if (Utils.lastInstallImplantBillDoer != null && Utils.lastInstallImplantBillDoer.Map == ___pawn.Map)
                    {
                        pos = Utils.lastInstallImplantBillDoer.Position;
                        map = Utils.lastInstallImplantBillDoer.Map;
                    }
                    else
                    {
                        pos = ___pawn.Position;
                        map = ___pawn.Map;
                    }

                    GenSpawn.Spawn(he.def.spawnThingOnRemoved, pos, map);

                    return true;
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] Pawn_HealthTracker.AddHediff " + ex.Message + " " + ex.StackTrace);
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_HealthTracker), "RemoveHediff")]
        public class RemoveHedff_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Hediff hediff, Pawn ___pawn)
            {
                if (hediff.def.defName != "ATPP_HediffVX0Chip") return;

                var cas = ___pawn.TryGetComp<CompAndroidState>();
                if (cas == null)
                    return;


                ___pawn.Kill(null);
            }
        }


        [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
        public class MakeDowned
        {
            [HarmonyPostfix]
            public static void Listener(Pawn ___pawn, DamageInfo? dinfo, Hediff hediff)
            {
                try
                {
                    if (!___pawn.IsSurrogateAndroid(true) || !___pawn.Faction.IsPlayer) return;

                    var cas = ___pawn.TryGetComp<CompAndroidState>();
                    if (cas == null)
                        return;


                    var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                    cso.stopControlledSurrogate(null);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn_HealthTracker.MakeDowned : " + e.Message + " - " + e.StackTrace);
                }
            }
        }


        [HarmonyPatch(typeof(Pawn_HealthTracker), "NotifyPlayerOfKilled")]
        public class NotifyPlayerOfKilled
        {
            [HarmonyPrefix]
            public static bool Listener(DamageInfo? dinfo, Hediff hediff, Caravan caravan, Pawn ___pawn)
            {
                try
                {
                    if (___pawn.IsBlankAndroid())
                        return false;


                    if (!___pawn.IsSurrogateAndroid() || ___pawn.IsPrisoner) return true;

                    Find.LetterStack.ReceiveLetter("ATPP_LetterSurrogateDisabled".Translate(___pawn.LabelShortCap),
                        "ATPP_LetterSurrogateDisabledDesc".Translate(___pawn.LabelShortCap), LetterDefOf.Death, ___pawn);
                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn_HealthTracker.NotifyPlayerOfKilled: " + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }
        }


        /*
         * On va faker pour PSYCHOLOGY le fait que les andorids possédes déjà un hediff anxiety fake
         */
        [HarmonyPatch(typeof(HediffSet), "GetFirstHediffOfDef")]
        public class GetFirstHediffOfDef
        {
            [HarmonyPostfix]
            public static void Listener(HediffSet __instance, ref Hediff __result, HediffDef def, bool mustBeVisible = false)
            {
                try
                {
                    if (!Utils.PSYCHOLOGY_LOADED)
                        return;


                    if (!__instance.pawn.IsAndroidTier() || !Utils.BlacklistedHediffsForAndroids.Contains(def.defName)) return;

                    var find = __instance.hediffs.FirstOrDefault(hediff => hediff.def == def && (!mustBeVisible || hediff.Visible)) ??
                               __instance.pawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_DummyHediff"));


                    __result = find;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] HediffSet.GetFirstHediffOfDef" + e.Message + " - " + e.StackTrace);
                }
            }
        }
    }
}