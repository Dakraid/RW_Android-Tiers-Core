using Verse;

namespace MOARANDROIDS
{
    public static class MechFallMoteMaker
    {
        public static void MakeMechFallMote(IntVec3 cell, Map map)
        {
            var mote = (Mote) ThingMaker.MakeThing(RimWorld.ThingDefOf.Mote_Bombardment);
            mote.exactPosition = cell.ToVector3Shifted();
            mote.Scale = 5f;
            mote.rotationRate = 0f;
            GenSpawn.Spawn(mote, cell, map);
        }
    }
}