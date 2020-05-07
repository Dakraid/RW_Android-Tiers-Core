using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnUtility_Patch

    {
        [HarmonyPatch(typeof(PawnUtility), "ShouldSendNotificationAbout")]
        public class ShouldSendNotificationAbout_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p, ref bool __result)
            {
                if (Utils.ignoredPawnNotifications == p)
                    __result = false;
            }
        }
    }
}