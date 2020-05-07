using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class ThoughtWorker_FeelingsTowardHumanity : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            var feel = DefDatabase<TraitDef>.GetNamed("FeelingsTowardHumanity", false);
            if (feel == null)
                return false;

            var num = p.story.traits.DegreeOfTrait(feel);
            var flag = !p.RaceProps.Humanlike;
            ThoughtState result;
            if (flag)
            {
                result = false;
            }
            else
            {
                var flag2 = !RelationsUtility.PawnsKnowEachOther(p, other);
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    var flag3 = other.IsAndroid();
                    //SI androide OU un cyborg
                    if (flag3 || other.health.hediffSet.CountAddedAndImplantedParts() >= 5)
                    {
                        result = false;
                    }
                    else
                    {
                        var flag4 = num == 1;
                        if (flag4)
                        {
                            result = ThoughtState.ActiveAtStage(0);
                        }
                        else
                        {
                            var flag5 = num == 2;
                            if (flag5)
                                result = ThoughtState.ActiveAtStage(1);
                            else
                                result = false;
                        }
                    }
                }
            }

            return result;
        }
    }
}