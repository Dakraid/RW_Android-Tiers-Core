using Verse;

namespace MOARANDROIDS
{
    public class CompProperties_Computer : CompProperties
    {
        public string ambiance;
        public bool isSecurityServer = false;
        public string type = "Computer";

        public CompProperties_Computer()
        {
            compClass = typeof(CompComputer);
        }
    }
}