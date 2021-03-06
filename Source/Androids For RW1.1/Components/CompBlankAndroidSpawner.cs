﻿using System;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompBlankAndroidSpawner : ThingComp
    {
        public CompProperties_BlankAndroidSpawner Spawnprops => props as CompProperties_BlankAndroidSpawner;

        public override void CompTick()
        {
            Spawn();
            parent.Destroy();
        }

        public void Spawn()
        {
            try
            {
                Utils.forceGeneratedAndroidToBeDefaultPainted = true;

                var request = new PawnGenerationRequest(Spawnprops.Pawnkind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f,
                    false, true, false, false, false, false, false, false, 0f, null, 0f);
                var blankAndroid = PawnGenerator.GeneratePawn(request);
                GenSpawn.Spawn(blankAndroid, parent.Position, parent.Map);

                blankAndroid.health.AddHediff(Utils.hediffBlankAndroid);

                var cas = blankAndroid.TryGetComp<CompAndroidState>();
                cas.isBlankAndroid = true;


                Utils.initBodyAsSurrogate(blankAndroid, false);

                var sn = "";
                switch (blankAndroid.def.defName)
                {
                    case Utils.T3:
                        sn = "T3";
                        break;
                    case Utils.T4:
                        sn = "T4";
                        break;
                }

                blankAndroid.Name = new NameTriple("", sn, "");


                Utils.forceGeneratedAndroidToBeDefaultPainted = false;
            }
            catch (Exception e)
            {
                Log.Message("Blank Android Spawn Error : " + e.Message + " " + e.StackTrace);
            }
        }
    }
}