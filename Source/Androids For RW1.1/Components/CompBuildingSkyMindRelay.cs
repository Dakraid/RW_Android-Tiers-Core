using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class CompBuildingSkyMindRelay : ThingComp
    {
        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);


            Utils.GCATPP.popRelayTower((Building) parent, map);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            Utils.GCATPP.popRelayTower((Building) parent, previousMap);
        }

        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            var build = (Building) parent;

            switch (signal)
            {
                case "PowerTurnedOn":
                    Utils.GCATPP.pushRelayTower(build);
                    break;
                case "PowerTurnedOff":
                    Utils.GCATPP.popRelayTower(build, build.Map);
                    break;
            }
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if (parent.TryGetComp<CompPowerTrader>().PowerOn)
                Utils.GCATPP.pushRelayTower((Building) parent);
        }
    }
}