using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    internal class HealthCardUtility_Patch
    {
        /*
         * Allow crafters to do doctor jobs (for androids)
         */
        [HarmonyPatch(typeof(HealthCardUtility), "DrawPawnHealthCard")]
        public class DrawPawnHealthCard_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
            {
                Utils.curSelPatientDrawMedOperationsTab = pawn;
                var cso = pawn.TryGetComp<CompSurrogateOwner>();
                if (cso != null && cso.skyCloudHost != null) return false;
                return true;
            }

            [HarmonyPostfix]
            public static void Postfix(Rect outRect, Pawn pawn, bool allowOperations, bool showBloodLoss, Thing thingForMedBills)
            {
                Utils.curSelPatientDrawMedOperationsTab = null;
            }
        }
    }
}