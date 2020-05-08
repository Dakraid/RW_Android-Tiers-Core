using System;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    [StaticConstructorOnStartup]
    public static class PostInitializationTweaker
    {
        static PostInitializationTweaker()
        {
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                var tweaker = thingDef.GetModExtension<AndroidTweaker>();
                if (tweaker == null) continue;

                var corpseDef = thingDef?.race?.corpseDef;
                if (corpseDef == null) continue;

                if (tweaker.tweakCorpseRot)
                {
                    corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_Rottable);
                    corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_SpawnerFilth);
                }


                var recipeDef = tweaker.recipeDef;
                if (!tweaker.tweakCorpseButcherProducts || recipeDef == null) continue;

                corpseDef.butcherProducts.Clear();

                foreach (var ingredient in recipeDef.ingredients)
                {
                    var finalCount = 0f;
                    var ingredientThingDef = ingredient.filter.AnyAllowedDef;
                    var requiredCount = ingredient.CountRequiredOfFor(ingredientThingDef, recipeDef);

                    if (tweaker.corpseButcherRoundUp)
                        finalCount = (float) Math.Ceiling(requiredCount * tweaker.corpseButcherProductsRatio);
                    else
                        finalCount = (float) Math.Floor(requiredCount * tweaker.corpseButcherProductsRatio);
                }
            }
        }
    }
}