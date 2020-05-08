using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace MOARANDROIDS
{
    public class GC_ATPP : GameComponent
    {
        public Faction androidFactionCoalition;
        public Faction androidFactionInsurrection;
        private bool appliedSettingsOnReload;
        private Dictionary<Building, IEnumerable<IntVec3>> cacheATN;

        private List<Thing> connectedThing = new List<Thing>();
        private Game game;
        private List<Thing> listerConnectedDevices = new List<Thing>();
        private List<Building> listerHackingServers = new List<Building>();

        public Dictionary<Map, List<Building>> listerHeatSensitiveDevices;

        public Dictionary<Building, List<Pawn>> listerLWPNAndroid = new Dictionary<Building, List<Pawn>>();

        private Dictionary<Map, List<Building>> listerReloadStation = new Dictionary<Map, List<Building>>();
        private List<Building> listerSecurityServers = new List<Building>();
        private List<Building> listerSkillServers = new List<Building>();
        private List<Building> listerSkyCloudCores = new List<Building>();
        private List<Building> listerSkyCloudCoresAbs = new List<Building>();
        private List<Thing> listerSkyMindable = new List<Thing>();
        private Dictionary<Map, List<Building>> listerSkyMindRelays = new Dictionary<Map, List<Building>>();
        private Dictionary<Map, List<Building>> listerSkyMindServers = new Dictionary<Map, List<Building>>();
        private List<Pawn> listerSkyMindUsers = new List<Pawn>();
        private Dictionary<Map, List<Building>> listerSkyMindWANServers = new Dictionary<Map, List<Building>>();
        private Dictionary<string, List<Pawn>> listerSurrogateAndroids = new Dictionary<string, List<Pawn>>();
        private List<Thing> listerVirusedThings = new List<Thing>();
        private int nbHackingPoints;

        private int nbHackingSlot;
        private int nbSecuritySlot;
        private int nbSkillPoints;
        private int nbSkillSlot;
        private int nbSlot;


        public Dictionary<string, string> QEEAndroidHair = new Dictionary<string, string>();
        public Dictionary<string, string> QEEAndroidHairColor = new Dictionary<string, string>();
        public Dictionary<string, string> QEESkinColor = new Dictionary<string, string>();
        private int S0NID = 1;
        private int S10NID = 1;

        private int S1NID = 1;
        private int S2NID = 1;
        private int S3NID = 1;
        private int S4NID = 1;

        private List<Settlement> savedIASCoalition = new List<Settlement>();
        private List<Settlement> savedIASInsurrection = new List<Settlement>();


        private int SkyCloureCoreID = 1;
        private int SX2KNID = 1;
        private int SX2NID = 1;
        private int SX3NID = 1;
        private int SX4NID = 1;
        public Dictionary<string, Pawn> VatGrowerLastPawnInProgress = new Dictionary<string, Pawn>();
        public Dictionary<string, bool> VatGrowerLastPawnIsTX = new Dictionary<string, bool>();

        public GC_ATPP(Game game)
        {
            this.game = game;
            Utils.GCATPP = this;
            initNull();

            if (!Utils.init)
            {
                try
                {
                    Utils.init = true;

                    try
                    {
                        CPaths.dummyRest = new Need_DummyRest(null);
                    }
                    catch (Exception)
                    {
                    }

                    Utils.CrafterDoctorJob = new List<WorkGiverDef>();

                    Utils.hediffHaveRXChip = DefDatabase<HediffDef>.GetNamed("ATPP_HediffRXChip");
                    Utils.hediffLowNetworkSignal = DefDatabase<HediffDef>.GetNamed("ATPP_LowNetworkSignal");
                    Utils.hediffHaveVX0Chip = DefDatabase<HediffDef>.GetNamed("ATPP_HediffVX0Chip");
                    Utils.hediffHaveVX1Chip = DefDatabase<HediffDef>.GetNamed("ATPP_HediffVX1Chip");
                    Utils.hediffHaveVX2Chip = DefDatabase<HediffDef>.GetNamed("ATPP_HediffVX2Chip");
                    Utils.hediffHaveVX3Chip = DefDatabase<HediffDef>.GetNamed("ATPP_HediffVX3Chip");
                    Utils.hediffRusted = DefDatabase<HediffDef>.GetNamed("ATPP_Rusted");
                    Utils.hediffNoHost = DefDatabase<HediffDef>.GetNamed("ATPP_NoHost");
                    Utils.hediffBlankAndroid = DefDatabase<HediffDef>.GetNamed("ATPP_BlankAndroid");

                    Utils.soundDefSurrogateConnection = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSurrogateConnection");
                    Utils.soundDefSurrogateConnectionStopped = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSurrogateDisconnect");
                    Utils.soundDefTurretConnection = DefDatabase<SoundDef>.GetNamed("ATPP_SoundTurretConnection");
                    Utils.soundDefTurretConnectionStopped = DefDatabase<SoundDef>.GetNamed("ATPP_SoundTurretDisconnect");
                    Utils.soundDefSkyCloudPrimarySystemsOnline = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudPrimarySystemsOnline");
                    Utils.soundDefSkyCloudAllMindDisconnected = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudAllMindDisconnected");
                    Utils.soundDefSkyCloudMindDeletionCompleted = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindDeletionCompleted");
                    Utils.soundDefSkyCloudMindMigrationCompleted = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindMigrationCompleted");
                    Utils.soundDefSkyCloudMindReplicationCompleted = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindReplicationCompleted");
                    Utils.soundDefSkyCloudPowerFailure = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudPowerFailure");
                    Utils.soundDefSkyCloudSkyMindNetworkOffline = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudSkyMindNetworkOffline");
                    Utils.soundDefSkyCloudDoorOpened = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudDoorOpened");
                    Utils.soundDefSkyCloudDoorClosed = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudDoorClosed");
                    Utils.soundDefSkyCloudDeviceActivated = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudDeviceActivated");
                    Utils.soundDefSkyCloudDeviceDeactivated = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudDeviceDeactivated");
                    Utils.soundDefSkyCloudMindDownloadCompleted = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindDownloadCompleted");
                    Utils.soundDefSkyCloudMindUploadCompleted = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindUploadCompleted");
                    Utils.soundDefSkyCloudMindQuarantineMentalState = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSkyCloudMindQuarantineMentalState");

                    Utils.thoughtDefVX0Puppet = DefDatabase<ThoughtDef>.GetNamed("ATPP_VX0PuppetThought");

                    Utils.soundDefSurrogateHacked = DefDatabase<SoundDef>.GetNamed("ATPP_SoundSurrogateHacked");
                    Utils.dummyHeddif = DefDatabase<HediffDef>.GetNamed("ATPP_DummyHediff");
                    Utils.ResearchProjectSkyMindLAN = DefDatabase<ResearchProjectDef>.GetNamed("ATPP_ResearchSkyMindLAN");
                    Utils.ResearchProjectSkyMindWAN = DefDatabase<ResearchProjectDef>.GetNamed("ATPP_ResearchSkyMindWAN");
                    Utils.ResearchAndroidBatteryOverload = DefDatabase<ResearchProjectDef>.GetNamed("ATPP_ResearchBatteryOverload");
                    Utils.statDefAndroidTending = DefDatabase<StatDef>.GetNamed("ATPP_AndroidTendQuality");

                    Utils.statDefAndroidSurgerySuccessChance = DefDatabase<StatDef>.GetNamed("AndroidSurgerySuccessChance");

                    Utils.traitSimpleMinded = DefDatabase<TraitDef>.GetNamed("SimpleMindedAndroid", false);


                    foreach (var el in Utils.ExceptionAndroidList.Where(el => !Utils.ExceptionAndroidWithSkinList.Contains(el))) Utils.ExceptionAndroidWithoutSkinList.Add(el);


                    if (DefDatabase<PawnKindDef>.GetNamed("ATPP_AndroidTX2CollectiveSoldier", false) != null)
                    {
                        Utils.TXSERIE_LOADED = true;


                        foreach (var el in Utils.ExceptionTXSerie) Utils.ExceptionAndroidCorpseList.Add("Corpse_" + el);
                    }

                    try
                    {
                        Utils.ExceptionRepairableFrameworkHediff = new List<HediffDef>
                            {HediffDefOf.Scratch, HediffDefOf.Bite, HediffDefOf.Burn, HediffDefOf.Cut, HediffDefOf.Gunshot, HediffDefOf.Stab, HediffDefOf.SurgicalCut};

                        var hd = DefDatabase<HediffDef>.GetNamed("Shredded", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                        hd = DefDatabase<HediffDef>.GetNamed("Frostbite", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                        hd = DefDatabase<HediffDef>.GetNamed("Mangled", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                        hd = DefDatabase<HediffDef>.GetNamed("SurgicalCut", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                        hd = DefDatabase<HediffDef>.GetNamed("Crush", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                        hd = DefDatabase<HediffDef>.GetNamed("Crack", false);
                        if (hd != null)
                            Utils.ExceptionRepairableFrameworkHediff.Add(hd);
                    }
                    catch (Exception e)
                    {
                        Log.Message("[ATPP] RepairableFrameworkHediffException " + e.Message + " " + e.StackTrace);
                    }


                    try
                    {
                        Utils.applySolarFlarePolicy();
                    }
                    catch (Exception ex)
                    {
                        Log.Message("[ATPP] applySolarSlarePolicy " + ex.Message + " " + ex.StackTrace);
                    }

                    try
                    {
                        Utils.applyT5ClothesPolicy();
                    }
                    catch (Exception ex)
                    {
                        Log.Message("[ATPP] applyT5ClothesPolicy " + ex.Message + " " + ex.StackTrace);
                    }

                    var r = DefDatabase<RecipeDef>.GetNamed("ATPP_DisassembleAndroid", false);

                    var tcd = DefDatabase<ThingCategoryDef>.GetNamed("alienCorpseCategory", false);

                    if (tcd != null && r != null)
                        foreach (var el in tcd.childThingDefs.Where(el => el != null).Where(el => !Utils.ExceptionAndroidCorpseList.Contains(el.defName)))
                        {
                            Log.Message("[ATPP] BlacklistigOtherAR  : " + el.defName);
                            r.fixedIngredientFilter.SetAllow(el, false);
                        }


                    if (Utils.TXSERIE_LOADED)
                        try
                        {
                            var tx4 = DefDatabase<ThingDef>.GetNamed("ATPP_Android4TX", false);
                            var tx3 = DefDatabase<ThingDef>.GetNamed("ATPP_Android3TX", false);

                            var humanMeat = DefDatabase<ThingDef>.GetNamed("Meat_Human", false);
                            if (tx3 != null && tx4 != null && humanMeat != null)
                            {
                                tx4.butcherProducts.Add(new ThingDefCountClass(humanMeat, 100));
                                tx3.butcherProducts.Add(new ThingDefCountClass(humanMeat, 100));
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] GC_ATPP.HumanMeatInjection " + e.Message + " " + e.StackTrace);
                        }

                    Utils.ExceptionAndroidCanReloadWithPowerList = Utils.ExceptionAndroidList.Concat(Utils.ExceptionAndroidAnimalPowered).ToList();


                    foreach (var td in DefDatabase<ThingDef>.AllDefsListForReading)
                        try
                        {
                            if (td?.race != null)
                            {
                                if (Utils.BIRDSANDBEES_LOADED && (Utils.ExceptionAndroidList.Contains(td.defName) || Utils.ExceptionAndroidAnimals.Contains(td.defName)))
                                {
                                    Log.Message("[ATPP] BIRDSANDBEES.fix " + td.defName);
                                    try
                                    {
                                        if (td.recipes != null)
                                            foreach (var cr in td.recipes.ToList().Where(cr =>
                                                cr.defName == "Neuter" || cr.defName == "InstallBasicReproductiveOrgans" || cr.defName == "InstallBionicReproductiveOrgans"))
                                                td.recipes.Remove(cr);


                                        if (td.race.body != null && td.race.body.corePart != null && td.race.body.corePart.parts != null)
                                        {
                                            foreach (var cbp in td.race.body.corePart.parts.ToList().Where(cbp => cbp.def.defName == "ReproductiveOrgans"))
                                                td.race.body.corePart.parts.Remove(cbp);
                                            foreach (var cbp in td.race.body.AllParts.ToList().Where(cbp => cbp.def.defName == "ReproductiveOrgans"))
                                                td.race.body.AllParts.Remove(cbp);
                                        }


                                        if (td.race.hediffGiverSets != null)
                                            foreach (var hg in td.race.hediffGiverSets.ToList().Where(hg => hg.defName == "HumanoidFertility"))
                                                td.race.hediffGiverSets.Remove(hg);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Message("[ATPP] BIRDSANDBEES.fix " + ex.Message + " " + ex.StackTrace);
                                    }
                                }

                                if (td.race.intelligence == Intelligence.Humanlike)
                                {
                                    var cp = new CompProperties {compClass = typeof(CompSkyMind)};
                                    td.comps.Add(cp);


                                    if (td.defName != "M7Mech")
                                    {
                                        cp = new CompProperties {compClass = typeof(CompSurrogateOwner)};
                                        td.comps.Add(cp);
                                    }

                                    cp = new CompProperties {compClass = typeof(CompAndroidState)};
                                    td.comps.Add(cp);


                                    if (!Utils.ExceptionAndroidList.Contains(td.defName)) continue;

                                    td.race.gestationPeriodDays = Utils.ExceptionAndroidListAdvanced.Contains(td.defName) ? 2 : 1;

                                    try
                                    {
                                        if (Settings.allowHumanDrugsForAndroids) continue;

                                        foreach (var el in td.AllRecipes.ToList().Where(el =>
                                            Enumerable.Any(Utils.BlacklistAndroidFood, blacklistedFood => el.defName == "Administer_" + blacklistedFood)))
                                            td.AllRecipes.Remove(el);
                                    }
                                    catch (Exception e)
                                    {
                                        Log.Message("[ATPP] RemovingAndroidAdministerFood " + e.Message + " " + e.StackTrace);
                                    }
                                }
                                else if (Utils.ExceptionAndroidAnimalPowered.Contains(td.defName))
                                {
                                    var cp = new CompProperties {compClass = typeof(CompAndroidState)};
                                    td.comps.Add(cp);
                                }
                            }
                            else
                            {
                                if (Utils.ExceptionAutodoors.Contains(td.defName))
                                {
                                    var cp = new CompProperties {compClass = typeof(CompAutoDoor)};
                                    td.comps.Add(cp);


                                    cp = new CompProperties {compClass = typeof(CompSkyMind)};
                                    td.comps.Add(cp);
                                }
                                else if (td.defName == "ATPP_AndroidPod" || td.defName == "ATPP_AndroidPodMech" || td.defName == "AndroidOperationBed")
                                {
                                    var cp = new CompProperties {compClass = typeof(CompAndroidPod)};
                                    td.comps.Add(cp);

                                    td.tickerType = TickerType.Normal;

                                    if (td.defName != "AndroidOperationBed") continue;

                                    var cp2 = new CompProperties_Power {compClass = typeof(CompPowerTrader), shortCircuitInRain = true, basePowerConsumption = 80};
                                    td.comps.Add(cp2);
                                }
                                else if (td.thingClass != null && (td.thingClass == typeof(Building_Turret) || td.thingClass.IsSubclassOf(typeof(Building_Turret))))
                                {
                                    var cp = new CompProperties {compClass = typeof(CompSkyMind)};
                                    td.comps.Add(cp);


                                    cp = new CompProperties {compClass = typeof(CompRemotelyControlledTurret)};
                                    td.comps.Add(cp);
                                }
                                else
                                {
                                    if (Utils.ExceptionSkyCloudCores.Contains(td.defName))
                                        continue;

                                    if (td.comps == null) continue;

                                    var found = false;
                                    var flickable = false;

                                    foreach (var e in td.comps.Where(e => e.compClass != null))
                                    {
                                        if (e.compClass == typeof(CompFlickable))
                                            flickable = true;

                                        if (e.compClass == typeof(CompPowerTrader) || e.compClass == typeof(CompPowerPlant) || e.compClass.IsSubclassOf(typeof(CompPowerPlant)))
                                            found = true;
                                    }

                                    if (!found || !flickable) continue;

                                    var cp = new CompProperties {compClass = typeof(CompSkyMind)};
                                    td.comps.Add(cp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] Runtime.Patching.Comps " + e.Message + " " + e.StackTrace);
                        }

                    Utils.M7Mech = DefDatabase<ThingDef>.GetNamed("M7Mech", false);

                    try
                    {
                        Utils.M7Mech.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Needs)));
                    }
                    catch (Exception)
                    {
                    }


                    var ttd = DefDatabase<ThinkTreeDef>.GetNamed("MechM7Like", false);

                    try
                    {
                        if (ttd != null)
                        {
                            ThinkNode tn = new ThinkNode_ConditionalMustKeepLyingDownM7Surrogate();
                            tn.subNodes.Add(new JobGiver_KeepLyingDown());

                            ttd.thinkRoot.subNodes.Insert(0, tn);

                            ThinkNode tnt = new ThinkNode_ConditionalM7Charging();

                            tnt.subNodes.Add(new JobGiver_GetFood());


                            var index = 0;
                            var found = false;
                            foreach (var el in ttd.thinkRoot.subNodes)
                            {
                                if (el is ThinkNode_ConditionalColonist)
                                {
                                    found = true;
                                    index--;
                                    break;
                                }

                                index++;
                            }

                            if (index < 0)
                                index = 1;

                            if (!found) index = 1;

                            ttd.thinkRoot.subNodes.Insert(index, tnt);

                            if (Utils.SEARCHANDDESTROY_LOADED)
                            {
                                var type = Utils.searchAndDestroyAssembly.GetType("SearchAndDestroy.ThinkNode_ConditionalSearchAndDestroy");
                                var sad = (ThinkNode) Activator.CreateInstance(type);
                                var tp = new ThinkNode_Priority();
                                var jgFE = new JobGiver_AIFightEnemies();
                                var trJgFE = Traverse.Create(jgFE);
                                trJgFE.Field("targetKeepRadius").SetValue(72);
                                trJgFE.Field("targetAcquireRadius").SetValue(200);

                                tp.subNodes.Add(jgFE);
                                tp.subNodes.Add(new JobGiver_AIGotoNearestHostile());

                                sad.subNodes.Add(tp);

                                ttd.thinkRoot.subNodes.Insert(index + 1, sad);
                            }
                        }
                        else
                        {
                            Log.Message("[ATPP] MechM7 ThinkTree not found");
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Message("[ATPP] MechM7 ThinkTree patching issue " + e.Message + " " + e.StackTrace);
                    }


                    if (Utils.SMARTMEDICINE_LOADED && Utils.smartMedicineAssembly != null)
                        try
                        {
                            var original = Utils.smartMedicineAssembly.GetType("SmartMedicine.FindBestMedicine").GetMethod("Find", BindingFlags.Static | BindingFlags.Public);
                            var prefix = typeof(Utils).GetMethod("FindBestMedicinePrefix", BindingFlags.Static | BindingFlags.Public);
                            var postfix = typeof(Utils).GetMethod("FindBestMedicinePostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] SmartMedicinePatch " + e.Message + " " + e.StackTrace);
                        }


                    if (Utils.MEDICINEPATCH_LOADED && Utils.medicinePatchAssembly != null)
                        try
                        {
                            var original = Utils.medicinePatchAssembly.GetType("ModMedicinePatch.ModMedicinePatch")
                                .GetMethod("DynamicMedicalCareSetter", BindingFlags.Static | BindingFlags.Public);
                            var prefix = typeof(Utils).GetMethod("DynamicMedicalCareSetterPrefix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix));

                            original = Utils.medicinePatchAssembly.GetType("ModMedicinePatch.MedicalCareSetter").GetMethod("_Postfix", BindingFlags.Static | BindingFlags.Public);
                            prefix = typeof(Utils).GetMethod("DynamicMedicalCareSetterPrefixPostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] MedicinePatchPatching " + e.Message + " " + e.StackTrace);
                        }


                    if (Utils.PRISONLABOR_LOADED)
                        try
                        {
                            MethodInfo postfix = null;
                            MethodInfo original = null;
                            MethodInfo prefix = null;


                            var t1 = Utils.prisonLaborAssembly.GetType("PrisonLabor.Core.PrisonLaborUtility");


                            if (t1 == null)
                            {
                                Log.Message("[ATPP] PrisonLabor V1 not detected trying add compatibility with old release");

                                original = Utils.prisonLaborAssembly.GetType("PrisonLabor.PrisonLaborUtility").GetMethod("WorkTime", BindingFlags.Static | BindingFlags.Public);
                                prefix = typeof(CPaths).GetMethod("PrisonLabor_WorkTimePrefix", BindingFlags.Static | BindingFlags.Public);
                                Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix));

                                original = Utils.prisonLaborAssembly.GetType("PrisonLabor.Need_Motivation")
                                    .GetMethod("get_LazinessRate", BindingFlags.Instance | BindingFlags.NonPublic);
                                prefix = typeof(CPaths).GetMethod("PrisonLabor_GetChangePointsPrefix", BindingFlags.Static | BindingFlags.Public);
                                postfix = typeof(CPaths).GetMethod("PrisonLabor_GetChangePointsPostfix", BindingFlags.Static | BindingFlags.Public);
                            }
                            else
                            {
                                original = t1.GetMethod("WorkTime", BindingFlags.Static | BindingFlags.Public);
                                prefix = typeof(CPaths).GetMethod("PrisonLabor_WorkTimePrefix", BindingFlags.Static | BindingFlags.Public);
                                Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix));

                                original = Utils.prisonLaborAssembly.GetType("PrisonLabor.Core.Needs.Need_Motivation")
                                    .GetMethod("GetChangePoints", BindingFlags.Instance | BindingFlags.NonPublic);
                                prefix = typeof(CPaths).GetMethod("PrisonLabor_GetChangePointsPrefix", BindingFlags.Static | BindingFlags.Public);
                                postfix = typeof(CPaths).GetMethod("PrisonLabor_GetChangePointsPostfix", BindingFlags.Static | BindingFlags.Public);
                            }

                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] PrisonLaborPatching " + e.Message + " " + e.StackTrace);
                        }


                    if (Utils.SAVEOURSHIP2_LOADED)
                        try
                        {
                            var original = Utils.saveOurShip2Assembly.GetType("SaveOurShip2.ShipInteriorMod2").GetMethod("hasSpaceSuit", BindingFlags.Static | BindingFlags.Public);
                            var postfix = typeof(CPaths).GetMethod("SaveOurShip2_hasSpaceSuit", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, null, new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] SaveOurShip2Patching " + e.Message + " " + e.StackTrace);
                        }

                    if (Utils.HOSPITALITY_LOADED)
                        try
                        {
                            var original = Utils.hospitalityAssembly.GetType("Hospitality.BedUtility").GetMethod("FindBedFor", BindingFlags.Static | BindingFlags.Public);
                            var prefix = typeof(CPaths).GetMethod("Hopistality_FindBedForPrefix", BindingFlags.Static | BindingFlags.Public);
                            var postfix = typeof(CPaths).GetMethod("Hopistality_FindBedForPostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] HospitalityPatching " + e.Message + " " + e.StackTrace);
                        }

                    if (Utils.POWERPP_LOADED)
                        try
                        {
                            var original = Utils.powerppAssembly.GetType("aRandomKiwi.PPP.CompLocalWirelessPowerEmitter")
                                .GetMethod("CompInspectStringExtra", BindingFlags.Instance | BindingFlags.Public);
                            var postfix = typeof(CPaths).GetMethod("PowerPP_CompLocalWirelessPowerEmitter_CompInspectStringExtra", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, null, new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] Power++Patching " + e.Message + " " + e.StackTrace);
                        }

                    if (Utils.QEE_LOADED)
                        try
                        {
                            var original = Utils.qeeAssembly.GetType("QEthics.Building_PawnVatGrower").GetMethod("GetGizmos", BindingFlags.Instance | BindingFlags.Public);
                            var postfix = typeof(CPaths).GetMethod("QEE_BuildingPawnVatGrower_GetGizmosPostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, null, new HarmonyMethod(postfix));

                            original = Utils.qeeAssembly.GetType("QEthics.Building_PawnVatGrower").GetMethod("TryMakeClone", BindingFlags.Instance | BindingFlags.Public);
                            var prefix = typeof(CPaths).GetMethod("QEE_BuildingPawnVatGrower_TryMakeClonePrefix", BindingFlags.Static | BindingFlags.Public);
                            postfix = typeof(CPaths).GetMethod("QEE_BuildingPawnVatGrower_TryMakeClonePostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));


                            original = Utils.qeeAssembly.GetType("QEthics.Building_PawnVatGrower").GetMethod("TryExtractProduct", BindingFlags.Instance | BindingFlags.Public);
                            prefix = typeof(CPaths).GetMethod("QEE_BUildingPawnVatGrower_TryExtractProductPrefix", BindingFlags.Static | BindingFlags.Public);
                            postfix = typeof(CPaths).GetMethod("QEE_BUildingPawnVatGrower_TryExtractProductPostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, new HarmonyMethod(prefix), new HarmonyMethod(postfix));

                            original = Utils.qeeAssembly.GetType("QEthics.Building_GrowerBase")
                                .GetMethod("get_CraftingProgressPercent", BindingFlags.Instance | BindingFlags.Public);
                            postfix = typeof(CPaths).GetMethod("QEE_Building_GrowerBase_get_CraftingProgressPercentPostfix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, null, new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] QEE Patching " + e.Message + " " + e.StackTrace);
                        }


                    try
                    {
                        var originalVanilla = typeof(PawnApparelGenerator).GetNestedType("PossibleApparelSet", BindingFlags.NonPublic | BindingFlags.Instance)
                            .GetMethod("PairOverlapsAnything", BindingFlags.Public | BindingFlags.Instance);
                        var postfixVanilla = typeof(CPaths).GetMethod("RimworldVanilla_PawnApparelGeneratorPossibleApparelSetPairOverlapsAnything",
                            BindingFlags.Static | BindingFlags.Public);

                        Utils.harmonyInstance.Patch(originalVanilla, null, new HarmonyMethod(postfixVanilla));
                    }
                    catch (Exception e)
                    {
                        Log.Message("[ATPP] PawnApparelGeneratorPatching " + e.Message + " " + e.StackTrace);
                    }


                    /*if (Utils.WORKTAB_LOADED)
                    {
                        try
                        {
                            
                            var original = Utils.workTabAssembly.GetType("WorkTab.Pawn_WorkSettings_CacheWorkGiversInOrder").GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
                            var postfix = typeof(CPaths).GetMethod("WorkTab_PrefixPostFix", BindingFlags.Static | BindingFlags.Public);
                            Utils.harmonyInstance.Patch(original, null, new HarmonyMethod(postfix));
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] WorkTabPatching " + e.Message + " " + e.StackTrace);
                        }
                    }*/


                    Utils.applyLivingPlantPolicy();


                    var doctor = DefDatabase<WorkTypeDef>.GetNamed("Doctor", false);
                    Utils.WorkTypeDefSmithing = DefDatabase<WorkTypeDef>.GetNamed("Smithing", false);


                    var nbJobDrC = 0;
                    foreach (var e in doctor.workGiversByPriority)
                        try
                        {
                            var cd = new WorkGiverDef
                            {
                                workType = Utils.WorkTypeDefSmithing,
                                priorityInType = e.priorityInType,
                                verb = e.verb,
                                gerund = e.gerund,
                                requiredCapacities = e.requiredCapacities,
                                label = e.label,
                                giverClass = e.giverClass,
                                billGiversAllAnimals = e.billGiversAllAnimals,
                                billGiversAllAnimalsCorpses = e.billGiversAllAnimalsCorpses,
                                billGiversAllHumanlikes = e.billGiversAllHumanlikes,
                                billGiversAllHumanlikesCorpses = e.billGiversAllHumanlikesCorpses,
                                billGiversAllMechanoidsCorpses = e.billGiversAllMechanoidsCorpses,
                                canBeDoneWhileDrafted = e.canBeDoneWhileDrafted,
                                tagToGive = e.tagToGive,
                                scanThings = e.scanThings,
                                scanCells = e.scanCells,
                                workTags = e.workTags,
                                autoTakeablePriorityDrafted = e.autoTakeablePriorityDrafted,
                                feedAnimalsOnly = e.feedAnimalsOnly,
                                feedHumanlikesOnly = e.feedHumanlikesOnly,
                                fixedBillGiverDefs = e.fixedBillGiverDefs,
                                emergency = e.emergency
                            };
                            cd.gerund = e.gerund;
                            cd.verb = e.verb;
                            cd.label = e.label;

                            cd.defName = "ATPP_CrafterHealer" + e.defName;

                            Utils.CrafterDoctorJob.Add(cd);
                            nbJobDrC++;
                        }
                        catch (Exception ex)
                        {
                            Log.Message("[ATPP] Duplication.DoctorWorkGiverDefs " + ex.Message + " " + ex.StackTrace);
                        }

                    Log.Message("[ATPP] " + nbJobDrC + " Care job collected for crafting injection");

                    try
                    {
                        foreach (var el in Utils.CrafterDoctorJob) DefDatabase<WorkGiverDef>.Add(el);
                    }
                    catch (Exception ex)
                    {
                        Log.Message("[ATPP] Duplication.DoctorWorkGiverDefs.AddWGD " + ex.Message + " " + ex.StackTrace);
                    }


                    Utils.WorkTypeDefSmithing.workGiversByPriority = Utils.CrafterDoctorJob.Concat(Utils.WorkTypeDefSmithing.workGiversByPriority).ToList();

                    try
                    {
                        var pkd = new List<string>
                            {"AndroidT1RaiderFactionSpecific", "AndroidT2RaiderFactionSpecific", "AndroidT3RaiderFactionSpecific", "AndroidT4RaiderFactionSpecific"};
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsPKDHostile.Add(p);

                        pkd = new List<string>
                        {
                            "ATPP_AndroidTX2RaiderFactionSpecific", "ATPP_AndroidTX2KRaiderFactionSpecific", "ATPP_AndroidTX3RaiderFactionSpecific",
                            "ATPP_AndroidTX4RaiderFactionSpecific"
                        };
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsXSeriePKDHostile.Add(p);

                        pkd = new List<string>
                        {
                            "ATPP_AndroidTX2IRaiderFactionSpecific", "ATPP_AndroidTX2KIRaiderFactionSpecific", "ATPP_AndroidTX3IRaiderFactionSpecific",
                            "ATPP_AndroidTX4IRaiderFactionSpecific"
                        };
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsXISeriePKDHostile.Add(p);


                        pkd = new List<string> {"AndroidT1CollectiveSoldier", "AndroidT2CollectiveSoldier", "AndroidT3CollectiveSoldier", "AndroidT4CollectiveSoldier"};
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsPKDNeutral.Add(p);

                        pkd = new List<string>
                            {"ATPP_AndroidTX2CollectiveSoldier", "ATPP_AndroidTX2KCollectiveSoldier", "ATPP_AndroidTX3CollectiveSoldier", "ATPP_AndroidTX4CollectiveSoldier"};
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsXSeriePKDNeutral.Add(p);

                        pkd = new List<string>
                            {"ATPP_AndroidTX2ICollectiveSoldier", "ATPP_AndroidTX2KICollectiveSoldier", "ATPP_AndroidTX3ICollectiveSoldier", "ATPP_AndroidTX4ICollectiveSoldier"};
                        foreach (var p in pkd.Select(x => DefDatabase<PawnKindDef>.GetNamed(x, false))) Utils.AndroidsXISeriePKDNeutral.Add(p);
                    }
                    catch (Exception ex)
                    {
                        Log.Message("[ATPP] PawnKindDefGathering " + ex.Message + " " + ex.StackTrace);
                    }


                    var selMentalBreaks = new List<string> {"Wander_Sad", "InsultingSpree", "TargetedInsultingSpree", "MurderousRage"};

                    foreach (var mb in selMentalBreaks.Select(ct => DefDatabase<MentalBreakDef>.GetNamed(ct, false)).Where(mb => mb != null))
                        Utils.VirusedRandomMentalBreak.Add(mb);

                    Log.Message("[ATPP] " + selMentalBreaks.Count + " MentalBreaks collected");


                    TraitDef t;
                    var selTraits = new List<string> {"Pyromaniac", "Wimp", "CreepyBreathing", "AnnoyingVoice", "Jealous", "NightOwl", "Brawler", "Nudist", "Gourmand"};

                    foreach (var st in selTraits)
                    {
                        t = DefDatabase<TraitDef>.GetNamed(st, false);
                        if (t != null)
                            Utils.RansomAddedBadTraits.Add(t);
                    }


                    var recipHU = DefDatabase<RecipeDef>.GetNamed("ATPP_CreateHellDrone");
                    var tdHU = DefDatabase<ThingDef>.GetNamed("ATPP_SHUSurrogateGeneratorAI");

                    if (Utils.HELLUNIT_LOADED)
                    {
                        ((CompProperties_SurrogateSpawner) tdHU.comps.First()).Pawnkind = DefDatabase<PawnKindDef>.GetNamed("AndroidHellUnit");
                    }
                    else
                    {
                        DefDatabase<RecipeDef>.AllDefsListForReading.Remove(recipHU);
                        DefDatabase<ThingDef>.AllDefsListForReading.Remove(tdHU);
                    }
                }
                catch (Exception e)
                {
                    Log.Message("[ATPP] GC_ATPP.CTOR(Init)  Fatal Error : " + e.Message + " - " + e.StackTrace);
                }
            }
            else
            {
                Utils.lastDoorOpenedVocalGT = 0;
                Utils.lastDoorClosedVocalGT = 0;
                Utils.lastDeviceActivatedVocalGT = 0;
                Utils.lastDeviceDeactivatedVocalGT = 0;
                Utils.lastPlayedVocalWarningNoSkyMindNetGT = 0;
            }
        }

        public override void LoadedGame()
        {
            base.LoadedGame();


            reconnectSurrogatesInCaravans();
            removeBlacklistedAndroidsHediffs();
            checkRemoveAndroidFactions();
        }


        private void removeBlacklistedAndroidsHediffs()
        {
            var toDel = new List<Hediff>();

            foreach (var map in Find.Maps)
            foreach (var p in map.mapPawns.AllPawns)
                if (p.IsAndroidTier() && p.health != null && p.health.hediffSet != null)
                {
                    toDel.AddRange(p.health.hediffSet.hediffs.Where(he => Utils.BlacklistAndroidHediff.Contains(he.def.defName)));

                    if (!toDel.Any()) continue;

                    foreach (var h in toDel) p.health.hediffSet.hediffs.Remove(h);
                    toDel.Clear();
                }
        }

        private void reconnectSurrogatesInCaravans()
        {
            foreach (var c in Find.World.worldObjects.Caravans)
            foreach (var p in c.pawns)
                if (p.IsSurrogateAndroid())
                {
                    var cas = p.TryGetComp<CompAndroidState>();
                    if (cas?.surrogateController != null) connectUser(p);
                }
        }

        public override void StartedNewGame()
        {
            base.StartedNewGame();
            initNull();
            reset();
            checkRemoveAndroidFactions();
        }

        public override void ExposeData()
        {
            base.ExposeData();


            if (Scribe.mode == LoadSaveMode.LoadingVars) reset();

            Scribe_Values.Look(ref SkyCloureCoreID, "ATPP_SkyCloureCoreID", 1);
            Scribe_Values.Look(ref S0NID, "ATPP_S0NID", 1);
            Scribe_Values.Look(ref S1NID, "ATPP_S1NID", 1);
            Scribe_Values.Look(ref S2NID, "ATPP_S2NID", 1);
            Scribe_Values.Look(ref S3NID, "ATPP_S3NID", 1);
            Scribe_Values.Look(ref S4NID, "ATPP_S4NID", 1);
            Scribe_Values.Look(ref SX2NID, "ATPP_SX2NID", 1);
            Scribe_Values.Look(ref SX2KNID, "ATPP_SX2KNID", 1);
            Scribe_Values.Look(ref SX3NID, "ATPP_SX3NID", 1);
            Scribe_Values.Look(ref SX4NID, "ATPP_SX4NID", 1);
            Scribe_Values.Look(ref S10NID, "ATPP_S10NID", 1);
            Scribe_Values.Look(ref nbSlot, "nbSlot");
            Scribe_Values.Look(ref nbSecuritySlot, "nbSecuritySlot");
            Scribe_Values.Look(ref nbSkillSlot, "nbSkillSlot");
            Scribe_Values.Look(ref nbHackingPoints, "ATPP_nbHackingPoints");
            Scribe_Values.Look(ref nbSkillPoints, "ATPP_nbSkillPoints");

            Scribe_Collections.Look(ref QEEAndroidHairColor, "ATPP_QEEAndroidHairColor", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref QEESkinColor, "ATPP_QEESkinColor", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref QEEAndroidHair, "ATPP_QEEAndroidHair", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref VatGrowerLastPawnIsTX, "ATPP_VatGrowerLastPawnIsTX", LookMode.Value, LookMode.Value);

            if (Scribe.mode == LoadSaveMode.PostLoadInit) initNull();
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            var CGT = Find.TickManager.TicksGame;


            if (CGT % 60 == 0)
            {
                if (!appliedSettingsOnReload)
                {
                    applyLowSkyMindNetworkSettings();
                    appliedSettingsOnReload = true;
                }

                checkVirusedThings();
            }


            if (CGT % 360 != 0) return;

            if (!Settings.disableLowNetworkMalus)
                checkSkyMindSignalPerformance();

            checkSkyMindAutoReconnect();


            checkSolarFlarStuffInCaravans();

            if (Utils.POWERPP_LOADED)
                checkDisconnectedFromLWPNAndroid();
        }

        /*
         * Vire les androids ne pouvant plus être alimentés
         */
        public void checkDisconnectedFromLWPNAndroid()
        {
            foreach (var el in listerLWPNAndroid)
            {
                var availablePower = Utils.getCurrentAvailableEnergy(el.Key.PowerComp.PowerNet);
                if (el.Value.Count == 0)
                    continue;

                var nbConn = 0;
                foreach (var android in el.Value.ToList())
                {
                    var cas = android.TryGetComp<CompAndroidState>();
                    if (cas != null && !cas.useBattery)
                    {
                        el.Value.Remove(android);
                        continue;
                    }

                    var nonFunctionalLWPN = el.Key.Destroyed || el.Key.IsBrokenDown() || !el.Key.TryGetComp<CompPowerTrader>().PowerOn;


                    var qtConsummed = Utils.getConsumedPowerByAndroid(android.def.defName);
                    if (nonFunctionalLWPN || availablePower - qtConsummed < 0 ||
                        el.Key.def.defName != "ARKPPP_LocalWirelessPowerEmitter" && nbConn >= Settings.maxAndroidByPortableLWPN)
                    {
                        el.Value.Remove(android);
                        el.Key.TryGetComp<CompPowerTrader>().PowerOutput += qtConsummed;
                        if (cas != null)
                            cas.connectedLWPNActive = false;
                    }
                    else
                    {
                        availablePower -= qtConsummed;

                        android.needs.food.CurLevelPercentage += Settings.percentageOfBatteryChargedEach6Sec;
                        if (android.needs.food.CurLevelPercentage <= 0.95f)
                            Utils.throwChargingMote(android);

                        nbConn++;
                    }
                }
            }
        }

        public void checkSolarFlarStuffInCaravans()
        {
            if (Find.WorldObjects == null)
                return;

            foreach (var c in Find.WorldObjects.Caravans)
            foreach (var p in c.pawns)
            {
                var cas = p.TryGetComp<CompAndroidState>();
                cas?.checkSolarFlareStuff();
            }
        }


        public void applyLowSkyMindNetworkSettings()
        {
            if (Settings.disableLowNetworkMalus)
            {
                Utils.removeAllSlowNetworkHediff();
            }
            else
            {
                if (Settings.disableLowNetworkMalusInCaravans)
                    Utils.removeAllSlowNetworkHediff(true);
            }
        }


        public void checkVirusedThings()
        {
            var GT = Find.TickManager.TicksGame;

            foreach (var t in listerVirusedThings.ToList())
            {
                var csm = t.TryGetComp<CompSkyMind>();

                if (csm == null)
                    continue;

                if (csm.hacked == 3 && GT >= csm.hackEndGT)
                    csm.tempHackingEnding();


                if (csm.infectedExplodeGT != -1 && GT >= csm.infectedExplodeGT)
                {
                    csm.infectedExplodeGT = -1;
                    csm.Infected = -1;


                    if (t is Pawn p)
                    {
                        Utils.makeAndroidBatteryOverload(p);
                    }
                    else
                    {
                        GenExplosion.DoExplosion(t.Position, t.Map, 3, DamageDefOf.Flame, null);

                        var build = (Building) t;
                        build.HitPoints = (int) (build.def.BaseMaxHitPoints * Rand.Range(0.05f, 0.5f));
                    }
                }


                if (csm.infectedEndGT == -1 || csm.infectedEndGT > GT) continue;

                csm.infectedEndGT = -1;
                csm.Infected = -1;
            }
        }

        private bool isThereSkyMindAntennaOrRelayInMap(Map map)
        {
            var ok = false;
            if (map == null)
                return false;


            if (listerSkyMindServers.ContainsKey(map) && listerSkyMindServers[map].Count > 0)
                ok = true;
            else if (listerSkyMindWANServers.ContainsKey(map) && listerSkyMindWANServers[map].Count > 0)
                ok = true;
            else if (listerSkyMindRelays.ContainsKey(map) && listerSkyMindRelays[map].Count > 0)
                ok = true;

            return ok;
        }


        public void checkSkyMindSignalPerformance()
        {
            foreach (var entry in listerSurrogateAndroids) checkSkyMindSignalPerformanceEntry(entry);
        }

        private void checkSkyMindSignalPerformanceEntry(KeyValuePair<string, List<Pawn>> entry)
        {
            var forceRemoveHediff = false;
            var MUID = entry.Key;

            if (MUID == "caravan" && Settings.disableLowNetworkMalusInCaravans)
                forceRemoveHediff = true;

            var map = entry.Key.getMapFromString();

            var ok = isThereSkyMindAntennaOrRelayInMap(map);


            if (forceRemoveHediff || ok && listerSurrogateAndroids.ContainsKey(MUID) && listerSurrogateAndroids[MUID].Count > 0)
                foreach (var s in listerSurrogateAndroids[MUID])
                {
                    if (s.Dead)
                        continue;

                    var he = s.health.hediffSet.GetFirstHediffOfDef(Utils.hediffLowNetworkSignal);
                    if (he != null)
                        s.health.RemoveHediff(he);
                }
            else

                foreach (var s in from s in listerSurrogateAndroids[MUID]
                    where !s.Dead && s.health.hediffSet.GetFirstHediffOfDef(Utils.hediffHaveRXChip) == null
                    let he = s.health.hediffSet.GetFirstHediffOfDef(Utils.hediffLowNetworkSignal)
                    where he == null
                    select s)
                    s.health.AddHediff(Utils.hediffLowNetworkSignal);
        }

        public void checkSkyMindAutoReconnect()
        {
            foreach (var el in from el in listerSkyMindable
                let csm = el.TryGetComp<CompSkyMind>()
                where csm != null
                where csm.autoconn && !csm.connected
                where csm.canBeConnectedToSkyMind()
                select el) Utils.GCATPP.connectUser(el);
        }


        /*
         * SUite a decrementation nb serveurs check si on doit deconnecté des users aleatoirement
         */
        public void checkNeedRandomlyDisconnectUsers()
        {
            if (nbSlot >= connectedThing.Count()) return;

            while (nbSlot < connectedThing.Count())
            {
                var c = connectedThing.RandomElement();
                disconnectUser(c);
            }
        }

        public void reProcessNbSlot()
        {
            List<Building> toDel = null;
            var prevNbSlot = nbSlot;
            nbSlot = 0;
            foreach (var el in listerSkyMindServers)
            {
                foreach (var el2 in el.Value)
                    if (el2 != null && el2.TryGetComp<CompPowerTrader>().PowerOn && !el2.IsBrokenDown())
                    {
                        nbSlot += 3;
                    }
                    else
                    {
                        if (toDel == null)
                            toDel = new List<Building>();

                        toDel.Add(el2);
                    }

                if (toDel == null) continue;

                foreach (var t in toDel) el.Value.Remove(t);
                toDel.Clear();
            }


            foreach (var el in listerSkyMindWANServers)
            {
                foreach (var el2 in el.Value)
                    if (el2 != null && el2.TryGetComp<CompPowerTrader>().PowerOn && !el2.IsBrokenDown())
                    {
                        nbSlot += 15;
                    }
                    else
                    {
                        if (toDel == null)
                            toDel = new List<Building>();

                        toDel.Add(el2);
                    }

                if (toDel == null) continue;

                foreach (var t in toDel) el.Value.Remove(t);
                toDel.Clear();
            }


            if (prevNbSlot != 0 && nbSlot == 0)
                Utils.playVocal("soundDefSkyCloudSkyMindNetworkOffline");
        }


        public void reProcessNbSecuritySlot()
        {
            List<Building> toDel = null;
            nbSecuritySlot = 0;
            foreach (var el in listerSecurityServers)
                if (el != null && el.TryGetComp<CompPowerTrader>().PowerOn && !el.IsBrokenDown())
                {
                    nbSecuritySlot += Utils.nbSecuritySlotsGeneratedBy(el);
                }
                else
                {
                    if (toDel == null)
                        toDel = new List<Building>();

                    toDel.Add(el);
                }

            if (toDel == null) return;

            {
                foreach (var el in toDel) listerSecurityServers.Remove(el);

                toDel.Clear();
            }
        }

        public void reProcessNbHackingSlot()
        {
            List<Building> toDel = null;
            nbHackingSlot = 0;
            foreach (var el in listerHackingServers)
                if (el != null && el.TryGetComp<CompPowerTrader>().PowerOn && !el.IsBrokenDown())
                {
                    nbHackingSlot += Utils.nbHackingSlotsGeneratedBy(el);
                }
                else
                {
                    if (toDel == null)
                        toDel = new List<Building>();

                    toDel.Add(el);
                }

            if (toDel == null) return;

            {
                foreach (var el in toDel) listerHackingServers.Remove(el);

                toDel.Clear();
            }
        }

        public void reProcessNbSkillSlot()
        {
            List<Building> toDel = null;
            nbSkillSlot = 0;
            foreach (var el in listerSkillServers)
                if (el != null && el.TryGetComp<CompPowerTrader>().PowerOn && !el.IsBrokenDown())
                {
                    nbSkillSlot += Utils.nbSkillSlotsGeneratedBy(el);
                }
                else
                {
                    if (toDel == null)
                        toDel = new List<Building>();

                    toDel.Add(el);
                }

            if (toDel == null) return;

            {
                foreach (var el in toDel) listerSkillServers.Remove(el);

                toDel.Clear();
            }
        }

        public int getNbSlotAvailable()
        {
            return nbSlot;
        }

        public int getNbThingsConnected()
        {
            return connectedThing.Count();
        }

        public int getNbSkyMindUsers()
        {
            return listerSkyMindUsers.Count();
        }

        public int getNbSurrogateAndroids()
        {
            return listerSurrogateAndroids.Sum(el => el.Value.Count());
        }

        public int getNbSlotSecurisedAvailable()
        {
            return nbSecuritySlot;
        }

        public int getNbHackingSlotAvailable()
        {
            return nbHackingSlot;
        }

        public int getNbSkillSlotAvailable()
        {
            return nbSkillSlot;
        }


        public int getNbSkillPoints()
        {
            return nbSkillPoints;
        }

        public int getNbHackingPoints()
        {
            return nbHackingPoints;
        }

        private void checkHeldThingsPawnInSkyMind(Pawn cpawn)
        {
            var th = cpawn.carryTracker.GetDirectlyHeldThings();
            if (th == null) return;

            foreach (var t in th)
            {
                Pawn cp = null;
                if (t is Pawn pawn)
                    cp = pawn;
                if (cp != null && !cp.Dead && (cp.def.defName.ContainsAny(Utils.ExceptionAndroidList) || cp.VXChipPresent())) connectedThing.Add(cp);
            }
        }

        public bool isConnectedToSkyMind(Thing colonist, bool tryAutoConnect = false)
        {
            while (true)
            {
                if (connectedThing.Contains(colonist)) return true;


                if (colonist is Pawn)
                {
                    var cso = colonist.TryGetComp<CompSurrogateOwner>();
                    var csc = cso?.skyCloudHost?.TryGetComp<CompSkyCloudCore>();
                    if (csc != null && csc.Booted()) return true;
                }

                if (!tryAutoConnect) return false;

                connectUser(colonist);
                tryAutoConnect = false;
            }
        }

        public bool connectUser(Thing thing)
        {
            if (connectedThing.Contains(thing))
            {
                if (!(thing is Pawn pawn)) return true;
                if (!pawn.IsSurrogateAndroid()) return true;

                foreach (var MUID in from entry in listerSurrogateAndroids.ToList() let MUID = entry.Key where entry.Value.Contains(pawn) select MUID)
                    pushSurrogateAndroidNotifyMapChanged(pawn, MUID);

                return true;
            }

            if (connectedThing.Count() >= nbSlot) return false;

            if (connectedThing.Contains(thing)) return true;

            {
                connectedThing.Add(thing);
                switch (thing)
                {
                    case Pawn pawn:
                    {
                        var cas = pawn.TryGetComp<CompAndroidState>();

                        if (cas != null && cas.isSurrogate)
                            pushSurrogateAndroid(pawn);
                        else
                            pushSkyMindUser(pawn);

                        pawn.BroadcastCompSignal("SkyMindNetworkUserConnected");
                        break;
                    }
                    case Building building:
                    {
                        var build = building;
                        if (!listerConnectedDevices.Contains(build))
                            listerConnectedDevices.Add(building);
                        build.BroadcastCompSignal("SkyMindNetworkUserConnected");
                        break;
                    }
                }
            }


            return true;
        }

        public void disconnectUser(Thing thing)
        {
            if (!connectedThing.Contains(thing)) return;

            connectedThing.Remove(thing);

            switch (thing)
            {
                case Pawn pawn:
                {
                    var cas = pawn.TryGetComp<CompAndroidState>();

                    if (cas != null && cas.isSurrogate)
                    {
                        var MUID = "caravan";
                        if (pawn.Map != null)
                            MUID = pawn.Map.GetUniqueLoadID();
                        popSurrogateAndroid(pawn, MUID);
                    }
                    else
                    {
                        popSkyMindUser(pawn);
                    }

                    pawn.BroadcastCompSignal("SkyMindNetworkUserDisconnected");
                    break;
                }
                case Building building:
                {
                    var build = building;
                    if (listerConnectedDevices.Contains(build))
                        listerConnectedDevices.Remove(building);
                    build.BroadcastCompSignal("SkyMindNetworkUserDisconnected");
                    break;
                }
            }
        }


        public void pushSurrogateAndroid(Pawn sx)
        {
            var MUID = "caravan";
            if (sx.Map != null)
                MUID = sx.Map.GetUniqueLoadID();

            if (!listerSurrogateAndroids.ContainsKey(MUID))
                listerSurrogateAndroids[MUID] = new List<Pawn>();

            if (!listerSurrogateAndroids[MUID].Contains(sx))
                listerSurrogateAndroids[MUID].Add(sx);


            /*if (!isThereSkyMindAntennaOrRelayInMap(sx.Map))
            {
                
                Hediff he = sx.health.hediffSet.GetFirstHediffOfDef(Utils.hediffLowNetworkSignal);
                if (he == null)
                    sx.health.AddHediff(Utils.hediffLowNetworkSignal);
            }*/
        }

        public void pushSurrogateAndroidNotifyMapChanged(Pawn sx, string prevMUID)
        {
            var currMUID = "caravan";
            if (sx.Map != null)
                currMUID = sx.Map.GetUniqueLoadID();

            if (listerSurrogateAndroids.ContainsKey(prevMUID) && prevMUID != currMUID)
            {
                listerSurrogateAndroids[prevMUID].Remove(sx);
                if (!listerSurrogateAndroids.ContainsKey(currMUID))
                    listerSurrogateAndroids[currMUID] = new List<Pawn>();

                listerSurrogateAndroids[currMUID].Add(sx);
            }

            checkSkyMindSignalPerformance();
        }

        public void popSurrogateAndroid(Pawn sx, string MUID)
        {
            if (listerSurrogateAndroids.ContainsKey(MUID) && listerSurrogateAndroids[MUID].Contains(sx))
                listerSurrogateAndroids[MUID].Remove(sx);
        }

        public void pushSkyMindUser(Pawn sx)
        {
            if (!listerSkyMindUsers.Contains(sx))
                listerSkyMindUsers.Add(sx);
        }

        public void popSkyMindUser(Pawn sx)
        {
            if (listerSkyMindUsers.Contains(sx))
                listerSkyMindUsers.Remove(sx);
        }


        public List<Pawn> getRandomSurrogateAndroids(int nb, bool withoutVirus = true)
        {
            var ret = new List<Pawn>();
            var tmp = ret.ToList();


            ret = listerSurrogateAndroids.Aggregate(ret, (current, e) => current.Concat(e.Value).ToList());

            if (withoutVirus)
                foreach (var e in from e in tmp let csm = e.TryGetComp<CompSkyMind>() where csm != null where csm.Infected != -1 select e)
                    ret.Remove(e);

            while (ret.Count > nb) ret.Remove(ret.RandomElement());

            return ret;
        }

        public List<Thing> getRandomDevices(int nb, bool withoutVirus = true)
        {
            var ret = listerConnectedDevices.ToList();
            var tmp = ret.ToList();

            if (withoutVirus)
                foreach (var e in from e in tmp let csm = e.TryGetComp<CompSkyMind>() where csm != null where csm.Infected != -1 select e)
                    ret.Remove(e);

            while (ret.Count > nb) ret.Remove(ret.RandomElement());

            return ret;
        }

        public List<Pawn> getRandomSkyMindUsers(int nb)
        {
            var ret = listerSkyMindUsers.ToList();

            while (ret.Count > nb) ret.Remove(ret.RandomElement());

            return ret;
        }

        public Pawn getRandomSkyMindUser()
        {
            return listerSkyMindUsers.Count == 0 ? null : listerSkyMindUsers.RandomElement();
        }


        public void pushSkyMindServer(Building build)
        {
            if (!listerSkyMindServers.ContainsKey(build.Map))
                listerSkyMindServers[build.Map] = new List<Building>();

            if (!build.TryGetComp<CompPowerTrader>().PowerOn) return;

            listerSkyMindServers[build.Map].Add(build);
            reProcessNbSlot();
        }

        public void popSkyMindServer(Building build, Map map)
        {
            if (listerSkyMindServers.ContainsKey(map) && listerSkyMindServers[map].Contains(build))
            {
                listerSkyMindServers[map].Remove(build);
                reProcessNbSlot();
            }

            checkNeedRandomlyDisconnectUsers();
        }

        public void pushSkyMindWANServer(Building build)
        {
            if (!listerSkyMindWANServers.ContainsKey(build.Map))
                listerSkyMindWANServers[build.Map] = new List<Building>();

            if (listerSkyMindWANServers[build.Map].Contains(build) || !build.TryGetComp<CompPowerTrader>().PowerOn) return;

            listerSkyMindWANServers[build.Map].Add(build);
            reProcessNbSlot();
        }

        public void popSkyMindWANServer(Building build, Map map)
        {
            if (listerSkyMindWANServers.ContainsKey(map) && listerSkyMindWANServers[map].Contains(build))
            {
                listerSkyMindWANServers[map].Remove(build);
                reProcessNbSlot();
            }

            checkNeedRandomlyDisconnectUsers();
        }

        public void pushReloadStation(Building build)
        {
            if (!listerReloadStation.ContainsKey(build.Map)) listerReloadStation[build.Map] = new List<Building>();

            listerReloadStation[build.Map].Add(build);
        }

        public void popReloadStation(Building build, Map map)
        {
            if (listerReloadStation.ContainsKey(map) && listerReloadStation[map].Contains(build)) listerReloadStation[map].Remove(build);
        }

        public Building getNearestReloadStation(Map map, Pawn android)
        {
            if (listerReloadStation.ContainsKey(map) || listerReloadStation[map].Count() == 0)
                return null;

            float dist = 0;
            Building ret = null;
            foreach (var el in listerReloadStation[map])
            {
                var curDist = android.Position.DistanceTo(el.Position);
                if (!(dist > curDist)) continue;

                dist = curDist;
                ret = el;
            }

            return ret;
        }

        public List<Building> getReloadStations(Map map)
        {
            return listerReloadStation.ContainsKey(map) ? listerReloadStation[map] : null;
        }

        /*
         * Obtention station de rechargement ayant des slots libres
         */
        public Building getFreeReloadStation(Map map, Pawn android)
        {
            return listerReloadStation.ContainsKey(map)
                ? (from el in listerReloadStation[map].OrderBy(b => b.Position.DistanceTo(android.Position))
                    where !el.Destroyed && !el.IsBrokenDown() && el.TryGetComp<CompPowerTrader>().PowerOn && el.Position.InAllowedArea(android)
                    let rs = el.TryGetComp<CompReloadStation>()
                    where rs != null
                    where rs.getNbAndroidReloading(true) < 8
                    let freePlace = rs.getFreeReloadPlacePos(android)
                    where freePlace != IntVec3.Invalid && android.CanReach(freePlace, PathEndMode.OnCell, Danger.Deadly)
                    select el).FirstOrDefault()
                : null;
        }

        public int getNextSkyCloudID()
        {
            return SkyCloureCoreID;
        }

        public void incNextSkyCloudID()
        {
            SkyCloureCoreID++;
        }

        public int getNextSXID(int v)
        {
            switch (v)
            {
                case 10:
                    return S10NID;
                case 4:
                    return S4NID;
                case 3:
                    return S3NID;
                case 2:
                    return S2NID;
                case 0:
                    return S0NID;
                case 12:
                    return SX2NID;
                case 120:
                    return SX2KNID;
                case 13:
                    return SX3NID;
                case 14:
                    return SX4NID;
                default:
                    return S1NID;
            }
        }

        public void incNextSXID(int v)
        {
            switch (v)
            {
                case 10:
                    S10NID++;
                    break;
                case 4:
                    S4NID++;
                    break;
                case 3:
                    S3NID++;
                    break;
                case 2:
                    S2NID++;
                    break;
                case 0:
                    S0NID++;
                    break;
                case 12:
                    SX2NID++;
                    break;
                case 120:
                    SX2KNID++;
                    break;
                case 13:
                    SX3NID++;
                    break;
                case 14:
                    SX4NID++;
                    break;
                default:
                    S1NID++;
                    break;
            }
        }

        /*
         * Obtention des devices heat sensible ayant un hotLevel egal a celui spécifié
         */
        public List<Thing> getHeatSensitiveDevicesByHotLevel(Map map, int hotLevel)
        {
            return !listerHeatSensitiveDevices.ContainsKey(map)
                ? null
                : listerHeatSensitiveDevices[map].Where(device => device.TryGetComp<CompHeatSensitive>().hotLevel == hotLevel).Cast<Thing>().ToList();
        }

        private void checkRemoveAndroidFactions()
        {
            androidFactionCoalition = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("AndroidFriendliesAtlas"));
            if (androidFactionCoalition != null)
            {
                if (Settings.androidsAreRare)
                {
                    if (!androidFactionCoalition.defeated)
                    {
                        foreach (var el in Find.WorldObjects.SettlementBases.ToList().Where(el => el.Faction == androidFactionCoalition))
                            savedIASCoalition.Add(el);

                        if (savedIASCoalition.Count != 0)
                            foreach (var el in savedIASCoalition)
                            {
                                try
                                {
                                    Find.WorldObjects.SettlementBases.Remove(el);
                                }
                                catch (Exception)
                                {
                                }

                                try
                                {
                                    Find.WorldObjects.Remove(el);
                                }
                                catch (Exception)
                                {
                                }
                            }

                        androidFactionCoalition.defeated = true;
                    }

                    androidFactionCoalition.def.hidden = true;
                    androidFactionCoalition = null;
                }
                else
                {
                    androidFactionCoalition.def.hidden = false;
                    if (Utils.FACTIONDISCOVERY_LOADED)
                        try
                        {
                            var method = Utils.factionDiscoveryAssembly.GetType("FactionDiscovery.MainUtilities")
                                .GetMethod("CreateBases", BindingFlags.Static | BindingFlags.NonPublic);

                            if (method != null)
                            {
                                androidFactionCoalition.def.hidden = false;

                                if (androidFactionCoalition.defeated)
                                {
                                    androidFactionCoalition.defeated = false;


                                    method.Invoke(null, new object[] {androidFactionCoalition});
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] checkRemoveAndroidFactions.androidFactionCoalition " + e.Message + " " + e.StackTrace);
                        }

                    savedIASCoalition.Clear();
                }
            }

            androidFactionInsurrection = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("AndroidRebellionAtlas"));
            if (androidFactionInsurrection == null) return;

            {
                if (Settings.androidsAreRare)
                {
                    if (!androidFactionInsurrection.defeated)
                    {
                        foreach (var el in Find.WorldObjects.SettlementBases.ToList().Where(el => el.Faction == androidFactionInsurrection))
                            savedIASInsurrection.Add(el);

                        if (savedIASInsurrection.Count != 0)
                            foreach (var el in savedIASInsurrection)
                            {
                                try
                                {
                                    Find.WorldObjects.SettlementBases.Remove(el);
                                }
                                catch (Exception)
                                {
                                }

                                try
                                {
                                    Find.WorldObjects.Remove(el);
                                }
                                catch (Exception)
                                {
                                }
                            }

                        androidFactionInsurrection.defeated = true;
                    }

                    androidFactionInsurrection.def.hidden = true;
                    androidFactionInsurrection = null;
                }
                else
                {
                    androidFactionInsurrection.def.hidden = false;
                    if (Utils.FACTIONDISCOVERY_LOADED)
                        try
                        {
                            var method = Utils.factionDiscoveryAssembly.GetType("FactionDiscovery.MainUtilities")
                                .GetMethod("CreateBases", BindingFlags.Static | BindingFlags.NonPublic);

                            if (method != null)
                            {
                                androidFactionInsurrection.def.hidden = false;

                                if (androidFactionInsurrection.defeated)
                                {
                                    androidFactionInsurrection.defeated = false;


                                    method.Invoke(null, new object[] {androidFactionInsurrection});
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Message("[ATPP] checkRemoveAndroidFactions. androidFactionInsurrection " + e.Message + " " + e.StackTrace);
                        }

                    savedIASInsurrection.Clear();
                }
            }
        }


        public int getNbDevices()
        {
            return listerConnectedDevices.Count();
        }


        public void pushHeatSensitiveDevices(Building build)
        {
            if (!listerHeatSensitiveDevices.ContainsKey(build.Map))
                listerHeatSensitiveDevices[build.Map] = new List<Building>();

            listerHeatSensitiveDevices[build.Map].Add(build);
        }

        public void popHeatSensitiveDevices(Building build, Map map)
        {
            if (!listerHeatSensitiveDevices.ContainsKey(map))
                return;

            listerHeatSensitiveDevices[map].Remove(build);
        }


        public void pushSecurityServer(Building build)
        {
            if (listerSecurityServers.Contains(build))
                return;

            listerSecurityServers.Add(build);
            reProcessNbSecuritySlot();
        }


        public void popSecurityServer(Building build)
        {
            if (!listerSecurityServers.Contains(build))
                return;

            listerSecurityServers.Remove(build);

            reProcessNbSecuritySlot();
        }

        public void pushSkillServer(Building build)
        {
            if (listerSkillServers.Contains(build))
                return;

            listerSkillServers.Add(build);
            reProcessNbSkillSlot();
        }

        public bool isThereSkillServers()
        {
            return listerSkillServers.Count() != 0;
        }


        public void popSkillServer(Building build)
        {
            if (!listerSkillServers.Contains(build))
                return;

            listerSkillServers.Remove(build);

            if (listerSkillServers.Count == 0 && nbSkillPoints != 0)
            {
                Find.LetterStack.ReceiveLetter("ATPP_LetterSkillPointsRemoved".Translate(), "ATPP_LetterSkillPointsRemovedDesc".Translate(nbSkillPoints), LetterDefOf.ThreatSmall);
                nbSkillPoints = 0;
            }

            reProcessNbSkillSlot();
        }

        public void pushHackingServer(Building build)
        {
            if (listerHackingServers.Contains(build))
                return;

            listerHackingServers.Add(build);
            reProcessNbHackingSlot();
        }


        public void popHackingServer(Building build)
        {
            if (!listerHackingServers.Contains(build))
                return;

            listerHackingServers.Remove(build);

            if (listerHackingServers.Count == 0 && nbHackingPoints != 0)
            {
                Find.LetterStack.ReceiveLetter("ATPP_LetterHackingPointsRemoved".Translate(), "ATPP_LetterHackingPointsRemovedDesc".Translate(nbHackingPoints),
                    LetterDefOf.ThreatSmall);
                nbHackingPoints = 0;
            }

            reProcessNbHackingSlot();
        }


        public void pushSkyCloudCore(Building build)
        {
            if (listerSkyCloudCores.Contains(build))
                return;

            listerSkyCloudCores.Add(build);
        }


        public void popSkyCloudCoreAbs(Building build)
        {
            if (!listerSkyCloudCoresAbs.Contains(build))
                return;

            listerSkyCloudCoresAbs.Remove(build);
        }

        public void pushSkyCloudCoreAbs(Building build)
        {
            if (listerSkyCloudCoresAbs.Contains(build))
                return;

            listerSkyCloudCoresAbs.Add(build);
        }


        public void popSkyCloudCore(Building build)
        {
            if (!listerSkyCloudCores.Contains(build))
                return;

            listerSkyCloudCores.Remove(build);
        }

        public List<Building> getAvailableSkyCloudCores()
        {
            return listerSkyCloudCores;
        }

        public bool isThereSkyCloudCore()
        {
            return listerSkyCloudCores.Any();
        }

        public bool isThereSkyCloudCoreAbs()
        {
            return listerSkyCloudCoresAbs.Any();
        }

        public void pushRelayTower(Building build)
        {
            if (!listerSkyMindRelays.ContainsKey(build.Map))
                listerSkyMindRelays[build.Map] = new List<Building>();

            if (listerSkyMindRelays[build.Map].Contains(build))
                return;

            listerSkyMindRelays[build.Map].Add(build);
        }


        public void popRelayTower(Building build, Map map)
        {
            if (!listerSkyMindRelays.ContainsKey(map) || !listerSkyMindRelays[map].Contains(build))
                return;

            listerSkyMindRelays[map].Remove(build);
        }

        public void incHackingPoints(int nb)
        {
            if (nbHackingPoints + nb > nbHackingSlot)
            {
                if (!(nbHackingPoints > nbHackingSlot))
                    nbHackingPoints = nbHackingSlot;
            }
            else
            {
                nbHackingPoints += nb;
            }
        }

        public void decHackingPoints(int nb)
        {
            if (nbHackingPoints - nb < 0)
                nbHackingPoints = 0;
            else
                nbHackingPoints -= nb;
        }

        public void incSkillPoints(int nb)
        {
            if (nbSkillPoints + nb > nbSkillSlot)
            {
                if (!(nbSkillPoints > nbSkillSlot))
                    nbSkillPoints = nbSkillSlot;
            }
            else
            {
                nbSkillPoints += nb;
            }
        }

        public void decSkillPoints(int nb)
        {
            if (nbSkillPoints - nb < 0)
                nbSkillPoints = 0;
            else
                nbSkillPoints -= nb;
        }

        public void pushSkyMindable(Thing thing)
        {
            if (listerSkyMindable.Contains(thing))
                return;

            listerSkyMindable.Add(thing);
        }


        public void popSkyMindable(Thing thing)
        {
            if (!listerSkyMindable.Contains(thing))
                return;

            listerSkyMindable.Remove(thing);
        }

        public void pushVirusedThing(Thing thing)
        {
            if (listerVirusedThings.Contains(thing))
                return;

            listerVirusedThings.Add(thing);
        }


        public void popVirusedThing(Thing thing)
        {
            if (!listerVirusedThings.Contains(thing))
                return;

            listerVirusedThings.Remove(thing);
        }


        public bool pushLWPNAndroid(Building LWPN, Pawn android)
        {
            if (!listerLWPNAndroid.ContainsKey(LWPN))
                listerLWPNAndroid[LWPN] = new List<Pawn>();


            var qtConsumed = Utils.getConsumedPowerByAndroid(android.def.defName);
            if (LWPN.Destroyed ||
                LWPN.def.defName != "ARKPPP_LocalWirelessPowerEmitter" &&
                (LWPN.def.defName != "ARKPPP_LocalWirelessPortablePowerEmitter" || listerLWPNAndroid[LWPN].Count() >= Settings.maxAndroidByPortableLWPN) ||
                !LWPN.TryGetComp<CompPowerTrader>().PowerOn || !(Utils.getCurrentAvailableEnergy(LWPN.PowerComp.PowerNet) - qtConsumed > 0)) return false;

            listerLWPNAndroid[LWPN].Add(android);

            LWPN.TryGetComp<CompPowerTrader>().PowerOutput -= qtConsumed;
            return true;
        }


        public void popLWPNAndroid(Building LWPN, Pawn android)
        {
            if (!listerLWPNAndroid.ContainsKey(LWPN))
                return;


            var qtConsumed = Utils.getConsumedPowerByAndroid(android.def.defName);
            LWPN.TryGetComp<CompPowerTrader>().PowerOutput += qtConsumed;

            listerLWPNAndroid[LWPN].Remove(android);
        }

        public int getNbAssistingMinds()
        {
            return listerSkyCloudCores.Select(c => c.TryGetComp<CompSkyCloudCore>()).Where(csc => csc != null && csc.Booted()).Sum(csc => csc.assistingMinds.Count());
        }

        private void reset()
        {
            appliedSettingsOnReload = false;
            listerReloadStation.Clear();
            listerSkyMindServers.Clear();
            cacheATN.Clear();
            connectedThing.Clear();
            listerSkyMindWANServers.Clear();
            listerHeatSensitiveDevices.Clear();
            listerSecurityServers.Clear();
            listerHackingServers.Clear();
            listerSurrogateAndroids.Clear();
            listerSkyMindUsers.Clear();
            listerSkyMindRelays.Clear();
            listerSkyCloudCores.Clear();
            listerSkyMindable.Clear();
            listerConnectedDevices.Clear();
            listerVirusedThings.Clear();
            listerSkyCloudCoresAbs.Clear();
            listerSkillServers.Clear();
            savedIASInsurrection.Clear();
            savedIASCoalition.Clear();
            listerLWPNAndroid.Clear();
            QEESkinColor.Clear();
            QEEAndroidHairColor.Clear();
            QEEAndroidHair.Clear();
            VatGrowerLastPawnInProgress.Clear();
            VatGrowerLastPawnIsTX.Clear();
        }

        private void initNull()
        {
            appliedSettingsOnReload = false;

            if (listerReloadStation == null)
                listerReloadStation = new Dictionary<Map, List<Building>>();
            if (listerSkyMindServers == null)
                listerSkyMindServers = new Dictionary<Map, List<Building>>();
            if (listerSkyMindWANServers == null)
                listerSkyMindWANServers = new Dictionary<Map, List<Building>>();
            if (cacheATN == null)
                cacheATN = new Dictionary<Building, IEnumerable<IntVec3>>();
            if (connectedThing == null)
                connectedThing = new List<Thing>();
            if (listerHeatSensitiveDevices == null)
                listerHeatSensitiveDevices = new Dictionary<Map, List<Building>>();
            if (listerSecurityServers == null)
                listerSecurityServers = new List<Building>();
            if (listerHackingServers == null)
                listerHackingServers = new List<Building>();
            if (listerSurrogateAndroids == null)
                listerSurrogateAndroids = new Dictionary<string, List<Pawn>>();
            if (listerSkyMindUsers == null)
                listerSkyMindUsers = new List<Pawn>();
            if (listerSkyMindRelays == null)
                listerSkyMindRelays = new Dictionary<Map, List<Building>>();
            if (listerSkyCloudCores == null)
                listerSkyCloudCores = new List<Building>();
            if (listerSkyMindable == null)
                listerSkyMindable = new List<Thing>();
            if (listerConnectedDevices == null)
                listerConnectedDevices = new List<Thing>();
            if (listerVirusedThings == null)
                listerVirusedThings = new List<Thing>();
            if (listerSkyCloudCoresAbs == null)
                listerSkyCloudCoresAbs = new List<Building>();
            if (listerSkillServers == null)
                listerSkillServers = new List<Building>();
            if (savedIASCoalition == null)
                savedIASCoalition = new List<Settlement>();
            if (savedIASInsurrection == null)
                savedIASInsurrection = new List<Settlement>();
            if (listerLWPNAndroid == null)
                listerLWPNAndroid = new Dictionary<Building, List<Pawn>>();
            if (QEEAndroidHair == null)
                QEEAndroidHair = new Dictionary<string, string>();
            if (QEEAndroidHairColor == null)
                QEEAndroidHairColor = new Dictionary<string, string>();
            if (QEESkinColor == null)
                QEESkinColor = new Dictionary<string, string>();
            if (VatGrowerLastPawnInProgress == null)
                VatGrowerLastPawnInProgress = new Dictionary<string, Pawn>();
            if (VatGrowerLastPawnIsTX == null)
                VatGrowerLastPawnIsTX = new Dictionary<string, bool>();
        }
    }
}