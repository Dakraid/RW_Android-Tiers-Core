using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Pawn_Patch
    {
        [HarmonyPatch(typeof(Pawn), "SetFaction")]
        public class SetFaction_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Faction newFaction, Pawn recruiter, Pawn __instance)
            {
                try
                {
                    if (__instance == null)
                        return;


                    var cas = __instance.TryGetComp<CompAndroidState>();
                    if (cas == null || !cas.isSurrogate || cas.externalController == null || newFaction == null || !newFaction.IsPlayer ||
                        Find.DesignatorManager.SelectedDesignator != null && Find.DesignatorManager.SelectedDesignator is Designator_SurrogateToHack) return;

                    if (cas.surrogateController != null)
                    {
                        Find.LetterStack.ReceiveLetter("ATPP_LetterTraitorOffline".Translate(), "ATPP_LetterTraitorOfflineDesc".Translate(__instance.LabelShortCap),
                            LetterDefOf.NegativeEvent);


                        if (cas.surrogateController.TryGetComp<CompSurrogateOwner>() != null)
                            cas.surrogateController.TryGetComp<CompSurrogateOwner>().disconnectControlledSurrogate(null);
                    }


                    cas.externalController = null;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn.SetFaction " + e.Message + " " + e.StackTrace);
                }
            }
        }


        [HarmonyPatch(typeof(Pawn), "Kill")]
        public class Kill
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
            {
                try
                {
                    if (__instance.IsSurrogateAndroid())
                    {
                        Utils.insideKillFuncSurrogate = true;


                        var csm = __instance.TryGetComp<CompSkyMind>();
                        if (csm != null)

                            csm.tempHackingEnding();
                    }


                    Utils.GCATPP.disconnectUser(__instance);


                    if (__instance.IsSurrogateAndroid(true))
                    {
                        var cas = __instance.TryGetComp<CompAndroidState>();
                        if (cas == null)
                            return true;


                        var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                        cso.stopControlledSurrogate(__instance, false, false, true);


                        cas.resetInternalState();
                    }


                    Utils.insideKillFuncSurrogate = false;
                    return true;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn.Kill(Error) : " + e.Message + " - " + e.StackTrace);

                    if (__instance.IsSurrogateAndroid())
                        Utils.insideKillFuncSurrogate = false;
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "PreKidnapped")]
        public class PreKidnapped_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn __instance, Pawn kidnapper)
            {
                try
                {
                    if (__instance.IsAndroidTier() || __instance.VXChipPresent() || __instance.IsSurrogateAndroid())

                        Utils.GCATPP.disconnectUser(__instance);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn.PreKidnapped(Error) : " + e.Message + " - " + e.StackTrace);
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "ButcherProducts")]
        public class ButcherProducts_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn butcher, float efficiency, Pawn __instance)
            {
                Utils.lastButcheredPawnIsAndroid = __instance.IsAndroidTier();
            }
        }

        [HarmonyPatch(typeof(Pawn), "GetGizmos")]
        public class GetGizmos_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                try
                {
                    var csm = __instance.TryGetComp<CompSkyMind>();


                    if (__instance.IsPrisoner || csm != null && csm.Hacked == 1)
                    {
                        IEnumerable<Gizmo> tmp;


                        if (__instance.VXChipPresent())
                        {
                            var cso = __instance.TryGetComp<CompSurrogateOwner>();
                            if (cso != null)
                            {
                                tmp = cso.CompGetGizmosExtra();
                                if (tmp != null)
                                    __result = __result.Concat(tmp);
                            }
                        }


                        if (__instance.IsAndroidTier())
                        {
                            var cas = __instance.TryGetComp<CompAndroidState>();

                            if (cas != null)
                            {
                                tmp = cas.CompGetGizmosExtra();
                                if (tmp != null)
                                    __result = __result.Concat(tmp);
                            }
                        }

                        if (csm != null && csm.Hacked == -1)
                        {
                            tmp = csm.CompGetGizmosExtra();
                            if (tmp != null)
                                __result = __result.Concat(tmp);
                        }
                    }


                    if (!__instance.IsPoweredAnimalAndroids()) return;

                    {
                        CompAndroidState cas = null;
                        cas = __instance.TryGetComp<CompAndroidState>();
                        if (cas == null) return;

                        var tmp = cas.CompGetGizmosExtra();
                        if (tmp != null)
                            __result = __result.Concat(tmp);
                    }
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] Pawn.GetGizmos " + e.Message + " " + e.StackTrace);
                }
            }
        }
    }
}