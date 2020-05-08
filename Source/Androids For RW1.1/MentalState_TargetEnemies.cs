using RimWorld;
using Verse.AI;

namespace MOARANDROIDS
{
    public class MentalState_ManhunterNotColony : MentalState
    {
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}