using RimWorld;

namespace MOARANDROIDS
{
    public class Thought_AssistedByMinds : Thought_Situational
    {
        public override string LabelCap => CurStage.label;

        protected override float BaseMoodOffset => Utils.GCATPP.getNbAssistingMinds() * Settings.nbMoodPerAssistingMinds;
    }
}