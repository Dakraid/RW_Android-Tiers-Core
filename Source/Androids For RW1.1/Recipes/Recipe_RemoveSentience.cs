using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Recipe_RemoveSentience : Recipe_SurgeryAndroids
    {
        private int upper;

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            for (var i = 0; i < recipe.appliedOnFixedBodyParts.Count; i++)
            {
                var part = recipe.appliedOnFixedBodyParts[i];
                var bpList = pawn.RaceProps.body.AllParts;
                for (var j = 0; j < bpList.Count; j++)
                {
                    var record = bpList[j];
                    if (record.def == part)
                        if (pawn.health.hediffSet.GetNotMissingParts().Contains(record))
                            if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record))
                                if (!pawn.health.hediffSet.hediffs.Any(x => x.Part == record && x.def == recipe.addsHediff))
                                    yield return record;
                }
            }
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            var flag = billDoer != null;
            var flag2 = flag;
            if (flag2)
            {
                var flag3 = !CheckSurgeryFailAndroid(billDoer, pawn, ingredients, part, null);
                var flag4 = flag3;
                if (flag4)
                {
                    pawn.health.AddHediff(recipe.addsHediff, part, null);
                    TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                    RemoveTraits(pawn, pawn.story.traits.allTraits);
                    upper = 30;
                }
                else
                {
                    upper = 3;
                }

                RandomCorruption(pawn);
            }
        }

        private void RemoveTraits(Pawn pawn, List<Trait> traits)
        {
            for (var i = 0; i < traits.Count; i++)
            {
                traits.Remove(traits[i]);
                i--;
            }

            string text = "Atlas_LossOfSelfWill".Translate(pawn.Name.ToStringShort);
            text = text.AdjustedFor(pawn);
            string label = "LetterLabelAtlas_LossOfSelfWill".Translate();
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, pawn);
        }

        private void RandomCorruption(Pawn pawn)
        {
            var rnd = new Random();
            var chance = rnd.Next(0, upper);

            {
                var check = chance == 1;
                if (check) pawn.health.AddHediff(HediffDefOf.CorruptMemory, pawn.health.hediffSet.GetBrain(), null);
            }
        }
    }
}