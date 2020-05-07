using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using Random = System.Random;

namespace MOARANDROIDS
{
    public class CompHeatSensitive : ThingComp
    {
        private int hotLevelInt;

        private bool isSkyCloudCore;

        private int nbTicksSinceHot3;

        private CompPowerTrader powerComp;

        //private SoundDef soundDefHot;
        private Sustainer sustainerHot;

        private int ticksBeforeMelt;

        public CompProperties_HeatSensitive Props => (CompProperties_HeatSensitive) props;

        public int hotLevel => hotLevelInt;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref hotLevelInt, "hotLevelInt");
            Scribe_Values.Look(ref ticksBeforeMelt, "ticksBeforeMelt");
            Scribe_Values.Look(ref nbTicksSinceHot3, "nbTicksSinceHot3");
        }

        public override void PostDraw()
        {
            Material iconMat = null;

            //Si temperature du device chaude
            if (powerComp.PowerOn && !parent.IsBrokenDown() && hotLevelInt != 0)
            {
                if (hotLevelInt == 1)
                    iconMat = Tex.matHotLevel1;
                else if (hotLevelInt == 2)
                    iconMat = Tex.matHotLevel2;
                else if (hotLevelInt == 3)
                    iconMat = Tex.matHotLevel3;

                var vector = parent.TrueCenter();
                vector.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.28125f;
                vector.x += parent.def.size.x / 4;

                vector.z -= 1;

                var num = (Time.realtimeSinceStartup + 397f * (parent.thingIDNumber % 571)) * 4f;
                var num2 = ((float) Math.Sin(num) + 1f) * 0.5f;
                num2 = 0.3f + num2 * 0.7f;
                var material = FadedMaterialPool.FadedVersionOf(iconMat, num2);
                Graphics.DrawMesh(MeshPool.plane05, vector, Quaternion.identity, material, 0);
            }
        }

        // Token: 0x06002851 RID: 10321 RVA: 0x00133CC9 File Offset: 0x001320C9
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            var rnd = new Random();

            base.PostSpawnSetup(respawningAfterLoad);
            if (parent != null)
                powerComp = parent.GetComp<CompPowerTrader>();

            if (Utils.ExceptionSkyCloudCores.Contains(parent.def.defName))
                isSkyCloudCore = true;

            //this.nbTicksSinceHot3 = 0;
            //this.hotLevelInt = 0;
            //Génération durée aléatoire 
            if (ticksBeforeMelt == 0)
                setNewExplosionThreshold();
            //this.soundDefHot = SoundDef.Named(this.Props.hotSoundDef);

            //Enregistrement dans la liste  dispositifs senseible a la temperature
            if (parent != null)
                Utils.GCATPP.pushHeatSensitiveDevices((Building) parent);
        }

        public void setNewExplosionThreshold()
        {
            if (isSkyCloudCore)
                ticksBeforeMelt = Rand.Range(Settings.nbHoursMinSkyCloudServerRunningHotBeforeExplode * 2500, Settings.nbHoursMaxSkyCloudServerRunningHotBeforeExplode * 2500);
            else
                ticksBeforeMelt = Rand.Range(Settings.nbHoursMinServerRunningHotBeforeExplode * 2500, Settings.nbHoursMaxServerRunningHotBeforeExplode * 2500);
        }

        public override void CompTick()
        {
            base.CompTick();

            if (Find.TickManager.TicksGame % 250 == 0) CheckTemperature();
        }

        public override void ReceiveCompSignal(string signal)
        {
            //Arret manuel ou  composant endommagé => arret ambiance
            if (signal == "FlickedOff" || signal == "Breakdown" || signal == "PowerTurnedOff")
            {
                if (hotLevelInt == 3)
                    StopSustainerHot();

                hotLevelInt = 0;
                nbTicksSinceHot3 = 0;
            }
        }


        private void CheckTemperature()
        {
            var currLevel = hotLevelInt;

            if (!powerComp.PowerOn || parent.IsBrokenDown())
            {
                if (currLevel == 3)
                    StopSustainerHot();
                return;
            }

            var ambientTemperature = parent.AmbientTemperature;

            //Détermine si le niveau de temperature du dispositif influe la hotLevelInt en fonction de ses props
            if (ambientTemperature >= Props.hot3)
            {
                hotLevelInt = 3;
                nbTicksSinceHot3 += 250;

                //On plait le bip bip que si nouveau level différent du level précédent
                if (currLevel != hotLevelInt) StartSustainerHot();
            }
            else
            {
                if (currLevel == 3)
                    StopSustainerHot();

                nbTicksSinceHot3 = 0;
                if (ambientTemperature >= Props.hot2)
                    hotLevelInt = 2;
                else if (ambientTemperature >= Props.hot1)
                    hotLevelInt = 1;
                else
                    hotLevelInt = 0;
            }

            //Meltingdown condition remplie on fait péter le serveur
            if (nbTicksSinceHot3 >= ticksBeforeMelt)
            {
                var rnd = new Random();
                //Reset le meltdown counter
                nbTicksSinceHot3 = 0;
                //Définition nouveau seuil d'explosion
                setNewExplosionThreshold();

                makeExplosion();
                Find.LetterStack.ReceiveLetter("ATPP_ComptHeatSensitiveComputerMeltTitle".Translate(), "ATPP_ComptHeatSensitiveComputerMeltDesc".Translate(),
                    LetterDefOf.NegativeEvent, new TargetInfo(parent.Position, parent.Map));
            }
        }

        public void makeExplosion()
        {
            //Passe le dispositif en broken
            if (parent != null)
            {
                var bd = parent.TryGetComp<CompBreakdownable>();
                if (bd != null)
                    bd.DoBreakdown();

                var b = (Building) parent;
                b.HitPoints -= (int) (b.HitPoints * Rand.Range(0.10f, 0.45f));

                if (isSkyCloudCore)
                    GenExplosion.DoExplosion(parent.Position, parent.Map, 8, DamageDefOf.Flame, null);
                else
                    GenExplosion.DoExplosion(parent.Position, parent.Map, 2, DamageDefOf.Flame, null);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            StopSustainerHot();

            //Desenregistrement dans la liste  dispositifs senseible a la temperature
            Utils.GCATPP.popHeatSensitiveDevices((Building) parent, map);
        }

        public override string CompInspectStringExtra()
        {
            if (parent == null)
                return "";

            if (powerComp != null && !powerComp.PowerOn)
                return "";

            if (hotLevelInt == 3)
                return "ATPP_CompHotSensitiveHot3Text".Translate();
            if (hotLevelInt == 2)
                return "ATPP_CompHotSensitiveHot2Text".Translate();
            if (hotLevelInt == 1)
                return "ATPP_CompHotSensitiveHot1Text".Translate();
            return "ATPP_CompHotSensitiveHot0Text".Translate();
        }


        private void StartSustainerHot()
        {
            if (sustainerHot == null && !Settings.disableServersAlarm)
            {
                var info = SoundInfo.InMap(parent);
                sustainerHot = Props.hotSoundDef.TrySpawnSustainer(info);
            }
        }

        private void StopSustainerHot()
        {
            if (sustainerHot != null)
            {
                sustainerHot.End();
                sustainerHot = null;
            }
        }
    }
}