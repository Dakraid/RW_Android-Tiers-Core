﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace MOARANDROIDS
{
    public static class Utils
    {
        public const string TX2 = "ATPP_Android2TX";
        public const string TX3 = "ATPP_Android3TX";
        public const string TX4 = "ATPP_Android4TX";
        public const string TX2K = "ATPP_Android2KTX";

        public const string TX2I = "ATPP_Android2ITX";
        public const string TX3I = "ATPP_Android3ITX";
        public const string TX4I = "ATPP_Android4ITX";
        public const string TX2KI = "ATPP_Android2KITX";

        public const string T1 = "Android1Tier";
        public const string T2 = "Android2Tier";
        public const string T3 = "Android3Tier";
        public const string T4 = "Android4Tier";
        public const string T5 = "Android5Tier";
        public const string M7 = "M7Mech";
        public const string HU = "AT_HellUnit";
        public const string K9 = "AndroidDog";
        public const string MUFF = "AndroidMuff";
        public const string NSolution = "RoboticCow";
        public const string Phytomining = "RoboticSheep";
        public const string RoboticFennec = "AndroidFox";

        public static bool init = false;
        public static Harmony harmonyInstance;

        public static List<WorkGiverDef> CrafterDoctorJob;

        public static bool PSYCHOLOGY_LOADED = false;
        public static bool HELLUNIT_LOADED = false;
        public static bool SMARTMEDICINE_LOADED = false;
        public static bool CELOADED = false;
        public static bool MEDICINEPATCH_LOADED = false;
        public static bool BIRDSANDBEES_LOADED = false;
        public static bool PRISONLABOR_LOADED = false;
        public static bool SAVEOURSHIP2_LOADED = false;
        public static bool WORKTAB_LOADED = false;
        public static bool HOSPITALITY_LOADED = false;
        public static bool SEARCHANDDESTROY_LOADED = false;
        public static bool FACTIONDISCOVERY_LOADED = false;
        public static bool POWERPP_LOADED = false;
        public static bool ANDROIDTIERSGYNOID_LOADED = false;
        public static bool QEE_LOADED = false;
        public static bool RIMMSQOL_LOADED = false;


        public static bool TXSERIE_LOADED = false;

        private static readonly FloatRange settlementsBasesPer100KTiles = new FloatRange(75f, 85f);

        public static Pawn curSelPatientDrawMedOperationsTab;

        public static int lastDoorOpenedVocalGT;
        public static int lastDoorClosedVocalGT;

        public static int lastDeviceActivatedVocalGT;
        public static int lastDeviceDeactivatedVocalGT;

        public static Pawn lastInstallImplantBillDoer;

        public static bool forceGeneratedAndroidToBeDefaultPainted = false;

        public static Assembly smartMedicineAssembly;
        public static Assembly medicinePatchAssembly;
        public static Assembly androidTiersAssembly;
        public static Assembly prisonLaborAssembly;
        public static Assembly saveOurShip2Assembly;
        public static Assembly hospitalityAssembly;
        public static Assembly searchAndDestroyAssembly;
        public static Assembly factionDiscoveryAssembly;
        public static Assembly powerppAssembly;
        public static Assembly qeeAssembly;


        public static string lastResolveAllGraphicsHeadGraphicPath = null;
        public static BodyTypeDef insideResolveApparelGraphicsLastBodyTypeDef;

        public static bool insideAddHumanlikeOrders = false;

        public static bool insideKillFuncSurrogate = false;
        public static string headGraphicPathToUse = "";

        public static bool lastButcheredPawnIsAndroid = false;
        public static int lastMemoryThoughtAgeBeforeReset = 0;

        public static int lastPlayedVocalWarningNoSkyMindNetGT;


        public static float PawnInventoryGeneratorLastInvNutritionValue = 0;
        public static bool PawnInventoryGeneratorCanHackInvNutritionValue = true;

        public static bool InsideBestFoodSourceOnMap = false;

        public static bool preventVX0Thought = false;

        public static WorkTypeDef WorkTypeDefSmithing;

        public static ThoughtDef thoughtDefVX0Puppet;

        public static TraitDef traitSimpleMinded;

        public static HediffDef hediffHaveRXChip;
        public static HediffDef hediffHaveVX0Chip;
        public static HediffDef hediffHaveVX1Chip;
        public static HediffDef hediffHaveVX2Chip;
        public static HediffDef hediffHaveVX3Chip;
        public static HediffDef hediffNoHost;
        public static HediffDef hediffLowNetworkSignal;
        public static HediffDef hediffRusted;
        public static HediffDef hediffBlankAndroid;

        public static SoundDef soundDefSkyCloudMindQuarantineMentalState;
        public static SoundDef soundDefSkyCloudMindUploadCompleted;
        public static SoundDef soundDefSkyCloudMindDownloadCompleted;
        public static SoundDef soundDefSkyCloudDeviceActivated;
        public static SoundDef soundDefSkyCloudDeviceDeactivated;
        public static SoundDef soundDefSkyCloudDoorOpened;
        public static SoundDef soundDefSkyCloudDoorClosed;
        public static SoundDef soundDefSkyCloudSkyMindNetworkOffline;
        public static SoundDef soundDefSkyCloudPowerFailure;
        public static SoundDef soundDefSkyCloudMindReplicationCompleted;
        public static SoundDef soundDefSkyCloudMindMigrationCompleted;
        public static SoundDef soundDefSkyCloudMindDeletionCompleted;
        public static SoundDef soundDefSkyCloudAllMindDisconnected;
        public static SoundDef soundDefSkyCloudPrimarySystemsOnline;
        public static SoundDef soundDefSurrogateConnection;
        public static SoundDef soundDefSurrogateConnectionStopped;
        public static SoundDef soundDefTurretConnection;
        public static SoundDef soundDefTurretConnectionStopped;
        public static SoundDef soundDefSurrogateHacked;

        public static StatDef statDefAndroidTending;
        public static StatDef statDefAndroidSurgerySuccessChance;

        public static List<MentalBreakDef> VirusedRandomMentalBreak = new List<MentalBreakDef>();

        public static List<TraitDef> RansomAddedBadTraits = new List<TraitDef>();


        public static Color SXColor = new Color(0.280f, 0.280f, 0.280f);

        public static Color androidCustomColorKhaki = new Color(0.29411f, 0.356862f, 0.16470f);
        public static Color androidCustomColorGreen = new Color(0.19f, 0.75f, 0.43f);
        public static Color androidCustomColorWhite = new Color(1.0f, 1.0f, 1.0f);
        public static Color androidCustomColorBlack = new Color(0.15f, 0.15f, 0.15f);
        public static Color androidCustomColorGray = new Color(0.50f, 0.50f, 0.50f);
        public static Color androidCustomColorBlue = new Color(0.25f, 0.44f, 0.69f);
        public static Color androidCustomColorRed = new Color(0.69f, 0.26f, 0.29f);
        public static Color androidCustomColorOrange = new Color(1.0f, 0.64705f, 0.0f);
        public static Color androidCustomColorYellow = new Color(0.8392f, 0.8274f, 0.1254f);
        public static Color androidCustomColorPurple = new Color(0.43f, 0.30f, 0.55f);
        public static Color androidCustomColorPink = new Color(0.90f, 0.60f, 0.83f);
        public static Color androidCustomColorCyan = new Color(0.33f, 0.69f, 0.83f);

        public static Color androidCustomColorRust = new Color(0.5607f, 0.2941f, 0.1764f);


        public static Pawn FindBestMedicinePatient;


        public static ThingDef M7Mech;

        public static List<string> BlacklistedHediffsForAndroids = new List<string> {"Anxiety"};

        public static HediffDef dummyHeddif;

        public static GC_ATPP GCATPP;

        public static Pawn ignoredPawnNotifications = null;

        public static List<PawnKindDef> AndroidsPKDNeutral = new List<PawnKindDef>();
        public static List<PawnKindDef> AndroidsXSeriePKDNeutral = new List<PawnKindDef>();
        public static List<PawnKindDef> AndroidsXISeriePKDNeutral = new List<PawnKindDef>();

        public static List<PawnKindDef> AndroidsPKDHostile = new List<PawnKindDef>();
        public static List<PawnKindDef> AndroidsXSeriePKDHostile = new List<PawnKindDef>();
        public static List<PawnKindDef> AndroidsXISeriePKDHostile = new List<PawnKindDef>();

        public static List<string> ExceptionCooler = new List<string> {"Cooler"};

        public static List<string> ExceptionHeater = new List<string> {"Heater"};


        public static List<string> ExceptionBlacklistedFactionNoSurrogate = new List<string>
        {
            "RRY_Yautja_JungleClan", "RRY_Yautja_BadBloodFaction", "RRY_Xenomorph",
            "ElderThing_Faction",
            "Harrowed",
            "FriendlyConstruct", "Crystal",
            "Imouto_Civil", "ImoutoTribe"
        };


        public static List<string> ExceptionSkinColors = new List<string> {"Verylight", "Light", "Fair", "Midbrown", "Darkbrown", "Verydark"};
        public static List<string> ExceptionHairColors = new List<string> {"Blond", "Black", "Auburn", "Grey", "Ginger", "White"};

        public static List<string> ExceptionBodyTypeDefnameAndroidWithSkinMale = new List<string>
        {
            "ATPP_BodyTypeMaleHurted22TX", "ATPP_BodyTypeMaleHurted12TX", "ATPP_BodyTypeMaleHurted12KTX", "ATPP_BodyTypeMaleHurted22KTX", "ATPP_BodyTypeMaleHurted13TX",
            "ATPP_BodyTypeMaleHurted23TX", "ATPP_BodyTypeMaleHurted14TX", "ATPP_BodyTypeMaleHurted24TX"
        };

        public static List<string> ExceptionBodyTypeDefnameAndroidWithSkinFemale = new List<string>
        {
            "ATPP_BodyTypeFemaleHurted12TX", "ATPP_BodyTypeFemaleHurted22TX", "ATPP_BodyTypeFemaleHurted22KTX", "ATPP_BodyTypeFemaleHurted12KTX", "ATPP_BodyTypeFemaleHurted13TX",
            "ATPP_BodyTypeFemaleHurted23TX", "ATPP_BodyTypeFemaleHurted14TX", "ATPP_BodyTypeFemaleHurted24TX"
        };

        public static List<string> ExceptionAutodoors = new List<string> {"Autodoor"};

        public static List<string> ExceptionSkyCloudCores = new List<string> {"ATPP_SkyCloudCore"};

        public static List<string> ExceptionQEEGS = new List<string>
            {"ATPP_GS_TX2KMale", "ATPP_GS_TX2KFemale", "ATPP_GS_TX3Male", "ATPP_GS_TX3Female", "ATPP_GS_TX4Male", "ATPP_GS_TX4Female"};

        public static List<string> ExceptionSkillServers = new List<string> {"ATPP_I500Skill", "ATPP_I300Skill", "ATPP_I100Skill"};
        public static List<string> ExceptionSecurityServers = new List<string> {"ATPP_I500Security", "ATPP_I300Security", "ATPP_I100Security"};
        public static List<string> ExceptionHackingServers = new List<string> {"ATPP_I500Hacking", "ATPP_I300Hacking", "ATPP_I100Hacking"};
        public static List<string> ExceptionSurrogatePodGuest = new List<string> {"ATPP_AndroidPodGuest", "AndroidOperationBedGuest"};
        public static List<string> ExceptionSurrogatePod = new List<string> {"ATPP_AndroidPod", "AndroidOperationBed"};
        public static List<string> ExceptionSurrogateM7Pod = new List<string> {"ATPP_AndroidPodMech"};
        public static string defNameOldSecurityServer = "ATPP_I100Security";
        public static string defNameBasicSecurityServer = "ATPP_I300Security";
        public static string defNameAdvancedSecurityServer = "ATPP_I500Security";

        public static string defNameOldHackingServer = "ATPP_I100Hacking";
        public static string defNameBasicHackingServer = "ATPP_I300Hacking";
        public static string defNameAdvancedHackingServer = "ATPP_I500Hacking";

        public static string defNameOldSkillServer = "ATPP_I100Skill";
        public static string defNameBasicSkillServer = "ATPP_I300Skill";
        public static string defNameAdvancedSkillServer = "ATPP_I500Skill";

        public static List<string> ExceptionBionicHaveFeet = new List<string> {"ARLeg", "HydraulicLeg", "MakeshiftRLeg", "BRLeg", "AR2Leg"};
        public static List<string> ExceptionBionicHaveHand = new List<string> {"MiningArm", "HydraulicArm", "MakeshiftRArm", "BRArm", "ARArm", "AR2Arm"};

        public static List<string> ExceptionPlayerStartingAndroidPawnKindList = new List<string>
        {
            "AndroidT1ColonistGeneral", "AndroidT2ColonistGeneral", "AndroidT3ColonistGeneral", "AndroidT4ColonistGeneral", "AndroidT5ColonistGeneral", "ATPP_Android2TXKind",
            "ATPP_Android3TXKind", "ATPP_Android4TXKind", "ATPP_Android2KTXKind", "ATPP_Android2KITXKind", "ATPP_Android2ITXKind", "ATPP_Android3ITXKind", "ATPP_Android4ITXKind"
        };

        public static List<string> ExceptionAndroidsDontRust = new List<string>
            {"ATPP_Android2TX", "ATPP_Android3TX", "ATPP_Android4TX", "ATPP_Android2KTX", "ATPP_Android2ITX", "ATPP_Android3ITX", "ATPP_Android4ITX", "ATPP_Android2KITX"};

        public static List<string> ExceptionTXSerie = new List<string>
            {"ATPP_Android2TX", "ATPP_Android3TX", "ATPP_Android4TX", "ATPP_Android2KTX", "ATPP_Android2ITX", "ATPP_Android3ITX", "ATPP_Android4ITX", "ATPP_Android2KITX"};

        public static List<string> ExceptionAndroidWithoutSkinList = new List<string>();
        public static List<string> ExceptionAndroidWithSkinList = new List<string> {"ATPP_Android2TX", "ATPP_Android3TX", "ATPP_Android4TX", "ATPP_Android2KTX"};

        public static List<string> ExceptionNanoKits = new List<string> {"ATPP_AndroidNanokitBasic", "ATPP_AndroidNanokitIntermediate", "ATPP_AndroidNanokitAdvanced"};

        public static List<string> ExceptionRegularAndroidList = new List<string>
        {
            "Android1Tier", "Android2Tier", "Android3Tier", "Android4Tier", "Android5Tier", "AT_HellUnit", "ATPP_Android2TX", "ATPP_Android3TX", "ATPP_Android4TX",
            "ATPP_Android2KTX", "ATPP_Android2ITX", "ATPP_Android2KITX", "ATPP_Android3ITX", "ATPP_Android4ITX"
        };

        public static List<string> ExceptionAndroidList = new List<string>
        {
            "Android1Tier", "Android2Tier", "Android3Tier", "Android4Tier", "Android5Tier", "M7Mech", "AT_HellUnit", "ATPP_Android2TX", "ATPP_Android3TX", "ATPP_Android4TX",
            "ATPP_Android2KTX", "ATPP_Android2ITX", "ATPP_Android2KITX", "ATPP_Android3ITX", "ATPP_Android4ITX"
        };

        public static List<string> ExceptionAndroidCorpseList = new List<string>
            {"Corpse_Android1Tier", "Corpse_Android2Tier", "Corpse_Android3Tier", "Corpse_Android4Tier", "Corpse_Android5Tier", "Corpse_AT_HellUnit", "Corpse_M7Mech"};

        public static List<string> ExceptionAndroidListBasic = new List<string>
            {"Android1Tier", "Android2Tier", "M7Mech", "ATPP_Android2TX", "ATPP_Android2KTX", "ATPP_Android2ITX"};

        public static List<string> ExceptionAndroidListAdvanced = new List<string>
            {"Android3Tier", "Android4Tier", "Android5Tier", "AT_HellUnit", "ATPP_Android3TX", "ATPP_Android4TX", "ATPP_Android3ITX", "ATPP_Android4ITX"};

        public static List<string> ExceptionAndroidAnimalPowered = new List<string> {"AndroidMuff", "AndroidDog", "RoboticSheep", "RoboticCow", "AndroidFox"};
        public static List<string> ExceptionAndroidAnimals = new List<string> {"AndroidMuff", "AndroidDog", "RoboticSheep", "RoboticCow", "AndroidChicken", "AndroidFox"};

        public static List<string> BlacklistAndroidHediff = new List<string>
        {
            "VacuumDamageHediff", "ZeroGSickness", "SpaceHypoxia", "ClinicalDeathAsphyxiation", "ClinicalDeathNoHeartbeat", "FatalRad", "RimatomicsRadiation", "RadiationIncurable"
        };

        public static List<string> BlacklistMindTraits = new List<string> {"NightOwl", "Insomniac", "Codependent", "HeavySleeper", "Polygamous", "Beauty", "Immunity"};

        public static List<string> BlacklistAndroidFood = new List<string>
            {"SmokeleafJoint", "Yayo", "PsychiteTea", "Flake", "Penoxycyline", "Luciferium", "GoJuice", "Ambrosia", "Beer", "RC2_Coffee", ""};

        public static List<string> ExceptionNeuralChip = new List<string> {"ATPP_HediffVX3Chip", "ATPP_HediffVX2Chip", "ATPP_HediffVX1Chip", "ATPP_HediffVX0Chip"};

        public static List<string> ExceptionVXNeuralChipSurgery = new List<string>
        {
            "ATPP_InstallVX0ChipOnAndroid", "ATPP_InstallVX0Chip", "ATPP_InstallVX1ChipOnAndroid", "ATPP_InstallVX1Chip", "ATPP_InstallVX2ChipOnAndroid", "ATPP_InstallVX2Chip",
            "ATPP_InstallVX3ChipOnAndroid", "ATPP_InstallVX3Chip"
        };

        public static List<string> ExceptionArtificialBrainsSurgery = new List<string>
            {"ATPP_InstallT1ArtificialBrain", "ATPP_InstallT2ArtificialBrain", "ATPP_InstallT3ArtificialBrain", "ATPP_InstallT4ArtificialBrain"};

        public static List<string> ExceptionAndroidOnlyHediffs = new List<string>
        {
            "PlatingSteel", "PlatingPlasteel", "PlatingComposite", "MiningArm", "HydraulicFrame", "HydraulicLeg",
            "HydraulicArm", "MakeshiftRLeg", "MakeshiftRArm", "ALReceptor", "ARLeg", "AHeatsink", "ACoolantPump", "ABattery", "AMStorage", "ATransformer", "AVAdapter",
            "AdvRearCounterweight", "AdvJawAndroid",
            "BRArm", "BRLeg", "BLReceptor", "BHeatsink", "BBattery", "BMStorage", "BasicRearCounterweight", "CrudeJawAndroid", "BCoolantPump", "BTransformer", "BVAdapter", "ARArm",
            "PositronMind",
            "SoldierMind", "BuilderMind", "SurgeonMind", "SpeedMind", "GeoMind", "MechanicMind", "CookingMind", "CharismaMind", "NegotiatorMind", "ZoologyMind", "AgriculturalMind",
            "UnskilledMind", "AR2Arm", "AR2Leg", "AL2Receptor", "A2Heatsink",
            "A2CoolantPump", "A2Battery", "A2MStorage", "A2Transformer", "A2VAdapter", "HearingSensorCrude", "HearingSensorAdv", "HearingSensorArch", "SmellSensorAdv",
            "EvolvingMind"
        };

        public static List<string> ExceptionAndroidCanReloadWithPowerList;


        public static List<string> AndroidOldAgeHediffCPU = new List<string> {"CorruptMemory"};
        public static List<string> AndroidOldAgeHediffCooling = new List<string> {"ExaggeratedHealing"};
        public static List<string> AndroidOldAgeHediffFramework = new List<string> {"DecayedFrame"};
        public static List<string> AndroidOldAgeHediffHydraulic = new List<string> {"FaultyPump", "WeakValves"};

        public static List<string> IgnoredThoughtsByAllAndroids = new List<string>
        {
            "SoakingWet", "EnvironmentCold", "AteWithoutTable", "EnvironmentHot", "SleptInCold", "SleptInHeat", "AteRawFood", "AteAwfulMeal", "AteKibble", "AteInsectMeatDirect",
            "AteInsectMeatAsIngredient"
        };

        public static List<string> IgnoredThoughtsByBasicAndroids = new List<string>
        {
            "KnowColonistOrganHarvested", "ColonistBanished", "WasImprisoned", "KnowGuestOrganHarvested", "KnowPrisonerSold", "AteRottenFood", "BondedAnimalBanished",
            "ColonistBanishedToDie", "ButcheredHumanlikeCorpse", "PrisonerBanishedToDie", "KnowButcheredHumanlikeCorpse", "EnvironmentDark", "ApparelDamaged", "DeadMansApparel",
            "HumanLeatherApparelSad", "HumanLeatherApparelHappy", "SoldPrisoner", "ExecutedPrisoner", "KilledColonyAnimal", "SleptOutside",
            "SleptOnGround", "KnowGuestExecuted", "KnowColonistExecuted", "KnowColonistDied", "WitnessedDeathAlly", "WitnessedDeathNonAlly",
            "WitnessedDeathFamily", "WitnessedDeathBloodlust", "KilledHumanlikeBloodlust", "PawnWithGoodOpinionDied", "PawnWithBadOpinionDied",
            "AteCorpse", "ObservedLayingCorpse", "ObservedLayingRottingCorpse", "AteHumanlikeMeatDirect", "AteHumanlikeMeatDirectCannibal", "AteHumanlikeMeatAsIngredient",
            "AteHumanlikeMeatAsIngredientCannibal", "ATPP_VX0PuppetThought"
        };

        public static List<string> IgnoredInteractionsByBasicAndroids = new List<string> {"RomanceAttempt", "MarriageProposal", "Breakup"};


        public static List<HediffDef> ExceptionRepairableFrameworkHediff;

        public static ResearchProjectDef ResearchProjectSkyMindLAN;
        public static ResearchProjectDef ResearchProjectSkyMindWAN;

        public static ResearchProjectDef ResearchAndroidBatteryOverload;


        public static int getConsumedPowerByAndroid(string defName)
        {
            var ret = 0;
            switch (defName)
            {
                case T1:
                    ret = Settings.wattConsumedByT1;
                    break;
                case TX2:
                case TX2I:
                case TX2K:
                case TX2KI:
                case T2:
                    ret = Settings.wattConsumedByT2;
                    break;
                case TX3:
                case TX3I:
                case T3:
                    ret = Settings.wattConsumedByT3;
                    break;
                case TX4:
                case TX4I:
                case T4:
                    ret = Settings.wattConsumedByT4;
                    break;
                case T5:
                    ret = Settings.wattConsumedByT5;
                    break;
                case M7:
                    ret = Settings.wattConsumedByM7;
                    break;
                case HU:
                    ret = Settings.wattConsumedByHellUnit;
                    break;
                case K9:
                    ret = Settings.wattConsumedByK9;
                    break;
                case MUFF:
                    ret = Settings.wattConsumedByMUFF;
                    break;
                case Phytomining:
                    ret = Settings.wattConsumedByPhytomining;
                    break;
                case NSolution:
                    ret = Settings.wattConsumedByNSolution;
                    break;
                case RoboticFennec:
                    ret = Settings.wattConsumedByFENNEC;
                    break;
            }

            return ret;
        }


        public static Building_Bed getAvailableAndroidPodForCharging(Pawn android, bool M7 = false)
        {
            var map = android.Map;
            Building_Bed ret = null;
            float dist = -1;


            if (android.ownership != null && android.ownership.OwnedBed != null)
            {
                if ((!M7 && android.ownership.OwnedBed.def.defName == "ATPP_AndroidPod"
                     || M7 && android.ownership.OwnedBed.def.defName == "ATPP_AndroidPodMech")
                    && android.CanReserveAndReach(android.ownership.OwnedBed, PathEndMode.OnCell, Danger.Deadly)
                    && android.ownership.OwnedBed.Position.InAllowedArea(android)
                    && !android.ownership.OwnedBed.Destroyed && android.ownership.OwnedBed.TryGetComp<CompPowerTrader>() != null &&
                    android.ownership.OwnedBed.TryGetComp<CompPowerTrader>().PowerOn)
                    return android.ownership.OwnedBed;

                return null;
            }

            foreach (var el in map.listerBuildings.allBuildingsColonistElecFire)

                if ((!M7 && el.def.defName == "ATPP_AndroidPod" || M7 && el.def.defName == "ATPP_AndroidPodMech")
                    && !el.Destroyed && el.TryGetComp<CompPowerTrader>() != null)
                {
                    var bed = (Building_Bed) el;

                    if (bed.Medical || android.IsPrisoner != bed.ForPrisoners || bed.GetCurOccupant(0) != null ||
                        bed.OwnersForReading.Count() != 0 && !bed.OwnersForReading.Contains(android) || !el.TryGetComp<CompPowerTrader>().PowerOn ||
                        !el.Position.InAllowedArea(android) || !android.CanReserveAndReach(el, PathEndMode.OnCell, Danger.Deadly)) continue;

                    var cdist = android.Position.DistanceTo(el.Position);

                    if (dist != -1 && !(cdist < dist)) continue;

                    ret = (Building_Bed) el;
                    dist = cdist;
                }


            if (ret != null) android.ownership.ClaimBedIfNonMedical(ret);

            return ret;
        }

        public static void playVocal(string vocal)
        {
            if (!GCATPP.isThereSkyCloudCore() || Settings.disableAbilitySkyCloudServerToTalk)
                return;

            var CGT = Find.TickManager.TicksGame;

            switch (vocal)
            {
                case "soundDefSkyCloudMindQuarantineMentalState":
                    soundDefSkyCloudMindQuarantineMentalState.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudMindDownloadCompleted":
                    soundDefSkyCloudMindDownloadCompleted.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudMindUploadCompleted":
                    soundDefSkyCloudMindUploadCompleted.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudDoorOpened":
                    if (CGT - lastDoorOpenedVocalGT >= 300)
                    {
                        soundDefSkyCloudDoorOpened.PlayOneShot(null);
                        lastDoorOpenedVocalGT = CGT;
                    }

                    break;
                case "soundDefSkyCloudDoorClosed":
                    if (CGT - lastDoorClosedVocalGT >= 300)
                    {
                        soundDefSkyCloudDoorClosed.PlayOneShot(null);
                        lastDoorClosedVocalGT = CGT;
                    }

                    break;
                case "soundDefSkyCloudSkyMindNetworkOffline":
                    if (CGT - lastPlayedVocalWarningNoSkyMindNetGT >= 900)
                    {
                        soundDefSkyCloudSkyMindNetworkOffline.PlayOneShot(null);
                        lastPlayedVocalWarningNoSkyMindNetGT = CGT;
                    }

                    break;
                case "soundDefSkyCloudMindReplicationCompleted":
                    soundDefSkyCloudMindReplicationCompleted.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudMindMigrationCompleted":
                    soundDefSkyCloudMindMigrationCompleted.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudPrimarySystemsOnline":
                    soundDefSkyCloudPrimarySystemsOnline.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudAllMindDisconnected":
                    soundDefSkyCloudAllMindDisconnected.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudMindDeletionCompleted":
                    soundDefSkyCloudMindDeletionCompleted.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudPowerFailure":
                    soundDefSkyCloudPowerFailure.PlayOneShot(null);
                    break;
                case "soundDefSkyCloudDeviceDeactivated":
                    if (CGT - lastDeviceActivatedVocalGT > 300)
                    {
                        soundDefSkyCloudDeviceDeactivated.PlayOneShot(null);
                        lastDeviceActivatedVocalGT = CGT;
                    }

                    break;
                case "soundDefSkyCloudDeviceActivated":
                    if (CGT - lastDeviceDeactivatedVocalGT > 300)
                    {
                        soundDefSkyCloudDeviceActivated.PlayOneShot(null);
                        lastDeviceDeactivatedVocalGT = CGT;
                    }

                    break;
            }
        }


        public static bool DynamicMedicalCareSetterPrefixPostfix(Rect rect, ref MedicalCareCategory medCare)
        {
            if (_DynamicMedicalCareSetter_NoAndroidSelected()) return true;


            MedicalCareUtility_Patch.MedicalCareSetter_Patch.Listener(rect, ref medCare);
            return false;
        }


        public static bool DynamicMedicalCareSetterPrefix(Rect rect, ref MedicalCareCategory medCare)
        {
            return _DynamicMedicalCareSetter_NoAndroidSelected();
        }

        public static bool _DynamicMedicalCareSetter_NoAndroidSelected()
        {
            var obj = Find.Selector.SelectedObjects;
            try
            {
                if (obj.Count != 1 || !(obj[0] is Pawn)) return true;

                var pawn = (Pawn) obj[0];

                return !ExceptionAndroidList.Contains(pawn.def.defName);
            }
            catch (Exception)
            {
                return true;
            }
        }


        public static bool FindBestMedicinePrefix(Pawn healer, Pawn patient, out int totalCount)
        {
            totalCount = 0;

            FindBestMedicinePatient = patient;
            return true;
        }

        public static void FindBestMedicinePostfix(Pawn healer, Pawn patient, out int totalCount)
        {
            totalCount = Medicine.GetMedicineCountToFullyHeal(patient);
            FindBestMedicinePatient = null;
        }

        public static void genericPostFixExtraCrafterDoctorJobs(Pawn pawn, Thing t, bool forced, ref bool __result, WorkGiver __instance)
        {
            if (!Settings.androidsCanOnlyBeHealedByCrafter)
            {
                if (__instance.def.workType == WorkTypeDefSmithing && CrafterDoctorJob.Contains(__instance.def)) __result = false;

                return;
            }


            if (__instance.def.workType == WorkTypeDefOf.Doctor)
            {
                if (t is Pawn tPawn && tPawn.IsAndroidTier())
                    __result = false;
            }
            else
            {
                if (!CrafterDoctorJob.Contains(__instance.def)) return;

                if (t is Pawn tPawn && tPawn.IsAndroidTier())
                {
                    var cso = pawn.TryGetComp<CompSurrogateOwner>();

                    if (cso == null || !cso.repairAndroids)
                        __result = false;
                }
                else
                {
                    __result = false;
                }
            }
        }

        public static Map getMapFromString(this string MUID)
        {
            return Find.Maps.FirstOrDefault(m => m.GetUniqueLoadID() == MUID);
        }

        public static void removeAllSlowNetworkHediff(bool onlyInCaravan = false)
        {
            if (Current.ProgramState != ProgramState.Playing)
                return;

            if (!onlyInCaravan)
                foreach (var m in Find.Maps)
                foreach (var p in m.mapPawns.FreeColonistsAndPrisoners)
                    if (p.IsSurrogateAndroid())
                    {
                        var he = p.health.hediffSet.GetFirstHediffOfDef(hediffLowNetworkSignal);
                        if (he != null)
                            p.health.RemoveHediff(he);
                    }


            foreach (var e in Find.WorldObjects.Caravans)
            foreach (var p in e.pawns)
                if (p.IsSurrogateAndroid())
                {
                    var he = p.health.hediffSet.GetFirstHediffOfDef(hediffLowNetworkSignal);
                    if (he != null)
                        p.health.RemoveHediff(he);
                }
        }

        public static void throwChargingMote(Pawn cp)
        {
            if (!(cp.needs.food.CurLevelPercentage < 1.0)) return;

            if (cp.Map.moteCounter.Saturated) return;

            if (cp.needs.food.CurLevelPercentage >= 0.80f)
                throwMote(DefDatabase<ThingDef>.GetNamed("ATPP_MoteBIII"), cp);
            else if (cp.needs.food.CurLevelPercentage >= 0.40f)
                throwMote(DefDatabase<ThingDef>.GetNamed("ATPP_MoteBII"), cp);
            else
                throwMote(DefDatabase<ThingDef>.GetNamed("ATPP_MoteBI"), cp);
        }

        public static Lord LordOnMapWhereFactionIsInvolved(Map map, Faction faction)
        {
            return map.lordManager.lords.FirstOrDefault(l => l.faction == faction);
        }


        public static Map getRandomMapOfPlayer()
        {
            return Find.Maps.FirstOrDefault(map => map.IsPlayerHome);
        }


        public static void restoreSavedSurrogateName(Pawn surrogate)
        {
            var cas = surrogate.TryGetComp<CompAndroidState>();

            if (cas == null)
                return;

            var tmp = cas.savedName.Split('§');

            if (tmp.Count() != 3)
                return;

            surrogate.Name = new NameTriple(tmp[0], tmp[1], tmp[2]);
        }

        public static string getSavedSurrogateNameNick(Pawn surrogate)
        {
            var cas = surrogate.TryGetComp<CompAndroidState>();

            if (cas == null)
                return "";

            var tmp = cas.savedName.Split('§');

            return tmp.Count() != 3 ? "" : tmp[1];
        }

        public static void saveSurrogateName(Pawn surrogate)
        {
            var cas = surrogate.TryGetComp<CompAndroidState>();

            if (cas == null)
                return;

            var name = (NameTriple) surrogate.Name;
            cas.savedName = name.First + "§" + name.Nick + "§" + name.Last;
        }


        public static void initBodyAsSurrogate(Pawn surrogate, bool addSimpleMinded = true)
        {
            surrogate.skills = new Pawn_SkillTracker(surrogate);
            surrogate.needs = new Pawn_NeedsTracker(surrogate);

            surrogate.story.traits.allTraits.Clear();


            if (addSimpleMinded)
            {
                var td = DefDatabase<TraitDef>.GetNamed("SimpleMindedAndroid", false);
                Trait t = null;
                if (td != null)
                    t = new Trait(td);

                if (t != null)
                    surrogate.story.traits.allTraits.Add(t);
            }

            notifTraitsChanged(surrogate);


            if (surrogate.kindDef.defName != "M7MechPawn") return;

            var he = surrogate.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("BatteryChargeMech", false));
            if (he != null) surrogate.health.RemoveHediff(he);
        }

        public static Pawn generateSurrogate(Faction faction, PawnKindDef kindDef, IntVec3 pos, Map map, bool spawn = false, bool external = false, int tile = -1,
            bool allowFood = true, bool inhabitant = false, int Gender = -1)
        {
            var gender = Verse.Gender.Male;
            if (Gender != -1) gender = Gender == 0 ? Verse.Gender.Male : Verse.Gender.Female;

            var request = new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, true, 1f, false, true, allowFood,
                false, inhabitant, false, false, fixedGender: gender);
            var surrogate = PawnGenerator.GeneratePawn(request);
            if (spawn)
                GenSpawn.Spawn(surrogate, pos, map);

            initBodyAsSurrogate(surrogate);

            setSurrogateName(surrogate, external);


            var cas = surrogate.TryGetComp<CompAndroidState>();
            cas?.initAsSurrogate();

            return surrogate;
        }

        public static void setSurrogateName(Pawn surrogate, bool external = false)
        {
            int SXVer;
            var prefix = "";

            switch (surrogate.def.defName)
            {
                case T1:
                    SXVer = 1;
                    break;
                case T2:
                    SXVer = 2;
                    break;
                case T3:
                    SXVer = 3;
                    break;
                case T4:
                    SXVer = 4;
                    break;
                case HU:
                    SXVer = 10;
                    break;
                case TX2:
                case TX2I:
                    SXVer = 12;
                    prefix = "X";
                    break;
                case TX2K:
                case TX2KI:
                    SXVer = 120;
                    prefix = "X";
                    break;
                case TX3:
                case TX3I:
                    SXVer = 13;
                    prefix = "X";
                    break;
                case TX4:
                case TX4I:
                    SXVer = 14;
                    prefix = "X";
                    break;
                case "M7Mech":
                    prefix = "M";
                    SXVer = 7;
                    break;
                default:
                    SXVer = 0;
                    break;
            }

            if (!external)
            {
                surrogate.Name = new NameTriple("", "S" + prefix + SXVer + "-" + GCATPP.getNextSXID(SXVer), "");
                GCATPP.incNextSXID(SXVer);
            }
            else
            {
                surrogate.Name = new NameTriple("", "S" + SXVer + "-" + Rand.Range(50, 1000), "");
            }
        }

        public static string TranslateTicksToTextIRLSeconds(int ticks)
        {
            return ticks < 2500 ? ticks.ToStringSecondsFromTicks() : ticks.ToStringTicksToPeriodVerbose();
        }


        public static bool isThereSolarFlare()
        {
            return Enumerable.Any(Find.Maps, map => map.IsPlayerHome && map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare));
        }

        public static bool anyPlayerColonnyHasEnoughtSilver(int price)
        {
            return Enumerable.Any(Find.Maps, map => map.IsPlayerHome && TradeUtility.ColonyHasEnoughSilver(map, price));
        }

        public static void anyPlayerColonnyPaySilver(int price)
        {
            foreach (var map in Find.Maps.Where(map => map.IsPlayerHome && TradeUtility.ColonyHasEnoughSilver(map, price)))
            {
                TradeUtility.LaunchSilver(map, price);
                break;
            }
        }

        public static bool antennaSelected()
        {
            foreach (var el in Find.Selector.SelectedObjects)
                if (el is Building b)
                    if (b.def.defName == "ATPP_SkyMindLAN" || b.def.defName == "ATPP_SkyMindWAN")
                        return true;

            return false;
        }

        public static bool anySkyMindNetResearched()
        {
            return ResearchProjectSkyMindLAN.IsFinished || ResearchProjectSkyMindWAN.IsFinished;
        }

        public static void refreshHediff(Pawn pawn)
        {
            pawn.health.summaryHealth.Notify_HealthChanged();
            pawn.health.capacities.Notify_CapacityLevelsDirty();

            if (pawn.health.hediffSet != null)
                foreach (var h in pawn.health.hediffSet.hediffs)
                    pawn.health.Notify_HediffChanged(h);

            pawn.health.hediffSet?.DirtyCache();
        }

        public static bool AnyPressed(this Widgets.DraggableResult result)
        {
            return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
        }

        public static bool ContainsAny(this string haystack, List<string> needles)
        {
            return Enumerable.Any(needles, needle => haystack == needle);
        }


        public static void throwMote(ThingDef moteDef, Pawn android)
        {
            var moteThrown = (MoteThrown) ThingMaker.MakeThing(moteDef);
            moteThrown.Scale = 0.6f;
            moteThrown.rotationRate = Rand.Range(-1, 1);
            moteThrown.exactPosition = android.Position.ToVector3();
            moteThrown.exactPosition += new Vector3(0.85f, 0f, 0.85f);
            moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f;
            moteThrown.SetVelocity(Rand.Range(30f, 60f), Rand.Range(0.35f, 0.55f));
            GenSpawn.Spawn(moteThrown, android.Position, android.Map);
        }

        public static bool IsAndroidGen(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType.defName == "Android" || pawn.RaceProps.FleshType.defName == "MechanisedInfantry" || pawn.RaceProps.FleshType.defName == "ChJDroid" ||
                   pawn.def.defName == "ChjAndroid";
        }

        public static bool IsAndroidTier(this Pawn pawn)
        {
            return ExceptionAndroidList.Contains(pawn.def.defName);
        }

        public static bool IsCyberAnimal(this Pawn pawn)
        {
            return ExceptionAndroidAnimals.Contains(pawn.def.defName);
        }

        public static bool IsBasicAndroidTier(this Pawn pawn)
        {
            return ExceptionAndroidListBasic.Contains(pawn.def.defName);
        }

        public static bool IsPoweredAnimalAndroids(this Pawn pawn)
        {
            return ExceptionAndroidAnimalPowered.Contains(pawn.def.defName);
        }

        public static Hediff HaveNotStackableVXChip(this Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.FirstOrDefault(h => ExceptionNeuralChip.Contains(h.def.defName));
        }

        public static bool IsSurrogateAndroid(this Pawn pawn, bool usedSurrogate = false, bool notUsedSurrogate = false)
        {
            /*if(!pawn.IsAndroidTier())
                return false;*/

            var cas = pawn.TryGetComp<CompAndroidState>();
            if (cas == null)
                return false;

            return cas.isSurrogate && (!usedSurrogate || cas.surrogateController != null) && (!notUsedSurrogate || cas.surrogateController == null);
        }

        public static bool IsBlankAndroid(this Pawn pawn)
        {
            var cas = pawn.TryGetComp<CompAndroidState>();
            return cas != null && cas.isBlankAndroid;
        }

        public static bool haveAndroidOldAgeHediff(this Pawn pawn, List<string> issues)
        {
            return Enumerable.Any(pawn.health.hediffSet.hediffs, h => issues.Contains(h.def.defName));
        }

        public static bool VXAndVX0ChipPresent(this Pawn pawn)
        {
            return pawn.VXChipPresent() || pawn.VX0ChipPresent();
        }

        public static bool VXChipPresent(this Pawn pawn)
        {
            return pawn.VX1ChipPresent() || pawn.VX2ChipPresent() || pawn.VX3ChipPresent();
        }

        public static bool VX0ChipPresent(this Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(hediffHaveVX0Chip) != null;
        }

        public static bool VX1ChipPresent(this Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(hediffHaveVX1Chip) != null;
        }

        public static bool VX2ChipPresent(this Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(hediffHaveVX2Chip) != null;
        }

        public static bool VX3ChipPresent(this Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(hediffHaveVX3Chip) != null;
        }

        public static void showFailedLetterMindUpload(string reason)
        {
            Find.LetterStack.ReceiveLetter("ATPP_LetterInterruptedUpload".Translate(), "ATPP_LetterInterruptedUploadDesc".Translate(reason), LetterDefOf.ThreatSmall);
        }


        public static void ShowFloatMenuAndroidCandidate(Pawn emitter, Action<Pawn> onClick)
        {
            var opts = (from colon in emitter.Map.mapPawns.FreeColonistsAndPrisoners
                where colon != emitter && !colon.Dead && GCATPP.isConnectedToSkyMind(colon) && !colon.Destroyed && colon.IsAndroid() && !colon.IsSurrogateAndroid()
                let cab = colon.TryGetComp<CompAndroidState>()
                where !cab.showUploadProgress && cab.uploadEndingGT == -1
                select new FloatMenuOption(colon.LabelShortCap, delegate { onClick(colon); })).ToList();

            opts.SortBy(x => x.Label);


            if (opts.Count == 0)
                opts.Add(new FloatMenuOption("ATPP_ConsciousnessUploadNoRecipient".Translate(), null));

            var floatMenuMap = new FloatMenu(opts, "ATPP_ConsciousnessSelectDestination".Translate());
            Find.WindowStack.Add(floatMenuMap);
        }


        public static void ShowFloatMenuPermuteOrDuplicateCandidate(Pawn emitter, Action<Pawn> onClick, bool excludeBlankAndroid = false)
        {
            var opts = (from colon in emitter.Map.mapPawns.FreeColonistsAndPrisoners
                let cas = colon.TryGetComp<CompAndroidState>()
                where colon != emitter && !colon.Dead && (colon.VX2ChipPresent() || colon.VX3ChipPresent()) && (!excludeBlankAndroid || cas != null && !cas.isBlankAndroid) &&
                      !colon.Destroyed && GCATPP.isConnectedToSkyMind(colon) && !colon.IsSurrogateAndroid()
                let csm = colon.TryGetComp<CompSkyMind>()
                where csm == null || csm.Infected == -1
                let cso = colon.TryGetComp<CompSurrogateOwner>()
                where cso.duplicateEndingGT == -1 && cso.permuteEndingGT == -1 && !cso.showPermuteProgress && !cso.showDuplicateProgress && (!cso.controlMode || !cso.isThereSX())
                select new FloatMenuOption(colon.LabelShortCap, delegate { onClick(colon); })).ToList();

            opts.SortBy(x => x.Label);


            if (opts.Count == 0)
                opts.Add(new FloatMenuOption("ATPP_NoVX2Recipient".Translate(), null));

            var floatMenuMap = new FloatMenu(opts, "ATPP_ConsciousnessSelectDestination".Translate());
            Find.WindowStack.Add(floatMenuMap);
        }


        public static void ShowFloatMenuSkyCloudCores(Action<Building> onClick, Building self = null)
        {
            var opts = (from core in GCATPP.getAvailableSkyCloudCores()
                where core != self
                where !core.Destroyed && core.TryGetComp<CompPowerTrader>().PowerOn
                let ccore = core.TryGetComp<CompSkyCloudCore>()
                where ccore != null
                select new FloatMenuOption(ccore.getName(), delegate { onClick(core); })).ToList();

            opts.SortBy(x => x.Label);


            if (opts.Count == 0)
                opts.Add(new FloatMenuOption("ATPP_NoSkyCloudCoreAvailable".Translate(), null));

            var floatMenuMap = new FloatMenu(opts, "");
            Find.WindowStack.Add(floatMenuMap);
        }

        public static void Duplicate(Pawn source, Pawn dest, bool overwriteAsDeath = true)
        {
            try
            {
                var destOrigLabelShort = dest.LabelShortCap;


                var st = new Pawn_StoryTracker(dest) {melanin = dest.story.melanin};

                var hair = new Color {a = dest.story.hairColor.a, r = dest.story.hairColor.r, g = dest.story.hairColor.g, b = dest.story.hairColor.b};
                st.hairColor = hair;
                st.crownType = dest.story.crownType;
                st.hairDef = dest.story.hairDef;

                if (source.story?.adulthood != null)
                {
                    BackstoryDatabase.TryGetWithIdentifier(source.story.adulthood.identifier, out var ah);
                    st.adulthood = ah;
                }


                if (source.story != null)
                {
                    BackstoryDatabase.TryGetWithIdentifier(source.story.childhood.identifier, out var ch);
                    st.childhood = ch;

                    st.bodyType = dest.story.bodyType;

                    foreach (var nt in source.story.traits.allTraits.Select(trait => new Trait(trait.def, trait.Degree)))

                        gainDirectTrait(st, nt);

                    st.title = dest.story.title;

                    var vhg1 = (string) Traverse.Create(dest.story).Field("headGraphicPath").GetValue();

                    dest.story = st;

                    Traverse.Create(dest.story).Field("headGraphicPath").SetValue(vhg1);
                }

                notifTraitsChanged(dest);

                var ps = new Pawn_SkillTracker(source);

                var allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                foreach (var skillDef in allDefsListForReading)
                {
                    var skill = ps.GetSkill(skillDef);
                    var skillSource = source.skills.GetSkill(skillDef);
                    skill.Level = skillSource.Level;

                    if (skillSource.TotallyDisabled) continue;

                    skill.passion = skillSource.passion;
                    skill.xpSinceLastLevel = skillSource.xpSinceLastLevel;
                    skill.xpSinceMidnight = skillSource.xpSinceMidnight;
                }

                dest.skills = ps;


                if (overwriteAsDeath)
                {
                    PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(dest, null, PawnDiedOrDownedThoughtsKind.Died);

                    var spouse = dest.GetSpouse();
                    if (spouse != null && !spouse.Dead && spouse.needs.mood != null)
                    {
                        var memories = spouse.needs.mood.thoughts.memories;
                        memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
                        memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
                    }

                    Traverse.Create(dest.relations).Method("AffectBondedAnimalsOnMyDeath").GetValue();
                }


                dest.relations = new Pawn_RelationsTracker(dest);

                var pn = new Pawn_NeedsTracker(dest);


                foreach (var x in source.needs.mood.thoughts.memories.Memories)
                {
                    if (x.otherPawn != null && x.otherPawn == source)
                        x.otherPawn = dest;

                    pn.mood.thoughts.memories.Memories.Add(x);
                }

                dest.needs = pn;


                if (overwriteAsDeath)
                    dest.health.NotifyPlayerOfKilled(null, null, null);

                var nam = (NameTriple) source.Name;
                dest.Name = new NameTriple(nam.First, nam.Nick, nam.Last);

                dest.Drawer.renderer.graphics.ResolveAllGraphics();
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] Utils.Duplicate : " + e.Message + " - " + e.StackTrace);
            }
        }

        public static void PermutePawn(Pawn p1, Pawn p2)
        {
            try
            {
                if (p1 == null || p2 == null)
                    return;


                var st1 = new Pawn_StoryTracker(p2) {melanin = p2.story.melanin};


                var hair1 = new Color {a = p2.story.hairColor.a, r = p2.story.hairColor.r, g = p2.story.hairColor.g, b = p2.story.hairColor.b};

                st1.hairColor = hair1;
                st1.crownType = p2.story.crownType;
                st1.hairDef = p2.story.hairDef;

                if (p1.story?.adulthood != null)
                {
                    BackstoryDatabase.TryGetWithIdentifier(p1.story.adulthood.identifier, out var ah);
                    st1.adulthood = ah;
                }

                if (p1.story != null)
                {
                    BackstoryDatabase.TryGetWithIdentifier(p1.story.childhood.identifier, out var ch);
                    st1.childhood = ch;


                    if (p2.story?.bodyType != null)
                        st1.bodyType = p2.story.bodyType;


                    foreach (var nt in p1.story.traits.allTraits.Select(trait => new Trait(trait.def, trait.Degree)))

                        gainDirectTrait(st1, nt);

                    st1.title = p2.story.title;


                    var st2 = new Pawn_StoryTracker(p1) {melanin = p1.story.melanin};

                    var hair2 = new Color {a = p1.story.hairColor.a, r = p1.story.hairColor.r, g = p1.story.hairColor.g, b = p1.story.hairColor.b};
                    st2.hairColor = hair2;

                    st2.crownType = p1.story.crownType;
                    st2.hairDef = p1.story.hairDef;

                    if (p2.story?.adulthood != null)
                    {
                        BackstoryDatabase.TryGetWithIdentifier(p2.story.adulthood.identifier, out var ah2);
                        st2.adulthood = ah2;
                    }


                    BackstoryDatabase.TryGetWithIdentifier(p2.story.childhood.identifier, out var ch2);
                    st2.childhood = ch2;


                    st2.bodyType = p1.story.bodyType;

                    foreach (var nt in p2.story.traits.allTraits.Select(trait => new Trait(trait.def, trait.Degree)))

                        gainDirectTrait(st2, nt);

                    st2.title = p1.story.title;


                    var vhg1 = (string) Traverse.Create(p1.story).Field("headGraphicPath").GetValue();
                    var vhg2 = (string) Traverse.Create(p2.story).Field("headGraphicPath").GetValue();


                    p1.story = st2;
                    p2.story = st1;

                    Traverse.Create(p1.story).Field("headGraphicPath").SetValue(vhg1);
                    Traverse.Create(p2.story).Field("headGraphicPath").SetValue(vhg2);
                }


                notifTraitsChanged(p1);
                notifTraitsChanged(p2);

                ResetCachedIncapableOf(p1);
                ResetCachedIncapableOf(p2);

                invertRelations(p1, p2);


                /*foreach (var wp in Find.WorldPawns.AllPawnsAlive)
                {
                    foreach(var rel in wp.relations.DirectRelations.ToList())
                    {
                        if (rel.otherPawn != null)
                        {
                            if (rel.otherPawn == p1)
                            {
                                rel.otherPawn = p2;
                            }
                            else if (rel.otherPawn == p2)
                            {
                                rel.otherPawn = p1;
                            }
                        }
                    }
                }*/


                /************ SKILLS ****************/

                var ps1 = new Pawn_SkillTracker(p1);
                var ps2 = new Pawn_SkillTracker(p2);

                var allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                foreach (var skills in allDefsListForReading)
                {
                    var skillDef = skills;
                    var skill = ps2.GetSkill(skillDef);
                    var skillSource = p1.skills.GetSkill(skillDef);
                    skill.levelInt = skillSource.levelInt;


                    skill.passion = skillSource.passion;
                    skill.xpSinceLastLevel = skillSource.xpSinceLastLevel;
                    skill.xpSinceMidnight = skillSource.xpSinceMidnight;


                    var skillDef2 = skills;
                    var skill2 = ps1.GetSkill(skillDef);
                    var skillSource2 = p2.skills.GetSkill(skillDef);
                    skill2.levelInt = skillSource2.levelInt;


                    skill2.passion = skillSource2.passion;
                    skill2.xpSinceLastLevel = skillSource2.xpSinceLastLevel;
                    skill2.xpSinceMidnight = skillSource2.xpSinceMidnight;
                }

                p1.skills = ps1;
                p2.skills = ps2;

                var ptt = p1.training;
                p1.training = p2.training;
                p2.training = ptt;


                /*Pawn_MindState pmt = p1.mindState;
                p1.mindState = p2.mindState;
                p2.mindState = pmt;*/


                /****************** Changement du journal ************************************/

                foreach (var log in Find.PlayLog.AllEntries)
                    if (log.Concerns(p1) || log.Concerns(p2))
                    {
                        var tlog = Traverse.Create(log);
                        var initiator = tlog.Field("initiator").GetValue<Pawn>();
                        var recipient = tlog.Field("recipient").GetValue<Pawn>();

                        if (initiator == p1)
                            initiator = p2;
                        else if (initiator == p2)
                            initiator = p1;

                        if (recipient == p2)
                            recipient = p2;
                        else if (recipient == p1)
                            recipient = p2;

                        tlog.Field("initiator").SetValue(initiator);
                        tlog.Field("recipient").SetValue(recipient);
                    }


                /****************** memoires ****************************/


                var pn1 = new Pawn_NeedsTracker(p1);
                var pn2 = new Pawn_NeedsTracker(p2);


                foreach (var x in p1.needs.mood.thoughts.memories.Memories)
                {
                    if (x.otherPawn != null && x.otherPawn == p2)
                        x.otherPawn = p1;

                    pn2.mood.thoughts.memories.Memories.Add(x);
                }

                foreach (var x in p2.needs.mood.thoughts.memories.Memories)
                {
                    if (x.otherPawn != null && x.otherPawn == p1)
                        x.otherPawn = p2;

                    pn1.mood.thoughts.memories.Memories.Add(x);
                }


                foreach (var mem in Find.Maps.SelectMany(map =>
                    from colon in map.mapPawns.AllPawns
                    where colon != p1 && colon != p2 && colon.needs.mood != null
                    from mem in colon.needs.mood.thoughts.memories.Memories.Where(mem => mem.otherPawn != null)
                    select mem))
                    if (mem.otherPawn == p1)
                        mem.otherPawn = p2;
                    else if (mem.otherPawn == p2) mem.otherPawn = p1;

                foreach (var mem in Find.WorldPawns.AllPawnsAlive.Where(wp => wp.needs.mood != null)
                    .SelectMany(wp => wp.needs.mood.thoughts.memories.Memories.Where(mem => mem.otherPawn != null)))
                    if (mem.otherPawn == p1)
                        mem.otherPawn = p2;
                    else if (mem.otherPawn == p2) mem.otherPawn = p1;


                /******************* NEEDS *******************************/


                if (p1.needs.food != null && p2.needs.food != null)
                {
                    pn1.food.CurLevel = p1.needs.food.CurLevel;
                    pn2.food.CurLevel = p2.needs.food.CurLevel;
                }


                if (p1.needs.joy != null && p2.needs.joy != null)
                {
                    pn1.joy.CurLevel = p1.needs.joy.CurLevel;
                    pn2.joy.CurLevel = p2.needs.joy.CurLevel;
                }


                if (p1.needs.comfort != null && p2.needs.comfort != null)
                {
                    pn1.comfort.CurLevel = p1.needs.comfort.CurLevel;
                    pn2.comfort.CurLevel = p2.needs.comfort.CurLevel;
                }


                if (p1.needs.roomsize != null && p2.needs.roomsize != null)
                {
                    pn1.roomsize.CurLevel = p1.needs.roomsize.CurLevel;
                    pn2.roomsize.CurLevel = p2.needs.roomsize.CurLevel;
                }


                if (p1.needs.rest != null && p2.needs.rest != null)
                {
                    pn1.rest.CurLevel = p1.needs.rest.CurLevel;
                    pn2.rest.CurLevel = p2.needs.rest.CurLevel;
                }

                if (p1.needs.mood != null && p2.needs.mood != null)
                {
                    pn1.mood.CurLevel = p2.needs.mood.CurLevel;
                    pn2.mood.CurLevel = p1.needs.mood.CurLevel;
                }

                p1.needs = pn1;
                p2.needs = pn2;


                pn1.AddOrRemoveNeedsAsAppropriate();
                pn2.AddOrRemoveNeedsAsAppropriate();


                /*foreach(var mem in p1.needs.mood.thoughts.memories.Memories)
                {
                    if (mem.otherPawn != null && mem.otherPawn == p1)
                    {
                        mem.otherPawn = p2;
                    }
                }

                foreach (var mem in p2.needs.mood.thoughts.memories.Memories)
                {
                    if (mem.otherPawn != null && mem.otherPawn == p2)
                    {
                        mem.otherPawn = p1;
                    }
                }*/

                /*ThingDef p1TD = p1.def;
                p1.def = p2.def;
                p2.def = p1TD;*/

                var nam = p1.Name;
                p1.Name = p2.Name;
                p2.Name = nam;

                p1.Drawer.renderer.graphics.ResolveAllGraphics();
                p2.Drawer.renderer.graphics.ResolveAllGraphics();


                /*************************************** PERMUTATION des LITS ********************************/
                var bedP1 = p1.ownership.OwnedBed;
                var bedP2 = p2.ownership.OwnedBed;

                /*if (bedP1 != null && bedP2 != null)
                {
                    p1.ownership.UnclaimBed();
                    p2.ownership.UnclaimBed();

                    p1.ownership.ClaimBedIfNonMedical(bedP2);
                    p2.ownership.ClaimBedIfNonMedical(bedP1);
                }*/
                if (bedP1 != null)
                    p1.ownership.UnclaimBed();
                if (bedP2 != null)
                    p2.ownership.UnclaimBed();

                if (bedP1 != null) p2.ownership.ClaimBedIfNonMedical(bedP1);
                if (bedP2 != null) p1.ownership.ClaimBedIfNonMedical(bedP2);


                /*Log.Message("P2 => " + p2.Label);
                foreach (Pawn current in p2.relations.pawnsWithDirectRelationsWithMe)
                {
                    Log.Message("=>" + current.Label);
                }*/
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] Utils.PermutePawn : " + e.Message + " - " + e.StackTrace);
            }
        }

        public static bool ShouldBeDead(this Pawn pawn)
        {
            if (pawn.Dead) return true;

            if (Enumerable.Any(pawn.health.hediffSet.hediffs, t => t.CauseDeathNow())) return true;

            if (pawn.health.ShouldBeDeadFromRequiredCapacity() != null) return true;

            var num = PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart);
            return num <= 0.0001f || pawn.health.ShouldBeDeadFromLethalDamageThreshold();
        }


        public static void ResetCachedIncapableOf(Pawn pawn)
        {
            pawn.ClearCachedDisabledWorkTypes();
            pawn.ClearCachedDisabledSkillRecords();
            var combinedDisabledWorkTags = pawn.CombinedDisabledWorkTags;
            if (combinedDisabledWorkTags == WorkTags.None) return;

            var list = (IEnumerable<WorkTags>) typeof(CharacterCardUtility).GetMethod("WorkTagsFrom", BindingFlags.Static | BindingFlags.NonPublic)
                ?.Invoke(null, new object[] {combinedDisabledWorkTags});
            if (list == null) return;

            var incapableList = list.Select(tag => tag.LabelTranslated().CapitalizeFirst()).ToList();
        }

        public static void ClearCachedDisabledWorkTypes(this Pawn pawn)
        {
            if (pawn != null) typeof(Pawn).GetField("cachedDisabledWorkTypes", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(pawn, null);
        }

        public static void ClearCachedDisabledSkillRecords(this Pawn pawn)
        {
            if (pawn.skills?.skills == null) return;

            var field = typeof(SkillRecord).GetField("cachedTotallyDisabled", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var record in pawn.skills.skills.Where(record => field != null))
                if (field != null)
                    field.SetValue(record, BoolUnknown.Unknown);
        }

        public static void invertRelations(Pawn p1, Pawn p2)
        {
            try
            {
                var pro1 = p1.relations;
                var pro2 = p2.relations;

                var pr1 = new Pawn_RelationsTracker(p1);
                var pr2 = new Pawn_RelationsTracker(p2);
                p1.relations = pr1;
                p2.relations = pr2;
                Pawn tmp1;


                foreach (var wp in Find.WorldPawns.AllPawnsAlive)
                {
                    if (wp?.relations?.DirectRelations == null)
                        continue;

                    foreach (var rel in wp.relations.DirectRelations.ToList().Where(rel => rel.otherPawn != null))
                    {
                        if (rel.otherPawn == p1)
                        {
                            wp.relations.TryRemoveDirectRelation(rel.def, p1);
                            wp.relations.AddDirectRelation(rel.def, p2);
                        }

                        if (rel.otherPawn != p2) continue;

                        wp.relations.TryRemoveDirectRelation(rel.def, p2);
                        wp.relations.AddDirectRelation(rel.def, p1);
                    }
                }


                foreach (var rel in pro2.DirectRelations.ToList())
                {
                    if (rel.otherPawn?.relations != null && rel.otherPawn != p1 && rel.otherPawn != p2)
                        rel.otherPawn.relations.TryRemoveDirectRelation(rel.def, p2);


                    tmp1 = rel.otherPawn == p1 ? p2 : rel.otherPawn;

                    pr1.AddDirectRelation(rel.def, tmp1);
                }


                foreach (var rel in pro1.DirectRelations.ToList())
                {
                    if (rel.otherPawn != null && rel.otherPawn.relations != null && rel.otherPawn != p1 && rel.otherPawn != p2)
                        rel.otherPawn.relations.TryRemoveDirectRelation(rel.def, p1);


                    tmp1 = rel.otherPawn == p2 ? p1 : rel.otherPawn;

                    pr2.AddDirectRelation(rel.def, tmp1);
                }

                p1.relations.everSeenByPlayer = true;
                p2.relations.everSeenByPlayer = true;

                foreach (var colon in Find.Maps.SelectMany(map => map.mapPawns.AllPawns.Where(colon => colon != p1 && colon != p2).Where(colon => colon.playerSettings != null)))
                    if (colon.playerSettings.Master != null && colon.playerSettings.Master == p1)
                        colon.playerSettings.Master = p2;
                    else if (colon.playerSettings.Master != null && colon.playerSettings.Master == p2)
                        colon.playerSettings.Master = p1;
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] Utils.InvertRelations : " + e.Message + " - " + e.StackTrace);
            }
        }


        public static void gainDirectTrait(Pawn_StoryTracker tr, Trait trait)
        {
            if (tr.traits.HasTrait(trait.def))
                return;

            tr.traits.allTraits.Add(trait);
        }

        public static void notifTraitsChanged(Pawn pawn)
        {
            pawn.Notify_DisabledWorkTypesChanged();


            pawn.skills?.Notify_SkillDisablesChanged();
            if (!pawn.Dead && pawn.RaceProps.Humanlike) pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
        }

        public static bool pawnCurrentlyControlRemoteSurrogate(Pawn pawn)
        {
            var cso = pawn.TryGetComp<CompSurrogateOwner>();
            return cso != null && cso.isThereSX();
        }


        public static void removeUploadHediff(Pawn cpawn, Pawn uploadRecipient)
        {
            Hediff he;


            if (cpawn != null)
            {
                he = cpawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
                if (he != null)
                    cpawn.health.RemoveHediff(he);
            }

            if (uploadRecipient == null) return;

            he = uploadRecipient.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("ATPP_ConsciousnessUpload"));
            if (he != null)
                uploadRecipient.health.RemoveHediff(he);
        }


        public static bool mindTransfertsAllowed(Pawn pawn, bool checkIsBlankAndroid = true)
        {
            var cas = pawn.TryGetComp<CompAndroidState>();
            if (cas != null && (cas.uploadEndingGT != -1 || cas.showUploadProgress || checkIsBlankAndroid && cas.isBlankAndroid))
                return false;

            var cso = pawn.TryGetComp<CompSurrogateOwner>();
            if (cso == null) return !pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare);
            if (cso.duplicateEndingGT != -1 || cso.showDuplicateProgress)
                return false;
            if (cso.permuteEndingGT != -1 || cso.showPermuteProgress)
                return false;
            if (cso.uploadToSkyCloudEndingGT != -1)
                return false;
            if (cso.downloadFromSkyCloudEndingGT != -1)
                return false;
            if (cso.mindAbsorptionEndingGT != -1)
                return false;

            return !pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare);
        }


        public static bool isThereNotControlledSurrogateInCaravan()
        {
            foreach (var c in Find.World.worldObjects.Caravans)
            foreach (var p in c.pawns)
            {
                if (p.Dead || p.Destroyed || !p.IsSurrogateAndroid()) continue;

                var cas = p.TryGetComp<CompAndroidState>();
                if (cas.surrogateController == null)
                    return true;
            }

            return false;
        }


        public static void ShowFloatMenuNotCOntrolledSurrogateInCaravan(Pawn emitter, Action<Pawn> onClick)
        {
            var opts = new List<FloatMenuOption>();

            foreach (var c in Find.World.worldObjects.Caravans)
            foreach (var colon in c.pawns)

                if (colon != emitter && !colon.Dead
                                     && !colon.Destroyed && colon.IsSurrogateAndroid(false, true))
                    opts.Add(new FloatMenuOption(colon.LabelShortCap, delegate { onClick(colon); }));
            opts.SortBy(x => x.Label);


            if (opts.Count == 0)
                return;

            var floatMenuMap = new FloatMenu(opts, "");
            Find.WindowStack.Add(floatMenuMap);
        }


        public static Pawn spawnCorpseCopy(Pawn pawn, bool kill = true)
        {
            /*PawnGenerationContext pgc = PawnGenerationContext.NonPlayer;
            if (!kill)
                pgc = PawnGenerationContext.PlayerStarter;*/

            var request = new PawnGenerationRequest(pawn.kindDef, Faction.OfAncients, PawnGenerationContext.NonPlayer, fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat,
                fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYearsFloat, fixedGender: pawn.gender, fixedMelanin: pawn.story.melanin);
            var p = PawnGenerator.GeneratePawn(request);


            p?.equipment?.DestroyAllEquipment();
            p?.apparel?.DestroyAll();
            p?.inventory?.DestroyAll();

            if (p == null) return p;

            p.Rotation = pawn.Rotation;


            p.story.melanin = pawn.story.melanin;
            p.story.bodyType = pawn.story.bodyType;
            var hair = new Color {a = pawn.story.hairColor.a, r = pawn.story.hairColor.r, g = pawn.story.hairColor.g, b = pawn.story.hairColor.b};
            p.story.hairColor = hair;
            p.story.crownType = pawn.story.crownType;
            p.story.hairDef = pawn.story.hairDef;


            if (pawn.inventory != null && pawn.inventory.innerContainer != null && p.inventory != null && p.inventory.innerContainer != null)
                try
                {
                    pawn.inventory.innerContainer.TryTransferAllToContainer(p.inventory.innerContainer);
                }
                catch (Exception ex)
                {
                    Log.Message("[ATPP] Utils.SpawnCorpse.transfertInventory " + ex.Message + " " + ex.StackTrace);
                }

            if (pawn.equipment != null && p.equipment != null)
                foreach (var e in pawn.equipment.AllEquipmentListForReading.ToList())
                    try
                    {
                        pawn.equipment.Remove(e);
                        p.equipment.AddEquipment(e);
                    }
                    catch (Exception ex)
                    {
                        Log.Message("[ATPP] Utils.SpawnCorpse.transfertEquipment " + ex.Message + " " + ex.StackTrace);
                    }


            if (pawn.apparel != null)
                if (p.apparel != null)
                {
                    p.apparel.DestroyAll();


                    foreach (var e in pawn.apparel.WornApparel.ToList())
                    {
                        pawn.apparel.Remove(e);
                        p.apparel.Wear(e);
                    }
                }


            p.health.hediffSet.Clear();


            foreach (var h in pawn.health.hediffSet.hediffs)
                try
                {
                    h.pawn = p;
                    p.health.AddHediff(h, h.Part);
                }
                catch (Exception)
                {
                }

            GenSpawn.Spawn(p, pawn.Position, pawn.Map);


            Duplicate(pawn, p, false);


            var alienComp = TryGetCompByTypeName(pawn, "AlienComp", "AlienRace");
            if (alienComp != null)
            {
                var crownType = (string) Traverse.Create(alienComp).Field("crownType").GetValue();

                var alienComp2 = TryGetCompByTypeName(p, "AlienComp", "AlienRace");
                if (alienComp2 != null) Traverse.Create(alienComp2).Field("crownType").SetValue(crownType);
            }


            p.Drawer.renderer.graphics.ResolveAllGraphics();


            if (kill)
            {
                p.Kill(null);
                p.SetFactionDirect(Faction.OfPlayer);
            }
            else
            {
                p.relations.everSeenByPlayer = true;

                p.SetFaction(Faction.OfPlayer);


                GCATPP.connectUser(p);
            }

            return p;
        }

        public static int nbSecuritySlotsGeneratedBy(Building build)
        {
            if (build.def.defName == defNameOldSecurityServer)
                return Settings.securitySlotForOldSecurityServers;
            if (build.def.defName == defNameBasicSecurityServer)
                return Settings.securitySlotForBasicSecurityServers;

            return build.def.defName == defNameAdvancedSecurityServer ? Settings.securitySlotForAdvancedSecurityServers : 0;
        }

        public static int nbHackingSlotsGeneratedBy(Building build)
        {
            if (build.def.defName == defNameOldHackingServer)
                return Settings.hackingSlotsForOldHackingServers;
            if (build.def.defName == defNameBasicHackingServer)
                return Settings.hackingSlotsForBasicHackingServers;

            return build.def.defName == defNameAdvancedHackingServer ? Settings.hackingSlotsForAdvancedHackingServers : 0;
        }

        public static int nbSkillSlotsGeneratedBy(Building build)
        {
            if (build.def.defName == defNameOldSkillServer)
                return Settings.skillSlotsForOldSkillServers;
            if (build.def.defName == defNameBasicSkillServer)
                return Settings.skillSlotsForBasicSkillServers;

            return build.def.defName == defNameAdvancedSkillServer ? Settings.skillSlotsForAdvancedSkillServers : 0;
        }

        public static int nbHackingPointsGeneratedBy(Building build)
        {
            if (build.def.defName == defNameOldHackingServer)
                return Settings.hackingNbpGeneratedOld;
            if (build.def.defName == defNameBasicHackingServer)
                return Settings.hackingNbpGeneratedBasic;

            return build.def.defName == defNameAdvancedHackingServer ? Settings.hackingNbpGeneratedAdvanced : 0;
        }

        public static int nbSkillPointsGeneratedBy(Building build)
        {
            if (build.def.defName == defNameOldSkillServer)
                return Settings.skillNbpGeneratedOld;
            if (build.def.defName == defNameBasicSkillServer)
                return Settings.skillNbpGeneratedBasic;

            return build.def.defName == defNameAdvancedSkillServer ? Settings.skillNbpGeneratedAdvanced : 0;
        }

        public static int getNbSkillPointsPerSkill(Pawn pawn, bool isMind = false)
        {
            if (isMind)
                return Settings.nbSkillPointsPerSkillT3;
            switch (pawn.def.defName)
            {
                case T1:
                    return Settings.nbSkillPointsPerSkillT1;
                case T2:
                case TX2:
                case TX2I:
                case TX2K:
                    return Settings.nbSkillPointsPerSkillT2;
                case T3:
                case TX3:
                case TX3I:
                    return Settings.nbSkillPointsPerSkillT3;
            }

            switch (pawn.def.defName)
            {
                case T4:
                case TX4:
                case TX3I:
                    return Settings.nbSkillPointsPerSkillT4;
                case T5:
                    return Settings.nbSkillPointsPerSkillT5;
                default:
                    return 0;
            }
        }

        public static int getNbSkillPointsToIncreasePassion(Pawn pawn, bool isMind = false)
        {
            if (isMind)
                return Settings.nbSkillPointsPassionT3;
            switch (pawn.def.defName)
            {
                case T1:
                    return Settings.nbSkillPointsPassionT1;
                case T2:
                case TX2:
                case TX2I:
                case TX2K:
                    return Settings.nbSkillPointsPassionT2;
                case T3:
                case TX3:
                case TX3I:
                    return Settings.nbSkillPointsPassionT3;
                case T4:
                case TX4:
                case TX4I:
                    return Settings.nbSkillPointsPassionT4;
                case T5:
                    return Settings.nbSkillPointsPassionT5;
                default:
                    return 0;
            }
        }

        public static ThingComp TryGetCompByTypeName(ThingWithComps thing, string typeName, string assemblyName = "")
        {
            return thing.AllComps.FirstOrDefault(comp => comp.GetType().Name == typeName);
        }

        public static GameComponent TryGetGameCompByTypeName(string typeName)
        {
            return Current.Game.components.FirstOrDefault(comp => comp.GetType().Name == typeName);
        }

        public static void removeAllTraits(Pawn target)
        {
            target.story.traits.allTraits.Clear();


            if (target.def.defName == T1)
            {
                var td = DefDatabase<TraitDef>.GetNamed("SimpleMindedAndroid", false);
                Trait t = null;
                if (td != null)
                    t = new Trait(td);
                if (t != null)
                    target.story.traits.allTraits.Add(t);
            }


            notifTraitsChanged(target);
        }

        public static void applySolarFlarePolicy()
        {
            var he = DefDatabase<HediffDef>.GetNamed("ATPP_SolarFlareAndroidImpact", false);
            if (he != null) he.stages[0].capMods[0].setMax = Settings.duringSolarFlaresAndroidsShouldBeDowned ? 0.1f : 0.6f;
        }

        public static void applyLivingPlantPolicy()
        {
            foreach (var td in ExceptionAndroidList.Where(e => e != M7).Select(e => DefDatabase<ThingDef>.GetNamed(e, false)).Where(td => td?.race != null))
                if (Settings.androidsCanConsumeLivingPlants)
                    td.race.foodType = (FoodTypeFlags) 3963;
                else
                    td.race.foodType = FoodTypeFlags.OmnivoreHuman;
        }

        public static void applyT5ClothesPolicy()
        {
            try
            {
                var td = DefDatabase<ThingDef>.GetNamed("Android5Tier", false);

                if (td == null)
                    return;

                var tr = Traverse.Create(td).Field("alienRace").Field("raceRestriction").Field("onlyUseRaceRestrictedApparel");

                tr.SetValue(!Settings.allowT5ToWearClothes);
            }
            catch (Exception e)
            {
                Log.Message("[ATPP] Utils.applyT5ClothesPolicy " + e.Message + " " + e.StackTrace);
            }
        }

        public static void removeMindBlacklistedTrait(Pawn mind)
        {
            List<Trait> toDel = null;
            foreach (var t in mind.story.traits.allTraits.Where(t => BlacklistMindTraits.Contains(t.def.defName)))
            {
                if (toDel == null)
                    toDel = new List<Trait>();
                toDel.Add(t);
            }

            if (toDel == null) return;

            {
                foreach (var t in toDel)
                    mind.story.traits.allTraits.Remove(t);
            }
        }

        public static void removeSimpleMindedTrait(Pawn cpawn)
        {
            if (!Settings.removeSimpleMindedTraitOnUpload || !cpawn.story.traits.HasTrait(traitSimpleMinded)) return;

            var toDel = cpawn.story.traits.allTraits.FirstOrDefault(t => t.def == traitSimpleMinded);

            if (toDel != null) cpawn.story.traits.allTraits.Remove(toDel);
        }

        public static void addSimpleMindedTraitForT1(Pawn cpawn)
        {
            if (cpawn.def.defName == T1 && !cpawn.story.traits.HasTrait(traitSimpleMinded)) cpawn.story.traits.GainTrait(new Trait(traitSimpleMinded, 0, true));
        }

        public static void makeAndroidBatteryOverload(Pawn android)
        {
            var batteryLevel = 1.0f;

            if (android.needs.food != null)
                batteryLevel = android.needs.food.CurLevelPercentage;

            float radius = 0;

            if (android.def.defName == M7)
                radius = 12 * batteryLevel;
            else
                radius = 5 * batteryLevel;

            if (radius == 0)
                radius = 1;

            GenExplosion.DoExplosion(android.Position, android.Map, radius, DamageDefOf.Bomb, android);

            if (!android.Dead)
                android.Kill(null);
        }

        public static void clearBlankAndroid(Pawn android)
        {
            if (!android.IsBlankAndroid()) return;

            var cas = android.TryGetComp<CompAndroidState>();
            if (cas == null) return;

            cas.isBlankAndroid = false;
            var he = android.health.hediffSet.GetFirstHediffOfDef(hediffBlankAndroid);
            if (he != null)
                android.health.RemoveHediff(he);
        }

        public static bool androidIsValidPodForCharging(Pawn android)
        {
            if (!android.InBed()) return false;

            var bed = android.CurrentBed();
            if (bed == null || !ExceptionSurrogatePod.Contains(bed.def.defName) && !ExceptionSurrogateM7Pod.Contains(bed.def.defName)) return false;

            var cpt = bed.TryGetComp<CompPowerTrader>();

            return !bed.IsBrokenDown() && cpt != null && cpt.PowerOn;
        }

        public static bool androidReloadingAtChargingStation(Pawn android)
        {
            if (android.CurJobDef == null || android.CurJobDef.defName != "ATPP_GoReloadBattery") return false;

            foreach (var adjPos in android.CellsAdjacent8WayAndInside())
            {
                var thingList = adjPos.GetThingList(android.Map);
                if (thingList == null) continue;

                foreach (var t in thingList)
                    if (t is Building station && station.Faction == Faction.OfPlayer && station.def.defName == "ATPP_ReloadStation")
                        return true;
            }

            return false;
        }

        public static void changeHARCrownType(Pawn pawn, string type)
        {
            var alienComp = TryGetCompByTypeName(pawn, "AlienComp", "AlienRace");
            if (alienComp == null) return;

            var alienComp2 = TryGetCompByTypeName(pawn, "AlienComp", "AlienRace");
            if (alienComp2 != null) Traverse.Create(alienComp2).Field("crownType").SetValue(type);
        }

        public static void changeTXBodyType(Pawn cp, int hurtedLevel)
        {
            var type = "";

            switch (hurtedLevel)
            {
                case 1:
                    switch (cp.def.defName)
                    {
                        case TX2:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted12TX" : "ATPP_BodyTypeMaleHurted12TX";
                            break;
                        case TX2K:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted12KTX" : "ATPP_BodyTypeMaleHurted12KTX";
                            break;
                        case TX3:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted13TX" : "ATPP_BodyTypeMaleHurted13TX";
                            break;
                        case TX4:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted14TX" : "ATPP_BodyTypeMaleHurted14TX";
                            break;
                    }

                    break;
                case 2:
                    switch (cp.def.defName)
                    {
                        case TX2:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted22TX" : "ATPP_BodyTypeMaleHurted22TX";
                            break;
                        case TX2K:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted22KTX" : "ATPP_BodyTypeMaleHurted22KTX";
                            break;
                        case TX3:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted23TX" : "ATPP_BodyTypeMaleHurted23TX";
                            break;
                        case TX4:
                            type = cp.gender == Gender.Female ? "ATPP_BodyTypeFemaleHurted24TX" : "ATPP_BodyTypeMaleHurted24TX";
                            break;
                    }

                    break;
                default:
                    type = cp.gender == Gender.Female ? "Female" : "Male";
                    break;
            }

            var bd = DefDatabase<BodyTypeDef>.GetNamed(type, false);
            if (bd != null)
                cp.story.bodyType = bd;
        }


        public static Color getHairColor(string color)
        {
            switch (color)
            {
                case "gray":
                    return new Color(0.65f, 0.65f, 0.65f);
                case "white":
                    return new Color(0.97f, 0.97f, 0.97f);
                case "blond":
                    return new Color(0.8863f, 0.7373f, 0.4549f);
                case "ginger":
                    return new Color(0.9961f, 0.3686f, 0.1412f);
                case "auburn":
                    return new Color(0.6157f, 0.2431f, 0.0471f);
                default:
                    return new Color(0.15f, 0.15f, 0.15f);
            }
        }

        public static Color getSkinColor(string color)
        {
            switch (color)
            {
                case "verylight":
                    return new Color(0.90764f, 0.8262f, 0.63333f, 1f);
                case "light":
                    return new Color(0.89764f, 0.75262f, 0.57333f, 1f);
                case "fair":
                    return new Color(0.89803f, 0.701960f, 0.46666f, 1f);
                case "midbrown":
                    return new Color(0.79803f, 0.501960f, 0.36666f, 1f);
                case "darkbrown":
                    return new Color(0.556862f, 0.360784f, 0.219607f, 1f);
                default:
                    return new Color(0.4176f, 0.2818f, 0.182f, 1f);
            }
        }

        /*
         * Calcul énergie disponible sur le PowerNet spécifié en parametre
         */
        public static float getCurrentAvailableEnergy(PowerNet pn)
        {
            return pn.CurrentStoredEnergy() + pn.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick;
        }
    }
}