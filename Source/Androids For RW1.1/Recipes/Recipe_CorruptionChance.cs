﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Recipe_MemoryCorruptionChance : Recipe_SurgeryAndroids
    {
        private int upper;


        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            var flag = billDoer != null;
            var flag2 = flag;
            if (!flag2) return;

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