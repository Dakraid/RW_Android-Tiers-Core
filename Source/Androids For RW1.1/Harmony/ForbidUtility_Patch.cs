using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class ForbidUtility_Patch

    {
        /*
         * PostFix évitant l'erreur déclenché par les androides quand ils soint forcés à manger de l'herbe/buisson...
         */
        [HarmonyPatch(typeof(ForbidUtility), "SetForbidden")]
        public class SetForbidden_Patch
        {
            [HarmonyPrefix]
            public static bool Listener(Thing t, bool value, ref bool warnOnFail)
            {
                try
                {
                    if (Current.ProgramState != ProgramState.Playing)
                        return true;

                    var obj = Find.Selector.SelectedObjects;

                    if (obj == null || obj.Count != 1 || !(obj[0] is Pawn)) return true;

                    var pawn = (Pawn) obj[0];

                    if (pawn.IsAndroidTier()) warnOnFail = false;

                    return true;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] ForbidUtility.SetForbidden " + e.Message + " " + e.StackTrace);
                    return true;
                }
            }
        }

        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(ForbidUtility), "IsForbidden")]
        [HarmonyPatch(new[] {typeof(Thing), typeof(Pawn)})]
        public class IsForbidden_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Thing t, Pawn pawn, ref bool __result)
            {
                try
                {
                    if (!Utils.HOSPITALITY_LOADED || !CPaths.HospitalityPatchInsideFindBedFor) return;

                    if (pawn.IsAndroidTier())
                        __result = !Utils.ExceptionSurrogatePodGuest.Contains(t.def.defName);
                    else
                        __result = Utils.ExceptionSurrogatePodGuest.Contains(t.def.defName);

                    //Log.Message(">>"+pawn.LabelShort+" "+t.def.defName+" "+__result);
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] ReservationUtility.CanReserveAndReach " + e.Message + " " + e.StackTrace);
                }
            }
        }
    }
}