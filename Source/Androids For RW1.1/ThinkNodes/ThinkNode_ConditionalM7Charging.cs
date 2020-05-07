using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class ThinkNode_ConditionalM7Charging : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            //QUe les SM7 controlés peuvent se recharger
            if (pawn.Downed || pawn.def.defName != Utils.M7 || !pawn.IsSurrogateAndroid(true)) return false;
            return pawn.needs.food != null && pawn.needs.food.CurLevelPercentage < 0.3f;
        }
    }
}