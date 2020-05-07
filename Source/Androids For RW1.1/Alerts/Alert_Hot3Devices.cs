using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Alert_Hot3Devices : Alert_Critical
    {
        public Alert_Hot3Devices()
        {
            defaultLabel = "ATPP_AlertHot3Devices".Translate();
            defaultExplanation = "ATPP_AlertHot3DevicesDesc".Translate();
            defaultPriority = AlertPriority.Critical;
        }

        public override AlertReport GetReport()
        {
            List<Thing> build = null;

            var maps = Find.Maps;
            foreach (var map in maps)
                if (build == null)
                    build = Utils.GCATPP.getHeatSensitiveDevicesByHotLevel(map, 3);
                else
                    build.AddRange(build);

            return build != null ? AlertReport.CulpritsAre(build) : false;
        }
    }
}