using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class TaleUtility_Patch

    {
        /*
         * Equivalent au patching en prefixe de la methode KILL, car aussinon il fallait un PREFIX avec les incompatibilitées que sa peut entrainer
         */
        [HarmonyPatch(typeof(TaleUtility), "Notify_PawnDied")]
        public class Notify_PawnDied_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn victim, DamageInfo? dinfo)
            {
                try
                {
                    if (victim.IsSurrogateAndroid())
                        Utils.insideKillFuncSurrogate = true;

                    Utils.GCATPP.disconnectUser(victim);


                    if (victim.IsSurrogateAndroid(true))
                    {
                        var cas = victim.TryGetComp<CompAndroidState>();
                        if (cas == null)
                            return;


                        var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                        cso.stopControlledSurrogate(victim);
                    }


                    Utils.insideKillFuncSurrogate = false;
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] TaleUtility.Notify_PawnDied(Error) : " + e.Message + " - " + e.StackTrace);

                    if (victim.IsSurrogateAndroid())
                        Utils.insideKillFuncSurrogate = false;
                }
            }
        }
    }
}