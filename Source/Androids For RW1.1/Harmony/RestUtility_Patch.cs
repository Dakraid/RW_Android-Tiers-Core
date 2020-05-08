using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class RestUtility_Patch

    {
        /*
         * PostFix évitant d'attribuer de need comfort et outdoor aux T1 et T2 et l'hygiene a l'ensemble des robots
         */
        [HarmonyPatch(typeof(RestUtility), "IsValidBedFor")]
        public class IsValidBedFor_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare,
                bool ignoreOtherReservations, ref bool __result)
            {
                try
                {
                    var bedIsSurrogateM7Pod = Utils.ExceptionSurrogateM7Pod.Contains(bedThing.def.defName);
                    var bedIsSurrogatePod = Utils.ExceptionSurrogatePod.Contains(bedThing.def.defName);

                    var sleeperIsSurrogate = sleeper.IsSurrogateAndroid();
                    var sleeperIsRegularAndroid = Utils.ExceptionRegularAndroidList.Contains(sleeper.def.defName);
                    var isSurrogateM7 = sleeper.def.defName == Utils.M7 && sleeperIsSurrogate;
                    var isSleepingSpot = bedThing.def.defName == "SleepingSpot" || bedThing.def.defName == "DoubleSleepingSpot";


                    if (bedIsSurrogateM7Pod)
                    {
                        if (!isSurrogateM7) __result = false;
                    }
                    else if (bedIsSurrogatePod)
                    {
                        if (!(sleeperIsRegularAndroid && sleeper.def.defName != Utils.M7))
                            __result = false;
                    }


                    if (bedIsSurrogatePod || bedIsSurrogateM7Pod || isSleepingSpot) return;

                    if (isSurrogateM7 || sleeperIsRegularAndroid)
                        __result = false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] RestUtility.IsValidBedFor : " + e.Message + " - " + e.StackTrace);
                }
            }
        }
    }
}