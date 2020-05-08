using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal static class FlickUtility_Patch
    {
        [HarmonyPatch(typeof(FlickUtility), "UpdateFlickDesignation")]
        public class UpdateFlickDesignation
        {
            [HarmonyPrefix]
            public static bool UpdateFlickDesignation_Prefix(Thing t)
            {
                try
                {
                    var CGT = Find.TickManager.TicksGame;
                    var csm = t.TryGetComp<CompSkyMind>();
                    if (csm == null)
                        return true;


                    if (csm.lastRemoteFlickGT == CGT)
                        return false;

                    string txt;


                    if (!Utils.GCATPP.isThereSkyCloudCore()) return true;
                    if (!csm.connected)
                        return true;

                    var cf = t.TryGetComp<CompFlickable>();
                    if (cf == null) return false;

                    if (cf.SwitchIsOn)
                    {
                        txt = "ATPP_FlickDisable".Translate();
                        Utils.playVocal("soundDefSkyCloudDeviceDeactivated");
                    }
                    else
                    {
                        txt = "ATPP_FlickEnable".Translate();
                        Utils.playVocal("soundDefSkyCloudDeviceActivated");
                    }

                    MoteMaker.ThrowText(t.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), t.Map, txt, Color.white);

                    cf.DoFlick();
                    csm.lastRemoteFlickGT = CGT;

                    return false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] FlickUtility.UpdateFlickDesignation " + e.Message + " " + e.StackTrace);
                    return true;
                }
            }
        }
    }
}