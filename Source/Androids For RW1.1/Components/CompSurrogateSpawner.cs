using System;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompSurrogateSpawner : ThingComp
    {
        public CompProperties_SurrogateSpawner Spawnprops => props as CompProperties_SurrogateSpawner;


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
                Utils.generateSurrogate(Faction.OfPlayer, Spawnprops.Pawnkind, parent.Position, parent.Map, true, false, -1, false, false, Spawnprops.gender);
                Utils.forceGeneratedAndroidToBeDefaultPainted = false;
            }
            catch (Exception e)
            {
                Log.Message("Surrogate Android Spawn Error : " + e.Message + " " + e.StackTrace);
            }
        }
    }
}