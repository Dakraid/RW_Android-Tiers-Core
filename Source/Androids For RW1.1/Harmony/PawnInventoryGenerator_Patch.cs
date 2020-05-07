using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    internal class PawnInventoryGenerator_Patch

    {
        [HarmonyPatch(typeof(PawnInventoryGenerator), "GiveRandomFood")]
        public class GiveRandomFood_PatchPostfix
        {
            [HarmonyPostfix]
            public static void Listener(Pawn p)
            {
                if (!Utils.PawnInventoryGeneratorCanHackInvNutritionValue) return;

                if (Utils.ExceptionAndroidList.Contains(p.def.defName) && p.def.defName != Utils.M7)
                    p.kindDef.invNutrition = Utils.PawnInventoryGeneratorLastInvNutritionValue;
            }
        }

        [HarmonyPatch(typeof(PawnInventoryGenerator), "GiveRandomFood")]
        public class GiveRandomFood_PatchPrefix
        {
            [HarmonyPrefix]
            public static bool Listener(Pawn p)
            {
                if (!Utils.PawnInventoryGeneratorCanHackInvNutritionValue) return true;

                if (!Utils.ExceptionAndroidList.Contains(p.def.defName) || p.def.defName == Utils.M7) return true;

                Utils.PawnInventoryGeneratorLastInvNutritionValue = p.kindDef.invNutrition;
                p.kindDef.invNutrition = 1.0f;

                return true;
            }
        }
    }
}