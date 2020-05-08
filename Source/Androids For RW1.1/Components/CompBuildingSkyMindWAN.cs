using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompBuildingSkyMindWAN : ThingComp
    {
        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            var build = (Building) parent;


            Utils.GCATPP.popSkyMindWANServer(build, map);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            Utils.GCATPP.popSkyMindWANServer((Building) parent, previousMap);
        }


        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            var build = (Building) parent;

            switch (signal)
            {
                case "PowerTurnedOn":
                    Utils.GCATPP.pushSkyMindWANServer(build);
                    break;
                case "PowerTurnedOff":
                    Utils.GCATPP.popSkyMindWANServer(build, build.Map);
                    break;
            }
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";

            if (parent.Map == null)
                return base.CompInspectStringExtra();

            ret += "ATPP_SkyMindAntennaSynthesis".Translate(Utils.GCATPP.getNbThingsConnected(), Utils.GCATPP.getNbSlotAvailable()) + "\n";

            return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if (parent.TryGetComp<CompPowerTrader>().PowerOn)
                Utils.GCATPP.pushSkyMindWANServer((Building) parent);
        }
    }
}