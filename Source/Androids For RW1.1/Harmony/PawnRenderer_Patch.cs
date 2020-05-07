using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnRenderer_Patch
    {
        [HarmonyPatch(typeof(PawnRenderer), "LayingFacing")]
        public class LayingFacing_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn ___pawn, ref Rot4 __result)
            {
                if (___pawn.def.defName == Utils.M7)
                    __result = Rot4.South;
            }
        }


        [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal")]
        [HarmonyPatch(new[] {typeof(Vector3), typeof(float), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool), typeof(bool)})]
        public class RenderPawnInternal_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref PawnRenderer __instance, Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType,
                bool portrait, bool headStump, bool invisible, Pawn ___pawn)
            {
                try
                {
                    var state = false;
                    CompAndroidState cas = null;
                    if (___pawn != null)
                        cas = ___pawn.TryGetComp<CompAndroidState>();

                    if (___pawn != null
                        && (___pawn.def.defName == Utils.TX2
                            || ___pawn.def.defName == Utils.TX2I
                            || ___pawn.def.defName == Utils.TX2KI
                            || ___pawn.def.defName == Utils.TX3I
                            || ___pawn.def.defName == Utils.TX4I
                            || ___pawn.def.defName == Utils.TX2K && (cas.TXHurtedHeadSet || cas.TXHurtedHeadSet2)
                            || ___pawn.def.defName == Utils.TX3 && (cas.TXHurtedHeadSet || cas.TXHurtedHeadSet2)
                            || ___pawn.def.defName == Utils.TX4 && (cas.TXHurtedHeadSet || cas.TXHurtedHeadSet2))
                        && !___pawn.Dead && !headStump)
                    {
                        if (!portrait)
                        {
                            var jobs = ___pawn.jobs;
                            if (jobs?.curDriver != null) state = !___pawn.jobs.curDriver.asleep;
                        }
                        else
                        {
                            state = true;
                        }
                    }

                    if (!state) return;

                    var quaternion = Quaternion.AngleAxis(angle, Vector3.up);
                    var a = rootLoc;
                    if (bodyFacing != Rot4.North)
                    {
                        a.y += 0.0281250011f;
                        rootLoc.y += 0.0234375f;
                    }
                    else
                    {
                        a.y += 0.0234375f;
                        rootLoc.y += 0.0281250011f;
                    }

                    var b = quaternion * __instance.BaseHeadOffsetAt(headFacing);
                    var loc = a + b + new Vector3(0f, 0.01f, 0f);
                    if (headFacing == Rot4.North) return;

                    var mesh = MeshPool.humanlikeHeadSet.MeshAt(headFacing);
                    var isHorizontal = headFacing.IsHorizontal;
                    var type = 1;
                    if (cas.TXHurtedHeadSet)
                        type = 2;
                    else if (cas.TXHurtedHeadSet2 || ___pawn.def.defName == Utils.TX2I || ___pawn.def.defName == Utils.TX2KI || ___pawn.def.defName == Utils.TX3I ||
                             ___pawn.def.defName == Utils.TX4I)
                        type = 3;

                    //Couleur yeux standard TX2
                    var color = new Color(0.9450f, 0.76862f, 0.05882f);


                    //Couleur yeux TX3/TX4 standard (bleu cyan)
                    if (___pawn.def.defName == Utils.TX3 || ___pawn.def.defName == Utils.TX4 || ___pawn.def.defName == Utils.TX3I || ___pawn.def.defName == Utils.TX4I)
                        color = new Color(0f, 0.972549f, 0.972549f);

                    // yeux rouges si drafté OU ennemis OU TX2K
                    if (___pawn.Drafted || ___pawn.Faction != null && ___pawn.Faction.HostileTo(Faction.OfPlayer) || ___pawn.def.defName == Utils.TX2K ||
                        ___pawn.def.defName == Utils.TX2KI)
                        color = new Color(0.75f, 0f, 0f, 1f);

                    var gender = "M";
                    if (___pawn.gender == Gender.Female)
                        gender = "F";

                    GenDraw.DrawMeshNowOrLater(mesh, loc, quaternion,
                        isHorizontal ? Tex.getEyeGlowEffect(color, gender, type, 0).MatSingle : Tex.getEyeGlowEffect(color, gender, type, 1).MatSingle, portrait);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] PawnRenderer.RenderPawnInternal " + e.Message + " " + e.StackTrace);
                }
            }
        }
    }
}