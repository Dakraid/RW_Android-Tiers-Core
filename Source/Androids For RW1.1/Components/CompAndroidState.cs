using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace MOARANDROIDS
{
    public class CompAndroidState : ThingComp
    {
        public bool autoPaintStarted;

        public int batteryExplosionEndingGT = -1;
        public int batteryExplosionStartingGT = -1;

        public Building connectedLWPN;
        public bool connectedLWPNActive;

        private CompSkyMind csm;

        //AndroidPaintColor
        public int customColor = (int) AndroidPaintColor.Default;

        public bool dontRust;

        //Stocke le pawn externe (n'appartenant pas au joueur) controllant le surrogate (le cas des groupes de factions alliés/neutre/ennemis)
        public Pawn externalController;
        public int forcedDamageLevel = -1;

        public int frameworkNaniteEffectGTEnd = -1;
        public int frameworkNaniteEffectGTStart = -1;

        public HairDef hair;

        public bool init;
        public bool isAndroidTIer;
        public bool isAndroidWithSkin;

        public bool isBlankAndroid;

        //Sert a identifier les surrogates biologiques
        public bool isOrganic;
        public bool isSurrogate;

        public Pawn lastController;
        public bool paintingIsRusted;

        public int paintingRustGT = -2;

        public string savedName = "";

        public bool showUploadProgress;

        public bool solarFlareEffectApplied;
        public Pawn surrogateController;

        public bool TXHurtedHeadSet;
        public bool TXHurtedHeadSet2;
        public int uploadEndingGT = -1;
        public Pawn uploadRecipient;
        public int uploadStartGT;

        //Stock le signal indiquant si le pawn à été attribué par le systeme de job pour faire du guarding ou non
        public bool useBattery;

        public bool UseBattery
        {
            get
            {
                //SI M7 surrogate forcé à utilsier la recharge en directe
                if (parent.def.defName == Utils.M7 && isSurrogate)
                    return true;

                return useBattery;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref autoPaintStarted, "ATPP_autoPaintStarted");

            Scribe_Values.Look(ref connectedLWPNActive, "ATPP_connectedLWPNActive");
            Scribe_References.Look(ref connectedLWPN, "ATPP_connectedLWPN");
            Scribe_Values.Look(ref isBlankAndroid, "ATPP_isBlankAndroid");
            Scribe_Values.Look(ref showUploadProgress, "ATPP_showUploadProgress");
            Scribe_Values.Look(ref useBattery, "ATPP_useBattery", Settings.defaultGeneratorMode == 1 ? false : true, true);
            Scribe_Values.Look(ref uploadEndingGT, "ATPP_uploadEndingGT", -1);
            Scribe_Values.Look(ref uploadStartGT, "ATPP_uploadStartGT");
            Scribe_Values.Look(ref isSurrogate, "ATPP_isSurrogate");
            Scribe_References.Look(ref surrogateController, "ATPP_surrogateController");
            Scribe_References.Look(ref lastController, "ATPP_lastController");
            Scribe_Values.Look(ref savedName, "ATPP_savedName", "");
            Scribe_Values.Look(ref frameworkNaniteEffectGTEnd, "ATPP_frameworkNaniteEffectGTEnd", -1);
            Scribe_Values.Look(ref frameworkNaniteEffectGTStart, "ATPP_frameworkNaniteEffectGTStart", -1);
            Scribe_Values.Look(ref paintingRustGT, "ATPP_paintingRustGT", -2);
            Scribe_Values.Look(ref paintingIsRusted, "ATPP_paintingIsRusted");

            Scribe_Values.Look(ref batteryExplosionEndingGT, "ATPP_batteryExplosionEndingGT", -1);


            Scribe_Values.Look(ref customColor, "ATPP_customColor", (int) AndroidPaintColor.Default);

            Scribe_Deep.Look(ref externalController, "ATPP_externalController");

            Scribe_References.Look(ref uploadRecipient, "ATPP_uploadRecipient");

            Scribe_Defs.Look(ref hair, "ATPP_hair");
        }

        public override void PostDraw()
        {
            Material avatar = null;

            if (uploadEndingGT != -1 || showUploadProgress)
                avatar = Tex.UploadInProgress;
            else 
                switch (Find.DesignatorManager.SelectedDesignator)
                {
                    case Designator_AndroidToControl _ when isSurrogate && surrogateController == null && csm != null && csm.Infected == -1:
                        avatar = Tex.SelectableSX;
                        break;
                    case Designator_SurrogateToHack _ when isSurrogate && parent.Faction != Faction.OfPlayer:
                        avatar = Tex.SelectableSXToHack;
                        break;
                    default:
                    {
                        if (isSurrogate && surrogateController != null && !Settings.hideRemotelyControlledDeviceIcon)
                            avatar = Tex.RemotelyControlledNode;
                        break;
                    }
                }

            if (avatar == null) return;
            
            var vector = parent.TrueCenter();
            vector.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.28125f;
            vector.z += 1.4f;
            vector.x += parent.def.size.x / 2;

            Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, avatar, 0);
        }

        public override void PostDrawExtraSelectionOverlays()

        {
            base.PostDrawExtraSelectionOverlays();

            //Dessin liaison entre controlleur et SX
            if (surrogateController != null && isSurrogate && surrogateController.Map == parent.Map)
                GenDraw.DrawLineBetween(parent.TrueCenter(), surrogateController.TrueCenter(), SimpleColor.Blue);

            if (surrogateController.TryGetComp<CompSurrogateOwner>() != null
                && surrogateController.TryGetComp<CompSurrogateOwner>().skyCloudHost != null
                && surrogateController.TryGetComp<CompSurrogateOwner>().skyCloudHost.Map == parent.Map)
                GenDraw.DrawLineBetween(parent.TrueCenter(), surrogateController.TryGetComp<CompSurrogateOwner>().skyCloudHost.TrueCenter(), SimpleColor.Red);

            if (uploadEndingGT != -1 && uploadRecipient != null || showUploadProgress)
                GenDraw.DrawLineBetween(parent.TrueCenter(), uploadRecipient.TrueCenter(), SimpleColor.Green);
        }

        public override void CompTick()
        {
            base.CompTick();

            //bool reconnectDirectExternalController = false;

            if (parent.Map == null || !parent.Spawned)
                return;

            var GT = Find.TickManager.TicksGame;

            if (!init)
            {
                checkTXWithSkinFacialTextureUpdate();
                init = true;
                //Reconexion auto au LWPN le cas echeant
                if (Utils.POWERPP_LOADED)
                    if (connectedLWPN != null && connectedLWPNActive)
                        if (!Utils.GCATPP.pushLWPNAndroid(connectedLWPN, (Pawn) parent))
                            connectedLWPNActive = false;
            }

            //Reconnection auto de l'etranger à son surrogate que si pas de solar flare en cours et toujours dans un Lord (Si le cas d'un ennemis check de l'etat de son Lord)
            if (GT % 120 == 0 && externalController != null
                              && surrogateController == null
                              && csm != null && csm.hacked != 3
                              //&& !externalController.Faction.HostileTo(Faction.OfPlayer)
                              && !parent.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
            {
                var cp = (Pawn) parent;
                //Log.Message("RepriseController "+(externalController != null)+" "+(surrogateController == null)+" "+(hacked != 3));
                //Lord lordInvolved = Utils.LordOnMapWhereFactionIsInvolved(parent.Map, hackOrigFaction);
                Lord lordInvolved = null;
                if (cp.Map.mapPawns.SpawnedPawnsInFaction(cp.Faction).Any(p => p != cp))
                {
                    var p2 = (Pawn) GenClosest.ClosestThing_Global(cp.Position, cp.Map.mapPawns.SpawnedPawnsInFaction(cp.Faction), 99999f,
                        p => p != cp && ((Pawn) p).GetLord() != null);
                    lordInvolved = p2.GetLord();
                }

                if (lordInvolved == null && !cp.IsPrisoner)
                {
                    var lordJob = new LordJob_DefendPoint(cp.Position);
                    lordInvolved = LordMaker.MakeNewLord(cp.Faction, lordJob, Find.CurrentMap);
                }


                //Si controlleur non player du surrogate mort OU surrogate hacké avais un lors il existe tjr mais il n'est plus actif
                if (externalController.Dead || csm != null && csm.hackOrigFaction.HostileTo(Faction.OfPlayer) && lordInvolved == null && !cp.IsPrisoner)
                {
                    //Rajout NoHost car comme en mode externalController on a pas remis le hediff pour eviter le bug bizard faisant que quand tentative integration ennemis hacké a un lord sa merdequand il a été down
                    addNoRemoteHostHediff();
                    externalController = null;
                }
                else
                {
                    //try
                    //
                    try
                    {
                        //Tentative de reconnection automatique du surrogate a son controlleur externe
                        var cso = externalController.TryGetComp<CompSurrogateOwner>();
                        cso.setControlledSurrogate((Pawn) parent, true);
                        cp.mindState.Reset();
                        cp.mindState.duty = null;
                        cp.jobs.StopAll();
                        cp.jobs.ClearQueuedJobs();
                        cp.ClearAllReservations();
                        if (cp.drafter != null)
                            cp.drafter.Drafted = false;

                        lordInvolved?.AddPawn(cp);
                    }
                    catch (Exception)
                    {
                    }
                    //cp.ClearMind();

                    //lordInvolved.AddPawn((Pawn)parent);
                    /*}
                    catch(Exception e)
                    {

                    }*/

                    //****************************************** Traitement des conditions spéciale de reintegration a certain Lords *************************************************************
                    //Log.Message("=>"+ lordInvolved.CurLordToil.ToString());

                    try
                    {
                        if (lordInvolved != null && lordInvolved.CurLordToil is LordToil_Siege)
                        {
                            var st = (LordToil_Siege) lordInvolved.CurLordToil;

                            //Attribution job defender au pawn
                            var p = (Pawn) parent;
                            //Traverse.Create( st ).Method("SetAsDefender").GetValue((Pawn)parent);
                            var data = (LordToilData_Siege) Traverse.Create(st).Property("Data").GetValue();
                            p.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.siegeCenter) {radius = data.baseRadius};
                            st.UpdateAllDuties();
                        }
                    }
                    catch (Exception)
                    {
                    }

                    //Log.Message("Current duty ==>"+cp.mindState.duty.def.defName);
                    //Log.Message("Current job ==>" + cp.CurJobDef.defName);
                }
            }

            if (GT % 120 == 0)
            {
                var cpawn = (Pawn) parent;

                if (uploadEndingGT != -1)
                {
                    checkInterruptedUpload();

                    //Atteinte d'un chargement d'upload de conscience
                    if (uploadRecipient != null && uploadEndingGT != -1 && uploadEndingGT < GT)
                    {
                        uploadEndingGT = -1;
                        uploadRecipient.TryGetComp<CompAndroidState>().uploadEndingGT = -1;

                        Utils.removeUploadHediff(cpawn, uploadRecipient);

                        Find.LetterStack.ReceiveLetter("ATPP_LetterUploadOK".Translate(), "ATPP_LetterUploadOKDesc".Translate(cpawn.LabelShortCap, uploadRecipient.LabelShortCap),
                            LetterDefOf.PositiveEvent, parent);

                        if (cpawn.def.defName == Utils.T1 && uploadRecipient.def.defName != Utils.T1)
                            Utils.removeSimpleMindedTrait(cpawn);
                        else
                            Utils.addSimpleMindedTraitForT1(uploadRecipient);

                        //On realise effectivement la permutation puis le kill de la source
                        Utils.PermutePawn(cpawn, uploadRecipient);

                        Utils.clearBlankAndroid(uploadRecipient);

                        //Report du blankAndroid pour le flagger dans la routine de kill
                        isBlankAndroid = true;


                        //Si destinataire de la duplication prisonnier Et emetteur pas prisonier on enleve la condition 
                        if (!cpawn.IsPrisoner && uploadRecipient.IsPrisoner)
                        {
                            if (uploadRecipient.Faction != Faction.OfPlayer) uploadRecipient.SetFaction(Faction.OfPlayer);

                            if (uploadRecipient.guest != null) uploadRecipient.guest.SetGuestStatus(null);
                        }

                        //SI destinataire de la duplication colon regular et emetteur prisonnier 
                        if (cpawn.IsPrisoner && !uploadRecipient.IsPrisoner)
                        {
                            if (uploadRecipient.Faction != cpawn.Faction) uploadRecipient.SetFaction(cpawn.Faction);

                            uploadRecipient.guest?.SetGuestStatus(Faction.OfPlayer, true);
                        }

                        if (!cpawn.Dead)
                            cpawn.Kill(null);


                        resetUploadStuff();
                    }
                }

                if (batteryExplosionEndingGT != -1 && batteryExplosionEndingGT < GT)
                {
                    Utils.makeAndroidBatteryOverload(cpawn);

                    return;
                }

                //Atteinte fin application des nanites sur un androide
                if (frameworkNaniteEffectGTEnd != -1 && GT >= frameworkNaniteEffectGTEnd && !cpawn.Dead)
                {
                    var chance = false;
                    var nb = 0;

                    //Chance que nanite fail
                    if (!Rand.Chance(Settings.percentageNanitesFail))
                    {
                        //Le cas echeant on enleve le rusting
                        clearRusted();

                        nb = cpawn.health.hediffSet.hediffs.RemoveAll(h => Utils.AndroidOldAgeHediffFramework.Contains(h.def.defName));
                        nb += cpawn.health.hediffSet.hediffs.RemoveAll(h =>
                            h.def == HediffDefOf.MissingBodyPart || Utils.ExceptionRepairableFrameworkHediff.Contains(h.def) && h.IsPermanent());
                        if (nb > 0) Utils.refreshHediff(cpawn);
                        chance = true;
                    }

                    if (nb == 0)
                    {
                        Messages.Message(chance ? "ATPP_NoBrokenStuffFound".Translate(cpawn.LabelShort) : "ATPP_BrokenStuffRepairFailed".Translate(cpawn.LabelShort), cpawn,
                            MessageTypeDefOf.NegativeEvent);
                    }
                    else
                    {
                        Messages.Message("ATPP_BrokenFrameworkRepaired".Translate(cpawn.LabelShort), cpawn, MessageTypeDefOf.PositiveEvent);
                    }


                    frameworkNaniteEffectGTEnd = -1;
                    frameworkNaniteEffectGTStart = -1;
                }
            }

            if (GT % 300 != 0) return;
            
            {
                var cp = (Pawn) parent;


                checkInfectionFix();
                checkTXWithSkinFacialTextureUpdate();

                /* Debugage PK opreationBed obtenu aprés androidPod
                List<ThingDef> beds = (List <ThingDef>) Traverse.CreateWithType("RimWorld.RestUtility").Field("bedDefsBestToWorst_Medical").GetValue();
                foreach (var e in RestUtility.AllBedDefBestToWorst)
                {
                    Log.Message(e.defName+" "+e.building.bed_maxBodySize+" "+e.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null));
                }*/

                //SI surrogate d'une conscience numérisée ET à un mentalbreak => déconnection et mise en place d'un timeout de fin de mentalbreak

                //Recharge auto de la barre de need food
                if (csm != null && csm.Infected == -1)
                {
                    //Recharge surrogate
                    if (Utils.androidIsValidPodForCharging(cp) && !isOrganic)
                    {
                        //cpawn.needs.food.CurLevel = cpawn.needs.food.MaxLevel;
                        cp.needs.food.CurLevelPercentage += Settings.percentageOfBatteryChargedEach6Sec;
                        Utils.throwChargingMote(cp);
                    }

                    if (isSurrogate && surrogateController == null)
                        addNoRemoteHostHediff();
                }

                //Atteinte du solarFlare que si android OU pucé (VXX)
                checkSolarFlareStuff();

                checkRusted();

                checkBlankAndroid();


                if (!Utils.POWERPP_LOADED) return;

                if (connectedLWPNActive || connectedLWPN == null) return;
                
                if (Utils.GCATPP.pushLWPNAndroid(connectedLWPN, cp))
                    connectedLWPNActive = true;
            }
        }

        /*
         * Essentially usefull to fix visual bugged state of lite virused androids (correct cases where the patch is not executed at the end of the mental break and the state not cleared)
         */
        public void checkInfectionFix()
        {
            var cp = (Pawn) parent;

            if (csm == null || csm.Infected != 4 || cp.InMentalState) return;
            
            csm.Infected = -1;
            var he = cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
            if (he == null) cp.health.AddHediff(Utils.hediffNoHost);
        }

        public void checkTXWithSkinFacialTextureUpdate()
        {
            try
            {
                var cp = (Pawn) parent;


                if (!isAndroidWithSkin) return;
                
                Utils.lastResolveAllGraphicsHeadGraphicPath = null;

                //Changement tete
                if (!TXHurtedHeadSet && cp.health.summaryHealth.SummaryHealthPercent <= 0.85f && cp.health.summaryHealth.SummaryHealthPercent > 0.45f || forcedDamageLevel == 1)
                {
                    TXHurtedHeadSet = true;

                    if (TXHurtedHeadSet2)
                    {
                        TXHurtedHeadSet2 = false;
                        if (hair != null)
                            cp.story.hairDef = hair;
                        hair = null;
                    }

                    Utils.changeTXBodyType(cp, 1);
                    Utils.changeHARCrownType(cp, "Average_Hurted");
                    cp.Drawer.renderer.graphics.ResolveAllGraphics();
                    PortraitsCache.SetDirty(cp);
                }
                else if (!TXHurtedHeadSet2 && cp.health.summaryHealth.SummaryHealthPercent <= 0.45f || forcedDamageLevel == 2)
                {
                    TXHurtedHeadSet = false;
                    TXHurtedHeadSet2 = true;
                    if (hair == null)
                        hair = cp.story.hairDef;
                    cp.story.hairDef = DefDatabase<HairDef>.GetNamed("Shaved", false);
                    Utils.changeTXBodyType(cp, 2);
                    Utils.changeHARCrownType(cp, "Average_Hurted2");
                    cp.Drawer.renderer.graphics.ResolveAllGraphics();
                    PortraitsCache.SetDirty(cp);
                }
                else
                {
                    if ((TXHurtedHeadSet || !init) && cp.health.summaryHealth.SummaryHealthPercent > 0.85f)
                    {
                        TXHurtedHeadSet = false;
                        TXHurtedHeadSet2 = false;
                        if (hair != null)
                        {
                            cp.story.hairDef = hair;
                            hair = null;
                        }

                        Utils.changeTXBodyType(cp, 0);
                        Utils.changeHARCrownType(cp, "Average_Normal");
                        cp.Drawer.renderer.graphics.ResolveAllGraphics();
                        PortraitsCache.SetDirty(cp);
                    }
                    else if ((TXHurtedHeadSet2 || !init) && cp.health.summaryHealth.SummaryHealthPercent > 0.45f)
                    {
                        TXHurtedHeadSet2 = false;
                        TXHurtedHeadSet = cp.health.summaryHealth.SummaryHealthPercent <= 0.85f;

                        cp.story.hairDef = hair;
                        hair = null;
                        if (cp.health.summaryHealth.SummaryHealthPercent <= 0.85f)
                        {
                            Utils.changeHARCrownType(cp, "Average_Hurted");
                            Utils.changeTXBodyType(cp, 1);
                        }
                        else
                        {
                            Utils.changeHARCrownType(cp, "Average_Normal");
                            Utils.changeTXBodyType(cp, 0);
                        }

                        cp.Drawer.renderer.graphics.ResolveAllGraphics();
                        PortraitsCache.SetDirty(cp);
                    }

                    //(string)Traverse.Create(p1.story).Field("headGraphicPath").GetValue();
                }

                if (!Utils.RIMMSQOL_LOADED || Utils.lastResolveAllGraphicsHeadGraphicPath == null) return;
                
                cp.story.GetType().GetField("headGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(cp.story, Utils.lastResolveAllGraphicsHeadGraphicPath);
                Utils.lastResolveAllGraphicsHeadGraphicPath = null;
                /*cp.Drawer.renderer.graphics.ResolveAllGraphics();
                        PortraitsCache.SetDirty(cp);*/
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] CompAndroidState.checkTXWithSkinFacialTextureUpdate " + e.Message + " " + e.StackTrace);
            }
        }

        public void checkBlankAndroid()
        {
            var cp = (Pawn) parent;

            if (cp.Dead || !isBlankAndroid) return;
            
            var he = cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffBlankAndroid);
            if (he == null)
                cp.health?.AddHediff(he);
        }

        public void checkRusted()
        {
            try
            {
                var cp = (Pawn) parent;

                //Entitées qui ne rust pas on degage et check avant de faire le menage des rust malplacés
                if (!isAndroidTIer || isAndroidWithSkin || dontRust)
                {
                    var he = cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
                    if (he != null)
                        cp.health.RemoveHediff(he);

                    return;
                }


                if (!Settings.androidsCanRust)
                {
                    if (paintingIsRusted)
                    {
                        paintingIsRusted = false;
                        paintingRustGT = -3;
                    }

                    var he = cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
                    if (he != null)
                        cp.health.RemoveHediff(he);
                }
                else
                {
                    //Reprise de la rouille interrompue
                    if (paintingRustGT == -3 && !paintingIsRusted) setRusted();

                    if (paintingRustGT != -1)
                    {
                        paintingRustGT -= 300;
                        if (paintingRustGT < 0)
                            paintingRustGT = 0;
                    }

                    //Lancement paint auto 1jour avant la fin d'expiration du timeout
                    if (Settings.allowAutoRepaint && (!cp.IsPrisoner || Settings.allowAutoRepaintForPrisoners) && !autoPaintStarted && paintingRustGT <= 60000)
                    {
                        //Déduction recipeDef
                        var color = (AndroidPaintColor) customColor;
                        var paintRecipeDefname = "";

                        switch (color)
                        {
                            case AndroidPaintColor.Black:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkBlack";
                                break;
                            case AndroidPaintColor.Blue:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkBlue";
                                break;
                            case AndroidPaintColor.Cyan:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkCyan";
                                break;
                            case AndroidPaintColor.None:
                            case AndroidPaintColor.Default:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkDefault";
                                break;
                            case AndroidPaintColor.Gray:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkGray";
                                break;
                            case AndroidPaintColor.Green:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkGreen";
                                break;
                            case AndroidPaintColor.Khaki:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkKhaki";
                                break;
                            case AndroidPaintColor.Orange:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkOrange";
                                break;
                            case AndroidPaintColor.Pink:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkPink";
                                break;
                            case AndroidPaintColor.Purple:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkPurple";
                                break;
                            case AndroidPaintColor.Red:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkRed";
                                break;
                            case AndroidPaintColor.White:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkWhite";
                                break;
                            case AndroidPaintColor.Yellow:
                                paintRecipeDefname = "ATPP_PaintAndroidFrameworkYellow";
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        var recipe = DefDatabase<RecipeDef>.GetNamed(paintRecipeDefname, false);
                        if (recipe != null)
                        {
                            //Renouvellement auto de la peinture (ajout operation auto)
                            cp.health.surgeryBills.AddBill(new Bill_Medical(recipe));
                            autoPaintStarted = true;
                        }
                    }

                    //Rouille de la peinture ?
                    if (paintingRustGT == 0 || paintingRustGT == -1 && cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted) == null)
                    {
                        paintingIsRusted = true;
                        cp.Drawer.renderer.graphics.ResolveAllGraphics();
                        PortraitsCache.SetDirty(cp);
                        cp.health.AddHediff(Utils.hediffRusted);

                        paintingRustGT = -1;
                    }
                    else
                    {
                        if (paintingIsRusted) return;
                        //Cas aberrant (possede hediff de rusted alors que pas rusted)
                        var cRusted = cp.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
                        if (cRusted != null) cp.health.RemoveHediff(cRusted);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] CompAndroidState.CheckRusted " + e.Message + " " + e.StackTrace);
            }
        }

        public void setRusted()
        {
            var cp = (Pawn) parent;

            paintingIsRusted = true;
            paintingRustGT = -1;
            cp.health.AddHediff(Utils.hediffRusted);
        }

        public void clearRusted()
        {
            var pawn = (Pawn) parent;

            paintingIsRusted = false;
            autoPaintStarted = false;
            paintingRustGT = Rand.Range(Settings.minDaysAndroidPaintingCanRust, Settings.maxDaysAndroidPaintingCanRust) * 60000;

            pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            if (Find.ColonistBar != null) PortraitsCache.SetDirty(pawn);


            //Retire du hediff de rouille
            var he = pawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
            if (he != null)
                pawn.health.RemoveHediff(he);
        }

        public void checkSolarFlareStuff()
        {
            try
            {
                var cp = (Pawn) parent;

                //Androids avec une peau pas affectés par le solarflare
                if (cp.def.defName == Utils.TX2 || cp.def.defName == Utils.TX3)
                    return;

                if (!isOrganic || cp.VXAndVX0ChipPresent())
                {
                    var solarFlareRunning = Utils.getRandomMapOfPlayer().gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare);

                    //Si android surrogate actuellement controllé par un étranger externe on le deconnecte
                    /*if (externalController != null && surrogateController != null && solarFlareRunning)
                    {
                        CompSurrogateOwner cso = surrogateController.TryGetComp<CompSurrogateOwner>();
                        if (cso != null)
                        {
                            cso.disconnectControlledSurrogate();
                        }
                    }*/

                    if (Settings.disableSolarFlareEffect)
                    {
                        //Retrait heddif si il avait été ajouté
                        if (solarFlareEffectApplied)
                        {
                            var cpawn = (Pawn) parent;
                            var he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_SolarFlareAndroidImpact"));
                            if (he != null)
                                cpawn.health.RemoveHediff(he);
                        }

                        solarFlareEffectApplied = false;
                        return;
                    }

                    //Application de l'effet
                    if (solarFlareRunning && !solarFlareEffectApplied)
                    {
                        var cpawn = (Pawn) parent;
                        //Ajout heddif
                        cpawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_SolarFlareAndroidImpact"));

                        solarFlareEffectApplied = true;
                    }

                    //Suppression de l'effet
                    if (solarFlareRunning || !solarFlareEffectApplied) return;
                    {
                        var cpawn = (Pawn) parent;
                        //Ajout heddif
                        var he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_SolarFlareAndroidImpact"));
                        if (he != null)
                            cpawn.health.RemoveHediff(he);

                        //Suppression de l'heddif
                        solarFlareEffectApplied = false;
                    }
                }
                else
                {
                    var he = cp.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_SolarFlareAndroidImpact"));
                    if (he != null)
                        cp.health.RemoveHediff(he);
                }
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] CompAndroidState.checkSolarFlareStuff " + e.Message + " " + e.StackTrace);
            }
        }

        public void addNoRemoteHostHediff()
        {
            var cpawn = (Pawn) parent;
            //Check si surrogate et pas de controlleur ET possede pas de noHost alors on l'ajoute (===> effet d'un item externe cleanant les heddifs)
            var he = cpawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
            if (he == null) cpawn.health.AddHediff(Utils.hediffNoHost);
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            csm = parent.TryGetComp<CompSkyMind>();
            var pawn = (Pawn) parent;
            isAndroidWithSkin = Utils.ExceptionAndroidWithSkinList.Contains(pawn.def.defName);
            dontRust = Utils.ExceptionAndroidsDontRust.Contains(pawn.def.defName);

            var isAndroidTier = pawn.IsAndroidTier();

            if (!isAndroidTier && !Utils.ExceptionAndroidAnimalPowered.Contains(pawn.def.defName)) isOrganic = true;

            if (isOrganic)
                useBattery = false;


            var MUID = parent.Map.GetUniqueLoadID();

            if (!respawningAfterLoad)
                Utils.GCATPP.pushSurrogateAndroidNotifyMapChanged((Pawn) parent, MUID);

            //Suppression traits blacklistés le cas echeant
            if (isAndroidTier && (!isSurrogate || isSurrogate && surrogateController != null && surrogateController.IsAndroidTier()))
                Utils.removeMindBlacklistedTrait(pawn);

            isAndroidTIer = isAndroidTier;

            checkInfectionFix();

            if (isAndroidTier)
            {
                if (isAndroidWithSkin)
                {
                    if (pawn.gender == Gender.Male)
                    {
                        var bd = DefDatabase<BodyTypeDef>.GetNamed("Male", false);
                        if (bd != null)
                            pawn.story.bodyType = bd;
                    }
                    else
                    {
                        var bd = DefDatabase<BodyTypeDef>.GetNamed("Female", false);
                        if (bd != null)
                            pawn.story.bodyType = bd;
                    }
                }

                if (pawn.ownership != null && pawn.ownership.OwnedBed != null)
                    if (pawn.ownership.OwnedBed.ForPrisoners != pawn.IsPrisoner)
                        pawn.ownership.UnclaimBed();
                //Starting du délais de rusting
                if (dontRust) return;
                
                if (paintingRustGT == -2) paintingRustGT = Rand.Range(Settings.minDaysAndroidPaintingCanRust, Settings.maxDaysAndroidPaintingCanRust) * 60000;

                if (paintingRustGT != -1 || !paintingIsRusted || pawn.health == null) return;
                    
                var he = pawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
                if (he == null) pawn.health.AddHediff(Utils.hediffRusted);
            }
            else
            {
                if (pawn.health != null)
                {
                    paintingIsRusted = false;
                    var he = pawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffRusted);
                    if (he != null)
                        pawn.health.RemoveHediff(he);
                }

                var cpawn = pawn;

                //Si VX0 dans une session en cours alors on chope le pawn permuté controleur
                if (surrogateController != null)
                    cpawn = surrogateController;

                //Reset du child et adulthood si VX0 organic
                if (!isSurrogate || !isOrganic || cpawn.story == null || cpawn.story.adulthood == null) return;
                
                if (cpawn.story.childhood != null)
                {
                    BackstoryDatabase.TryGetWithIdentifier("MercenaryRecruit", out var bs);
                    if (bs != null)
                        cpawn.story.childhood = bs;
                }

                cpawn.story.adulthood = null;
                //Reset incapable of
                Utils.ResetCachedIncapableOf(cpawn);
            }
        }


        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            if (Utils.POWERPP_LOADED && connectedLWPN != null)
            {
                if (connectedLWPNActive)
                    Utils.GCATPP.popLWPNAndroid(connectedLWPN, (Pawn) parent);

                connectedLWPN = null;
                connectedLWPNActive = false;
            }

            //Si surrogate on notifis le changement de map de ce dernier pour qu'il soit correctement traqué
            if (!isSurrogate) return;

            var MUID = "caravan";
            if (map != null)
                MUID = map.GetUniqueLoadID();

            Utils.GCATPP.pushSurrogateAndroidNotifyMapChanged((Pawn) parent, MUID);
        }


        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            Utils.removeUploadHediff((Pawn) parent, uploadRecipient);

            if (uploadEndingGT != -1)
                checkInterruptedUpload();

            /*if (isSurrogate)
                Utils.GCATPP.popSurrogateAndroid((Pawn)parent);*/

            if (!isSurrogate || previousMap != null) return;
            
            var MUID = "caravan";
            Utils.GCATPP.pushSurrogateAndroidNotifyMapChanged((Pawn) parent, MUID);
        }

        public override void ReceiveCompSignal(string signal)
        {
            base.ReceiveCompSignal(signal);

            switch (signal)
            {
                case "SkyMindNetworkUserConnected":
                    break;
                case "SkyMindNetworkUserDisconnected":
                    //On va  invoquer le checkInterruption pour les duplicate et permutation 
                    checkInterruptedUpload();
                    break;
            }
        }

        private bool isRegularM7()
        {
            //Les M7Mech standard ne sont pas controlables
            return !isSurrogate && isM7();
        }

        private bool isM7()
        {
            //Les M7Mech standard ne sont pas controlables
            return parent.def.defName == "M7Mech";
        }


        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            var pawn = (Pawn) parent;
            var isPrisoner = pawn.IsPrisoner;
            var transfertAllowed = Utils.mindTransfertsAllowed((Pawn) parent);

            //Si androide virusé (hacking) ajout boutton permettant de le shutdown
            if (csm != null && csm.Hacked == 1)
            {
                yield return new Command_Action
                {
                    icon = Tex.StopVirused,
                    defaultLabel = "ATPP_StopVirused".Translate(),
                    defaultDesc = "ATPP_StopVirusedDesc".Translate(),
                    action = delegate { pawn.Kill(null); }
                };

                yield break;
            }

            if (!isOrganic && pawn.Faction == Faction.OfPlayer)
            {
                //Ajout possibilité de lancer l'explosion d'un androide a distance

                if (Utils.ResearchAndroidBatteryOverload.IsFinished)
                {
                    var tex = Tex.ForceAndroidToExplode;

                    if (batteryExplosionEndingGT != -1)
                        tex = Tex.ForceAndroidToExplodeDisabled;

                    yield return new Command_Action
                    {
                        icon = tex,
                        defaultLabel = "ATPP_OverloadAndroid".Translate(),
                        defaultDesc = "ATPP_OverloadAndroidDesc".Translate(),
                        action = delegate
                        {
                            if (batteryExplosionEndingGT != -1)
                                return;

                            Find.WindowStack.Add(new Dialog_Msg("ATPP_UploadMakeAndroidBatteryOverloadConfirm".Translate(),
                                "ATPP_UploadMakeAndroidBatteryOverloadConfirmDesc".Translate(), delegate
                                {
                                    batteryExplosionStartingGT = Find.TickManager.TicksGame;
                                    batteryExplosionEndingGT = batteryExplosionStartingGT + 930;
                                }));
                        }
                    };
                }

                //Si POWER++ chargé ajout possibilité de rattaché android à un LWPN
                if (Utils.POWERPP_LOADED && useBattery)
                {
                    var tex = Tex.LWPNConnected;

                    if (connectedLWPN == null || !connectedLWPNActive || connectedLWPN.Destroyed || !connectedLWPN.TryGetComp<CompPowerTrader>().PowerOn)
                        tex = Tex.LWPNNotConnected;

                    yield return new Command_Action
                    {
                        icon = tex,
                        defaultLabel = "ARKPPP_LWPSel".Translate(),
                        defaultDesc = "",
                        action = delegate
                        {
                            FloatMenu floatMenuMap;

                            var opts = (from build in parent.Map.listerBuildings.allBuildingsColonist
                                where (build.def.defName == "ARKPPP_LocalWirelessPowerEmitter" || build.def.defName == "ARKPPP_LocalWirelessPortablePowerEmitter") && !build.IsBrokenDown() && build.TryGetComp<CompPowerTrader>().PowerOn
                                let compLWPNEmitter = Utils.TryGetCompByTypeName(build, "CompLocalWirelessPowerEmitter", "Power++")
                                where compLWPNEmitter != null
                                let lib = getConnectedLWPNLabel(build)
                                select new FloatMenuOption("ARKPPP_WPNListRow".Translate(lib, ((int) Utils.getCurrentAvailableEnergy(build.PowerComp.PowerNet)).ToString()), delegate
                                {
                                    if (connectedLWPN != null)
                                    {
                                        Utils.GCATPP.popLWPNAndroid(connectedLWPN, pawn);
                                        connectedLWPNActive = false;
                                        connectedLWPN = null;
                                    }

                                    if (Utils.GCATPP.pushLWPNAndroid(build, pawn))
                                    {
                                        connectedLWPN = build;
                                        connectedLWPNActive = true;
                                    }
                                    else
                                    {
                                        Messages.Message("ATPP_MessageLWPNNoSlotAvailable".Translate(), MessageTypeDefOf.NegativeEvent);
                                    }
                                })).ToList();

                            //SI pas choix affichage de la raison 
                            if (opts.Count == 0)
                                opts.Add(new FloatMenuOption("ATPP_NoAvailableLWPN".Translate(), null));

                            //Si le recepteur est configuré pour se connecter a un LWPN définis on ajoute une option de deconnexion
                            if (connectedLWPN != null)
                                opts.Add(new FloatMenuOption("ARKPPP_ClearCurrentWPNConnection".Translate(), delegate
                                {
                                    if (connectedLWPN != null)
                                        Utils.GCATPP.popLWPNAndroid(connectedLWPN, (Pawn) parent);

                                    connectedLWPN = null;
                                    connectedLWPNActive = false;
                                }));

                            floatMenuMap = new FloatMenu(opts, "ARKPPP_LocalWirelessPowerSelectorListTitle".Translate());
                            Find.WindowStack.Add(floatMenuMap);
                        }
                    };
                }
            }

            if (!isM7())
            {
                if (!isOrganic)
                    yield return new Command_Toggle
                    {
                        icon = Tex.Battery,
                        defaultLabel = "ATPP_UseBattery".Translate(),
                        defaultDesc = "ATPP_UseBatteryDesc".Translate(),
                        isActive = () => useBattery,
                        toggleAction = delegate
                        {
                            useBattery = !useBattery;
                            if (!useBattery && connectedLWPNActive) Utils.GCATPP.popLWPNAndroid(connectedLWPN, (Pawn) parent);
                        }
                    };


                if (!Utils.GCATPP.isConnectedToSkyMind(pawn) || isPrisoner)
                    yield break;

                var uploadInProgress = showUploadProgress || uploadEndingGT != -1;

                if (!isOrganic && !isSurrogate && Utils.anySkyMindNetResearched())
                {
                    var selTex = Tex.UploadConsciousness;


                    if (!transfertAllowed)
                        selTex = Tex.UploadConsciousnessDisabled;

                    yield return new Command_Action
                    {
                        icon = selTex,
                        defaultLabel = "ATPP_UploadConsciousness".Translate(),
                        defaultDesc = "ATPP_UploadConsciousnessDesc".Translate(),
                        action = delegate
                        {
                            if (!transfertAllowed)
                                return;

                            Utils.ShowFloatMenuAndroidCandidate((Pawn) parent,
                                delegate(Pawn target)
                                {
                                    Find.WindowStack.Add(new Dialog_Msg("ATPP_UploadConsciousnessConfirm".Translate(parent.LabelShortCap, target.LabelShortCap),
                                        "ATPP_UploadConsciousnessConfirmDesc".Translate(parent.LabelShortCap, target.LabelShortCap) + "\n" +
                                        "ATPP_WarningSkyMindDisconnectionRisk".Translate(), delegate { OnPermuteConfirmed((Pawn) parent, target); }));
                                });
                        }
                    };
                }
            }

            //Permet de deconnecter l'utilisateur connecté sur le robot
            if (isSurrogate)
            {
                if (surrogateController != null)
                {
                    yield return new Command_Action
                    {
                        icon = Tex.AndroidToControlTargetDisconnect,
                        defaultLabel = "ATPP_AndroidToControlTargetDisconnect".Translate(),
                        defaultDesc = "ATPP_AndroidToControlTargetDisconnectDesc".Translate(),
                        action = delegate
                        {
                            var cso = surrogateController.TryGetComp<CompSurrogateOwner>();
                            if (cso != null)
                                cso.disconnectControlledSurrogate((Pawn) parent);
                        }
                    };
                }
                else
                {
                    if (lastController != null)
                        //Permet au surrogate de se relier au dernier controller
                        yield return new Command_Action
                        {
                            icon = Tex.AndroidSurrogateReconnectToLastController,
                            defaultLabel = "ATPP_AndroidSurrogateReconnectToLastController".Translate(),
                            defaultDesc = "ATPP_AndroidSurrogateReconnectToLastControllerDesc".Translate(),
                            action = delegate
                            {
                                if (lastController == null || lastController.Dead)
                                {
                                    Messages.Message("ATPP_CannotReconnectToLastSurrogateController".Translate(), MessageTypeDefOf.NegativeEvent);
                                    return;
                                }

                                var VX3Owner = lastController.VX3ChipPresent();
                                var cso = lastController.TryGetComp<CompSurrogateOwner>();
                                if (cso != null)
                                {
                                    //Check so lastController est un mind dans ce cas check qu'il ne fait pas deja autre chose
                                    if (cso.skyCloudHost != null)
                                    {
                                        var csc = cso.skyCloudHost.TryGetComp<CompSkyCloudCore>();
                                        if (csc == null || csc.mindIsBusy(lastController))
                                        {
                                            Messages.Message("ATPP_CannotReconnectToLastSurrogateController".Translate(), MessageTypeDefOf.NegativeEvent);
                                            return;
                                        }
                                    }

                                    //Si controller deconnecté tenttive reconnection au SkyMind
                                    var isConnected = true;
                                    if (!Utils.GCATPP.isConnectedToSkyMind(lastController))
                                        if (!Utils.GCATPP.connectUser(lastController))
                                            isConnected = false;

                                    //Deja en session le lastUser on jerte
                                    if (!isConnected || !VX3Owner && cso.isThereSX() || VX3Owner && cso.availableSX.Count + 1 > Settings.VX3MaxSurrogateControllableAtOnce ||
                                        !cso.controlMode)
                                    {
                                        Messages.Message("ATPP_CannotReconnectToLastSurrogateController".Translate(), MessageTypeDefOf.NegativeEvent);
                                        return;
                                    }

                                    cso.setControlledSurrogate((Pawn) parent);
                                }
                            }
                        };
                }
            }
            else
            {
                //Si pas un surrogate

                if (!Utils.GCATPP.isConnectedToSkyMind(parent) || isBlankAndroid) yield break;
                
                var cso = parent.TryGetComp<CompSurrogateOwner>();

                //Pas d'organique ou de controlleur de surrogate en corus de session peuvent faire l'operation d'augmentation de points
                if (!isOrganic && (cso == null || !cso.isThereSX()))
                    yield return new Command_Action
                    {
                        icon = Tex.SkillUp,
                        defaultLabel = "ATPP_Skills".Translate(),
                        defaultDesc = "ATPP_SkillsDesc".Translate(),
                        action = delegate { Find.WindowStack.Add(new Dialog_SkillUp((Pawn) parent)); }
                    };
            }
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";
            try
            {
                if (parent.Map == null || isRegularM7())
                    return base.CompInspectStringExtra();

                /*foreach (var cbp in parent.def.race.body.corePart.parts.ToList())
                {

                        Log.Message("1=>"+cbp.def.defName);
                }

                foreach (var cbp in ((Pawn)parent).RaceProps.body.AllParts)
                {
                     Log.Message("2=>" + cbp.def.defName);
                }*/


                var lvl = 0;
                var cp = (Pawn) parent;

                if (cp.needs.food != null)
                    lvl = (int) (cp.needs.food.CurLevelPercentage * 100);

                if (!isOrganic)
                {
                    ret += "ATPP_BatteryLevel".Translate(lvl) + "\n";

                    if (Utils.POWERPP_LOADED && connectedLWPN != null)
                    {
                        if (connectedLWPNActive)
                            ret += "ATPP_LWPNConnected".Translate(getConnectedLWPNLabel(connectedLWPN)) + "\n";
                        else
                            ret += "ATPP_LWPNDisconnected".Translate(getConnectedLWPNLabel(connectedLWPN)) + "\n";
                    }

                    if (batteryExplosionEndingGT != -1)
                    {
                        float p;
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - batteryExplosionStartingGT) / (float) (batteryExplosionEndingGT - batteryExplosionStartingGT));
                        ret += "ATPP_BatteryExplodeInProgress".Translate(((int) (p * 100)).ToString()) + "\n";
                    }

                    if (uploadEndingGT != -1 || showUploadProgress)
                    {
                        //Calcul pourcentage de transfert
                        float p;
                        string action;

                        if (uploadEndingGT == -1)
                        {
                            var cab = uploadRecipient.TryGetComp<CompAndroidState>();
                            p = Math.Min(1.0f, (Find.TickManager.TicksGame - cab.uploadStartGT) / (float) (cab.uploadEndingGT - cab.uploadStartGT));
                            action = "ATPP_DownloadPercentage";
                        }
                        else
                        {
                            p = Math.Min(1.0f, (Find.TickManager.TicksGame - uploadStartGT) / (float) (uploadEndingGT - uploadStartGT));
                            action = "ATPP_UploadPercentage";
                        }


                        ret += action.Translate(((int) (p * 100)).ToString()) + "\n";
                    }

                    if (frameworkNaniteEffectGTEnd != -1)
                    {
                        float p;
                        p = Math.Min(1.0f, (Find.TickManager.TicksGame - frameworkNaniteEffectGTStart) / (float) (frameworkNaniteEffectGTEnd - frameworkNaniteEffectGTStart));
                        ret += "ATPP_NaniteFrameworkRepairingInProgress".Translate(((int) (p * 100)).ToString()) + "\n";
                    }

                    if (paintingIsRusted) ret += "ATPP_Rusted".Translate() + "\n";
                }

                if (!isSurrogate) return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
                
                if (surrogateController != null)
                    ret += "ATPP_RemotelyControlledBy".Translate(((Pawn) parent).LabelShortCap) + "\n";

                if (lastController != null && externalController == null)
                    ret += "ATPP_PreviousSurrogateControllerIs".Translate(lastController.LabelShortCap) + "\n";


                if (surrogateController == null) return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
                    
                var cso = surrogateController.TryGetComp<CompSurrogateOwner>();
                if (cso == null || !surrogateController.VX3ChipPresent()) return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
                        
                if (cso.SX == parent)
                    ret += "ATPP_VX3SurrogateTypePrimary".Translate() + "\n";
                else
                    ret += "ATPP_VX3SurrogateTypeSecondary".Translate() + "\n";

                return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] CompAndroidState.CompInspectStringExtra " + e.Message + " " + e.StackTrace);
                return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
            }
        }


        public string getConnectedLWPNLabel(Building LWPNEmitter)
        {
            if (LWPNEmitter == null)
                return "";

            var ret = "";
            var GC_PPP = Utils.TryGetGameCompByTypeName("GC_PPP");
            var GCPPP = Traverse.Create(GC_PPP);
            var compLWPNEmitter = Utils.TryGetCompByTypeName(LWPNEmitter, "CompLocalWirelessPowerEmitter", "Power++");
            if (compLWPNEmitter == null) return ret;
            
            var LWPNID = (string) Traverse.Create(compLWPNEmitter).Field("LWPNID").GetValue();
            ret = (string) GCPPP.Method("getLWPNLabel", LWPNID, false).GetValue();

            return ret;
        }

        /*
         * Detecte un cas d'interruption est le cas echeant tue les protagoniste de l'upload tous en affichant un message d'erreur
         */
        public void checkInterruptedUpload()
        {
            var killSelf = false;
            var cpawn = (Pawn) parent;

            var recipientDeadOrNull = uploadRecipient == null || uploadRecipient.Dead;
            var recipientConnected = false;
            var emitterConnected = false;
            if (uploadRecipient != null && Utils.GCATPP.isConnectedToSkyMind(uploadRecipient, true))
                recipientConnected = true;

            if (Utils.GCATPP.isConnectedToSkyMind(cpawn))
                emitterConnected = true;

            if (isSurrogate && surrogateController != null)
            {
                var cso = surrogateController.TryGetComp<CompSurrogateOwner>();
                if (cso == null)
                    return;

                //Surrogate en cours on check si clones toujours connecté
                if (cso.isThereSX() && cso.availableSX != null)
                {
                    var hostBadConn = false;

                    //Si surrogateController stoclé dans le skyCloud
                    if (cso.skyCloudHost != null)
                        hostBadConn = !cso.skyCloudHost.TryGetComp<CompSkyCloudCore>().isRunning();
                    else
                        hostBadConn = !Utils.GCATPP.isConnectedToSkyMind(surrogateController, true);

                    var surrogateBadConn = !Utils.GCATPP.isConnectedToSkyMind(cpawn, true);

                    if (hostBadConn || surrogateBadConn)
                    {
                        //Log.Message("DDDDD==>"+ (!Utils.GCATPP.isConnectedToSkyMind(cpawn))+" "+ (!Utils.GCATPP.isConnectedToSkyMind(SX)));

                        var disconnectedPawn = cpawn;
                        var invertedPawn = surrogateController;
                        if (hostBadConn)
                        {
                            disconnectedPawn = surrogateController;
                            invertedPawn = cpawn;
                        }

                        //Notification de la deconnexion accidentelle
                        if (disconnectedPawn != null && invertedPawn != null && disconnectedPawn.Faction == Faction.OfPlayer)
                            Messages.Message("ATPP_SurrogateUnexpectedDisconnection".Translate(invertedPawn.LabelShortCap), disconnectedPawn, MessageTypeDefOf.NegativeEvent);

                        //un ou les deux des composantes sont déconnectés ===> on lance la deconnection du SX
                        cso.stopControlledSurrogate(cpawn);
                    }
                }
            }

            //Si hote plus valide alors on arrete le processus et on kill les deux androids
            if (uploadEndingGT == -1 || (!recipientDeadOrNull && !cpawn.Dead && emitterConnected && recipientConnected)) return;
            
            var reason = "";
            if (recipientDeadOrNull)
            {
                reason = "ATPP_LetterInterruptedUploadDescCompHostDead".Translate();
                killSelf = true;
            }

            if (cpawn.Dead)
            {
                reason = "ATPP_LetterInterruptedUploadDescCompSourceDead".Translate();
                if (uploadRecipient != null && !uploadRecipient.Dead) uploadRecipient.Kill(null);
            }

            if (reason == "")
                if (!recipientConnected || !emitterConnected)
                {
                    reason = "ATPP_LetterInterruptedUploadDescCompDiconnectionError".Translate();

                    killSelf = true;
                    if (uploadRecipient != null && !uploadRecipient.Dead) uploadRecipient.Kill(null);
                }

            resetUploadStuff();

            if (killSelf)
                if (!cpawn.Dead)
                    cpawn.Kill(null);

            Utils.showFailedLetterMindUpload(reason);
        }

        public void initAsSurrogate()
        {
            // on va lui ajouter un hediff afin de le downer en permanence(pas d'hote)
            var cpawn = (Pawn) parent;

            isSurrogate = true;
            addNoRemoteHostHediff();
        }

        public void resetInternalState()
        {
            resetUploadStuff();
        }

        private void resetUploadStuff()
        {
            if (uploadRecipient != null)
            {
                var cab = uploadRecipient.TryGetComp<CompAndroidState>();
                cab.showUploadProgress = false;
                cab.uploadRecipient = null;
            }

            uploadStartGT = 0;
            uploadEndingGT = -1;
            uploadRecipient = null;
        }

        private void OnPermuteConfirmed(Pawn source, Pawn dest)
        {
            //Ajout hediff de transfert aux deux androids
            source.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
            dest.health.AddHediff(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));

            var CGT = Find.TickManager.TicksGame;
            uploadRecipient = dest;
            uploadStartGT = CGT;
            uploadEndingGT = CGT + Settings.mindUploadHour * 2500;

            var cab = dest.TryGetComp<CompAndroidState>();
            cab.showUploadProgress = true;
            cab.uploadRecipient = (Pawn) parent;

            Messages.Message("ATPP_StartUpload".Translate(source.LabelShortCap, dest.LabelShortCap), parent, MessageTypeDefOf.PositiveEvent);
        }
    }
}