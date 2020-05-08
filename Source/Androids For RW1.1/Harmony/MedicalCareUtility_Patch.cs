using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MOARANDROIDS
{
    internal class MedicalCareUtility_Patch
    {
        private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
        {
            return pawn.playerSettings.medCare;
        }

        private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
        {
            for (var i = 0; i < 5; i++)
            {
                var mc = (MedicalCareCategory) i;
                yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
                {
                    option = new FloatMenuOption(mc.GetLabel(), delegate { p.playerSettings.medCare = mc; }),
                    payload = mc
                };
            }
        }

        /*
         * Permet de savoir dans AllowsMedicine si le patient est un android
         */
        [HarmonyPatch(typeof(MedicalCareUtility), "AllowsMedicine")]
        public class AllowsMedicine_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(MedicalCareCategory cat, ThingDef meds, ref bool __result)
            {
                if (Utils.FindBestMedicinePatient == null || !Utils.SMARTMEDICINE_LOADED)
                    return;


                if (Utils.FindBestMedicinePatient.IsAndroidTier() || Utils.FindBestMedicinePatient.IsCyberAnimal())
                {
                    if (!Utils.ExceptionNanoKits.Contains(meds.defName))
                        __result = false;
                }
                else
                {
                    if (Utils.ExceptionNanoKits.Contains(meds.defName))
                        __result = false;
                }
            }
        }


        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(MedicalCareUtility), "MedicalCareSelectButton")]
        public class MedicalCareSelectButton_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Rect rect, Pawn pawn)
            {
                try
                {
                    if (!Utils.ExceptionAndroidList.Contains(pawn.def.defName) && !pawn.IsCyberAnimal()) return true;

                    Func<Pawn, MedicalCareCategory> getPayload = MedicalCareSelectButton_GetMedicalCare;
                    var menuGenerator = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareSelectButton_GenerateMenu);
                    Texture2D buttonIcon;
                    var index = (int) pawn.playerSettings.medCare;
                    switch (index)
                    {
                        case 0:
                            buttonIcon = Tex.NoCare;
                            break;
                        case 1:
                            buttonIcon = Tex.NoMed;
                            break;
                        case 2:
                            buttonIcon = Tex.NanoKitBasic;
                            break;
                        case 3:
                            buttonIcon = Tex.NanoKitIntermediate;
                            break;
                        default:
                            buttonIcon = Tex.NanoKitAdvanced;
                            break;
                    }

                    Widgets.Dropdown(rect, pawn, getPayload, menuGenerator, null, buttonIcon, null, null, null, true);
                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] MedicalCareUtility.MedicalCareSelectButton : " + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }

            private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
            {
                return pawn.playerSettings.medCare;
            }

            private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
            {
                for (var i = 0; i < 5; i++)
                {
                    var mc = (MedicalCareCategory) i;
                    yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
                    {
                        option = new FloatMenuOption(mc.GetLabel(), delegate { p.playerSettings.medCare = mc; }),
                        payload = mc
                    };
                }
            }
        }

        [HarmonyPatch(typeof(MedicalCareUtility), "MedicalCareSetter")]
        public class MedicalCareSetter_Patch
        {
            private static bool medicalCarePainting;


            [HarmonyPrefix]
            public static bool Listener(Rect rect, ref MedicalCareCategory medCare)
            {
                var obj = Find.Selector.SelectedObjects;
                try
                {
                    if (obj.Count != 1 || !(obj[0] is Pawn)) return true;

                    var pawn = (Pawn) obj[0];

                    if (!Utils.ExceptionAndroidList.Contains(pawn.def.defName) && !pawn.IsCyberAnimal()) return true;

                    var rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
                    for (var i = 0; i < 5; i++)
                    {
                        var mc = (MedicalCareCategory) i;
                        Widgets.DrawHighlightIfMouseover(rect2);
                        Texture2D tex;
                        string text;

                        switch (i)
                        {
                            case 0:
                                tex = Tex.NoCare;
                                text = "ATPP_NanoKitsNoCare".Translate();
                                break;
                            case 1:
                                tex = Tex.NoMed;
                                text = "ATPP_NanoKitsNoKitJustVisit".Translate();
                                break;
                            case 2:
                                tex = Tex.NanoKitBasic;
                                text = "ATPP_NanoKitsBasic".Translate();
                                break;
                            case 3:
                                tex = Tex.NanoKitIntermediate;
                                text = "ATPP_NanoKitsIntermediate".Translate();
                                break;
                            default:
                                tex = Tex.NanoKitAdvanced;
                                text = "ATPP_NanoKitsAdvanced".Translate();
                                break;
                        }

                        GUI.DrawTexture(rect2, tex);
                        var draggableResult = Widgets.ButtonInvisibleDraggable(rect2);
                        if (draggableResult == Widgets.DraggableResult.Dragged) medicalCarePainting = true;
                        if (medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc || draggableResult.AnyPressed())
                        {
                            medCare = mc;
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                        }

                        if (medCare == mc) Widgets.DrawBox(rect2, 3);
                        TooltipHandler.TipRegion(rect2, () => text, 632165 + i * 17);
                        rect2.x += rect2.width;
                    }

                    if (!Input.GetMouseButton(0)) medicalCarePainting = false;

                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] MedicalCareUtility.MedicalCareSetter(Error) : " + e.Message + " - " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}