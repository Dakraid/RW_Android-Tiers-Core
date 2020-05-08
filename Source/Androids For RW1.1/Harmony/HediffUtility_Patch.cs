using HarmonyLib;
using Verse;

namespace MOARANDROIDS
{
    internal class HediffUtility_Patch

    {
        [HarmonyPatch(typeof(HediffUtility), "CountAddedAndImplantedParts")]
        public class CountAddedParts_Patch
        {
            [HarmonyPostfix]
            public static void Listener(HediffSet hs, ref int __result)
            {
                if (hs.pawn.story != null && hs.pawn.story.traits.HasTrait(TraitDefOf.Transhumanist)
                                          && (Utils.ExceptionAndroidList.Contains(hs.pawn.def.defName) || hs.pawn.TryGetComp<CompSurrogateOwner>() != null &&
                                              hs.pawn.TryGetComp<CompSurrogateOwner>().skyCloudHost != null))
                    __result += 20;
            }
        }
    }
}