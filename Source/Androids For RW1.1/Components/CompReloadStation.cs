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

            //Au demarrage si l'emetteur on réapplique le retrait de ce courant convertis en sans fil
            //substractPowerTransmitted();
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            //Retire de la liste des emetteurs de la map
            Utils.GCATPP.popReloadStation((Building) parent, map);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
        }

        public override void ReceiveCompSignal(string signal)
        {
            //Quand un WPN sans va ou revient on raffraichis les offres des factions uniquement
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

            //ret += "\n" + "ARKPPP_StormPerturbationInfo".Translate((int)(perturbation * 100));

            return ret;
        }

        public override void CompTick()
        {
            if (!parent.Spawned) return;

            var CGT = Find.TickManager.TicksGame;
            if (CGT % 60 == 0)
                //Rafraichissement qt de courant consommé
                refreshPowerConsumed();

            if (CGT % 360 == 0)
                //Augmentation énergie (barre de faim) des androids présents
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
                
                foreach (var cp in thingList.Select(t => t as Pawn).Where(cp => cp != null && Utils.ExceptionAndroidCanReloadWithPowerList.Contains(cp.def.defName) && cp.CurJobDef.defName == "ATPP_GoReloadBattery").Where(cp => cp.needs.food.CurLevelPercentage < 1.0))
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

                //Si pas déjà d'android dessus
                if (!ok) continue;

                if (!android.CanReach(adjPos, PathEndMode.OnCell, Danger.Deadly)) continue;
                    
                if (!android.Map.pawnDestinationReservationManager.IsReserved(adjPos))
                    return adjPos;
            }

            return IntVec3.Invalid;
        }

        //Comptabilisation du nombre d'androids sur les bords directes du batiments en train de recharger
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
                    //Il sagit d'un android 
                    if (!(t is Pawn cp) || !cp.IsColonist || !Utils.ExceptionAndroidList.Contains(cp.def.defName) ||
                        cp.CurJobDef.defName != "ATPP_GoReloadBattery") continue;
                        
                    if (countOnly)
                        ret++;
                    else
                        //Si son job est "reloading" alors incrémentation (il ne sagit pas d'un android ne faisant que passer)
                        ret += Utils.getConsumedPowerByAndroid(cp.def.defName);
                }
            }

            return ret;
        }
    }
}