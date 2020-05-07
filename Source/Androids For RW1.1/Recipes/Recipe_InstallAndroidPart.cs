using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Recipe_InstallArtificialBodyPartAndroid : Recipe_SurgeryAndroids
    {
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            return from part in recipe.appliedOnFixedBodyParts
                let bpList = pawn.RaceProps.body.AllParts
                let part1 = part
                from record in from record in bpList
                    where record.def == part1
                    let record1 = record
                    let diffs = from x in pawn.health.hediffSet.hediffs
                        where x.Part == record1
                        select x
                    where diffs.Count() != 1 || diffs.First().def != recipe.addsHediff
                    where record.parent == null || pawn.health.hediffSet.GetNotMissingParts().Contains(record.parent)
                    where !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record)
                    select record
                select record;
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                if (CheckSurgeryFailAndroid(billDoer, pawn, ingredients, part, bill)) return;

                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
            }
            else if (pawn.Map != null)
            {
                MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
            }
            else
            {
                pawn.health.RestorePart(part);
            }

            pawn.health.AddHediff(recipe.addsHediff, part, null);
        }
    }
}