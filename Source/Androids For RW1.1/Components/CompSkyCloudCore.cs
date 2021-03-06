﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MOARANDROIDS
{
    public class CompSkyCloudCore : ThingComp
    {
        public List<Pawn> assistingMinds = new List<Pawn>();

        public int bootGT = -2;
        public Dictionary<Pawn, Building> controlledTurrets = new Dictionary<Pawn, Building>();

        public List<Pawn> controlledTurretsKeys = new List<Pawn>();
        public List<Building> controlledTurretsValues = new List<Building>();
        public Dictionary<Pawn, int> inMentalBreak = new Dictionary<Pawn, int>();

        public List<Pawn> inMentalBreakKeys = new List<Pawn>();
        public List<int> inMentalBreakValues = new List<int>();
        public List<Pawn> replicatingMinds = new List<Pawn>();

        public int SID = -1;


        public List<Pawn> storedMinds = new List<Pawn>();

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref SID, "ATPP_SID", -1);
            Scribe_Values.Look(ref bootGT, "ATPP_bootGT", -2);
            Scribe_Collections.Look(ref storedMinds, false, "ATPP_storedMinds", LookMode.Deep);
            Scribe_Collections.Look(ref assistingMinds, false, "ATPP_assistingMinds", LookMode.Reference);
            Scribe_Collections.Look(ref replicatingMinds, false, "ATPP_replicatingMinds", LookMode.Reference);

            Scribe_Collections.Look(ref inMentalBreak, "ATPP_inMentalBreak", LookMode.Reference, LookMode.Value, ref inMentalBreakKeys, ref inMentalBreakValues);


            Scribe_Collections.Look(ref controlledTurrets, "ATPP_controlledTurrets", LookMode.Reference, LookMode.Reference, ref controlledTurretsKeys,
                ref controlledTurretsValues);

            if (Scribe.mode != LoadSaveMode.PostLoadInit) return;

            if (storedMinds == null)
                storedMinds = new List<Pawn>();

            if (controlledTurrets == null)
                controlledTurrets = new Dictionary<Pawn, Building>();

            if (inMentalBreak == null)
                inMentalBreak = new Dictionary<Pawn, int>();

            storedMinds.RemoveAll(item => item == null);
        }


        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();


            foreach (var csx in storedMinds.Select(p => p.TryGetComp<CompSurrogateOwner>()).SelectMany(cso => cso.availableSX.Where(csx => parent.Map == csx.Map)))
                GenDraw.DrawLineBetween(parent.TrueCenter(), csx.TrueCenter(), SimpleColor.Red);


            foreach (var p in controlledTurrets.Where(p => parent.Map == p.Value.Map))
                GenDraw.DrawLineBetween(parent.TrueCenter(), p.Value.TrueCenter(), SimpleColor.Red);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            if (SID == -1)
            {
                SID = Utils.GCATPP.getNextSkyCloudID();
                Utils.GCATPP.incNextSkyCloudID();
            }

            if (Booted()) Utils.GCATPP.pushSkyCloudCore((Building) parent);

            Utils.GCATPP.pushSkyCloudCoreAbs((Building) parent);


            foreach (var m in storedMinds) Utils.removeMindBlacklistedTrait(m);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            stopAllMindsActivities();


            Utils.GCATPP.popSkyCloudCore((Building) parent);
            Utils.GCATPP.popSkyCloudCoreAbs((Building) parent);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);


            if (storedMinds.Count == 0) return;

            disconnectAllSurrogates();
            disconnectAllRemotelyControlledTurrets();

            Find.LetterStack.ReceiveLetter("ATPP_destroyedMindsDueToDestroyedSkyCloudCore".Translate(storedMinds.Count),
                "ATPP_destroyedMindsDueToDestroyedSkyCloudCoreDesc".Translate(storedMinds.Count, getName()), LetterDefOf.ThreatBig);

            foreach (var p in storedMinds) p.Kill(null);
        }

        public override void ReceiveCompSignal(string signal)
        {
            switch (signal)
            {
                case "PowerTurnedOff":
                {
                    if (bootGT == -1)
                        Utils.playVocal("soundDefSkyCloudPowerFailure");

                    bootGT = -2;
                    stopAllMindsActivities(true);
                    Utils.GCATPP.popSkyCloudCore((Building) parent);
                    break;
                }


                case "PowerTurnedOn":
                    bootGT = Find.TickManager.TicksGame + Settings.secToBootSkyCloudCore * 60;
                    break;
            }
        }


        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var build = (Building) parent;


            if (!storedMinds.Any() || !build.TryGetComp<CompPowerTrader>().PowerOn || !Booted()) yield break;

            yield return new Command_Action
            {
                icon = Tex.processInfo,
                defaultLabel = "ATPP_ProcessInfo".Translate(),
                defaultDesc = "ATPP_ProcessInfoDesc".Translate(),
                action = delegate { showFloatMenuMindsStored(delegate(Pawn p) { Find.WindowStack.Add(new Dialog_InfoCard(p)); }, false, false, false, true); }
            };

            yield return new Command_Action
            {
                icon = Tex.processRemove,
                defaultLabel = "ATPP_ProcessRemove".Translate(),
                defaultDesc = "ATPP_ProcessRemoveDesc".Translate(),
                action = delegate
                {
                    showFloatMenuMindsStored(delegate(Pawn p)
                    {
                        Find.WindowStack.Add(new Dialog_Msg("ATPP_ProcessRemove".Translate(), "ATPP_ProcessRemoveDescConfirm".Translate(p.LabelShortCap, getName()), delegate
                        {
                            stopMindActivities(p);

                            RemoveMind(p);
                            p.Kill(null);

                            Messages.Message("ATPP_ProcessRemoveOK".Translate(p.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);

                            Utils.playVocal("soundDefSkyCloudMindDeletionCompleted");
                        }));
                    });
                }
            };

            yield return new Command_Action
            {
                icon = Tex.processDuplicate,
                defaultLabel = "ATPP_ProcessDuplicate".Translate(),
                defaultDesc = "ATPP_ProcessDuplicateDesc".Translate(),
                action = delegate
                {
                    showFloatMenuMindsStored(delegate(Pawn p)
                    {
                        var cso = p.TryGetComp<CompSurrogateOwner>();
                        if (cso == null)
                            return;

                        var GT = Find.TickManager.TicksGame;

                        cso.replicationStartGT = GT;
                        cso.replicationEndingGT = GT + Settings.mindReplicationHours * 2500;

                        replicatingMinds.Add(p);
                        stopMindActivities(p);

                        Messages.Message("ATPP_ProcessDuplicateOK".Translate(p.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
                    });
                }
            };

            yield return new Command_Action
            {
                icon = Tex.processAssist,
                defaultLabel = "ATPP_ProcessAssist".Translate(),
                defaultDesc = "ATPP_ProcessAssistDesc".Translate(),
                action = delegate
                {
                    var opts = new List<FloatMenuOption>();

                    opts.Add(new FloatMenuOption("ATPP_ProcessAssistAssignedMinds".Translate(), delegate
                    {
                        List<FloatMenuOption> optsAdd = null;


                        if (assistingMinds.Count > 0)
                            optsAdd = new List<FloatMenuOption>
                            {
                                new FloatMenuOption("-" + "ATPP_ProcessAssistUnassignAll".Translate(), delegate
                                {
                                    var nb = 0;
                                    foreach (var m in storedMinds.Where(m => assistingMinds.Contains(m)))
                                    {
                                        assistingMinds.Remove(m);
                                        nb++;
                                    }

                                    if (nb > 0)
                                        Messages.Message("ATPP_ProcessMassUnassist".Translate(nb), parent, MessageTypeDefOf.PositiveEvent);
                                })
                            };

                        showFloatMenuMindsStored(delegate(Pawn p)
                        {
                            assistingMinds.Remove(p);

                            Messages.Message("ATPP_ProcessUnassistOK".Translate(p.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
                        }, false, false, false, false, optsAdd, false, true);
                    }));


                    opts.Add(new FloatMenuOption("ATPP_ProcessAssistUnassignedMinds".Translate(), delegate
                    {
                        List<FloatMenuOption> optsAdd = null;


                        if (storedMinds.Count > 0 && getNbUnassistingMinds() > 0)
                            optsAdd = new List<FloatMenuOption>
                            {
                                new FloatMenuOption("-" + "ATPP_ProcessAssistAssignAll".Translate(), delegate
                                {
                                    var nb = 0;
                                    foreach (var m in storedMinds.Where(m => !assistingMinds.Contains(m)))
                                    {
                                        stopMindActivities(m);
                                        assistingMinds.Add(m);
                                        nb++;
                                    }

                                    if (nb > 0)
                                        Messages.Message("ATPP_ProcessMassAssist".Translate(nb), parent, MessageTypeDefOf.PositiveEvent);
                                })
                            };

                        showFloatMenuMindsStored(delegate(Pawn p)
                        {
                            stopMindActivities(p);
                            assistingMinds.Add(p);

                            Messages.Message("ATPP_ProcessAssistOK".Translate(p.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
                        }, false, false, false, false, optsAdd, true);
                    }));

                    var floatMenuMap = new FloatMenu(opts);
                    Find.WindowStack.Add(floatMenuMap);
                }
            };

            yield return new Command_Action
            {
                icon = Tex.processMigrate,
                defaultLabel = "ATPP_ProcessMigrate".Translate(),
                defaultDesc = "ATPP_ProcessMigrateDesc".Translate(),
                action = delegate
                {
                    showFloatMenuMindsStored(delegate(Pawn p)
                    {
                        Utils.ShowFloatMenuSkyCloudCores(delegate(Building core)
                        {
                            var cso = p.TryGetComp<CompSurrogateOwner>();
                            stopMindActivities(p);
                            cso.startMigration(core);
                        }, (Building) parent);
                    });
                }
            };

            yield return new Command_Action
            {
                icon = Tex.processSkillUp,
                defaultLabel = "ATPP_Skills".Translate(),
                defaultDesc = "ATPP_SkillsDesc".Translate(),
                action = delegate { showFloatMenuMindsStored(delegate(Pawn p) { Find.WindowStack.Add(new Dialog_SkillUp(p, true)); }, false, false, false, true); }
            };

            yield return new Command_Action
            {
                icon = Tex.AndroidToControlTarget,
                defaultLabel = "ATPP_AndroidToControlTarget".Translate(),
                defaultDesc = "ATPP_AndroidToControlTargetDesc".Translate(),
                action = delegate
                {
                    showFloatMenuMindsStored(delegate(Pawn p)
                    {
                        var opts = new List<FloatMenuOption>();
                        foreach (var m in Find.Maps)
                        {
                            string lib;
                            if (m == Find.CurrentMap)
                                lib = "ATPP_ThisCurrentMap".Translate(m.Parent.Label);
                            else
                                lib = m.Parent.Label;

                            opts.Add(new FloatMenuOption(lib, delegate
                            {
                                Current.Game.CurrentMap = m;
                                var x = new Designator_AndroidToControl(p, true);
                                Find.DesignatorManager.Select(x);
                            }));
                        }

                        if (opts.Count == 0) return;

                        {
                            if (opts.Count == 1)
                            {
                                var x = new Designator_AndroidToControl(p, true);
                                Find.DesignatorManager.Select(x);
                            }
                            else
                            {
                                var floatMenuMap = new FloatMenu(opts);
                                Find.WindowStack.Add(floatMenuMap);
                            }
                        }
                    }, true, true, false, false, null, true);
                }
            };

            if (Utils.isThereNotControlledSurrogateInCaravan())

                yield return new Command_Action
                {
                    icon = Tex.AndroidToControlTargetRecovery,
                    defaultLabel = "ATPP_AndroidToControlTargetRecoverCaravan".Translate(),
                    defaultDesc = "ATPP_AndroidToControlTargetRecoverCaravanDesc".Translate(),
                    action = delegate
                    {
                        showFloatMenuMindsStored(delegate(Pawn p)
                        {
                            Utils.ShowFloatMenuNotCOntrolledSurrogateInCaravan(p, delegate(Pawn sSX)
                            {
                                var cso = p.TryGetComp<CompSurrogateOwner>();
                                if (cso == null)
                                    return;

                                if (!Utils.GCATPP.isConnectedToSkyMind(sSX))

                                    if (!Utils.GCATPP.connectUser(sSX))
                                        return;

                                cso.setControlledSurrogate(sSX);
                            });
                        }, true, true, false, false, null, true);
                    }
                };

            if (getNbMindsConnectedToSurrogate() != 0 || controlledTurrets.Count() != 0)
                yield return new Command_Action
                {
                    icon = Tex.AndroidToControlTargetDisconnect,
                    defaultLabel = "ATPP_AndroidToControlTargetDisconnect".Translate(),
                    defaultDesc = "ATPP_AndroidToControlTargetDisconnectDesc".Translate(),
                    action = delegate
                    {
                        var opts = new List<FloatMenuOption>
                        {
                            new FloatMenuOption("ATPP_ProcessDisconnectAllSurrogates".Translate(), delegate
                            {
                                disconnectAllSurrogates();
                                disconnectAllRemotelyControlledTurrets();
                                Utils.playVocal("soundDefSkyCloudAllMindDisconnected");
                            })
                        };

                        showFloatMenuMindsStored(delegate(Pawn p)
                        {
                            var cso = p.TryGetComp<CompSurrogateOwner>();
                            if (cso != null && cso.isThereSX()) cso.disconnectControlledSurrogate(null);
                            stopRemotelyControlledTurret(p);
                        }, false, false, true, false, opts);
                    }
                };
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";
            var build = (Building) parent;

            if (parent.Map == null)
                return base.CompInspectStringExtra();

            ret += getName() + "\n";
            ret += "ATPP_CentralCoreNbStoredMind".Translate(storedMinds.Count) + "\n";
            ret += "ATPP_CentralCoreNbAssistingMinds".Translate(assistingMinds.Count) + "\n";

            if (!build.TryGetComp<CompPowerTrader>().PowerOn) return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();

            if (!Booted())
                ret += "ATPP_SkyCloudCoreBooting".Translate((int) Math.Max(0, bootGT - Find.TickManager.TicksGame).TicksToSeconds());
            else

                foreach (var m in storedMinds)
                {
                    var cso = m.TryGetComp<CompSurrogateOwner>();

                    if (cso == null)
                        continue;

                    if (cso.replicationEndingGT != -1)
                    {
                        var p = Math.Min(1.0f, (Find.TickManager.TicksGame - cso.replicationStartGT) / (float) (cso.replicationEndingGT - cso.replicationStartGT));

                        ret += "=>" + "ATPP_CentralCoreReplicationInProgress".Translate(m.LabelShortCap, ((int) (p * 100)).ToString()) + "\n";
                    }


                    else if (cso.migrationEndingGT != -1 && cso.migrationSkyCloudHostDest != null)
                    {
                        var csc2 = cso.migrationSkyCloudHostDest.TryGetComp<CompSkyCloudCore>();
                        var p = Math.Min(1.0f, (Find.TickManager.TicksGame - cso.migrationStartGT) / (float) (cso.migrationEndingGT - cso.migrationStartGT));

                        ret += "=>" + "ATPP_CentralCoreMigrationInProgress".Translate(m.LabelShortCap, csc2.getName(), ((int) (p * 100)).ToString()) + "\n";
                    }

                    else if (inMentalBreak.ContainsKey(m))
                    {
                        ret += "=>" + "ATPP_CentralCoreProcessInMentalBreak".Translate(m.LabelShortCap) + "\n";
                    }
                }

            return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
        }

        public bool Booted()
        {
            return bootGT == -1;
        }

        public override void CompTick()
        {
            if (!parent.Spawned) return;

            var CGT = Find.TickManager.TicksGame;
            if (CGT % 60 == 0)
            {
                refreshPowerConsumed();


                if (bootGT > 0 && bootGT < CGT)
                {
                    bootGT = -1;

                    if (((Building) parent).TryGetComp<CompPowerTrader>().PowerOn)
                        Utils.GCATPP.pushSkyCloudCore((Building) parent);


                    Find.LetterStack.ReceiveLetter("ATPP_SkyCloudCoreBooted".Translate(getName()), "ATPP_SkyCloudCoreBootedDesc".Translate(getName()), LetterDefOf.NeutralEvent,
                        parent);


                    Utils.playVocal("soundDefSkyCloudPrimarySystemsOnline");
                }
            }

            if (CGT % 120 == 0)
            {
                List<CompSurrogateOwner> migrationsDone = null;
                List<CompSurrogateOwner> replicationDone = null;

                foreach (var cso in storedMinds.Select(m => m.TryGetComp<CompSurrogateOwner>()).Where(cso => cso != null))
                {
                    cso.checkInterruptedUpload();


                    if (cso.replicationEndingGT != -1 && cso.replicationEndingGT < CGT)
                    {
                        if (replicationDone == null)
                            replicationDone = new List<CompSurrogateOwner>();
                        replicationDone.Add(cso);
                    }

                    if (cso.migrationEndingGT == -1 || cso.migrationEndingGT >= CGT) continue;

                    if (migrationsDone == null)
                        migrationsDone = new List<CompSurrogateOwner>();
                    migrationsDone.Add(cso);
                }

                if (migrationsDone != null)
                {
                    foreach (var md in migrationsDone) md.OnMigrated();

                    Utils.playVocal("soundDefSkyCloudMindMigrationCompleted");
                }

                if (replicationDone != null)
                {
                    foreach (var md in replicationDone) md.OnReplicate();

                    Utils.playVocal("soundDefSkyCloudMindReplicationCompleted");
                }
            }

            if (CGT % 600 != 0) return;

            if (!((Building) parent).TryGetComp<CompPowerTrader>().PowerOn || inMentalBreak.Count <= 0) return;

            var keys = new List<Pawn>(inMentalBreak.Keys);
            foreach (var ck in keys)
            {
                inMentalBreak[ck] -= 600;
                if (inMentalBreak[ck] > 0) continue;

                inMentalBreak.Remove(ck);

                Messages.Message("ATPP_ProcessNoLongerInMentalBreak".Translate(ck.LabelShortCap, getName()), parent, MessageTypeDefOf.PositiveEvent);
            }
        }

        public void setMentalBreak(Pawn mind)
        {
            if (!storedMinds.Contains(mind) || inMentalBreak.ContainsKey(mind))
                return;


            mind.mindState.Reset();


            if (mind.IsSurrogateAndroid())
            {
                var cas = mind.TryGetComp<CompAndroidState>();
                if (cas == null || cas.surrogateController == null)
                    return;

                mind = cas.surrogateController;
            }


            stopMindActivities(mind);

            inMentalBreak[mind] = Rand.Range(Settings.minDurationMentalBreakOfDigitisedMinds, Settings.maxDurationMentalBreakOfDigitisedMinds) * 2500;
            Utils.playVocal("soundDefSkyCloudMindQuarantineMentalState");
        }

        public int getNbMindsConnectedToSurrogate()
        {
            return storedMinds.Select(m => m.TryGetComp<CompSurrogateOwner>()).Count(cso => cso != null && cso.isThereSX());
        }

        public void stopAllMindsActivities(bool serverShutdown = false)
        {
            foreach (var m in storedMinds) stopMindActivities(m, serverShutdown);
        }

        public void stopMindActivities(Pawn mind, bool serverShutdown = false)
        {
            stopRemotelyControlledTurret(mind);
            var cso = mind.TryGetComp<CompSurrogateOwner>();
            if (cso != null && cso.isThereSX()) cso.disconnectControlledSurrogate(null);

            if (!serverShutdown && assistingMinds.Contains(mind))
                assistingMinds.Remove(mind);
        }

        public bool mindIsBusy(Pawn mind)
        {
            var cso = mind.TryGetComp<CompSurrogateOwner>();
            return controlledTurrets.ContainsKey(mind) || replicatingMinds.Contains(mind) || inMentalBreak.ContainsKey(mind) || assistingMinds.Contains(mind) ||
                   cso != null && cso.migrationEndingGT != -1;
        }

        public bool isRunning()
        {
            var build = (Building) parent;

            return !build.Destroyed && !build.IsBrokenDown() && build.TryGetComp<CompPowerTrader>().PowerOn;
        }

        public string getName()
        {
            return "Core-" + SID;
        }

        public float getPowerConsumed()
        {
            return storedMinds.Count() * Settings.powerConsumedByStoredMind + parent.TryGetComp<CompPowerTrader>().Props.basePowerConsumption;
        }


        public int getNbUnassistingMinds()
        {
            return storedMinds.Count(m => !assistingMinds.Contains(m));
        }

        public void refreshPowerConsumed()
        {
            parent.TryGetComp<CompPowerTrader>().powerOutputInt = -getPowerConsumed();
        }


        public void setRemotelyControlledTurret(Pawn mind, Building turret)
        {
            if (!storedMinds.Contains(mind) || controlledTurrets.ContainsKey(mind))
                return;

            var crt = turret.TryGetComp<CompRemotelyControlledTurret>();
            crt.controller = mind;
            controlledTurrets[mind] = turret;

            Utils.soundDefTurretConnection.PlayOneShot(null);
            MoteMaker.ThrowDustPuffThick(turret.Position.ToVector3Shifted(), turret.Map, 4.0f, Color.blue);

            Messages.Message("ATPP_SurrogateConnectionOK".Translate(mind.LabelShortCap, turret.LabelShortCap), turret, MessageTypeDefOf.PositiveEvent);
        }

        public void stopRemotelyControlledTurret(Pawn mind)
        {
            if (!controlledTurrets.ContainsKey(mind))
                return;

            var crt = controlledTurrets[mind].TryGetComp<CompRemotelyControlledTurret>();
            crt.controller = null;

            controlledTurrets.Remove(mind);
            Utils.soundDefTurretConnectionStopped.PlayOneShot(null);
        }

        private void disconnectAllRemotelyControlledTurrets()
        {
            foreach (var e in controlledTurrets.ToList()) stopRemotelyControlledTurret(e.Key);
        }

        private void disconnectAllSurrogates()
        {
            foreach (var cso in storedMinds.Select(m => m.TryGetComp<CompSurrogateOwner>())) cso?.stopControlledSurrogate(null);
        }

        public void RemoveMind(Pawn mind)
        {
            stopRemotelyControlledTurret(mind);

            if (assistingMinds.Contains(mind))
                assistingMinds.Remove(mind);
            if (replicatingMinds.Contains(mind))
                replicatingMinds.Remove(mind);

            storedMinds.Remove(mind);
        }

        public void showFloatMenuMindsStored(Action<Pawn> onClick, bool hideSurrogate = false, bool hideTurretController = false, bool showOnlyConnectedDevices = false,
            bool resolveSurrogates = false, List<FloatMenuOption> supOpts = null, bool hideAssistingMinds = false, bool showOnlyAssistingMinds = false)
        {
            var opts = new List<FloatMenuOption>();

            if (supOpts != null) opts = opts.Concat(supOpts).ToList();


            foreach (var m in storedMinds)
            {
                var cso = m.TryGetComp<CompSurrogateOwner>();
                var isSurrogateController = cso != null && cso.isThereSX();
                var turretController = controlledTurrets.ContainsKey(m);
                var isAssistingMind = assistingMinds.Contains(m);


                if (inMentalBreak.ContainsKey(m))
                    continue;


                if (replicatingMinds.Contains(m))
                    continue;


                if (cso != null && cso.migrationEndingGT != -1)
                    continue;

                if (hideAssistingMinds && isAssistingMind || showOnlyAssistingMinds && !isAssistingMind)
                    continue;

                if (hideTurretController && turretController)
                    continue;

                if (hideSurrogate && isSurrogateController)
                    continue;

                var name = m.LabelShortCap;

                if (isSurrogateController)
                {
                    if (!cso.isThereSX())
                        name = m.LabelShortCap;
                    else
                        name = cso.SX != null ? cso.SX.LabelShortCap : m.LabelShortCap;
                }

                if (showOnlyConnectedDevices)
                    if (!isSurrogateController && !turretController)
                        continue;


                opts.Add(new FloatMenuOption(name, delegate
                {
                    var sel = m;
                    if (resolveSurrogates && isSurrogateController)
                    {
                        if (cso != null && cso.isThereSX())
                        {
                            if (cso.isThereSX())
                                sel = cso.SX ?? m;
                            else
                                sel = m;
                        }
                        else
                        {
                            sel = m;
                        }
                    }

                    onClick(sel);
                }));
            }

            opts.SortBy(x => x.Label);

            if (opts.Count == 0)
                return;

            var floatMenuMap = new FloatMenu(opts, "ATPP_SkyCloudFloatMenuMindsStoredSelect".Translate());
            Find.WindowStack.Add(floatMenuMap);
        }
    }
}