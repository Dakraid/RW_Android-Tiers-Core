using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class StockGenerator_SlaveAndroids : StockGenerator
    {
        private PawnKindDef pawnKind = null;
        private bool randomisePawnKind = false;

        private readonly bool respectPopulationIntent = false;

        public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
        {
            if (respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent) yield break;

            var count = countRange.RandomInRange;
            for (var i = 0; i < count; i++)
            {
                if (!(from fac in Find.FactionManager.AllFactionsVisible
                    where fac != Faction.OfPlayer && fac.def.humanlikeFaction
                    select fac).TryRandomElement(out var androidFaction))
                    yield break;

                var rnd = new Random();
                PawnKindDef android;
                if (randomisePawnKind)
                    switch (rnd.Next(1, 4))
                    {
                        case 1:
                            android = PawnKindDefOf.AndroidT1ColonistGeneral;
                            break;
                        case 2:
                            android = PawnKindDefOf.AndroidT2ColonistGeneral;
                            break;
                        case 3:
                            android = PawnKindDefOf.AndroidT3ColonistGeneral;
                            break;
                        case 4:
                            android = PawnKindDefOf.AndroidT4ColonistGeneral;
                            break;
                        default:
                            android = PawnKindDefOf.AndroidT1ColonistGeneral;
                            break;
                    }
                else
                    android = pawnKind;

                var fac1 = androidFaction;
                var request = new PawnGenerationRequest(android, fac1, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, !trader.orbital, true,
                    true, false);
                yield return PawnGenerator.GeneratePawn(request);
            }
        }

        public override bool HandlesThingDef(ThingDef thingDef)
        {
            return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.None;
        }
    }
}