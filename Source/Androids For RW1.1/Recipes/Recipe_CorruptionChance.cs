using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000009 RID: 9
    public class Recipe_MemoryCorruptionChance : Recipe_SurgeryAndroids
    {
        private int upper;

        // Token: 0x0600000C RID: 12 RVA: 0x000021D8 File Offset: 0x000003D8
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
                    upper = 60;
                }
                else
                {
                    upper = 15;
                }

                RandomCorruption(pawn);
            }
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