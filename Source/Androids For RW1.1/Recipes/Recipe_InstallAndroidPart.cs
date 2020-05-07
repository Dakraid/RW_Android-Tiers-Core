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
            for (var i = 0; i < recipe.appliedOnFixedBodyParts.Count; i++)
            {
                var part = recipe.appliedOnFixedBodyParts[i];
                var bpList = pawn.RaceProps.body.AllParts;
                for (var j = 0; j < bpList.Count; j++)
                {
                    var record = bpList[j];
                    if (record.def == part)
                    {
                        var diffs = from x in pawn.health.hediffSet.hediffs
                            where x.Part == record
                            select x;
                        if (diffs.Count() != 1 || diffs.First().def != recipe.addsHediff)
                            if (record.parent == null || pawn.health.hediffSet.GetNotMissingParts().Contains(record.parent))
                                if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record))
                                    yield return record;
                    }
                }
            }
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