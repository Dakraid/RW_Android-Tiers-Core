using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Alert_Hot2Devices : Alert
    {
        public Alert_Hot2Devices()
        {
            defaultLabel = "ATPP_AlertHot2Devices".Translate();
            defaultExplanation = "ATPP_AlertHot2DevicesDesc".Translate();
            defaultPriority = AlertPriority.High;
        }

        public override AlertReport GetReport()
        {
            List<Thing> build = null;

            var maps = Find.Maps;
            foreach (var map in maps)
                if (build == null)
                    build = Utils.GCATPP.getHeatSensitiveDevicesByHotLevel(map, 2);
                else
                    build.AddRange(build);

            return build != null ? AlertReport.CulpritsAre(build) : false;
        }
    }
}