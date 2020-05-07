using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000441 RID: 1089
    public class Recipe_InstallImplantAndroid : Recipe_SurgeryAndroids
    {
        // Token: 0x06001252 RID: 4690 RVA: 0x0008CB84 File Offset: 0x0008AF84
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            return from part in recipe.appliedOnFixedBodyParts let bpList = pawn.RaceProps.body.AllParts let part1 = part from record in from record in bpList where record.def == part1 where pawn.health.hediffSet.GetNotMissingParts().Contains(record) where !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) let record1 = record where !pawn.health.hediffSet.hediffs.Any(x => x.Part == record1 && x.def == recipe.addsHediff) select record select record;
        }

        // Token: 0x06001253 RID: 4691 RVA: 0x0008CBB0 File Offset: 0x0008AFB0
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                if (CheckSurgeryFailAndroid(billDoer, pawn, ingredients, part, bill)) return;

                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
            }

            pawn.health.AddHediff(recipe.addsHediff, part, null);
        }
    }
}