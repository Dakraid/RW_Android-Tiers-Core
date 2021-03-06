﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MOARANDROIDS
{
    public class CompSkyMind : ThingComp
    {
        public bool autoconn;
        public bool connected;
        public int hacked = -1;
        public int hackEndGT = -1;
        public Faction hackOrigFaction;
        public bool hackWasPrisoned;
        private int infected = -1;

        public int infectedEndGT = -1;

        public int infectedExplodeGT = -1;


        private string infectedPreviousState = "";
        public int lastRemoteFlickGT;

        public int Infected
        {
            get => infected;

            set
            {
                var pVal = infected;
                infected = value;
                if (value == -1 && pVal != -1)
                {
                    parent.SetFaction(Faction.OfPlayer);

                    if (Utils.ExceptionCooler.Contains(parent.def.defName) || Utils.ExceptionHeater.Contains(parent.def.defName))
                    {
                        parent.TryGetComp<CompTempControl>().targetTemperature = (float) Convert.ToDouble(infectedPreviousState);
                    }
                    else
                    {
                        var cf = parent.TryGetComp<CompFlickable>();
                        if (cf != null) cf.SwitchIsOn = true;
                    }

                    Utils.GCATPP.popVirusedThing(parent);
                }
                else
                {
                    switch (value)
                    {
                        case 1:

                            parent.SetFaction(Faction.OfAncientsHostile);
                            break;

                        case 2:

                            parent.SetFaction(Faction.OfAncientsHostile);
                            infectedExplodeGT = Find.TickManager.TicksGame + Settings.nbSecExplosiveVirusTakeToExplode * 60;
                            break;

                        case 3:

                            parent.SetFaction(Faction.OfAncientsHostile);
                            break;
                    }

                    if (parent is Pawn)
                    {
                        var p = (Pawn) parent;

                        var cas = parent.TryGetComp<CompAndroidState>();
                        if (cas == null)
                            return;


                        if (cas.surrogateController != null)
                        {
                            var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                            if (cso != null)
                            {
                                if (value == 3)
                                    cso.disconnectControlledSurrogate(p);
                                else

                                    cso.disconnectControlledSurrogate(p, false, true);
                            }
                        }

                        if (value != 3)
                        {
                            var he = p.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
                            if (he != null)
                                p.health.RemoveHediff(he);
                        }

                        switch (value)
                        {
                            case 1:

                                parent.SetFactionDirect(Faction.OfAncientsHostile);
                                break;

                            case 2:

                                parent.SetFactionDirect(Faction.OfAncientsHostile);
                                infectedExplodeGT = Find.TickManager.TicksGame + Settings.nbSecExplosiveVirusTakeToExplode * 60;
                                break;
                        }

                        Utils.soundDefSurrogateHacked.PlayOneShot(null);
                    }
                    else
                    {
                        if (Utils.ExceptionCooler.Contains(parent.def.defName))
                        {
                            infectedPreviousState = parent.TryGetComp<CompTempControl>().targetTemperature.ToString();
                            parent.TryGetComp<CompTempControl>().targetTemperature = +200;
                        }
                        else if (Utils.ExceptionHeater.Contains(parent.def.defName))
                        {
                            infectedPreviousState = parent.TryGetComp<CompTempControl>().targetTemperature.ToString();
                            parent.TryGetComp<CompTempControl>().targetTemperature = -200;
                        }
                        else if (parent.def.thingClass == typeof(Building_Turret) || parent.def.thingClass.IsSubclassOf(typeof(Building_Turret)))
                        {
                        }
                        else
                        {
                            var cf = parent.TryGetComp<CompFlickable>();
                            if (cf != null) cf.SwitchIsOn = false;
                        }
                    }

                    Utils.GCATPP.pushVirusedThing(parent);
                }
            }
        }

        public int Hacked
        {
            get => hacked;

            set
            {
                hacked = value;
                if (value == -1)
                    Utils.GCATPP.popVirusedThing(parent);
                else
                    Utils.GCATPP.pushVirusedThing(parent);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref connected, "ATPP_connected");
            Scribe_Values.Look(ref autoconn, "ATPP_autoconn");

            Scribe_Values.Look(ref infectedExplodeGT, "ATPP_infectedExplodeGT", -1);
            Scribe_Values.Look(ref infected, "ATPP_infected", -1);
            Scribe_Values.Look(ref infectedEndGT, "ATPP_infectedEndGT", -1);
            Scribe_Values.Look(ref hackEndGT, "ATPP_hackEndGT", -1);
            Scribe_Values.Look(ref hacked, "ATPP_hacked", -1);
            Scribe_Values.Look(ref lastRemoteFlickGT, "ATPP_lastRemoteFlickGT");

            Scribe_Values.Look(ref hackWasPrisoned, "ATPP_hackWasPrisoned");

            Scribe_Values.Look(ref infectedPreviousState, "ATPP_infectedPreviousState", "");

            Scribe_References.Look(ref hackOrigFaction, "ATPP_hackOrigFaction");
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            Utils.GCATPP.popVirusedThing(parent);
            Utils.GCATPP.popSkyMindable(parent);
            Utils.GCATPP.disconnectUser(parent);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            Utils.GCATPP.popSkyMindable(parent);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (connected)
                if (!Utils.GCATPP.connectUser(parent))
                    connected = false;

            if (infected != -1 || hacked != -1) Utils.GCATPP.pushVirusedThing(parent);

            Utils.GCATPP.pushSkyMindable(parent);
        }


        public override void PostDraw()
        {
            Material avatar = null;

            if (Utils.antennaSelected() && connected)
                avatar = Tex.ConnectedUser;

            switch (infected)
            {
                case 1:
                    avatar = Tex.virus;
                    break;
                case 2:
                    avatar = Tex.explosiveVirus;
                    break;
                case 3:
                    avatar = Tex.cryptolocker;
                    break;
                case 4:
                    avatar = Tex.virusLite;
                    break;
                default:
                    switch (hacked)
                    {
                        case 1:
                            avatar = Tex.Virused;
                            break;
                        case 2:
                            avatar = Tex.ExplosiveVirused;
                            break;
                        case 3:
                            avatar = Tex.HackedTemp;
                            break;
                    }

                    break;
            }

            if (avatar == null) return;

            var vector = parent.TrueCenter();
            vector.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.28125f;
            vector.z += 1.4f;
            vector.x += parent.def.size.x / 2;

            Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, avatar, 0);
        }


        public bool canBeConnectedToSkyMind()
        {
            if (parent is Pawn pawn) return pawn.VXChipPresent() || pawn.IsAndroidTier();

            var c = parent.TryGetComp<CompPowerTrader>();
            return c != null && c.PowerOn;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var casx = parent.TryGetComp<CompAndroidState>();


            if (infected != -1 || parent.Faction != Faction.OfPlayer && casx != null && casx.isSurrogate)
                yield break;

            switch (parent)
            {
                case Pawn pawn when !pawn.VXChipPresent() && !pawn.IsAndroidTier():
                    yield break;

                case Pawn pawn:
                {
                    var cas = parent.TryGetComp<CompAndroidState>();
                    if (cas != null && !cas.isSurrogate && pawn.def.defName == "M7Mech")
                        yield break;
                    break;
                }

                case Building _ when !Utils.GCATPP.isThereSkyCloudCore():
                    yield break;
            }

            yield return new Command_Toggle
            {
                icon = Tex.SkyMindAutoConn,
                defaultLabel = "ATPP_SkyMindAutoConn".Translate(),
                defaultDesc = "ATPP_SkyMindAutoConnDesc".Translate(),
                isActive = () => autoconn,
                toggleAction = delegate { autoconn = !autoconn; }
            };

            yield return new Command_Toggle
            {
                icon = Tex.SkyMindConn,
                defaultLabel = "ATPP_SkyMindConn".Translate(),
                defaultDesc = "ATPP_SkyMindConnDesc".Translate(),
                isActive = () => connected,
                toggleAction = delegate
                {
                    if (!connected)
                    {
                        if (!Utils.GCATPP.connectUser(parent))
                        {
                            if (Utils.GCATPP.getNbSlotAvailable() == 0)
                                Messages.Message("ATPP_CannotConnectToSkyMindNoNet".Translate(), parent, MessageTypeDefOf.NegativeEvent);
                            else
                                Messages.Message("ATPP_CannotConnectToSkyMind".Translate(), parent, MessageTypeDefOf.NegativeEvent);
                        }
                    }
                    else
                    {
                        Utils.GCATPP.disconnectUser(parent);
                    }
                }
            };
        }

        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            switch (signal)
            {
                case "SkyMindNetworkUserConnected":

                    connected = true;
                    break;
                case "SkyMindNetworkUserDisconnected":

                    connected = false;
                    break;
            }
        }

        public void tempHackingEnding()
        {
            if (hacked != 3)
                return;

            var cp = (Pawn) parent;

            var cas = cp.TryGetComp<CompAndroidState>();
            if (cas.surrogateController != null)
            {
                var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                cso.stopControlledSurrogate(cp, true);
            }

            cp.SetFaction(hackOrigFaction);

            if (hackWasPrisoned)
                if (cp.guest != null)
                {
                    cp.guest.SetGuestStatus(Faction.OfPlayer, true);
                    if (cp.workSettings == null)
                    {
                        cp.workSettings = new Pawn_WorkSettings(cp);
                        cp.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
                    }
                }

            if (cp.jobs != null)
            {
                cp.jobs.StopAll();
                cp.jobs.ClearQueuedJobs();
            }

            cp.mindState?.Reset(true);

            PawnComponentsUtility.AddAndRemoveDynamicComponents(cp);
            hacked = -1;
            hackEndGT = -1;
            hackWasPrisoned = false;

            cp.Map.attackTargetsCache.UpdateTarget(cp);

            foreach (var p in cp.Map.mapPawns.AllPawnsSpawned)
            {
                var aim = p.mindState?.enemyTarget;

                if (aim == null || !(aim is Pawn pawnTarget)) continue;


                if (pawnTarget != cp) continue;

                if (p.jobs == null) continue;

                p.jobs.StopAll();
                p.jobs.ClearQueuedJobs();
            }

            Find.ColonistBar.MarkColonistsDirty();
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";

            if (parent.Map == null)
                return base.CompInspectStringExtra();

            switch (infected)
            {
                case 1 when infectedEndGT != -1:
                    ret += "ATPP_TempInfection".Translate((infectedEndGT - Find.TickManager.TicksGame).ToStringTicksToPeriodVerbose()) + "\n";
                    break;
                case 3:
                    ret += "ATPP_CryptoLockedSurrogate".Translate() + "\n";
                    break;
                case 2:
                    ret += "ATPP_ExplosiveVirus".Translate((int) Math.Max(0, infectedExplodeGT - Find.TickManager.TicksGame).TicksToSeconds()) + "\n";
                    break;
            }

            if (hacked == 3) ret += "ATPP_TempHackingLosingControlIn".Translate((int) Math.Max(0, hackEndGT - Find.TickManager.TicksGame).TicksToSeconds()) + "\n";

            if (Utils.GCATPP.isConnectedToSkyMind(parent)) ret += "ATPP_SkyMindDetected".Translate() + "\n";

            return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
        }
    }
}