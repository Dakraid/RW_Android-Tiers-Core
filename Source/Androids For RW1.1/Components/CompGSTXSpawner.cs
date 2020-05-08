using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompGSTXSpawner : ThingComp
    {
        public CompProperties_GSTXSpawner Spawnprops => props as CompProperties_GSTXSpawner;


        public override void CompTick()
        {
            Spawn();
            parent.Destroy();
        }

        public void Spawn()
        {
            try
            {
                ThingDef td = null;
                td = DefDatabase<ThingDef>.GetNamed(Spawnprops.GSThing, false);
                var pawnKind = "";
                var gender = Gender.Male;
                var source = "";

                if (td == null) return;

                var thing = ThingMaker.MakeThing(td);
                GenSpawn.Spawn(thing, parent.Position, parent.Map);

                switch (Spawnprops.GSThing)
                {
                    case "ATPP_GS_TX2KMale":
                        pawnKind = "ATPP_Android2KTXKind";
                        gender = Gender.Male;
                        source = "TX2K";
                        break;
                    case "ATPP_GS_TX2KFemale":
                        pawnKind = "ATPP_Android2KTXKind";
                        gender = Gender.Female;
                        source = "TX2K Surrogate";
                        break;
                    case "ATPP_GS_TX3Male":
                        pawnKind = "ATPP_Android3TXKind";
                        gender = Gender.Male;
                        source = "TX3";
                        break;
                    case "ATPP_GS_TX3Female":
                        pawnKind = "ATPP_Android3TXKind";
                        gender = Gender.Female;
                        source = "TX3";
                        break;
                    case "ATPP_GS_TX4Male":
                        pawnKind = "ATPP_Android4TXKind";
                        gender = Gender.Male;
                        source = "TX4";
                        break;
                    case "ATPP_GS_TX4Female":
                        pawnKind = "ATPP_Android4TXKind";
                        gender = Gender.Female;
                        source = "TX4";
                        break;
                }

                if (Spawnprops.surrogate == 1) source += " (Surrogate)";

                var baseThing = Traverse.Create(thing);

                baseThing.Field("isAlien").SetValue(true);
                baseThing.Field("crownTypeAlien").SetValue("Average_Normal");
                baseThing.Field("bodyType").SetValue(BodyTypeDefOf.Male);
                baseThing.Field("pawnKindDef").SetValue(DefDatabase<PawnKindDef>.GetNamed(pawnKind, false));
                baseThing.Field("gender").SetValue(gender);
                baseThing.Field("sourceName").SetValue(source);
            }
            catch (Exception e)
            {
                Log.Message("QEE GenomeSequenser Spawn Error : " + e.Message + " " + e.StackTrace);
            }
        }
    }
}