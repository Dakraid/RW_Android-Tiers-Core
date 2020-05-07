using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x020007C0 RID: 1984
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
            for (var i = 0; i < maps.Count; i++)
                if (build == null)
                    build = Utils.GCATPP.getHeatSensitiveDevicesByHotLevel(maps[i], 2);
                else
                    build.AddRange(build);

            if (build != null)
                return AlertReport.CulpritsAre(build);
            return false;
        }
    }
}