using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class Need_Food_Patch

    {
        /*
         * Mise en place d'un Maxlevel raisonable car avec l'algo de base de RW il st demeusuré de part la taille des M7 et donc la batterie (food) met BC de temps a s'écouler
         */
        [HarmonyPatch(typeof(Need_Food), "get_HungerRate")]
        public class MaxLevel_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn ___pawn, ref float __result)
            {
                if (___pawn.def == Utils.M7Mech && ___pawn.IsSurrogateAndroid()) __result = 1.5f;
            }
        }


        [HarmonyPatch(typeof(Need_Food), "get_FoodFallPerTick")]
        public class FoodFallPerTick_Patch
        {
            [HarmonyPostfix]
            public static void Listener(Pawn ___pawn, ref float __result)
            {
                if (!___pawn.IsAndroidTier())
                    return;

                if (Utils.androidIsValidPodForCharging(___pawn) || Utils.androidReloadingAtChargingStation(___pawn)) __result = 0f;
            }
        }

        /*[HarmonyPatch(typeof(Need_Food), "NeedInterval")]
        public class IsFrozen
        {
            [HarmonyPostfix]
            public static void Listener(float ___curLevelInt, Pawn ___pawn, Need_Food __instance)
            {
                if(___pawn.def.defName == "M7Mech")
                {
                    Log.Message("=>" + ___curLevelInt );
                }
            }
        }*/
    }
}