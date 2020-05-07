using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class SickPawnVisitUtility_Patch

    {
        /*
         * PostFix permettant d'éviter que des medecins viennes visiter des pateint T1/T2/M7
         */
        [HarmonyPatch(typeof(SickPawnVisitUtility), "CanVisit")]
        public class CanVisit_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn pawn, Pawn sick, JoyCategory maxPatientJoy, ref bool __result)
            {
                if (sick.IsBasicAndroidTier()) __result = false;
            }
        }
    }
}