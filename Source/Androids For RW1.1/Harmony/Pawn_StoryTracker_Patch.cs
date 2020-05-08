using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class Pawn_StoryTracker_Patch
    {
        [HarmonyPatch(typeof(Pawn_StoryTracker), "get_SkinColor")]
        public class SkinColor_Patch
        {
            [HarmonyPostfix]
            public static void Listener(ref Color __result, Pawn ___pawn)
            {
                if (Utils.ExceptionAndroidWithoutSkinList.Contains(___pawn.def.defName))
                {
                    if (___pawn.IsSurrogateAndroid())

                        __result = Utils.SXColor;

                    var cas = ___pawn.TryGetComp<CompAndroidState>();

                    if (cas == null) return;

                    var pc = (AndroidPaintColor) cas.customColor;
                    if (Settings.androidsCanRust && cas.paintingIsRusted)
                        __result = Utils.androidCustomColorRust;
                    else if (pc != AndroidPaintColor.None && pc != AndroidPaintColor.Default)
                        switch (pc)
                        {
                            case AndroidPaintColor.Green:
                                __result = Utils.androidCustomColorGreen;
                                break;
                            case AndroidPaintColor.Black:
                                __result = Utils.androidCustomColorBlack;
                                break;
                            case AndroidPaintColor.Gray:
                                __result = Utils.androidCustomColorGray;
                                break;
                            case AndroidPaintColor.White:
                                __result = Utils.androidCustomColorWhite;
                                break;
                            case AndroidPaintColor.Blue:
                                __result = Utils.androidCustomColorBlue;
                                break;
                            case AndroidPaintColor.Cyan:
                                __result = Utils.androidCustomColorCyan;
                                break;
                            case AndroidPaintColor.Red:
                                __result = Utils.androidCustomColorRed;
                                break;
                            case AndroidPaintColor.Orange:
                                __result = Utils.androidCustomColorOrange;
                                break;
                            case AndroidPaintColor.Yellow:
                                __result = Utils.androidCustomColorYellow;
                                break;
                            case AndroidPaintColor.Purple:
                                __result = Utils.androidCustomColorPurple;
                                break;
                            case AndroidPaintColor.Pink:
                                __result = Utils.androidCustomColorPink;
                                break;
                            case AndroidPaintColor.Khaki:
                                __result = Utils.androidCustomColorKhaki;
                                break;
                            case AndroidPaintColor.None:
                                break;
                            case AndroidPaintColor.Default:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                }
            }
        }
    }
}