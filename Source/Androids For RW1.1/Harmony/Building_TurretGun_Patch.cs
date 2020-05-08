using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    [HarmonyPatch(typeof(Building_TurretGun), "get_CanSetForcedTarget")]
    public static class CanSetForcedTarget_Patch
    {
        private static readonly FieldInfo mannableComp = AccessTools.Field(typeof(Building_TurretGun), "mannableComp");


        [HarmonyPostfix]
        public static void Listener(Building_TurretGun __instance, ref bool __result)
        {
            var crt = __instance.TryGetComp<CompRemotelyControlledTurret>();

            if (crt?.controller == null)
                return;

            var mannable = (CompMannable) mannableComp.GetValue(__instance);

            __result = __result || mannable == null;
        }
    }

    [HarmonyPatch(typeof(Building_TurretGun), "DrawExtraSelectionOverlays")]
    public static class DrawExtraSelectionOverlays_Patch
    {
        [HarmonyPostfix]
        public static void Listener(Building_TurretGun __instance)
        {
            var crt = __instance.TryGetComp<CompRemotelyControlledTurret>();

            if (crt?.controller == null)
                return;

            CompSurrogateOwner csc = null;
            var csm = __instance.TryGetComp<CompSkyMind>();

            csc = crt.controller.TryGetComp<CompSurrogateOwner>();

            if (csm != null && csm.connected && crt.controller != null && csc != null && csc.skyCloudHost != null && csc.skyCloudHost.Map == __instance.Map)
                GenDraw.DrawLineBetween(__instance.TrueCenter(), csc.skyCloudHost.TrueCenter(), SimpleColor.Red);
        }
    }
}