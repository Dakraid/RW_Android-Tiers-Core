using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class CompReloadStation : ThingComp
    {
        protected CompProperties_ReloadStation PropsPowerCollector => (CompProperties_ReloadStation) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            Utils.GCATPP.pushReloadStation((Building) parent);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);


            Utils.GCATPP.popReloadStation((Building) parent, map);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
        }

        public override void ReceiveCompSignal(string signal)
        {
            if (signal == "PowerTurnedOff" || signal == "PowerTurnedOn")
            {
            }
        }

        public override string CompInspectStringExtra()
        {
            var ret = base.CompInspectStringExtra();

            if (parent == null)
                return ret;

            if (ret == null)
                ret = "";

            if (ret != "")
                ret += "\n";


            return ret;
        }

        public override void CompTick()
        {
            if (!parent.Spawned) return;

            var CGT = Find.TickManager.TicksGame;
            if (CGT % 60 == 0)

                refreshPowerConsumed();

            if (CGT % 360 == 0)

                incAndroidPower();
        }

        public float getPowerConsumed()
        {
            return getNbAndroidReloading() + parent.TryGetComp<CompPowerTrader>().Props.basePowerConsumption;
        }


        public void refreshPowerConsumed()
        {
            parent.TryGetComp<CompPowerTrader>().powerOutputInt = -getPowerConsumed();
        }

        public void incAndroidPower()
        {
            var parentPos = parent.Position;
            foreach (var adjPos in ((Building) parent).CellsAdjacent8WayAndInside())
            {
                var thingList = adjPos.GetThingList(parent.Map);
                if (thingList == null) continue;

                foreach (var cp in thingList.Select(t => t as Pawn)
                    .Where(cp => cp != null && Utils.ExceptionAndroidCanReloadWithPowerList.Contains(cp.def.defName) && cp.CurJobDef.defName == "ATPP_GoReloadBattery")
                    .Where(cp => cp.needs.food.CurLevelPercentage < 1.0))
                {
                    cp.needs.food.CurLevelPercentage += Settings.percentageOfBatteryChargedEach6Sec;
                    Utils.throwChargingMote(cp);
                }
            }
        }

        public IntVec3 getFreeReloadPlacePos(Pawn android)
        {
            foreach (var adjPos in ((Building) parent).CellsAdjacent8WayAndInside())
            {
                var ok = true;
                var thingList = adjPos.GetThingList(parent.Map);
                if (thingList == null) continue;

                if (thingList.Select(t => t as Pawn).Any(cp => cp != null && cp.IsColonist && Utils.ExceptionAndroidList.Contains(cp.def.defName))) ok = false;


                if (!ok) continue;

                if (!android.CanReach(adjPos, PathEndMode.OnCell, Danger.Deadly)) continue;

                if (!android.Map.pawnDestinationReservationManager.IsReserved(adjPos))
                    return adjPos;
            }

            return IntVec3.Invalid;
        }


        public int getNbAndroidReloading(bool countOnly = false)
        {
            var ret = 0;
            var parentPos = parent.Position;
            foreach (var adjPos in ((Building) parent).CellsAdjacent8WayAndInside())
            {
                var thingList = adjPos.GetThingList(parent.Map);
                if (thingList == null) continue;

                foreach (var t in thingList)
                {
                    if (!(t is Pawn cp) || !cp.IsColonist || !Utils.ExceptionAndroidList.Contains(cp.def.defName) ||
                        cp.CurJobDef.defName != "ATPP_GoReloadBattery") continue;

                    if (countOnly)
                        ret++;
                    else

                        ret += Utils.getConsumedPowerByAndroid(cp.def.defName);
                }
            }

            return ret;
        }
    }
}