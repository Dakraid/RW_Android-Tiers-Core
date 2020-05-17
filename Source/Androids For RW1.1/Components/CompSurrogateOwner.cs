using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MOARANDROIDS
{
    public class CompSurrogateOwner : ThingComp
    {
        public List<Pawn> availableSX = new List<Pawn>();


        public bool controlMode;
        public int downloadFromSkyCloudEndingGT = -1;

        public int downloadFromSkyCloudStartGT;
        public int duplicateEndingGT = -1;
        public Pawn duplicateRecipient;

        public int duplicateStartGT;

        public bool externalController = true;


        public List<Pawn> extraSX = new List<Pawn>();

        public int migrationEndingGT = -1;


        public Building migrationSkyCloudHostDest;
        public int migrationStartGT;
        public int mindAbsorptionEndingGT = -1;

        public int mindAbsorptionStartGT;
        public int permuteEndingGT = -1;
        public Pawn permuteRecipient;

        public int permuteStartGT;
        public SkillDef ransomwareSkillStolen;
        public int ransomwareSkillValue = -1;

        public TraitDef ransomwareTraitAdded;

        public bool repairAndroids;

        public int replicationEndingGT = -1;


        public int replicationStartGT;


        public List<string> savedSkillsBecauseM7Control;
        public List<string> savedWorkAffectationBecauseM7Control;
        public bool showDuplicateProgress;

        public bool showPermuteProgress;

        public Pawn skyCloudDownloadRecipient;

        public Building skyCloudHost;

        public Building skyCloudRecipient;


        public Pawn SX;
        public int uploadToSkyCloudEndingGT = -1;

        public int uploadToSkyCloudStartGT;

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref controlMode, "ATPP_controlMode");
            Scribe_References.Look(ref SX, "ATPP_SX");
            Scribe_References.Look(ref permuteRecipient, "ATPP_permuteRecipient");
            Scribe_References.Look(ref duplicateRecipient, "ATPP_duplicateRecipient");
            Scribe_References.Look(ref skyCloudRecipient, "ATPP_skyCloudRecipient");
            Scribe_References.Look(ref skyCloudDownloadRecipient, "ATPP_skyCloudDownloadRecipient");

            Scribe_Values.Look(ref repairAndroids, "ATPP_repairAndroids");

            Scribe_Values.Look(ref migrationEndingGT, "ATPP_migrationEndingGT", -1);
            Scribe_Values.Look(ref migrationStartGT, "ATPP_migrationStartGT");
            Scribe_References.Look(ref migrationSkyCloudHostDest, "ATPP_migrationSkyCloudHostDest");


            Scribe_Values.Look(ref mindAbsorptionEndingGT, "ATPP_mindAbsorptionEndingGT", -1);
            Scribe_Values.Look(ref mindAbsorptionStartGT, "ATPP_mindAbsorptionStartGT");

            Scribe_Values.Look(ref downloadFromSkyCloudEndingGT, "ATPP_downloadFromSkyCloudEndingGT", -1);
            Scribe_Values.Look(ref downloadFromSkyCloudStartGT, "ATPP_downloadFromSkyCloudStartGT");
            Scribe_Values.Look(ref uploadToSkyCloudEndingGT, "ATPP_uploadToSkyCloudEndingGT", -1);
            Scribe_Values.Look(ref uploadToSkyCloudStartGT, "ATPP_uploadToSkyCloudStartGT");
            Scribe_Values.Look(ref permuteEndingGT, "ATPP_permuteEndingGT", -1);
            Scribe_Values.Look(ref permuteStartGT, "ATPP_permuteStartGT");
            Scribe_Values.Look(ref duplicateEndingGT, "ATPP_duplicateEndingGT", -1);
            Scribe_Values.Look(ref duplicateStartGT, "ATPP_duplicateStartGT");
            Scribe_Values.Look(ref showPermuteProgress, "ATPP_showPermuteProgress");
            Scribe_Values.Look(ref showDuplicateProgress, "ATPP_showDuplicateProgress");

            Scribe_Values.Look(ref replicationStartGT, "ATPP_replicationStartGT");
            Scribe_Values.Look(ref replicationEndingGT, "ATPP_replicationEndingGT", -1);


            Scribe_References.Look(ref skyCloudHost, "ATPP_skyCloudHost");
            Scribe_Defs.Look(ref ransomwareSkillStolen, "ATPP_ransomwareSkillStolen");
            Scribe_Values.Look(ref ransomwareSkillValue, "ATPP_ransomwareSkillValue", -1);
            Scribe_Defs.Look(ref ransomwareTraitAdded, "ATPP_ransomwareTraitAdded");
            Scribe_Values.Look(ref externalController, "ATPP_CSIOExternalController");
            Scribe_Collections.Look(ref savedSkillsBecauseM7Control, "ATPP_savedSkillsBecauseM7Control", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref savedWorkAffectationBecauseM7Control, "ATPP_savedWorkAffectationBecauseM7Control", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref extraSX, false, "ATPP_extraSX", LookMode.Reference);

            if (Scribe.mode != LoadSaveMode.PostLoadInit) return;

            if (extraSX == null)
                extraSX = new List<Pawn>();


            if (SX != null)
                availableSX.Add(SX);
            if (extraSX.Count > 0)
                availableSX = availableSX.Concat(extraSX).ToList();

            availableSX.RemoveAll(item => item == null);
        }

        public override void CompTick()
        {
            base.CompTick();

            if (parent.Map == null || !parent.Spawned)
                return;

            var GT = Find.TickManager.TicksGame;

            if (GT % 600 == 0)
            {
                var cpawn = (Pawn) parent;
                if (cpawn.IsKidnapped()) Utils.GCATPP.disconnectUser(cpawn);
            }

            if (GT % 120 != 0 || permuteEndingGT == -1 && duplicateEndingGT == -1 && uploadToSkyCloudEndingGT == -1 && migrationEndingGT == -1 && mindAbsorptionEndingGT == -1 &&
                downloadFromSkyCloudEndingGT == -1 && (!controlMode || SX == null)) return;
            {
                var cpawn = (Pawn) parent;
                checkInterruptedUpload();


                if (permuteEndingGT != -1 && permuteEndingGT < GT)
                {
                    checkInterruptedUpload();
                    if (permuteEndingGT == -1)
                        return;

                    permuteEndingGT = -1;
                    permuteRecipient.TryGetComp<CompSurrogateOwner>().permuteEndingGT = -1;

                    Utils.removeUploadHediff(cpawn, permuteRecipient);

                    Find.LetterStack.ReceiveLetter("ATPP_LetterPermuteOK".Translate(), "ATPP_LetterPermuteOKDesc".Translate(cpawn.LabelShortCap, permuteRecipient.LabelShortCap),
                        LetterDefOf.PositiveEvent, parent);

                    Utils.PermutePawn(cpawn, permuteRecipient);


                    Utils.clearBlankAndroid(permuteRecipient);


                    if (permuteRecipient.def.defName == Utils.T1)
                        Utils.addSimpleMindedTraitForT1(permuteRecipient);
                    else
                        Utils.removeSimpleMindedTrait(permuteRecipient);


                    if (cpawn.def.defName == Utils.T1)
                        Utils.addSimpleMindedTraitForT1(cpawn);
                    else
                        Utils.removeSimpleMindedTrait(cpawn);

                    if (cpawn.IsPrisoner || permuteRecipient.IsPrisoner)
                        dealWithPrisonerRecipientPermute(cpawn, permuteRecipient);

                    resetUploadStuff();
                }


                if (duplicateEndingGT != -1 && duplicateEndingGT < GT)
                {
                    checkInterruptedUpload();
                    if (duplicateEndingGT == -1)
                        return;

                    duplicateEndingGT = -1;
                    duplicateRecipient.TryGetComp<CompSurrogateOwner>().duplicateEndingGT = -1;

                    Utils.removeUploadHediff(cpawn, duplicateRecipient);

                    Find.LetterStack.ReceiveLetter("ATPP_LetterDuplicateOK".Translate(),
                        "ATPP_LetterDuplicateOKDesc".Translate(cpawn.LabelShortCap, duplicateRecipient.LabelShortCap), LetterDefOf.PositiveEvent, parent);

                    Utils.Duplicate(cpawn, duplicateRecipient);


                    Utils.clearBlankAndroid(duplicateRecipient);


                    if (duplicateRecipient.def.defName == Utils.T1)
                        Utils.addSimpleMindedTraitForT1(duplicateRecipient);
                    else
                        Utils.removeSimpleMindedTrait(duplicateRecipient);


                    dealWithPrisonerRecipient(cpawn, duplicateRecipient);


                    resetUploadStuff();
                }


                if (uploadToSkyCloudEndingGT != -1 && uploadToSkyCloudEndingGT < GT)
                {
                    checkInterruptedUpload();
                    if (uploadToSkyCloudEndingGT == -1)
                        return;

                    uploadToSkyCloudEndingGT = -1;
                    Utils.removeUploadHediff(cpawn, null);

                    var csc = skyCloudRecipient.TryGetComp<CompSkyCloudCore>();

                    Find.LetterStack.ReceiveLetter("ATPP_LetterSkyCloudUploadOK".Translate(), "ATPP_LetterSkyCloudUploadOKDesc".Translate(cpawn.LabelShortCap, csc.getName()),
                        LetterDefOf.PositiveEvent, parent);


                    csc.storedMinds.Add(cpawn);


                    Utils.removeMindBlacklistedTrait(cpawn);


                    Utils.removeSimpleMindedTrait(cpawn);


                    var killMode = Settings.skyCloudUploadModeForSourceMind == 2;

                    var corpse = Utils.spawnCorpseCopy(cpawn, killMode);


                    if (Settings.skyCloudUploadModeForSourceMind == 1)
                    {
                        foreach (var he in corpse.health.hediffSet.hediffs.ToList().Where(he => he.def == Utils.hediffHaveVX2Chip || he.def == Utils.hediffHaveVX3Chip))
                            corpse.health.RemoveHediff(he);

                        BodyPartRecord bpr = null;
                        bpr = corpse.health.hediffSet.GetBrain();


                        Utils.preventVX0Thought = true;
                        corpse.health.AddHediff(Utils.hediffHaveVX0Chip, bpr);
                        Utils.preventVX0Thought = false;
                    }


                    Utils.GCATPP.disconnectUser(cpawn);


                    cpawn.DeSpawn();

                    skyCloudHost = skyCloudRecipient;


                    Utils.playVocal("soundDefSkyCloudMindDownloadCompleted");


                    resetUploadStuff();
                }


                if (downloadFromSkyCloudEndingGT != -1 && downloadFromSkyCloudEndingGT < GT)
                {
                    checkInterruptedUpload();
                    if (downloadFromSkyCloudEndingGT == -1)
                        return;

                    downloadFromSkyCloudEndingGT = -1;
                    Utils.removeUploadHediff(cpawn, null);

                    var cso = skyCloudDownloadRecipient.TryGetComp<CompSurrogateOwner>();
                    if (cso.skyCloudHost == null)
                        return;

                    var csc = cso.skyCloudHost.TryGetComp<CompSkyCloudCore>();


                    csc.stopMindActivities(skyCloudDownloadRecipient);


                    Find.LetterStack.ReceiveLetter("ATPP_LetterSkyCloudDownloadOK".Translate(),
                        "ATPP_LetterSkyCloudDownloadOKDesc".Translate(skyCloudDownloadRecipient.LabelShortCap, cpawn.LabelShortCap), LetterDefOf.PositiveEvent, parent);


                    Utils.PermutePawn(skyCloudDownloadRecipient, cpawn);


                    var cas = skyCloudDownloadRecipient.TryGetComp<CompAndroidState>();
                    if (cas != null)
                        cas.isBlankAndroid = true;


                    Utils.clearBlankAndroid(cpawn);


                    Utils.addSimpleMindedTraitForT1(cpawn);


                    csc.RemoveMind(skyCloudDownloadRecipient);


                    dealWithPrisonerRecipientPermute(cpawn, skyCloudDownloadRecipient);

                    Utils.playVocal("soundDefSkyCloudMindUploadCompleted");


                    skyCloudDownloadRecipient.Kill(null);

                    resetUploadStuff();
                }

                if (mindAbsorptionEndingGT == -1 || mindAbsorptionEndingGT >= GT) return;

                var average = 0;
                var sum = cpawn.skills.skills.Sum(sr => sr.levelInt);
                average = (int) (sum / (float) cpawn.skills.skills.Count());

                var nbp = (int) (average / (float) 20 * Rand.Range(1000, 5000));

                Utils.GCATPP.incSkillPoints(nbp);

                Find.LetterStack.ReceiveLetter("ATPP_MindAbsorptionDone".Translate(), "ATPP_MindAbsorptionDoneDesc".Translate(cpawn.LabelShortCap, nbp),
                    LetterDefOf.PositiveEvent, cpawn);

                resetUploadStuff();

                cpawn.Kill(null);
            }
        }

        public void dealWithPrisonerRecipient(Pawn cpawn, Pawn recipient)
        {
            if (!cpawn.IsPrisoner && recipient.IsPrisoner)
            {
                if (recipient.Faction != Faction.OfPlayer) recipient.SetFaction(Faction.OfPlayer);

                if (recipient.guest != null) recipient.guest.SetGuestStatus(null);
            }


            if (!cpawn.IsPrisoner || recipient.IsPrisoner) return;

            if (recipient.Faction != cpawn.Faction) recipient.SetFaction(cpawn.Faction);

            recipient.guest?.SetGuestStatus(Faction.OfPlayer, true);
        }

        public void dealWithPrisonerRecipientPermute(Pawn cpawn, Pawn recipient)
        {
            if (!cpawn.IsPrisoner && recipient.IsPrisoner)
            {
                var tmp = recipient.Faction;

                if (recipient.Faction != Faction.OfPlayer)
                    recipient.SetFaction(Faction.OfPlayer);

                recipient.guest?.SetGuestStatus(null);

                if (cpawn.Faction != tmp)
                    cpawn.SetFaction(tmp);

                cpawn.guest?.SetGuestStatus(Faction.OfPlayer, true);

                recipient.workSettings?.EnableAndInitialize();
            }
            else

            {
                var tmp = cpawn.Faction;

                if (cpawn.Faction != Faction.OfPlayer)
                    cpawn.SetFaction(Faction.OfPlayer);

                cpawn.guest?.SetGuestStatus(null);

                if (recipient.Faction != tmp)
                    recipient.SetFaction(tmp);

                recipient.guest?.SetGuestStatus(Faction.OfPlayer, true);

                cpawn.workSettings?.EnableAndInitialize();
            }
        }

        public void OnReplicate()
        {
            var cpawn = (Pawn) parent;
            checkInterruptedUpload();
            if (replicationEndingGT == -1)
                return;

            replicationEndingGT = -1;
            var csc = skyCloudHost.TryGetComp<CompSkyCloudCore>();

            var request = new PawnGenerationRequest(cpawn.kindDef, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true,
                true, false, true, false, false, fixedBiologicalAge: cpawn.ageTracker.AgeBiologicalYearsFloat, fixedChronologicalAge: cpawn.ageTracker.AgeChronologicalYearsFloat,
                fixedGender: cpawn.gender, fixedMelanin: cpawn.story.melanin);
            var clone = PawnGenerator.GeneratePawn(request);


            foreach (var h in clone.health.hediffSet.hediffs.ToList()) clone.health.RemoveHediff(h);

            var cso = clone.TryGetComp<CompSurrogateOwner>();
            cso.skyCloudHost = skyCloudHost;

            Utils.Duplicate(cpawn, clone, false);


            try
            {
                var baseName = clone.LabelShort;
                var start = 1;
                var tmp = 0;

                foreach (var result in csc.storedMinds.Where(m => m.LabelShort.StartsWith(baseName)).Select(m => Regex.Match(m.LabelShort, @" \d+$", RegexOptions.RightToLeft))
                    .Where(result => result.Success))
                {
                    if (!int.TryParse(result.Value, out var tmp2)) continue;

                    if (tmp2 > tmp)
                        tmp = tmp2;
                }

                if (tmp > 0)
                {
                    start = tmp + 1;

                    var idx = baseName.LastIndexOf(' ');
                    if (idx != -1) baseName = baseName.Substring(0, idx);
                }

                for (var i = start;; i++)
                {
                    var destName = baseName + " " + i;
                    var ok = csc.storedMinds.All(m => m.LabelShort != destName);

                    if (!ok) continue;

                    var nt = (NameTriple) clone.Name;
                    clone.Name = new NameTriple(nt.First, destName, nt.Last);
                    break;
                }
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] ReplicateMind.SetNewName " + e.Message + " " + e.StackTrace);
            }

            csc.storedMinds.Add(clone);


            csc.replicatingMinds.Remove(cpawn);

            Find.LetterStack.ReceiveLetter("ATPP_LetterSkyCloudReplicateOK".Translate(),
                "ATPP_LetterSkyCloudReplicateOKDesc".Translate(cpawn.LabelShortCap, skyCloudHost.TryGetComp<CompSkyCloudCore>().getName()), LetterDefOf.PositiveEvent, parent);

            resetUploadStuff();
        }

        public void OnMigrated()
        {
            var cpawn = (Pawn) parent;
            checkInterruptedUpload();
            if (migrationEndingGT == -1)
                return;

            migrationEndingGT = -1;

            var csc = skyCloudHost.TryGetComp<CompSkyCloudCore>();
            var csc2 = migrationSkyCloudHostDest.TryGetComp<CompSkyCloudCore>();


            csc.RemoveMind(cpawn);
            csc2.storedMinds.Add(cpawn);
            skyCloudHost = migrationSkyCloudHostDest;

            Find.LetterStack.ReceiveLetter("ATPP_LetterSkyCloudMigrateOK".Translate(),
                "ATPP_LetterSkyCloudMigrateOKDesc".Translate(cpawn.LabelShortCap, csc.getName(), csc2.getName()), LetterDefOf.PositiveEvent, skyCloudHost);
            resetUploadStuff();
        }

        public override void PostDraw()
        {
            Material avatar = null;

            if (permuteEndingGT != -1 || showPermuteProgress || duplicateEndingGT != -1 || showDuplicateProgress || uploadToSkyCloudEndingGT != -1 ||
                downloadFromSkyCloudEndingGT != -1)
                avatar = Tex.UploadInProgress;

            if (avatar == null) return;

            var vector = parent.TrueCenter();
            vector.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.28125f;
            vector.z += 1.4f;
            vector.x += parent.def.size.x / 2;

            Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, avatar, 0);
        }


        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            if (controlMode && SX != null)
            {
                disconnectControlledSurrogate(null);
                controlMode = false;
            }

            var cpawn = (Pawn) parent;

            var he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
            if (he != null)
            {
                cpawn.health.hediffSet.hediffs.Remove(he);
                he.PostRemoved();
            }

            he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_InRemoteControlMode"));
            if (he == null) return;

            cpawn.health.hediffSet.hediffs.Remove(he);
            he.PostRemoved();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            base.PostDrawExtraSelectionOverlays();

            if (permuteEndingGT != -1 && permuteRecipient != null || showPermuteProgress)
                GenDraw.DrawLineBetween(parent.TrueCenter(), permuteRecipient.TrueCenter(), SimpleColor.Green);

            if (duplicateEndingGT != -1 && duplicateRecipient != null || showDuplicateProgress)
                GenDraw.DrawLineBetween(parent.TrueCenter(), duplicateRecipient.TrueCenter(), SimpleColor.Green);


            if (uploadToSkyCloudEndingGT != -1 && skyCloudRecipient.Map == parent.Map)
                GenDraw.DrawLineBetween(parent.TrueCenter(), skyCloudRecipient.TrueCenter(), SimpleColor.Green);


            if (!controlMode) return;

            if (SX != null && SX.Map == parent.Map) GenDraw.DrawLineBetween(parent.TrueCenter(), SX.Position.ToVector3(), SimpleColor.Blue);
            if (extraSX.Count <= 0) return;

            foreach (var e in extraSX.Where(e => e.Map == parent.Map))
                GenDraw.DrawLineBetween(parent.TrueCenter(), e.Position.ToVector3(), SimpleColor.Blue);
        }

        private void toggleControlMode()
        {
            var cpawn = (Pawn) parent;


            if (duplicateEndingGT != -1 || permuteEndingGT != -1)
            {
                controlMode = false;
                return;
            }

            controlMode = !controlMode;
            if (controlMode)
            {
                addRemoteControlHediff();
            }
            else
            {
                disconnectControlledSurrogate(null);
                removeRemoteControlHediff();
            }
        }

        private void addRemoteControlHediff()
        {
            var cpawn = (Pawn) parent;
            cpawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_InRemoteControlMode"));
        }

        private void removeRemoteControlHediff()
        {
            var cpawn = (Pawn) parent;
            var he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_InRemoteControlMode"));
            if (he != null)
                cpawn.health.RemoveHediff(he);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var pawn = (Pawn) parent;
            var isPrisonner = pawn.IsPrisoner;
            var cas = pawn.TryGetComp<CompAndroidState>();
            var isBlankAndroid = cas != null && cas.isBlankAndroid;


            if (!isPrisonner && !(cas != null && cas.isSurrogate && cas.surrogateController == null) && pawn.workSettings != null &&
                pawn.workSettings.WorkIsActive(Utils.WorkTypeDefSmithing) && Settings.androidsCanOnlyBeHealedByCrafter)
                yield return new Command_Toggle
                {
                    icon = Tex.RepairAndroid,
                    defaultLabel = "ATPP_RepairAndroids".Translate(),
                    defaultDesc = "ATPP_RepairAndroidsDesc".Translate(),
                    isActive = () => repairAndroids,
                    toggleAction = delegate { repairAndroids = !repairAndroids; }
                };


            if (!pawn.VXChipPresent())
                yield break;


            if (cas != null && cas.isSurrogate)
                yield break;

            if (!isPrisonner && !isBlankAndroid)
                yield return new Command_Toggle
                {
                    icon = Tex.SurrogateMode,
                    defaultLabel = "ATPP_EnableSurrogateControlMode".Translate(),
                    defaultDesc = "ATPP_EnableSurrogateControlModeDesc".Translate(),
                    isActive = () => controlMode,
                    toggleAction = delegate { toggleControlMode(); }
                };

            if (!Utils.GCATPP.isConnectedToSkyMind(pawn))
                yield break;


            var transfertAllowed = Utils.mindTransfertsAllowed((Pawn) parent);

            if (isPrisonner)


                if (mindAbsorptionEndingGT == -1 && Utils.GCATPP.isThereSkillServers())
                {
                    var tex = Tex.MindAbsorption;
                    if (!transfertAllowed)
                        tex = Tex.MindAbsorptionDisabled;

                    var allowed = transfertAllowed;
                    yield return new Command_Action
                    {
                        icon = tex,
                        defaultLabel = "ATPP_MindAbsorption".Translate(),
                        defaultDesc = "ATPP_MindAbsorptionDesc".Translate(),
                        action = delegate
                        {
                            if (!allowed)
                                return;

                            Find.WindowStack.Add(new Dialog_Msg("ATPP_MindAbsorptionConfirm".Translate(),
                                "ATPP_MindAbsorptionConfirmDesc".Translate(parent.LabelShortCap) + "\n" + "ATPP_WarningSkyMindDisconnectionRisk".Translate(), delegate
                                {
                                    mindAbsorptionStartGT = Find.TickManager.TicksGame;
                                    mindAbsorptionEndingGT = mindAbsorptionStartGT + 5000;

                                    Messages.Message("ATPP_MindAbsorptionStarted".Translate(pawn.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
                                }));
                        }
                    };
                }


            if (controlMode && !isPrisonner)
            {
                if (SX == null && availableSX.Count == 0 || pawn.VX3ChipPresent() && availableSX.Count < Settings.VX3MaxSurrogateControllableAtOnce)
                {
                    yield return new Command_Action
                    {
                        icon = Tex.AndroidToControlTarget,
                        defaultLabel = "ATPP_AndroidToControlTarget".Translate(),
                        defaultDesc = "ATPP_AndroidToControlTargetDesc".Translate(),
                        action = delegate
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
                                    var x = new Designator_AndroidToControl((Pawn) parent);
                                    Find.DesignatorManager.Select(x);
                                }));
                            }

                            if (opts.Count == 0) return;

                            {
                                if (opts.Count == 1)
                                {
                                    var x = new Designator_AndroidToControl((Pawn) parent);
                                    Find.DesignatorManager.Select(x);
                                }
                                else
                                {
                                    var floatMenuMap = new FloatMenu(opts);
                                    Find.WindowStack.Add(floatMenuMap);
                                }
                            }
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
                                Utils.ShowFloatMenuNotCOntrolledSurrogateInCaravan((Pawn) parent, delegate(Pawn sSX)
                                {
                                    if (!Utils.GCATPP.isConnectedToSkyMind(sSX))

                                        if (!Utils.GCATPP.connectUser(sSX))
                                            return;

                                    setControlledSurrogate(sSX);
                                });
                            }
                        };
                }

                if (availableSX.Count > 0)
                    yield return new Command_Action
                    {
                        icon = Tex.AndroidToControlTargetDisconnect,
                        defaultLabel = "ATPP_AndroidToControlTargetDisconnect".Translate(),
                        defaultDesc = "ATPP_AndroidToControlTargetDisconnectDesc".Translate(),
                        action = delegate { disconnectControlledSurrogate(null); }
                    };
            }


            if (!pawn.VX2ChipPresent() && !pawn.VX3ChipPresent())
                yield break;

            var selTex = Tex.Permute;


            if (controlMode)
                transfertAllowed = false;

            if (!transfertAllowed)
                selTex = Tex.PermuteDisabled;


            yield return new Command_Action
            {
                icon = selTex,
                defaultLabel = "ATPP_Permute".Translate(),
                defaultDesc = "ATPP_PermuteDesc".Translate(),
                action = delegate
                {
                    if (!transfertAllowed)
                        return;

                    Utils.ShowFloatMenuPermuteOrDuplicateCandidate((Pawn) parent,
                        delegate(Pawn target)
                        {
                            Find.WindowStack.Add(new Dialog_Msg("ATPP_PermuteConsciousnessConfirm".Translate(parent.LabelShortCap, target.LabelShortCap),
                                "ATPP_PermuteConsciousnessConfirmDesc".Translate(parent.LabelShortCap, target.LabelShortCap) + "\n" +
                                "ATPP_WarningSkyMindDisconnectionRisk".Translate(), delegate { OnPermuteConfirmed((Pawn) parent, target); }));
                        }, true);
                }
            };

            selTex = transfertAllowed ? Tex.Duplicate : Tex.DuplicateDisabled;


            yield return new Command_Action
            {
                icon = selTex,
                defaultLabel = "ATPP_Duplicate".Translate(),
                defaultDesc = "ATPP_DuplicateDesc".Translate(),
                action = delegate
                {
                    if (!transfertAllowed)
                        return;

                    Utils.ShowFloatMenuPermuteOrDuplicateCandidate((Pawn) parent,
                        delegate(Pawn target)
                        {
                            Find.WindowStack.Add(new Dialog_Msg("ATPP_DuplicateConsciousnessConfirm".Translate(parent.LabelShortCap, target.LabelShortCap),
                                "ATPP_DuplicateConsciousnessConfirmDesc".Translate(parent.LabelShortCap, target.LabelShortCap) + "\n" +
                                "ATPP_WarningSkyMindDisconnectionRisk".Translate(), delegate { OnDuplicateConfirmed((Pawn) parent, target); }));
                        });
                }
            };


            if (!Utils.GCATPP.isThereSkyCloudCore()) yield break;

            {
                if (transfertAllowed && !isPrisonner)
                    selTex = Tex.UploadToSkyCloud;
                else
                    selTex = Tex.UploadToSkyCloudDisabled;


                yield return new Command_Action
                {
                    icon = selTex,
                    defaultLabel = "ATPP_MoveToSkyCloud".Translate(),
                    defaultDesc = "ATPP_MoveToSkyCloudDesc".Translate(),
                    action = delegate
                    {
                        if (!transfertAllowed || isPrisonner)
                            return;

                        Utils.ShowFloatMenuSkyCloudCores(delegate(Building target)
                        {
                            Find.WindowStack.Add(new Dialog_Msg("ATPP_MoveToSkyCloud".Translate(),
                                "ATPP_MoveToSkyCloudDesc".Translate() + "\n" + "ATPP_WarningSkyMindDisconnectionRisk".Translate(),
                                delegate { OnMoveConsciousnessToSkyCloudCore((Pawn) parent, target); }));
                        });
                    }
                };

                var transfertAllowed2 = Utils.mindTransfertsAllowed((Pawn) parent, false);
                if (controlMode)
                    transfertAllowed2 = false;

                selTex = transfertAllowed2 ? Tex.DownloadFromSkyCloud : Tex.DownloadFromSkyCloudDisabled;

                yield return new Command_Action
                {
                    icon = selTex,
                    defaultLabel = "ATPP_MoveFromSkyCloud".Translate(),
                    defaultDesc = "ATPP_MoveFromSkyCloudDesc".Translate(),
                    action = delegate
                    {
                        if (!transfertAllowed2)
                            return;

                        Utils.ShowFloatMenuSkyCloudCores(delegate(Building target)
                        {
                            var csc = target.TryGetComp<CompSkyCloudCore>();

                            csc.showFloatMenuMindsStored(delegate(Pawn mind)
                            {
                                Find.WindowStack.Add(new Dialog_Msg("ATPP_MoveFromSkyCloud".Translate(),
                                    "ATPP_MoveFromSkyCloudDesc".Translate() + "\n" + "ATPP_WarningSkyMindDisconnectionRisk".Translate(),
                                    delegate { OnMoveConsciousnessFromSkyCloudCore(mind); }));
                            });
                        });
                    }
                };
            }
        }

        public void disconnectControlledSurrogate(Pawn surrogate, bool externalController = false, bool preventNoHost = false)
        {
            stopControlledSurrogate(surrogate, externalController, preventNoHost);
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";
            try
            {
                if (parent.Map == null)
                    return base.CompInspectStringExtra();

                if (permuteEndingGT != -1 || showPermuteProgress)
                {
                    float p;
                    if (permuteEndingGT == -1)
                    {
                        var cso = permuteRecipient.TryGetComp<CompSurrogateOwner>();
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - cso.permuteStartGT) / (float) (cso.permuteEndingGT - cso.permuteStartGT));
                    }
                    else
                    {
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - permuteStartGT) / (float) (permuteEndingGT - permuteStartGT));
                    }

                    ret += "ATPP_PermutationPercentage".Translate(((int) (p * 100)).ToString()) + "\n";
                }


                if (duplicateEndingGT != -1 || showDuplicateProgress)
                {
                    float p;
                    if (duplicateEndingGT == -1)
                    {
                        var cso = duplicateRecipient.TryGetComp<CompSurrogateOwner>();
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - cso.duplicateStartGT) / (float) (cso.duplicateEndingGT - cso.duplicateStartGT));
                    }
                    else
                    {
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - duplicateStartGT) / (float) (duplicateEndingGT - duplicateStartGT));
                    }

                    ret += "ATPP_DuplicationPercentage".Translate(((int) (p * 100)).ToString()) + "\n";
                }

                if (uploadToSkyCloudEndingGT != -1)
                {
                    var p = Math.Min(1.0f, (Find.TickManager.TicksGame - uploadToSkyCloudStartGT) / (float) (uploadToSkyCloudEndingGT - uploadToSkyCloudStartGT));
                    ret += "ATPP_UploadSkyCloudPercentage".Translate(((int) (p * 100)).ToString()) + "\n";
                }

                if (downloadFromSkyCloudEndingGT != -1)
                {
                    var p = Math.Min(1.0f, (Find.TickManager.TicksGame - downloadFromSkyCloudStartGT) / (float) (downloadFromSkyCloudEndingGT - downloadFromSkyCloudStartGT));
                    ret += "ATPP_DownloadFromSkyCloudPercentage".Translate(((int) (p * 100)).ToString()) + "\n";
                }

                if (mindAbsorptionEndingGT != -1)
                {
                    var p = Math.Min(1.0f, (Find.TickManager.TicksGame - mindAbsorptionStartGT) / (float) (mindAbsorptionEndingGT - mindAbsorptionStartGT));
                    ret += "ATPP_MindAbsorptionProgress".Translate(((int) (p * 100)).ToString()) + "\n";
                }

                if (!controlMode || availableSX.Count <= 0) return ret.TrimEnd('\r', '\n', ' ') + base.CompInspectStringExtra();

                var lst = "";
                foreach (var s in availableSX)
                    if (SX == s)
                        lst += Utils.getSavedSurrogateNameNick(SX) + ", ";
                    else
                        lst += Utils.getSavedSurrogateNameNick(s) + ", ";

                ret += "ATPP_RemotelyControl".Translate(lst.TrimEnd(' ', ',')) + "\n";

                return ret.TrimEnd('\r', '\n', ' ') + base.CompInspectStringExtra();
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] CompSurrogateOwner.CompInspectStringExtra " + e.Message + " " + e.StackTrace);
                return ret.TrimEnd('\r', '\n', ' ') + base.CompInspectStringExtra();
            }
        }

        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            switch (signal)
            {
                case "SkyMindNetworkUserConnected":
                    break;
                case "SkyMindNetworkUserDisconnected":

                    if (controlMode)
                        disconnectControlledSurrogate(null);

                    checkInterruptedUpload();
                    break;
            }
        }


        public void setControlledSurrogate(Pawn controlled, bool external = false)
        {
            var cp = (Pawn) parent;
            if (cp == null)
                return;

            var VX3Host = cp.VX3ChipPresent();

            var cas = controlled?.TryGetComp<CompAndroidState>();

            if (cas == null || SX != null && !VX3Host || VX3Host && availableSX.Count + 1 > Settings.VX3MaxSurrogateControllableAtOnce ||
                !external && (!controlMode || !Utils.GCATPP.isConnectedToSkyMind(cp) || !Utils.GCATPP.isConnectedToSkyMind(controlled)))
                return;

            externalController = external;

            if (!external)
            {
                Utils.soundDefSurrogateConnection.PlayOneShot(null);
                MoteMaker.ThrowDustPuffThick(controlled.Position.ToVector3Shifted(), controlled.Map, 4.0f, Color.blue);
            }


            cas.surrogateController = cp;
            cas.lastController = cp;

            var inMainSX = false;

            if (SX == null)
            {
                SX = controlled;
                inMainSX = true;
            }
            else
            {
                extraSX.Add(controlled);
            }

            availableSX.Add(controlled);

            if (!external)
                Messages.Message("ATPP_SurrogateConnectionOK".Translate(cp.LabelShortCap, controlled.LabelShortCap), cp, MessageTypeDefOf.PositiveEvent);


            if (inMainSX)

                if (controlled.def.defName == "M7Mech" && savedSkillsBecauseM7Control == null)
                {
                    savedSkillsBecauseM7Control = new List<string>();
                    savedWorkAffectationBecauseM7Control = new List<string>();

                    foreach (var k in cp.skills.skills.Select(s => s.def.defName + "-" + s.levelInt + "-" + s.xpSinceLastLevel + "-" + s.xpSinceMidnight))
                        savedSkillsBecauseM7Control.Add(k);

                    if (cp.workSettings != null && cp.workSettings.EverWork)
                        foreach (var k in DefDatabase<WorkTypeDef>.AllDefsListForReading.Select(el => el.defName + "-" + cp.workSettings.GetPriority(el)))
                            savedWorkAffectationBecauseM7Control.Add(k);
                }


            if (!inMainSX)
            {
                Utils.saveSurrogateName(controlled);
                Utils.Duplicate(SX, controlled, false);
            }
            else
            {
                Utils.PermutePawn(cp, controlled);


                Utils.saveSurrogateName(cp);

                var nam = (NameTriple) controlled.Name;
                cp.Name = new NameTriple(nam.First, nam.Nick, nam.Last);
            }


            var he = controlled.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
            if (he != null)
                controlled.health.RemoveHediff(he);

            controlled.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_RemotelyControlled"));


            Find.ColonistBar.MarkColonistsDirty();


            if (externalController) return;

            {
                if (cp.workSettings != null && cp.workSettings.EverWork)
                    foreach (var el in DefDatabase<WorkTypeDef>.AllDefsListForReading)
                        controlled.workSettings.SetPriority(el, cp.workSettings.GetPriority(el));
                controlled.playerSettings.AreaRestriction = cp.playerSettings.AreaRestriction;
                controlled.playerSettings.hostilityResponse = cp.playerSettings.hostilityResponse;


                for (var i = 0; i != 24; i++) controlled.timetable.SetAssignment(i, cp.timetable.GetAssignment(i));
            }
        }

        /*
         * Check s'il y a au moin un SX de connecté a ce controller 
         */
        public bool isThereSX()
        {
            return SX != null || extraSX.Count > 0;
        }

        public void stopControlledSurrogate(Pawn surrogate, bool externalController = false, bool preventNoHost = false, bool noPrisonedSurrogateConversion = false)
        {
            var cp = (Pawn) parent;


            if (!isThereSX() || surrogate != null && !availableSX.Contains(surrogate))
                return;


            if (surrogate == null)
            {
                foreach (var s in extraSX)
                {
                    Utils.initBodyAsSurrogate(s);

                    Utils.restoreSavedSurrogateName(s);
                }

                if (SX != null)
                {
                    Utils.restoreSavedSurrogateName(cp);
                    Utils.PermutePawn(SX, cp);
                }
            }
            else
            {
                if (extraSX.Contains(surrogate))
                {
                    Utils.initBodyAsSurrogate(surrogate);

                    Utils.restoreSavedSurrogateName(surrogate);
                }
                else
                {
                    Utils.restoreSavedSurrogateName(cp);
                    Utils.PermutePawn(surrogate, cp);
                }
            }

            foreach (var csurrogate in availableSX.Where(csurrogate => surrogate == null || surrogate == csurrogate))
            {
                try
                {
                    if (csurrogate == SX && csurrogate.def.defName == "M7Mech" && savedSkillsBecauseM7Control != null)
                    {
                        foreach (var k in savedSkillsBecauseM7Control)
                        {
                            var vals = k.Split('-');
                            if (vals.Count() != 4)
                                continue;

                            var sd = DefDatabase<SkillDef>.GetNamed(vals[0], false);
                            if (sd == null)
                                continue;

                            var levelInt = 0;
                            float xpSinceLastLevel = 0;
                            float xpSinceMidnight = 0;

                            try
                            {
                                levelInt = int.Parse(vals[1]);
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                xpSinceLastLevel = float.Parse(vals[2]);
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                xpSinceMidnight = float.Parse(vals[3]);
                            }
                            catch (Exception)
                            {
                            }

                            foreach (var s in cp.skills.skills.Where(s => s.def == sd))
                            {
                                s.levelInt = levelInt;
                                s.xpSinceLastLevel = xpSinceLastLevel;
                                s.xpSinceMidnight = xpSinceMidnight;
                                break;
                            }
                        }

                        if (cp.workSettings != null)
                            foreach (var k in savedWorkAffectationBecauseM7Control)
                            {
                                var vals = k.Split('-');
                                if (vals.Count() != 2)
                                    continue;

                                var wtd = DefDatabase<WorkTypeDef>.GetNamed(vals[0], false);
                                if (wtd == null)
                                    continue;

                                var priority = 0;
                                try
                                {
                                    priority = int.Parse(vals[1]);
                                }
                                catch (Exception)
                                {
                                }

                                cp.workSettings.SetPriority(wtd, priority);
                            }

                        savedSkillsBecauseM7Control = null;
                        savedWorkAffectationBecauseM7Control = null;
                    }
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] stopControlledSurrogate.(savedSkillsBecauseM7Control) : " + e.Message + " - " + e.StackTrace);
                }


                var cas = csurrogate.TryGetComp<CompAndroidState>();
                if (cas != null)
                    cas.surrogateController = null;


                var he = csurrogate.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_RemotelyControlled"));
                if (he != null)
                {
                    if (Utils.insideKillFuncSurrogate)
                    {
                        csurrogate.health.hediffSet.hediffs.Remove(he);
                        he.PostRemoved();
                    }
                    else
                    {
                        csurrogate.health.RemoveHediff(he);
                    }
                }


                if (!externalController && !preventNoHost)
                {
                    if (Utils.insideKillFuncSurrogate)
                        csurrogate.health.hediffSet.AddDirect(HediffMaker.MakeHediff(Utils.hediffNoHost, csurrogate));
                    else
                        csurrogate.health.AddHediff(Utils.hediffNoHost);
                }


                he = csurrogate.health.hediffSet.GetFirstHediffOfDef(Utils.hediffLowNetworkSignal);
                if (he != null)
                {
                    if (Utils.insideKillFuncSurrogate)
                    {
                        csurrogate.health.hediffSet.hediffs.Remove(he);
                        he.PostRemoved();
                    }
                    else
                    {
                        csurrogate.health.RemoveHediff(he);
                    }
                }


                Utils.soundDefSurrogateConnectionStopped.PlayOneShot(null);


                if (csurrogate.IsPrisoner && !noPrisonedSurrogateConversion)
                {
                    if (csurrogate.Faction != Faction.OfPlayer) csurrogate.SetFaction(Faction.OfPlayer);

                    if (csurrogate.guest != null) csurrogate.guest.SetGuestStatus(null);

                    if (cp.guest != null) cp.guest.SetGuestStatus(Faction.OfPlayer, true);


                    removeRemoteControlHediff();
                    controlMode = false;
                    Utils.GCATPP.disconnectUser(cp);
                }


                if (csurrogate.def.defName != "M7Mech" && csurrogate.workSettings != null && csurrogate.workSettings.EverWork)
                    foreach (var el in DefDatabase<WorkTypeDef>.AllDefsListForReading.Where(el => csurrogate.workSettings.GetPriority(el) != 0))
                        csurrogate.workSettings.SetPriority(el, 3);

                if (csurrogate.playerSettings != null)
                {
                    csurrogate.playerSettings.AreaRestriction = null;
                    csurrogate.playerSettings.hostilityResponse = HostilityResponseMode.Flee;
                }

                if (csurrogate.timetable == null) continue;

                for (var i = 0; i != 24; i++)
                    csurrogate.timetable.SetAssignment(i, TimeAssignmentDefOf.Anything);
            }


            if (surrogate == null)
            {
                extraSX.Clear();
                SX = null;
                availableSX.Clear();
            }
            else
            {
                if (extraSX.Contains(surrogate))
                {
                    extraSX.Remove(surrogate);
                    availableSX.Remove(surrogate);
                }
                else
                {
                    availableSX.Remove(SX);
                    SX = null;
                }
            }
        }

        public void clearRansomwareVar()
        {
            ransomwareSkillValue = -1;
            ransomwareSkillStolen = null;
            ransomwareTraitAdded = null;
        }

        private void OnPermuteConfirmed(Pawn source, Pawn dest)
        {
            source.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
            dest.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));

            var CGT = Find.TickManager.TicksGame;
            permuteRecipient = dest;
            permuteStartGT = CGT;
            permuteEndingGT = CGT + 60 - CGT % 60 + Settings.mindPermutationHours * 2500;

            var cso = dest.TryGetComp<CompSurrogateOwner>();
            cso.showPermuteProgress = true;
            cso.permuteRecipient = (Pawn) parent;

            Messages.Message("ATPP_StartPermute".Translate(source.LabelShortCap, dest.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
        }

        private void OnDuplicateConfirmed(Pawn source, Pawn dest)
        {
            source.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
            dest.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));

            var CGT = Find.TickManager.TicksGame;
            duplicateRecipient = dest;
            duplicateStartGT = CGT;
            duplicateEndingGT = CGT + 60 - CGT % 60 + Settings.mindDuplicationHours * 2500;

            var cso = dest.TryGetComp<CompSurrogateOwner>();
            cso.showDuplicateProgress = true;
            cso.duplicateRecipient = (Pawn) parent;


            Messages.Message("ATPP_StartDuplicate".Translate(source.LabelShortCap, dest.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
        }

        private void OnMoveConsciousnessToSkyCloudCore(Pawn source, Building dest)
        {
            source.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));

            var CGT = Find.TickManager.TicksGame;
            skyCloudRecipient = dest;
            uploadToSkyCloudStartGT = CGT;
            uploadToSkyCloudEndingGT = CGT + 60 - CGT % 60 + Settings.mindUploadToSkyCloudHours * 2500;

            var csc = dest.TryGetComp<CompSkyCloudCore>();

            Messages.Message("ATPP_StartSkyCloudUpload".Translate(source.LabelShortCap, csc.getName()), parent, MessageTypeDefOf.PositiveEvent);
        }

        private void OnMoveConsciousnessFromSkyCloudCore(Pawn source)
        {
            var dest = (Pawn) parent;
            dest.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));

            var CGT = Find.TickManager.TicksGame;
            skyCloudDownloadRecipient = source;
            downloadFromSkyCloudStartGT = CGT;
            downloadFromSkyCloudEndingGT = CGT + 60 - CGT % 60 + Settings.mindUploadToSkyCloudHours * 2500;

            var cso = source.TryGetComp<CompSurrogateOwner>();

            if (cso != null && cso.isThereSX())
                if (cso.SX != null)
                {
                }

            Messages.Message("ATPP_StartSkyCloudDownload".Translate(source.LabelShortCap, dest.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
        }

        public void startMigration(Building dest)
        {
            var csc2 = dest.TryGetComp<CompSkyCloudCore>();

            var CGT = Find.TickManager.TicksGame;
            migrationSkyCloudHostDest = dest;
            migrationStartGT = CGT;
            migrationEndingGT = CGT + 60 - CGT % 60 + Settings.mindSkyCloudMigrationHours * 2500;

            Messages.Message("ATPP_StartSkyCloudMigration".Translate(((Pawn) parent).LabelShortCap, skyCloudHost.TryGetComp<CompSkyCloudCore>().getName(), csc2.getName()), parent,
                MessageTypeDefOf.PositiveEvent);
        }

        /*
         * Detecte un cas d'interruption est le cas echeant tue les protagoniste de l'upload tous en affichant un message d'erreur
         */
        public void checkInterruptedUpload()
        {
            var killSelf = false;
            var cpawn = (Pawn) parent;


            if (permuteEndingGT == -1 && duplicateEndingGT == -1 && uploadToSkyCloudEndingGT == -1 && downloadFromSkyCloudEndingGT == -1 && mindAbsorptionEndingGT == -1 &&
                migrationEndingGT == -1 && replicationEndingGT == -1) return;

            var permuteRecipientDead = false;
            if (permuteRecipient != null)
                permuteRecipientDead = permuteRecipient.Dead;

            var duplicateRecipientDead = false;
            if (duplicateRecipient != null)
                duplicateRecipientDead = duplicateRecipient.Dead;

            var recipientConnected = false;

            var emitterConnected = false;

            if (permuteRecipient != null && Utils.GCATPP.isConnectedToSkyMind(permuteRecipient, true))
                recipientConnected = true;

            if (duplicateRecipient != null && Utils.GCATPP.isConnectedToSkyMind(duplicateRecipient, true))
                recipientConnected = true;

            if (Utils.GCATPP.isThereSkillServers())
                recipientConnected = true;


            if (skyCloudRecipient != null && skyCloudRecipient.TryGetComp<CompPowerTrader>().PowerOn
                || replicationEndingGT != -1 && skyCloudHost.TryGetComp<CompPowerTrader>().PowerOn
                || migrationSkyCloudHostDest != null && migrationSkyCloudHostDest.TryGetComp<CompPowerTrader>().PowerOn
                || skyCloudDownloadRecipient != null && skyCloudDownloadRecipient.TryGetComp<CompSurrogateOwner>() != null &&
                skyCloudDownloadRecipient.TryGetComp<CompSurrogateOwner>().skyCloudHost.TryGetComp<CompPowerTrader>().PowerOn)
                recipientConnected = true;


            if (Utils.GCATPP.isConnectedToSkyMind(cpawn, true)
                || replicationEndingGT != -1 && skyCloudHost.TryGetComp<CompPowerTrader>().PowerOn
                || skyCloudHost != null && skyCloudHost.TryGetComp<CompPowerTrader>().PowerOn)
                emitterConnected = true;


            if (!permuteRecipientDead && !duplicateRecipientDead && !cpawn.Dead && emitterConnected && recipientConnected) return;

            var showMindUploadNotif = true;
            var reason = "";
            if (permuteRecipientDead || duplicateRecipientDead)
            {
                reason = "ATPP_LetterInterruptedUploadDescCompHostDead".Translate();
                killSelf = true;
            }

            if (cpawn.Dead)
            {
                reason = "ATPP_LetterInterruptedUploadDescCompSourceDead".Translate();
                if (permuteRecipient != null && !permuteRecipient.Dead)
                    permuteRecipient.Kill(null);

                if (duplicateRecipient != null && !duplicateRecipient.Dead)
                    duplicateRecipient.Kill(null);
            }

            if (reason == "")
            {
                if (mindAbsorptionEndingGT != -1)
                {
                    if (!cpawn.Dead)
                        killSelf = true;

                    showMindUploadNotif = false;
                    Find.LetterStack.ReceiveLetter("ATPP_LetterInterruptedMindAbsorption".Translate(),
                        "ATPP_LetterInterruptedMindAbsorptionDesc".Translate(cpawn.LabelShortCap), LetterDefOf.ThreatSmall);
                }
                else if (replicationEndingGT != -1)
                {
                    var csc = skyCloudHost.TryGetComp<CompSkyCloudCore>();
                    csc.replicatingMinds.Remove((Pawn) parent);
                    showMindUploadNotif = false;

                    Find.LetterStack.ReceiveLetter("ATPP_LetterInterruptedSkyCloudReplication".Translate(),
                        "ATPP_LetterInterruptedSkyCloudReplicationDesc".Translate(cpawn.LabelShortCap), LetterDefOf.ThreatSmall);
                }
                else if (!recipientConnected || !emitterConnected)
                {
                    reason = "ATPP_LetterInterruptedUploadDescCompDiconnectionError".Translate();

                    killSelf = true;
                    if (permuteEndingGT != -1)
                    {
                        if (permuteRecipient != null && !permuteRecipient.Dead) permuteRecipient.Kill(null);
                    }
                    else if (duplicateEndingGT != -1)
                    {
                        if (duplicateRecipient != null && !duplicateRecipient.Dead) duplicateRecipient.Kill(null);
                    }
                }
            }

            resetUploadStuff();

            if (killSelf && !cpawn.Dead) cpawn.Kill(null);

            if (showMindUploadNotif)
                Utils.showFailedLetterMindUpload(reason);
        }

        private void resetUploadStuff()
        {
            if (duplicateRecipient != null)
            {
                var cso = duplicateRecipient.TryGetComp<CompSurrogateOwner>();
                cso.showDuplicateProgress = false;
                cso.duplicateRecipient = null;
            }

            if (permuteRecipient != null)
            {
                var cso = permuteRecipient.TryGetComp<CompSurrogateOwner>();
                cso.showPermuteProgress = false;
                cso.permuteRecipient = null;
            }

            permuteStartGT = 0;
            duplicateStartGT = 0;
            permuteEndingGT = -1;
            duplicateEndingGT = -1;
            duplicateRecipient = null;
            permuteRecipient = null;
            skyCloudRecipient = null;
            skyCloudDownloadRecipient = null;
            uploadToSkyCloudStartGT = 0;
            uploadToSkyCloudEndingGT = -1;
            downloadFromSkyCloudStartGT = 0;
            downloadFromSkyCloudEndingGT = -1;
            migrationSkyCloudHostDest = null;
            migrationEndingGT = -1;
            migrationStartGT = 0;
            replicationStartGT = 0;
            replicationEndingGT = -1;
            mindAbsorptionEndingGT = -1;
            mindAbsorptionStartGT = -1;
        }
    }
}