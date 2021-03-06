﻿using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace MOARANDROIDS
{
    public class CompComputer : ThingComp
    {
        private SoundDef ambiance;
        private bool isHackingServer;

        private bool isSecurityServer;
        private bool isSkillServer;
        private CompPowerTrader powerComp;

        private Sustainer sustainer;

        public CompProperties_Computer Props => (CompProperties_Computer) props;

        public override void PostExposeData()
        {
            base.PostExposeData();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerComp = parent.GetComp<CompPowerTrader>();

            if (Props.ambiance != "None")
                ambiance = SoundDef.Named(Props.ambiance);

            if (Utils.ExceptionSkillServers.Contains(parent.def.defName))
            {
                isSkillServer = true;
                Utils.GCATPP.pushSkillServer((Building) parent);
            }

            if (Utils.ExceptionSecurityServers.Contains(parent.def.defName))
            {
                isSecurityServer = true;
                Utils.GCATPP.pushSecurityServer((Building) parent);
            }

            if (Utils.ExceptionHackingServers.Contains(parent.def.defName))
            {
                isHackingServer = true;
                Utils.GCATPP.pushHackingServer((Building) parent);
            }

            if (!respawningAfterLoad) return;

            if (powerComp.PowerOn)
                StartSustainer();
        }

        public override void CompTick()
        {
            base.CompTick();
            var GT = Find.TickManager.TicksGame;


            if (isHackingServer && GT % 1800 == 0 && !(((Building) parent).IsBrokenDown() || !((Building) parent).TryGetComp<CompPowerTrader>().PowerOn))
                Utils.GCATPP.incHackingPoints(Utils.nbHackingPointsGeneratedBy((Building) parent));

            if (isSkillServer && GT % 1800 == 0 && !(((Building) parent).IsBrokenDown() || !((Building) parent).TryGetComp<CompPowerTrader>().PowerOn))
                Utils.GCATPP.incSkillPoints(Utils.nbSkillPointsGeneratedBy((Building) parent));
        }

        public override void ReceiveCompSignal(string signal)
        {
            var host = (Building) parent;
            switch (signal)
            {
                case "FlickedOff":
                case "ScheduledOff":
                case "Breakdown":
                case "PowerTurnedOff":
                {
                    if (isSkillServer)
                        Utils.GCATPP.popSkillServer(host);
                    if (isSecurityServer)
                        Utils.GCATPP.popSecurityServer(host);
                    if (isHackingServer)
                        Utils.GCATPP.popHackingServer(host);

                    StopSustainer();
                    break;
                }

                case "PowerTurnedOn":
                {
                    if (isSkillServer)
                        Utils.GCATPP.pushSkillServer(host);
                    if (isSecurityServer)
                        Utils.GCATPP.pushSecurityServer(host);
                    if (isHackingServer)
                        Utils.GCATPP.pushHackingServer(host);
                    StartSustainer();
                    break;
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var build = (Building) parent;


            if (Settings.disableSkyMindSecurityStuff || build.IsBrokenDown() || !build.TryGetComp<CompPowerTrader>().PowerOn)
                yield break;

            var nbp = Utils.GCATPP.getNbHackingPoints();

            bool canVirusExplosive, canHack, canTempHack;
            var canVirus = canVirusExplosive = canHack = canTempHack = false;

            var powered = !parent.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare) && !build.IsBrokenDown() && build.TryGetComp<CompPowerTrader>().PowerOn;


            if (!isHackingServer) yield break;

            if (nbp - Settings.costPlayerVirus >= 0)
                canVirus = true && powered;

            var tex = canVirus ? Tex.PlayerVirus : Tex.PlayerVirusDisabled;

            yield return new Command_Action
            {
                icon = tex,
                defaultLabel = "ATPP_UploadVirus".Translate(),
                defaultDesc = "ATPP_UploadVirusDesc".Translate(),
                action = delegate
                {
                    if (canVirus)
                        showFloatMapHackMenu(1);
                    else
                        Messages.Message("ATPP_CannotHackNotEnoughtHackingPoints".Translate(Settings.costPlayerVirus), MessageTypeDefOf.NegativeEvent);
                }
            };

            if (nbp - Settings.costPlayerExplosiveVirus >= 0)
                canVirusExplosive = true && powered;

            tex = canVirusExplosive ? Tex.PlayerExplosiveVirus : Tex.PlayerExplosiveVirusDisabled;

            yield return new Command_Action
            {
                icon = tex,
                defaultLabel = "ATPP_UploadExplosiveVirus".Translate(),
                defaultDesc = "ATPP_UploadExplosiveVirusDesc".Translate(),
                action = delegate
                {
                    if (canVirusExplosive)
                        showFloatMapHackMenu(2);
                    else
                        Messages.Message("ATPP_CannotHackNotEnoughtHackingPoints".Translate(Settings.costPlayerExplosiveVirus), MessageTypeDefOf.NegativeEvent);
                }
            };

            if (nbp - Settings.costPlayerHackTemp >= 0)
                canTempHack = true && powered;

            tex = canTempHack ? Tex.PlayerHackingTemp : Tex.PlayerHackingTempDisabled;

            yield return new Command_Action
            {
                icon = tex,
                defaultLabel = "ATPP_HackTemp".Translate(),
                defaultDesc = "ATPP_HackTempDesc".Translate(),
                action = delegate
                {
                    if (canTempHack)
                        showFloatMapHackMenu(3);
                    else
                        Messages.Message("ATPP_CannotHackNotEnoughtHackingPoints".Translate(Settings.costPlayerHackTemp), MessageTypeDefOf.NegativeEvent);
                }
            };

            if (nbp - Settings.costPlayerHack >= 0)
                canHack = true && powered;

            tex = canHack ? Tex.PlayerHacking : Tex.PlayerHackingDisabled;

            yield return new Command_Action
            {
                icon = tex,
                defaultLabel = "ATPP_Hack".Translate(),
                defaultDesc = "ATPP_HackDesc".Translate(),
                action = delegate
                {
                    if (canHack)
                        showFloatMapHackMenu(4);
                    else
                        Messages.Message("ATPP_CannotHackNotEnoughtHackingPoints".Translate(Settings.costPlayerHack), MessageTypeDefOf.NegativeEvent);
                }
            };
        }

        private void showFloatMapHackMenu(int hackType)
        {
            var opts = new List<FloatMenuOption>();
            foreach (var m in Find.Maps)
            {
                var lib = "";
                if (m == Find.CurrentMap)
                    lib = "ATPP_ThisCurrentMap".Translate(m.Parent.Label);
                else
                    lib = m.Parent.Label;

                opts.Add(new FloatMenuOption(lib, delegate
                {
                    Current.Game.CurrentMap = m;
                    var x = new Designator_SurrogateToHack(hackType);
                    Find.DesignatorManager.Select(x);
                }));
            }

            if (opts.Count == 0) return;
            {
                if (opts.Count == 1)
                {
                    var x = new Designator_SurrogateToHack(hackType);
                    Find.DesignatorManager.Select(x);
                }
                else
                {
                    var floatMenuMap = new FloatMenu(opts);
                    Find.WindowStack.Add(floatMenuMap);
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";

            if (parent.Map == null)
                return base.CompInspectStringExtra();

            if (isSecurityServer)
            {
                ret += "ATPP_SecurityServersSynthesis".Translate(Utils.GCATPP.getNbSlotSecurisedAvailable(), Utils.GCATPP.getNbThingsConnected()) + "\n";
                ret += "ATTP_SecuritySlotsAdded".Translate(Utils.nbSecuritySlotsGeneratedBy((Building) parent)) + "\n";
            }

            if (isHackingServer)
            {
                ret += "ATPP_HackingServersSynthesis".Translate(Utils.GCATPP.getNbHackingPoints(), Utils.GCATPP.getNbHackingSlotAvailable()) + "\n";
                ret += "ATTP_HackingProducedPoints".Translate(Utils.nbHackingPointsGeneratedBy((Building) parent)) + "\n";
                ret += "ATTP_HackingSlotsAdded".Translate(Utils.nbHackingSlotsGeneratedBy((Building) parent)) + "\n";
            }

            if (!isSkillServer) return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();

            ret += "ATPP_SkillServersSynthesis".Translate(Utils.GCATPP.getNbSkillPoints(), Utils.GCATPP.getNbSkillSlotAvailable()) + "\n";
            ret += "ATTP_SkillProducedPoints".Translate(Utils.nbSkillPointsGeneratedBy((Building) parent)) + "\n";
            ret += "ATTP_SkillSlotsAdded".Translate(Utils.nbSkillSlotsGeneratedBy((Building) parent)) + "\n";

            return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            StopSustainer();

            if (isSecurityServer) Utils.GCATPP.popSecurityServer((Building) parent);

            if (isHackingServer) Utils.GCATPP.popHackingServer((Building) parent);

            if (isSkillServer) Utils.GCATPP.popSkillServer((Building) parent);
        }


        private void StartSustainer()
        {
            if (sustainer != null || Props.ambiance == "None" || Settings.disableServersAmbiance) return;

            var info = SoundInfo.InMap(parent);
            sustainer = ambiance.TrySpawnSustainer(info);
        }

        private void StopSustainer()
        {
            if (sustainer == null || Props.ambiance == "None") return;

            sustainer.End();
            sustainer = null;
        }
    }
}