using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    public class CompAutoDoor : ThingComp
    {
        private Building_Door doorRef;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            doorRef = parent as Building_Door;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (!Utils.GCATPP.isThereSkyCloudCore() || !Utils.GCATPP.isConnectedToSkyMind(parent)) yield break;


            if (parent.TryGetComp<CompPowerTrader>() == null || !parent.TryGetComp<CompPowerTrader>().PowerOn
                                                             || parent.IsBrokenDown())
                yield break;

            if (doorRef.Open)
                yield return new Command_Action
                {
                    icon = Tex.texAutoDoorClose,
                    defaultLabel = "ATPP_AutoDoorClose".Translate(),
                    defaultDesc = "ATPP_AutoDoorCloseDescription".Translate(),
                    action = delegate
                    {
                        var holdOpenInt = Traverse.Create(doorRef).Field("holdOpenInt").GetValue<bool>();
                        if (Traverse.Create(doorRef).Field("holdOpenInt").GetValue<bool>())
                            Traverse.Create(doorRef).Field("holdOpenInt").SetValue(false);

                        Traverse.Create(doorRef).Method("DoorTryClose").GetValue();
                        MoteMaker.ThrowText(doorRef.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), doorRef.Map, "ATPP_AutoDoorCloseMoteText".Translate(), Color.white);
                        Utils.playVocal("soundDefSkyCloudDoorClosed");
                    }
                };
            else
                yield return new Command_Action
                {
                    icon = Tex.texAutoDoorOpen,
                    defaultLabel = "ATPP_AutoDoorOpen".Translate(),
                    defaultDesc = "ATPP_AutoDoorOpenDescription".Translate(),
                    action = delegate
                    {
                        if (!Traverse.Create(doorRef).Field("holdOpenInt").GetValue<bool>())
                            Traverse.Create(doorRef).Field("holdOpenInt").SetValue(true);

                        doorRef.StartManualOpenBy(null);
                        MoteMaker.ThrowText(doorRef.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), doorRef.Map, "ATPP_AutoDoorOpenMoteText".Translate(), Color.white);

                        Utils.playVocal("soundDefSkyCloudDoorOpened");
                    }
                };
        }
    }
}