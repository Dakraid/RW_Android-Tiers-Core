﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompAndroidSpawner1T : ThingComp
    {
        public override void CompTick()
        {
            CheckShouldSpawn();
        }

        private void CheckShouldSpawn()
        {
            SpawnDude();
            parent.Destroy();
        }

        public void SpawnDude()
        {
            var pawnKindDef = new List<PawnKindDef>
            {
                PawnKindDefOf.AndroidT1Colonist
            }.RandomElement();
            var request = new PawnGenerationRequest(pawnKindDef, Faction.OfPlayer);
            var pawn = PawnGenerator.GeneratePawn(request);


            GenSpawn.Spawn(pawn, parent.Position, parent.Map);
        }
    }
}