using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlueLeakTest
{
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("GetTooltip")]
    public static class HealthCardUtility_GetTooltip
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var tipStringExtraGetter = AccessTools.Property(typeof(Hediff), nameof(Hediff.TipStringExtra)).GetGetMethod();
            var labelHelper = AccessTools.Method(typeof(HealthCardUtility_GetTooltip)
                , nameof(TransformBleedingToLeakingIfFemale));

            foreach (var code in instructions)
            {
                yield return code;
                if (code.opcode == OpCodes.Callvirt && code.operand == tipStringExtraGetter)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1); //string, Pawn on stack
                    yield return new CodeInstruction(OpCodes.Call, labelHelper); //Consume 2, leave string
                }
            }
        }

        public static string TransformBleedingToLeakingIfFemale(string original, Pawn pawn)
        {
            if (pawn.IsAndroid())
                return original.Replace("BleedingRate".Translate(), "AT_Leaking".Translate());
            return original;
        }
    }
}