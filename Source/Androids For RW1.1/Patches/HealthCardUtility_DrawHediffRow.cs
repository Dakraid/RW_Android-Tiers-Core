using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BlueLeakTest
{
    [HarmonyPatch(typeof(HealthCardUtility))]
    [HarmonyPatch("DrawHediffRow")]
    [StaticConstructorOnStartup]
    public static class HealthCardUtility_DrawHediffRow
    {
        private static Texture2D leakingIcon;


        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var bleedingIconField = AccessTools.Field(typeof(HealthCardUtility), "BleedingIcon");
            var labelColorGetter = AccessTools.Property(typeof(Hediff), nameof(Hediff.LabelColor)).GetGetMethod();
            var iconHelper = AccessTools.Method(typeof(HealthCardUtility_DrawHediffRow)
                , nameof(TransformIconColorBlueIfFemale));
            var labelHelper = AccessTools.Method(typeof(HealthCardUtility_DrawHediffRow)
                , nameof(TransformLabelColorRedToBlueIfFemale));

            leakingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Medical/Leaking");

            foreach (var code in instructions)
            {
                yield return code;

                if (code.opcode != OpCodes.Ldsfld || (FieldInfo) code.operand != bleedingIconField) continue;

                Log.Message("Patching");
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Call, iconHelper);


                /*  if(code.opcode == OpCodes.Callvirt && code.operand == labelColorGetter) {
                        yield return new CodeInstruction(OpCodes.Ldarg_1);  
                        yield return new CodeInstruction(OpCodes.Call, labelHelper); 
                    }   */
            }
        }

        public static Texture2D TransformIconColorBlueIfFemale(Texture2D original, Pawn pawn)
        {
            return pawn.IsAndroid() ? leakingIcon : original;
        }

        public static Color TransformLabelColorRedToBlueIfFemale(Color original, Pawn pawn)
        {
            return pawn.IsAndroid() ? Color.cyan : original;
        }
    }
}