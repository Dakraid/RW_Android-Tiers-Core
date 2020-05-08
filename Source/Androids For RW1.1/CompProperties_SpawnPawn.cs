using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_SpawnPawn : CompProperties_UseEffect
    {
        public int amount = 1;


        public FactionDef forcedFaction;


        public PawnKindDef pawnKind;


        public string pawnSpawnedStringKey = "AndroidSpawnedDroidMessageText";


        public bool sendMessage = true;


        public bool usePlayerFaction = true;


        public CompProperties_SpawnPawn()
        {
            compClass = typeof(CompUseEffect_SpawnAndroid);
        }
    }
}