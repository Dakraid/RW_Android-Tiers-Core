using System;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    /// <summary>
    ///     Tweaks ThingDefs after the game has been made.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class PostInitializationTweaker
    {
        static PostInitializationTweaker()
        {
            //Start tweaking.
            //IEnumerable<ThingDef> corpseDefs = DefDatabase<ThingDef>.AllDefs.Where(thingDef => thingDef.defName.EndsWith("_Corpse"));

            foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                //If the Def got a AndroidTweaker do stuff, otherwise do not bother.
                var tweaker = thingDef.GetModExtension<AndroidTweaker>();
                if (tweaker == null) continue;

                var corpseDef = thingDef?.race?.corpseDef;
                if (corpseDef == null) continue;
                //Removes corpse rotting.
                if (tweaker.tweakCorpseRot)
                {
                    corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_Rottable);
                    corpseDef.comps.RemoveAll(compProperties => compProperties is CompProperties_SpawnerFilth);
                }

                //Modifies the butchering products by importing the costs from a recipe.
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