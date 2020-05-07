using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class CompPowerTrader_Patch
    {
        [HarmonyPatch(typeof(CompPowerTrader), "SetUpPowerVars")]
        public class SetUpPowerVars_Patch
        {
            [HarmonyPostfix]
            public static void Listener(CompPowerTrader __instance, ref float ___powerOutputInt)
            {
                var rs = __instance.parent.TryGetComp<CompReloadStation>();
                rs?.refreshPowerConsumed();
            }
        }
    }
}