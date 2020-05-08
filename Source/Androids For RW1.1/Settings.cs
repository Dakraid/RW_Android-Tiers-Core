using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    public class Settings : ModSettings
    {
        public static bool keepPuppetBackstory;
        public static float percentageChanceMaleAndroidModel = 0.5f;
        public static bool allowT5ToWearClothes;
        public static int maxAndroidByPortableLWPN = 5;
        public static bool allowAutoRepaint = true;
        public static bool allowAutoRepaintForPrisoners = true;
        public static bool preventM7T5AppearingInCharacterScreen = true;

        public static int nbSkillPointsPassionT1 = 4;
        public static int nbSkillPointsPassionT2 = 5;
        public static int nbSkillPointsPassionT3 = 6;
        public static int nbSkillPointsPassionT4 = 7;
        public static int nbSkillPointsPassionT5 = 8;


        public static bool androidsAreRare;
        public static int minDaysAndroidPaintingCanRust = 35;
        public static int maxDaysAndroidPaintingCanRust = 90;

        public static float chanceGeneratedAndroidCanBePaintedOrRust = 0.15f;
        public static bool androidsCanRust = true;
        public static bool removeSimpleMindedTraitOnUpload = true;
        public static int skyCloudUploadModeForSourceMind = 2;
        public static bool allowHumanDrugsForT3PlusAndroids = true;
        public static bool allowHumanDrugsForAndroids;

        public static bool removeComfortNeedForT3T4 = true;
        public static int nbSkillPointsPerSkillT1 = 150;
        public static int nbSkillPointsPerSkillT2 = 250;
        public static int nbSkillPointsPerSkillT3 = 600;
        public static int nbSkillPointsPerSkillT4 = 1000;
        public static int nbSkillPointsPerSkillT5 = 1250;

        public static int minHoursNaniteFramework = 8;
        public static int maxHoursNaniteFramework = 48;

        public static bool basicAndroidsRandomSKills = true;
        public static int defaultSkillT1Shoot = 4;
        public static int defaultSkillT1Melee = 4;
        public static int defaultSkillT1Construction = 4;
        public static int defaultSkillT1Mining = 4;
        public static int defaultSkillT1Cooking = 4;
        public static int defaultSkillT1Plants = 4;
        public static int defaultSkillT1Animals = 4;
        public static int defaultSkillT1Crafting = 4;
        public static int defaultSkillT1Artistic;
        public static int defaultSkillT1Medical = 4;
        public static int defaultSkillT1Social;
        public static int defaultSkillT1Intellectual;

        public static int defaultSkillT2Shoot = 6;
        public static int defaultSkillT2Melee = 6;
        public static int defaultSkillT2Construction = 6;
        public static int defaultSkillT2Mining = 6;
        public static int defaultSkillT2Cooking = 6;
        public static int defaultSkillT2Plants = 6;
        public static int defaultSkillT2Animals = 6;
        public static int defaultSkillT2Crafting = 6;
        public static int defaultSkillT2Artistic;
        public static int defaultSkillT2Medical = 6;
        public static int defaultSkillT2Social;
        public static int defaultSkillT2Intellectual;


        public static int VX3MaxSurrogateControllableAtOnce = 6;
        public static int secToBootSkyCloudCore = 30;
        public static bool androidsCanConsumeLivingPlants;
        public static bool hideMenuAllowingForceEatingLivingPlants;

        public static bool notRemoveAllTraitsFromT1T2;
        public static bool notRemoveAllSkillPassionsForBasicAndroids;
        public static bool disableAbilitySkyCloudServerToTalk;
        public static int minDurationMentalBreakOfDigitisedMinds = 4;
        public static int maxDurationMentalBreakOfDigitisedMinds = 36;

        public static bool androidsCanOnlyBeHealedByCrafter;
        public static bool hideInactiveSurrogates = true;
        public static int nbHourLiteHackingDeviceAttackLastMin = 1;
        public static int nbHourLiteHackingDeviceAttackLastMax = 8;

        public static int hackingSlotsForOldHackingServers = 50;
        public static int hackingSlotsForBasicHackingServers = 100;
        public static int hackingSlotsForAdvancedHackingServers = 200;

        public static int skillSlotsForOldSkillServers = 50;
        public static int skillSlotsForBasicSkillServers = 100;
        public static int skillSlotsForAdvancedSkillServers = 200;

        public static int securitySlotForOldSecurityServers = 2;
        public static int securitySlotForBasicSecurityServers = 5;
        public static int securitySlotForAdvancedSecurityServers = 10;

        public static int skillNbpGeneratedOld = 5;
        public static int skillNbpGeneratedBasic = 10;
        public static int skillNbpGeneratedAdvanced = 20;

        public static int hackingNbpGeneratedOld = 5;
        public static int hackingNbpGeneratedBasic = 10;
        public static int hackingNbpGeneratedAdvanced = 20;

        public static int nbMoodPerAssistingMinds = 1;
        public static bool hideRemotelyControlledDeviceIcon;
        public static int powerConsumedByStoredMind = 350;
        public static bool disableLowNetworkMalusInCaravans = true;
        public static bool disableLowNetworkMalus;
        public static bool disableServersAlarm = true;
        public static bool disableServersAmbiance;
        public static bool otherFactionsCanUseSurrogate = true;
        public static int mindReplicationHours = 2;
        public static int mindUploadHour = 2;
        public static int mindPermutationHours = 4;
        public static int mindDuplicationHours = 8;
        public static int mindUploadToSkyCloudHours = 4;
        public static int mindSkyCloudMigrationHours = 3;

        public static bool duringSolarFlaresAndroidsShouldBeDowned;

        public static int defaultGeneratorMode = 1;
        public static float percentageNanitesFail = 0.08f;
        public static int wattConsumedByT1 = 100;
        public static int wattConsumedByT2 = 200;
        public static int wattConsumedByT3 = 300;
        public static int wattConsumedByT4 = 400;
        public static int wattConsumedByT5 = 500;
        public static int wattConsumedByM7 = 700;
        public static int wattConsumedByHellUnit = 350;
        public static int wattConsumedByK9 = 250;
        public static int wattConsumedByMUFF = 350;
        public static int wattConsumedByPhytomining = 200;
        public static int wattConsumedByNSolution = 250;
        public static int wattConsumedByFENNEC = 250;


        public static int ransomCostT1 = 150;
        public static int ransomCostT2 = 250;
        public static int ransomCostT3 = 350;
        public static int ransomCostT4 = 500;
        public static int ransomCostT5 = 800;

        public static bool disableSolarFlareEffect;
        public static bool disableSkyMindSecurityStuff;
        public static bool androidsCanUseOrganicMedicine;
        public static float riskSecurisedSecuritySystemGetVirus = 0.25f;
        public static float riskCryptolockerScam = 0.25f;
        public static float riskRansomwareScam = 0.25f;
        public static int nbSecExplosiveVirusTakeToExplode = 45;

        public static int nbHoursMinServerRunningHotBeforeExplode = 4;
        public static int nbHoursMaxServerRunningHotBeforeExplode = 12;

        public static int nbHoursMinSkyCloudServerRunningHotBeforeExplode = 8;
        public static int nbHoursMaxSkyCloudServerRunningHotBeforeExplode = 24;

        public static bool androidNeedToEatMore = true;
        public static float percentageOfBatteryChargedEach6Sec = 0.05f;

        public static int ransomwareMinSilverToPayForBasTrait = 500;
        public static int ransomwareMaxSilverToPayForBasTrait = 2500;

        public static int ransomwareSilverToPayToRestoreSkillPerLevel = 175;

        public static int costPlayerHack = 1200;
        public static int costPlayerHackTemp = 150;
        public static int costPlayerVirus = 500;
        public static int costPlayerExplosiveVirus = 1000;

        public static int nbSecDurationTempHack = 40;

        public static Vector2 scrollPosition = Vector2.zero;

        public static float percentageOfSurrogateInAnotherFactionGroup = 0.35f;
        public static float percentageOfSurrogateInAnotherFactionGroupMin = 0.05f;

        public static void DoSettingsWindowContents(Rect inRect)
        {
            inRect.yMin += 15f;
            inRect.yMax -= 15f;

            var defaultColumnWidth = inRect.width - 50;
            var list = new Listing_Standard {ColumnWidth = defaultColumnWidth};


            var outRect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            var scrollRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height * 12f);
            Widgets.BeginScrollView(outRect, ref scrollPosition, scrollRect);

            list.Begin(scrollRect);

            list.ButtonImage(Tex.SettingsHeader, 850, 128);
            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionGeneral".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();


            list.CheckboxLabeled("ATPP_SettingsVX0KeepBodyBackstory".Translate(), ref keepPuppetBackstory);

            if (Utils.ANDROIDTIERSGYNOID_LOADED)
            {
                list.Label("ATPP_SettingsChanceCreatedAndroidsAreMale".Translate((int) (percentageChanceMaleAndroidModel * 100)));
                percentageChanceMaleAndroidModel = list.Slider(percentageChanceMaleAndroidModel, 0.0f, 1.0f);
            }


            var prevAllowT5ToWearClothes = allowT5ToWearClothes;
            list.CheckboxLabeled("ATPP_SettingsAllowT5ToWearClothes".Translate(), ref allowT5ToWearClothes);

            if (allowT5ToWearClothes != prevAllowT5ToWearClothes) Utils.applyT5ClothesPolicy();

            if (Utils.POWERPP_LOADED)
            {
                list.Label("ATPP_SettingsMaxAndroidByPortableLWPN".Translate(maxAndroidByPortableLWPN));
                maxAndroidByPortableLWPN = (int) list.Slider(maxAndroidByPortableLWPN, 1, 100);
            }


            list.CheckboxLabeled("ATPP_SettingsAllowAutoRepaint".Translate(), ref allowAutoRepaint);

            if (allowAutoRepaint)
                list.CheckboxLabeled("ATPP_SettingsAllowAutoRepaintPrisoned".Translate(), ref allowAutoRepaintForPrisoners);

            list.CheckboxLabeled("ATPP_SettingsAndroidsAreRare".Translate(), ref androidsAreRare);

            list.Label("ATPP_SettingsChanceGeneratedAndroidCanBePaintedOrRust".Translate((int) (chanceGeneratedAndroidCanBePaintedOrRust * 100)));
            chanceGeneratedAndroidCanBePaintedOrRust = list.Slider(chanceGeneratedAndroidCanBePaintedOrRust, 0.0f, 1.0f);

            list.CheckboxLabeled("ATPP_SettingsAndroidsCanRust".Translate(), ref androidsCanRust);

            if (androidsCanRust)
            {
                list.Label("ATPP_SettingsMinDaysBeforeAndroidsPaintingCanRust".Translate(minDaysAndroidPaintingCanRust));
                minDaysAndroidPaintingCanRust = (int) list.Slider(minDaysAndroidPaintingCanRust, 1, 200);

                list.Label("ATPP_SettingsMaxDaysBeforeAndroidsPaintingCanRust".Translate(maxDaysAndroidPaintingCanRust));
                maxDaysAndroidPaintingCanRust = (int) list.Slider(maxDaysAndroidPaintingCanRust, 1, 200);

                if (maxDaysAndroidPaintingCanRust < minDaysAndroidPaintingCanRust)
                    minDaysAndroidPaintingCanRust = maxDaysAndroidPaintingCanRust;
            }

            list.CheckboxLabeled("ATPP_SettingsRemoveSimpleMindedTraitOnUpload".Translate(), ref removeSimpleMindedTraitOnUpload);

            list.CheckboxLabeled("ATPP_SettingsAllowHumanDrugsFor".Translate(), ref allowHumanDrugsForAndroids);


            list.CheckboxLabeled("ATPP_SettingsAllowHumanDrugsForT3P".Translate(), ref allowHumanDrugsForT3PlusAndroids);

            list.CheckboxLabeled("ATPP_RemoveComfortNeedForT3T4".Translate(), ref removeComfortNeedForT3T4);

            list.Label("ATPP_SettingsMinHoursNaniteBankFramework".Translate(minHoursNaniteFramework));
            minHoursNaniteFramework = (int) list.Slider(minHoursNaniteFramework, 1, 200);

            list.Label("ATPP_SettingsMaxHoursNaniteBankFramework".Translate(maxHoursNaniteFramework));
            maxHoursNaniteFramework = (int) list.Slider(maxHoursNaniteFramework, 1, 200);

            if (maxHoursNaniteFramework < minHoursNaniteFramework)
                minHoursNaniteFramework = maxHoursNaniteFramework;


            list.CheckboxLabeled("ATPP_SettingsPreventM7T5AppearingInCharacterEditor".Translate(), ref preventM7T5AppearingInCharacterScreen);

            var prevAndroidsCanConsumeLivingPlants = androidsCanConsumeLivingPlants;
            list.CheckboxLabeled("ATPP_SettingsAndroidsCanEatLivingPlants".Translate(), ref androidsCanConsumeLivingPlants);

            if (androidsCanConsumeLivingPlants != prevAndroidsCanConsumeLivingPlants) Utils.applyLivingPlantPolicy();

            if (androidsCanConsumeLivingPlants) list.CheckboxLabeled("ATPP_SettingsHideAbilityToForceEatingLivingPlants".Translate(), ref hideMenuAllowingForceEatingLivingPlants);

            list.CheckboxLabeled("ATPP_SettingsNotRemoveSkillsPassionForBasicAndroids".Translate(), ref notRemoveAllSkillPassionsForBasicAndroids);

            list.CheckboxLabeled("ATPP_SettingsNotRemoveAllTraitsOfBasicAndroids".Translate(), ref notRemoveAllTraitsFromT1T2);

            list.CheckboxLabeled("ATPP_SettingsAndroidsOnlyHealedByCrafter".Translate(), ref androidsCanOnlyBeHealedByCrafter);

            var prevHideInactiveSurrogates = hideInactiveSurrogates;
            list.CheckboxLabeled("ATPP_SettingsHideInactiveSurrogates".Translate(), ref hideInactiveSurrogates);

            if (hideRemotelyControlledDeviceIcon != prevHideInactiveSurrogates)
                if (Current.ProgramState == ProgramState.Playing)
                    Find.ColonistBar.MarkColonistsDirty();

            var prevDisableLowNetworkMalusInCaravan = disableLowNetworkMalusInCaravans;
            list.CheckboxLabeled("ATPP_SettingsDisableLowNetworkMalusInCaravans".Translate(), ref disableLowNetworkMalusInCaravans);

            if (prevDisableLowNetworkMalusInCaravan != disableLowNetworkMalusInCaravans)
                if (disableLowNetworkMalusInCaravans)
                    Utils.removeAllSlowNetworkHediff(true);


            var prevDisableLowNetworkMalus = disableLowNetworkMalus;
            list.CheckboxLabeled("ATPP_SettingsDisableLowNetworkMalus".Translate(), ref disableLowNetworkMalus);

            if (prevDisableLowNetworkMalus != disableLowNetworkMalus)
                if (disableLowNetworkMalus)
                    Utils.removeAllSlowNetworkHediff();

            list.CheckboxLabeled("ATPP_SettingsDisableServersAmbianceNoises".Translate() + "ATPP_SettingsAppliedToNextSaveLoading".Translate(), ref disableServersAmbiance);
            list.CheckboxLabeled("ATPP_SettingsDisableServersAlarm".Translate() + "ATPP_SettingsAppliedToNextSaveLoading".Translate(), ref disableServersAlarm);

            var prevDuringSolarFlaresAndroidsShouldBeDowned = duringSolarFlaresAndroidsShouldBeDowned;

            list.CheckboxLabeled("ATPP_SettingsDuringSolarFlareAndroidsShouldBeDowned".Translate(), ref duringSolarFlaresAndroidsShouldBeDowned);

            if (prevDuringSolarFlaresAndroidsShouldBeDowned != duringSolarFlaresAndroidsShouldBeDowned) Utils.applySolarFlarePolicy();

            list.CheckboxLabeled("ATPP_SettingsOtherFactionsCanUseSurrogate".Translate(), ref otherFactionsCanUseSurrogate);

            list.Label("ATPP_SettingsPercentageOfSurrogateInAnotherFactionGroupMin".Translate((int) (percentageOfSurrogateInAnotherFactionGroupMin * 100)));
            percentageOfSurrogateInAnotherFactionGroupMin = list.Slider(percentageOfSurrogateInAnotherFactionGroupMin, 0.0f, 1.0f);

            list.Label("ATPP_SettingsPercentageOfSurrogateInAnotherFactionGroup".Translate((int) (percentageOfSurrogateInAnotherFactionGroup * 100)));
            percentageOfSurrogateInAnotherFactionGroup = list.Slider(percentageOfSurrogateInAnotherFactionGroup, 0.0f, 1.0f);

            if (percentageOfSurrogateInAnotherFactionGroup < percentageOfSurrogateInAnotherFactionGroupMin)
                percentageOfSurrogateInAnotherFactionGroupMin = percentageOfSurrogateInAnotherFactionGroup;

            var buffNbSkillPointsPerSkillT1 = nbSkillPointsPerSkillT1.ToString();
            var buffNbSkillPointsPerSkillT2 = nbSkillPointsPerSkillT2.ToString();
            var buffNbSkillPointsPerSkillT3 = nbSkillPointsPerSkillT3.ToString();
            var buffNbSkillPointsPerSkillT4 = nbSkillPointsPerSkillT4.ToString();
            var buffNbSkillPointsPerSkillT5 = nbSkillPointsPerSkillT5.ToString();

            var buffNbSkillPointsPassionT1 = nbSkillPointsPassionT1.ToString();
            var buffNbSkillPointsPassionT2 = nbSkillPointsPassionT2.ToString();
            var buffNbSkillPointsPassionT3 = nbSkillPointsPassionT3.ToString();
            var buffNbSkillPointsPassionT4 = nbSkillPointsPassionT4.ToString();
            var buffNbSkillPointsPassionT5 = nbSkillPointsPassionT5.ToString();

            list.Label("ATPP_SettingsNbSkillPointsPerSkill".Translate("T1"));
            list.TextFieldNumeric(ref nbSkillPointsPerSkillT1, ref buffNbSkillPointsPerSkillT1, 1, 999999);

            list.Label("ATPP_SettingsNbSkillPointsPerSkill".Translate("T2"));
            list.TextFieldNumeric(ref nbSkillPointsPerSkillT2, ref buffNbSkillPointsPerSkillT2, 1, 999999);

            list.Label("ATPP_SettingsNbSkillPointsPerSkill".Translate("T3"));
            list.TextFieldNumeric(ref nbSkillPointsPerSkillT3, ref buffNbSkillPointsPerSkillT3, 1, 999999);

            list.Label("ATPP_SettingsNbSkillPointsPerSkill".Translate("T4"));
            list.TextFieldNumeric(ref nbSkillPointsPerSkillT4, ref buffNbSkillPointsPerSkillT4, 1, 999999);

            list.Label("ATPP_SettingsNbSkillPointsPerSkill".Translate("T5"));
            list.TextFieldNumeric(ref nbSkillPointsPerSkillT5, ref buffNbSkillPointsPerSkillT5, 1, 999999);

            list.Label("ATPP_SettingsPassionCost".Translate("T1"));
            list.TextFieldNumeric(ref nbSkillPointsPassionT1, ref buffNbSkillPointsPassionT1, 1, 999999);

            list.Label("ATPP_SettingsPassionCost".Translate("T2"));
            list.TextFieldNumeric(ref nbSkillPointsPassionT2, ref buffNbSkillPointsPassionT2, 1, 999999);

            list.Label("ATPP_SettingsPassionCost".Translate("T3"));
            list.TextFieldNumeric(ref nbSkillPointsPassionT3, ref buffNbSkillPointsPassionT3, 1, 999999);

            list.Label("ATPP_SettingsPassionCost".Translate("T4"));
            list.TextFieldNumeric(ref nbSkillPointsPassionT4, ref buffNbSkillPointsPassionT4, 1, 999999);

            list.Label("ATPP_SettingsPassionCost".Translate("T5"));
            list.TextFieldNumeric(ref nbSkillPointsPassionT5, ref buffNbSkillPointsPassionT5, 1, 999999);


            list.Label("ATPP_SettingsNbSlotAddedForSkillServers".Translate("I-100", skillSlotsForOldSkillServers));
            skillSlotsForOldSkillServers = (int) list.Slider(skillSlotsForOldSkillServers, 1, 1000);

            list.Label("ATPP_SettingsNbSlotAddedForSkillServers".Translate("I-300", skillSlotsForBasicSkillServers));
            skillSlotsForBasicSkillServers = (int) list.Slider(skillSlotsForBasicSkillServers, 1, 1000);

            list.Label("ATPP_SettingsNbSlotAddedForSkillServers".Translate("I-500", skillSlotsForAdvancedSkillServers));
            skillSlotsForAdvancedSkillServers = (int) list.Slider(skillSlotsForAdvancedSkillServers, 1, 1000);


            list.Label("ATPP_SettingsNbPointsProducedForSkillServers".Translate("I-100", skillNbpGeneratedOld));
            skillNbpGeneratedOld = (int) list.Slider(skillNbpGeneratedOld, 1, 100);

            list.Label("ATPP_SettingsNbPointsProducedForSkillServers".Translate("I-300", skillNbpGeneratedBasic));
            skillNbpGeneratedBasic = (int) list.Slider(skillNbpGeneratedBasic, 1, 100);

            list.Label("ATPP_SettingsNbPointsProducedForSkillServers".Translate("I-500", skillNbpGeneratedAdvanced));
            skillNbpGeneratedAdvanced = (int) list.Slider(skillNbpGeneratedAdvanced, 1, 100);


            //ATPP_SettingsRiskGetLittleVirusEvenWithSecurityServers*
            list.CheckboxLabeled("ATPP_SettingsDisableSolarFlareImpactOnAndroids".Translate(), ref disableSolarFlareEffect);
            list.CheckboxLabeled("ATPP_SettingsFoodGenerateLessEnergy".Translate(), ref androidNeedToEatMore);
            GUI.color = Color.yellow;
            list.Label("ATPP_SettingsDefaultAndroidGeneratorMode".Translate());
            GUI.color = Color.white;
            if (list.RadioButton_NewTemp("ATPP_SettingsDefaultAndroidGeneratorModeBiomass".Translate(), defaultGeneratorMode == 1))
                defaultGeneratorMode = 1;
            if (list.RadioButton_NewTemp("ATPP_SettingsDefaultAndroidGeneratorModeBattery".Translate(), defaultGeneratorMode == 2))
                defaultGeneratorMode = 2;

            list.Label("ATPP_SettingsPercentageOfAndroidBatteryReloadedEachXSec".Translate((int) (percentageOfBatteryChargedEach6Sec * 100)));
            percentageOfBatteryChargedEach6Sec = list.Slider(percentageOfBatteryChargedEach6Sec, 0.01f, 1.0f);

            list.Label("ATPP_SettingsNbHoursMinSkyCloudServerRunningHotBeforeExplode".Translate(nbHoursMinSkyCloudServerRunningHotBeforeExplode));
            nbHoursMinSkyCloudServerRunningHotBeforeExplode = (int) list.Slider(nbHoursMinSkyCloudServerRunningHotBeforeExplode, 1, 200);

            list.Label("ATPP_SettingsNbHoursMaxSkyCloudServerRunningHotBeforeExplode".Translate(nbHoursMaxSkyCloudServerRunningHotBeforeExplode));
            nbHoursMaxSkyCloudServerRunningHotBeforeExplode = (int) list.Slider(nbHoursMaxSkyCloudServerRunningHotBeforeExplode, 1, 200);

            if (nbHoursMaxSkyCloudServerRunningHotBeforeExplode < nbHoursMinSkyCloudServerRunningHotBeforeExplode)
                nbHoursMinSkyCloudServerRunningHotBeforeExplode = nbHoursMaxSkyCloudServerRunningHotBeforeExplode;


            list.Label("ATPP_SettingsNbHoursMinServerRunningHotBeforeExplode".Translate(nbHoursMinServerRunningHotBeforeExplode));
            nbHoursMinServerRunningHotBeforeExplode = (int) list.Slider(nbHoursMinServerRunningHotBeforeExplode, 1, 200);

            list.Label("ATPP_SettingsNbHoursMaxServerRunningHotBeforeExplode".Translate(nbHoursMaxServerRunningHotBeforeExplode));
            nbHoursMaxServerRunningHotBeforeExplode = (int) list.Slider(nbHoursMaxServerRunningHotBeforeExplode, 1, 200);

            if (nbHoursMaxServerRunningHotBeforeExplode < nbHoursMinServerRunningHotBeforeExplode)
                nbHoursMinServerRunningHotBeforeExplode = nbHoursMaxServerRunningHotBeforeExplode;

            list.GapLine();
            list.Label("ATPP_SettingsMindUploadDuration".Translate(mindUploadHour));
            mindUploadHour = (int) list.Slider(mindUploadHour, 1, 48);

            list.Label("ATPP_SettingsNanitePercentageFail".Translate((int) (percentageNanitesFail * 100)));
            percentageNanitesFail = list.Slider(percentageNanitesFail, 0.0f, 1.0f);


            var buffWattConsumedByT1 = wattConsumedByT1.ToString();
            var buffWattConsumedByT2 = wattConsumedByT2.ToString();
            var buffWattConsumedByT3 = wattConsumedByT3.ToString();
            var buffWattConsumedByT4 = wattConsumedByT4.ToString();
            var buffWattConsumedByT5 = wattConsumedByT5.ToString();
            var buffWattConsumedByM7 = wattConsumedByM7.ToString();
            var buffWattConsumedByHellUnit = wattConsumedByHellUnit.ToString();
            var buffWattConsumedByMUFF = wattConsumedByMUFF.ToString();
            var buffWattConsumedByNSolution = wattConsumedByNSolution.ToString();
            var buffWattConsumedByK9 = wattConsumedByK9.ToString();
            var buffWattConsumedByPhytomining = wattConsumedByPhytomining.ToString();
            var buffWattConsumedByFENNEC = wattConsumedByFENNEC.ToString();

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("T1"));
            list.TextFieldNumeric(ref wattConsumedByT1, ref buffWattConsumedByT1, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("T2"));
            list.TextFieldNumeric(ref wattConsumedByT2, ref buffWattConsumedByT2, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("T3"));
            list.TextFieldNumeric(ref wattConsumedByT3, ref buffWattConsumedByT3, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("T4"));
            list.TextFieldNumeric(ref wattConsumedByT4, ref buffWattConsumedByT4, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("T5"));
            list.TextFieldNumeric(ref wattConsumedByT5, ref buffWattConsumedByT5, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("M7"));
            list.TextFieldNumeric(ref wattConsumedByM7, ref buffWattConsumedByM7, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("HellDrone"));
            list.TextFieldNumeric(ref wattConsumedByHellUnit, ref buffWattConsumedByHellUnit, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("MUFF"));
            list.TextFieldNumeric(ref wattConsumedByMUFF, ref buffWattConsumedByMUFF, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("K9"));
            list.TextFieldNumeric(ref wattConsumedByK9, ref buffWattConsumedByK9, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("PhytoMining"));
            list.TextFieldNumeric(ref wattConsumedByPhytomining, ref buffWattConsumedByPhytomining, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("NSolution"));
            list.TextFieldNumeric(ref wattConsumedByNSolution, ref buffWattConsumedByNSolution, 1, 999999);

            list.Label("ATPP_SettingsCharchingStationWattIncrease".Translate("NSolution"));
            list.TextFieldNumeric(ref wattConsumedByFENNEC, ref buffWattConsumedByFENNEC, 1, 999999);


            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionSkills".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();


            list.CheckboxLabeled("ATPP_SettingsRandomSkillsMode".Translate(), ref basicAndroidsRandomSKills);

            if (!basicAndroidsRandomSKills)
            {
                GUI.color = Color.yellow;
                list.Label("T1 :");
                GUI.color = Color.white;

                //T1 default skills 
                list.Label("Shooting".Translate() + " " + defaultSkillT1Shoot + "/20");
                defaultSkillT1Shoot = (int) list.Slider(defaultSkillT1Shoot, 0, 20);

                list.Label("Melee".Translate() + " " + defaultSkillT1Melee + "/20");
                defaultSkillT1Melee = (int) list.Slider(defaultSkillT1Melee, 0, 20);

                list.Label("Construction".Translate() + " " + defaultSkillT1Construction + "/20");
                defaultSkillT1Construction = (int) list.Slider(defaultSkillT1Construction, 0, 20);

                list.Label("Mining".Translate() + " " + defaultSkillT1Mining + "/20");
                defaultSkillT1Mining = (int) list.Slider(defaultSkillT1Mining, 0, 20);

                list.Label("Cooking".Translate() + " " + defaultSkillT1Cooking + "/20");
                defaultSkillT1Cooking = (int) list.Slider(defaultSkillT1Cooking, 0, 20);

                list.Label("Plants".Translate() + " " + defaultSkillT1Plants + "/20");
                defaultSkillT1Plants = (int) list.Slider(defaultSkillT1Plants, 0, 20);

                list.Label("Animals".Translate() + " " + defaultSkillT1Animals + "/20");
                defaultSkillT1Animals = (int) list.Slider(defaultSkillT1Animals, 0, 20);

                list.Label("Crafting".Translate() + " " + defaultSkillT1Crafting + "/20");
                defaultSkillT1Crafting = (int) list.Slider(defaultSkillT1Crafting, 0, 20);

                list.Label("Artistic".Translate() + " " + defaultSkillT1Artistic + "/20");
                defaultSkillT1Artistic = (int) list.Slider(defaultSkillT1Artistic, 0, 20);

                list.Label("Medicine".Translate() + " " + defaultSkillT1Medical + "/20");
                defaultSkillT1Medical = (int) list.Slider(defaultSkillT1Medical, 0, 20);

                list.Label("Social".Translate() + " " + defaultSkillT1Social + "/20");
                defaultSkillT1Social = (int) list.Slider(defaultSkillT1Social, 0, 20);

                list.Label("Intellectual".Translate() + " " + defaultSkillT1Intellectual + "/20");
                defaultSkillT1Intellectual = (int) list.Slider(defaultSkillT1Intellectual, 0, 20);


                GUI.color = Color.yellow;
                list.Label("T2 :");
                GUI.color = Color.white;

                list.Label("Shooting".Translate() + " " + defaultSkillT2Shoot + "/20");
                defaultSkillT2Shoot = (int) list.Slider(defaultSkillT2Shoot, 0, 20);

                list.Label("Melee".Translate() + " " + defaultSkillT2Melee + "/20");
                defaultSkillT2Melee = (int) list.Slider(defaultSkillT2Melee, 0, 20);

                list.Label("Construction".Translate() + " " + defaultSkillT2Construction + "/20");
                defaultSkillT2Construction = (int) list.Slider(defaultSkillT2Construction, 0, 20);

                list.Label("Mining".Translate() + " " + defaultSkillT2Mining + "/20");
                defaultSkillT2Mining = (int) list.Slider(defaultSkillT2Mining, 0, 20);

                list.Label("Cooking".Translate() + " " + defaultSkillT2Cooking + "/20");
                defaultSkillT2Cooking = (int) list.Slider(defaultSkillT2Cooking, 0, 20);

                list.Label("Plants".Translate() + " " + defaultSkillT2Plants + "/20");
                defaultSkillT2Plants = (int) list.Slider(defaultSkillT2Plants, 0, 20);

                list.Label("Animals".Translate() + " " + defaultSkillT2Animals + "/20");
                defaultSkillT2Animals = (int) list.Slider(defaultSkillT2Animals, 0, 20);

                list.Label("Crafting".Translate() + " " + defaultSkillT2Crafting + "/20");
                defaultSkillT2Crafting = (int) list.Slider(defaultSkillT2Crafting, 0, 20);

                list.Label("Artistic".Translate() + " " + defaultSkillT2Artistic + "/20");
                defaultSkillT2Artistic = (int) list.Slider(defaultSkillT2Artistic, 0, 20);

                list.Label("Medicine".Translate() + " " + defaultSkillT2Medical + "/20");
                defaultSkillT2Medical = (int) list.Slider(defaultSkillT2Medical, 0, 20);

                list.Label("Social".Translate() + " " + defaultSkillT2Social + "/20");
                defaultSkillT2Social = (int) list.Slider(defaultSkillT2Social, 0, 20);

                list.Label("Intellectual".Translate() + " " + defaultSkillT2Intellectual + "/20");
                defaultSkillT2Intellectual = (int) list.Slider(defaultSkillT2Intellectual, 0, 20);
            }


            list.Gap(3);
            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionSkyCloud".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();

            GUI.color = Color.yellow;
            list.Label("ATPP_SettingsUploadMindToSkyCloudCoreMode".Translate());
            GUI.color = Color.white;
            if (list.RadioButton_NewTemp("ATTP_SettingsUploadMindToSkyCloudCoreModeNothing".Translate(), skyCloudUploadModeForSourceMind == 0))
                skyCloudUploadModeForSourceMind = 0;
            if (list.RadioButton_NewTemp("ATTP_SettingsUploadMindToSkyCloudCoreModeVX0".Translate(), skyCloudUploadModeForSourceMind == 1))
                skyCloudUploadModeForSourceMind = 1;
            if (list.RadioButton_NewTemp("ATTP_SettingsUploadMindToSkyCloudCoreModeLethal".Translate(), skyCloudUploadModeForSourceMind == 2))
                skyCloudUploadModeForSourceMind = 2;

            list.Gap();

            list.Label("ATPP_SettingsMindUploadToSkyCloudDuration".Translate(mindUploadToSkyCloudHours));
            mindUploadToSkyCloudHours = (int) list.Slider(mindUploadToSkyCloudHours, 1, 72);

            list.Label("ATPP_SettingsSecToBootASkyCloudCore".Translate(secToBootSkyCloudCore));
            secToBootSkyCloudCore = (int) list.Slider(secToBootSkyCloudCore, 1, 300);

            list.Label("ATPP_SettingsMinDurationOfMindMentalBreak".Translate(minDurationMentalBreakOfDigitisedMinds));
            minDurationMentalBreakOfDigitisedMinds = (int) list.Slider(minDurationMentalBreakOfDigitisedMinds, 1, 100);

            list.Label("ATPP_SettingsMaxDurationOfMindMentalBreak".Translate(maxDurationMentalBreakOfDigitisedMinds));
            maxDurationMentalBreakOfDigitisedMinds = (int) list.Slider(maxDurationMentalBreakOfDigitisedMinds, 1, 100);

            if (maxDurationMentalBreakOfDigitisedMinds < minDurationMentalBreakOfDigitisedMinds)
                minDurationMentalBreakOfDigitisedMinds = maxDurationMentalBreakOfDigitisedMinds;

            list.Label("ATPP_SettingsMindReplicationDuration".Translate(mindReplicationHours));
            mindReplicationHours = (int) list.Slider(mindReplicationHours, 1, 72);

            //ATPP_SettingsSkyCloudBuffPerAssistingMind
            list.Label("ATPP_SettingsSkyCloudBuffPerAssistingMind".Translate(nbMoodPerAssistingMinds));
            nbMoodPerAssistingMinds = (int) list.Slider(nbMoodPerAssistingMinds, 1, 20);

            list.CheckboxLabeled("ATPP_SettingsHideRemotelyControlledIcon".Translate(), ref hideRemotelyControlledDeviceIcon);

            var buffPowerConsumedByStoredMind = powerConsumedByStoredMind.ToString();

            list.Label("ATPP_SettingsPowerConsumedByStoredMind".Translate());
            list.TextFieldNumeric(ref powerConsumedByStoredMind, ref buffPowerConsumedByStoredMind, 1, 999999);

            list.CheckboxLabeled("ATPP_SettingsDisableAbilityOfSkyCloudServersToTalk".Translate(), ref disableAbilitySkyCloudServerToTalk);

            list.Gap(3);
            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionSecurity".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();


            list.CheckboxLabeled("ATPP_SettingsDisableSecurityStuff".Translate(), ref disableSkyMindSecurityStuff);

            if (!disableSkyMindSecurityStuff)
            {
                list.Label("ATPP_SettingsMindMigrationCloudDuration".Translate(mindSkyCloudMigrationHours));
                mindSkyCloudMigrationHours = (int) list.Slider(mindSkyCloudMigrationHours, 1, 72);

                list.Label("ATPP_SettingsMinNbHoursLiteHackingDeviceAttackLast".Translate(nbHourLiteHackingDeviceAttackLastMin));
                nbHourLiteHackingDeviceAttackLastMin = (int) list.Slider(nbHourLiteHackingDeviceAttackLastMin, 1, 100);

                list.Label("ATPP_SettingsMaxNbHoursLiteHackingDeviceAttackLast".Translate(nbHourLiteHackingDeviceAttackLastMax));
                nbHourLiteHackingDeviceAttackLastMax = (int) list.Slider(nbHourLiteHackingDeviceAttackLastMax, 1, 100);

                var buffCostPlayerVirus = costPlayerVirus.ToString();
                var buffCostPlayerExplosiveVirus = costPlayerExplosiveVirus.ToString();
                var buffCostPlayerHackTemp = costPlayerHackTemp.ToString();
                var buffCostPlayerHack = costPlayerHack.ToString();

                list.Label("ATPP_SettingsCostHackingPoints".Translate("ATPP_SettingsCostVirus".Translate()));
                list.TextFieldNumeric(ref costPlayerVirus, ref buffCostPlayerVirus, 1, 999999);

                list.Label("ATPP_SettingsCostHackingPoints".Translate("ATPP_SettingsCostVirusExplosive".Translate()));
                list.TextFieldNumeric(ref costPlayerExplosiveVirus, ref buffCostPlayerExplosiveVirus, 1, 999999);

                list.Label("ATPP_SettingsCostHackingPoints".Translate("ATPP_SettingsCostHackTemp".Translate()));
                list.TextFieldNumeric(ref costPlayerHackTemp, ref buffCostPlayerHackTemp, 1, 999999);

                list.Label("ATPP_SettingsCostHackingPoints".Translate("ATPP_SettingsCostHack".Translate()));
                list.TextFieldNumeric(ref costPlayerHack, ref buffCostPlayerHack, 1, 999999);

                //costPlayerVirus

                list.Label("ATPP_SettingsTempHackDuration".Translate(nbSecDurationTempHack * 100));
                nbSecDurationTempHack = (int) list.Slider(nbSecDurationTempHack, 1, 600);

                var buffRansomwareMinSilverToPayForBasTrait = ransomwareMinSilverToPayForBasTrait.ToString();
                var buffRansomwareMaxSilverToPayForBasTrait = ransomwareMaxSilverToPayForBasTrait.ToString();
                var buffRansomwareSilverToPayToRestoreSkillPerLevel = ransomwareSilverToPayToRestoreSkillPerLevel.ToString();

                list.Label("ATPP_SettingsMinSilverToRemoveBadTrait".Translate());
                list.TextFieldNumeric(ref ransomwareMinSilverToPayForBasTrait, ref buffRansomwareMinSilverToPayForBasTrait, 1, 999999);

                list.Label("ATPP_SettingsMaxSilverToRemoveBadTrait".Translate());
                list.TextFieldNumeric(ref ransomwareMaxSilverToPayForBasTrait, ref buffRansomwareMaxSilverToPayForBasTrait, 1, 999999);

                list.Label("ATPP_SettingsRansomwareSilverPerSkillLevel".Translate());
                list.TextFieldNumeric(ref ransomwareSilverToPayToRestoreSkillPerLevel, ref buffRansomwareSilverToPayToRestoreSkillPerLevel, 1, 999999);


                list.Label("ATPP_SettingsRiskFactionNotRemoveRansomwareEffect".Translate((int) (riskRansomwareScam * 100)));
                riskRansomwareScam = list.Slider(riskRansomwareScam, 0.01f, 1.0f);

                list.Label("ATPP_SettingsRiskGetLittleVirusEvenWithSecurityServers".Translate((int) (riskSecurisedSecuritySystemGetVirus * 100)));
                riskSecurisedSecuritySystemGetVirus = list.Slider(riskSecurisedSecuritySystemGetVirus, 0.0f, 1.0f);

                var buffRansomCostT1 = ransomCostT1.ToString();
                var buffRansomCostT2 = ransomCostT2.ToString();
                var buffRansomCostT3 = ransomCostT3.ToString();
                var buffRansomCostT4 = ransomCostT4.ToString();
                var buffRansomCostT5 = ransomCostT5.ToString();

                list.Label("ATPP_SettingsRansomCost".Translate("T1"));
                list.TextFieldNumeric(ref ransomCostT1, ref buffRansomCostT1, 1, 999999);

                list.Label("ATPP_SettingsRansomCost".Translate("T2"));
                list.TextFieldNumeric(ref ransomCostT2, ref buffRansomCostT2, 1, 999999);

                list.Label("ATPP_SettingsRansomCost".Translate("T3"));
                list.TextFieldNumeric(ref ransomCostT3, ref buffRansomCostT3, 1, 999999);

                list.Label("ATPP_SettingsRansomCost".Translate("T4"));
                list.TextFieldNumeric(ref ransomCostT4, ref buffRansomCostT4, 1, 999999);

                list.Label("ATPP_SettingsRansomCost".Translate("T5"));
                list.TextFieldNumeric(ref ransomCostT5, ref buffRansomCostT5, 1, 999999);


                list.Label("ATPP_SettingsRiskFactionNotRemoveCryptolocker".Translate((int) (riskCryptolockerScam * 100)));
                riskCryptolockerScam = list.Slider(riskCryptolockerScam, 0.01f, 1.0f);


                list.CheckboxLabeled("ATPP_SettingsAndroidCanUseOrganicMedicine".Translate(), ref androidsCanUseOrganicMedicine);

                //ATPP_SettingsDelayExplosiveVirus
                list.Label("ATPP_SettingsDelayExplosiveVirus".Translate(nbSecExplosiveVirusTakeToExplode));
                nbSecExplosiveVirusTakeToExplode = (int) list.Slider(nbSecExplosiveVirusTakeToExplode, 1, 240);

                list.Label("ATPP_SettingsNbSlotAddedForSecurityServers".Translate("I-100", securitySlotForOldSecurityServers));
                securitySlotForOldSecurityServers = (int) list.Slider(securitySlotForOldSecurityServers, 1, 100);

                list.Label("ATPP_SettingsNbSlotAddedForSecurityServers".Translate("I-300", securitySlotForBasicSecurityServers));
                securitySlotForBasicSecurityServers = (int) list.Slider(securitySlotForBasicSecurityServers, 1, 100);

                list.Label("ATPP_SettingsNbSlotAddedForSecurityServers".Translate("I-500", securitySlotForAdvancedSecurityServers));
                securitySlotForAdvancedSecurityServers = (int) list.Slider(securitySlotForAdvancedSecurityServers, 1, 100);


                list.Label("ATPP_SettingsNbSlotAddedForHackingServers".Translate("I-100", hackingSlotsForOldHackingServers));
                hackingSlotsForOldHackingServers = (int) list.Slider(hackingSlotsForOldHackingServers, 1, 1000);

                list.Label("ATPP_SettingsNbSlotAddedForHackingServers".Translate("I-300", hackingSlotsForBasicHackingServers));
                hackingSlotsForBasicHackingServers = (int) list.Slider(hackingSlotsForBasicHackingServers, 1, 1000);

                list.Label("ATPP_SettingsNbSlotAddedForHackingServers".Translate("I-500", hackingSlotsForAdvancedHackingServers));
                hackingSlotsForAdvancedHackingServers = (int) list.Slider(hackingSlotsForAdvancedHackingServers, 1, 1000);


                list.Label("ATPP_SettingsNbPointsProducedForHackingServers".Translate("I-100", hackingNbpGeneratedOld));
                hackingNbpGeneratedOld = (int) list.Slider(hackingNbpGeneratedOld, 1, 100);

                list.Label("ATPP_SettingsNbPointsProducedForHackingServers".Translate("I-300", hackingNbpGeneratedBasic));
                hackingNbpGeneratedBasic = (int) list.Slider(hackingNbpGeneratedBasic, 1, 100);

                list.Label("ATPP_SettingsNbPointsProducedForHackingServers".Translate("I-500", hackingNbpGeneratedAdvanced));
                hackingNbpGeneratedAdvanced = (int) list.Slider(hackingNbpGeneratedAdvanced, 1, 100);
            }

            list.Gap(3);
            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionVX3".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();

            list.Label("ATPP_SettingsVX3MaxSurrogateControllableAtOnce".Translate(VX3MaxSurrogateControllableAtOnce));
            VX3MaxSurrogateControllableAtOnce = (int) list.Slider(VX3MaxSurrogateControllableAtOnce, 1, 50);

            list.Gap(3);
            list.GapLine();
            list.Gap(10);
            GUI.color = Color.cyan;
            list.Label("ATPP_SettingsSectionVX2".Translate());
            GUI.color = Color.white;
            list.Gap(10);
            list.GapLine();


            list.Label("ATPP_SettingsMindPermutationDuration".Translate(mindPermutationHours));
            mindPermutationHours = (int) list.Slider(mindPermutationHours, 1, 72);

            list.Label("ATPP_SettingsMindDuplicationDuration".Translate(mindDuplicationHours));
            mindDuplicationHours = (int) list.Slider(mindDuplicationHours, 1, 72);


            list.End();
            Widgets.EndScrollView();
            //settings.Write();
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref androidsCanOnlyBeHealedByCrafter, "androidsCanOnlyBeHealedByCrafter");
            Scribe_Values.Look(ref mindSkyCloudMigrationHours, "mindSkyCloudMigrationHours", 4);
            Scribe_Values.Look(ref mindUploadHour, "mindUploadHour", 3);
            Scribe_Values.Look(ref mindReplicationHours, "mindReplicationHours", 2);
            Scribe_Values.Look(ref mindDuplicationHours, "mindDuplicationHours", 8);
            Scribe_Values.Look(ref mindUploadToSkyCloudHours, "mindUploadToSkyCloudHours", 8);
            Scribe_Values.Look(ref mindPermutationHours, "mindPermutationHours", 4);

            Scribe_Values.Look(ref defaultGeneratorMode, "defaultGeneratorMode", 1);
            Scribe_Values.Look(ref percentageNanitesFail, "percentageNanitesFail", 0.08f);
            Scribe_Values.Look(ref percentageOfBatteryChargedEach6Sec, "percentageOfBatteryChargedEach6Sec", 0.05f);

            Scribe_Values.Look(ref wattConsumedByT1, "wattConsumedByT1", 90);
            Scribe_Values.Look(ref wattConsumedByT2, "wattConsumedByT2", 150);
            Scribe_Values.Look(ref wattConsumedByT3, "wattConsumedByT3", 200);
            Scribe_Values.Look(ref wattConsumedByT4, "wattConsumedByT4", 250);
            Scribe_Values.Look(ref wattConsumedByT5, "wattConsumedByT5", 350);
            Scribe_Values.Look(ref wattConsumedByM7, "wattConsumedByM7", 500);
            Scribe_Values.Look(ref wattConsumedByHellUnit, "wattConsumedByHellDrone", 350);
            Scribe_Values.Look(ref wattConsumedByK9, "wattConsumedByK9", 250);
            Scribe_Values.Look(ref wattConsumedByPhytomining, "wattConsumedByPhytomining", 200);
            Scribe_Values.Look(ref wattConsumedByNSolution, "wattConsumedByNSolution", 250);
            Scribe_Values.Look(ref wattConsumedByMUFF, "wattConsumedByMUFF", 350);
            Scribe_Values.Look(ref wattConsumedByFENNEC, "wattConsumedByFENNEC", 250);


            Scribe_Values.Look(ref nbHoursMaxServerRunningHotBeforeExplode, "nbHoursMaxServerRunningHotBeforeExplode", 4);
            Scribe_Values.Look(ref nbHoursMinServerRunningHotBeforeExplode, "nbHoursMinServerRunningHotBeforeExplode", 12);

            Scribe_Values.Look(ref nbHoursMaxSkyCloudServerRunningHotBeforeExplode, "nbHoursMaxSkyCloudServerRunningHotBeforeExplode", 12);
            Scribe_Values.Look(ref nbHoursMinSkyCloudServerRunningHotBeforeExplode, "nbHoursMinSkyCloudServerRunningHotBeforeExplode", 48);

            Scribe_Values.Look(ref riskSecurisedSecuritySystemGetVirus, "riskSecurisedSecuritySystemGetVirus", 0.25f);

            Scribe_Values.Look(ref androidNeedToEatMore, "androidNeedToEatMore", true);
            Scribe_Values.Look(ref androidsCanUseOrganicMedicine, "androidsCanUseOrganicMedicine");

            Scribe_Values.Look(ref nbSecExplosiveVirusTakeToExplode, "nbSecExplosiveVirusTakeToExplode", 45);
            Scribe_Values.Look(ref riskCryptolockerScam, "riskCryptolockerScam", 0.25f);
            Scribe_Values.Look(ref riskRansomwareScam, "riskRansomwareScam", 0.25f);
            Scribe_Values.Look(ref percentageOfSurrogateInAnotherFactionGroup, "percentageOfSurrogateInAnotherFactionGroup", 0.35f);
            Scribe_Values.Look(ref percentageOfSurrogateInAnotherFactionGroupMin, "percentageOfSurrogateInAnotherFactionGroupMin", 0.05f);

            Scribe_Values.Look(ref otherFactionsCanUseSurrogate, "otherFactionsCanUseSurrogate", true);

            Scribe_Values.Look(ref ransomCostT1, "ransomCostT1", 150);
            Scribe_Values.Look(ref ransomCostT2, "ransomCostT2", 250);
            Scribe_Values.Look(ref ransomCostT3, "ransomCostT3", 350);
            Scribe_Values.Look(ref ransomCostT4, "ransomCostT4", 500);
            Scribe_Values.Look(ref ransomCostT5, "ransomCostT5", 800);

            Scribe_Values.Look(ref costPlayerExplosiveVirus, "costPlayerExplosiveVirus", 1000);
            Scribe_Values.Look(ref costPlayerVirus, "costPlayerVirus", 500);
            Scribe_Values.Look(ref costPlayerHack, "costPlayerHack", 1500);
            Scribe_Values.Look(ref costPlayerHackTemp, "costPlayerHackTemp", 150);

            Scribe_Values.Look(ref nbSecDurationTempHack, "nbSecDurationTempHack", 40);

            Scribe_Values.Look(ref duringSolarFlaresAndroidsShouldBeDowned, "duringSolarFlaresAndroidsShouldBeDowned");

            Scribe_Values.Look(ref disableServersAlarm, "disableServersAlarm", true);
            Scribe_Values.Look(ref disableServersAmbiance, "disableServersAmbiance");
            Scribe_Values.Look(ref disableLowNetworkMalus, "disableLowNetworkMalus");
            Scribe_Values.Look(ref disableLowNetworkMalusInCaravans, "disableLowNetworkMalusInCaravans", true);
            Scribe_Values.Look(ref hideRemotelyControlledDeviceIcon, "hideRemotelyControlledDeviceIcon");
            Scribe_Values.Look(ref nbMoodPerAssistingMinds, "nbMoodPerAssistingMinds", 1);

            Scribe_Values.Look(ref securitySlotForOldSecurityServers, "securitySlotForOldSecurityServers", 2);
            Scribe_Values.Look(ref securitySlotForBasicSecurityServers, "securitySlotForBasicSecurityServers", 5);
            Scribe_Values.Look(ref securitySlotForAdvancedSecurityServers, "securitySlotForAdvancedSecurityServers", 10);

            Scribe_Values.Look(ref hackingSlotsForOldHackingServers, "hackingSlotsForOldHackingServers", 50);
            Scribe_Values.Look(ref hackingSlotsForBasicHackingServers, "hackingSlotsForBasicHackingServers", 100);
            Scribe_Values.Look(ref hackingSlotsForAdvancedHackingServers, "hackingSlotsForAdvancedHackingServers", 200);

            Scribe_Values.Look(ref nbHourLiteHackingDeviceAttackLastMin, "nbHourLiteHackingDeviceAttackLastMin", 1);
            Scribe_Values.Look(ref nbHourLiteHackingDeviceAttackLastMax, "nbHourLiteHackingDeviceAttackLastMax", 8);

            Scribe_Values.Look(ref hideInactiveSurrogates, "hideInactiveSurrogates", true);

            Scribe_Values.Look(ref minDurationMentalBreakOfDigitisedMinds, "minDurationMentalBreakOfDigitisedMinds", 4);
            Scribe_Values.Look(ref maxDurationMentalBreakOfDigitisedMinds, "maxDurationMentalBreakOfDigitisedMinds", 36);

            Scribe_Values.Look(ref secToBootSkyCloudCore, "secToBootSkyCloudCore", 30);
            Scribe_Values.Look(ref powerConsumedByStoredMind, "powerConsumedByStoredMind", 350);
            Scribe_Values.Look(ref disableAbilitySkyCloudServerToTalk, "disableAbilitySkyCloudServerToTalk");

            Scribe_Values.Look(ref notRemoveAllTraitsFromT1T2, "notRemoveAllTraitsFromT1T2");

            Scribe_Values.Look(ref androidsCanConsumeLivingPlants, "androidsCanConsumeLivingPlants", true);
            Scribe_Values.Look(ref hideMenuAllowingForceEatingLivingPlants, "hideMenuAllowingForceEatingLivingPlants");

            Scribe_Values.Look(ref notRemoveAllSkillPassionsForBasicAndroids, "notRemoveAllSkillPassionsForBasicAndroids");


            Scribe_Values.Look(ref VX3MaxSurrogateControllableAtOnce, "VX3MaxSurrogateControllableAtOnce", 6);

            Scribe_Values.Look(ref defaultSkillT1Animals, "defaultSkillT1Animals", 4);
            Scribe_Values.Look(ref defaultSkillT1Artistic, "defaultSkillT1Artistic");
            Scribe_Values.Look(ref defaultSkillT1Construction, "defaultSkillT1Construction", 4);
            Scribe_Values.Look(ref defaultSkillT1Cooking, "defaultSkillT1Cooking", 4);
            Scribe_Values.Look(ref defaultSkillT1Crafting, "defaultSkillT1Crafting", 4);
            Scribe_Values.Look(ref defaultSkillT1Intellectual, "defaultSkillT1Intellectual");
            Scribe_Values.Look(ref defaultSkillT1Medical, "defaultSkillT1Medical", 4);
            Scribe_Values.Look(ref defaultSkillT1Melee, "defaultSkillT1Melee", 4);
            Scribe_Values.Look(ref defaultSkillT1Mining, "defaultSkillT1Mining", 4);
            Scribe_Values.Look(ref defaultSkillT1Plants, "defaultSkillT1Plants", 4);
            Scribe_Values.Look(ref defaultSkillT1Shoot, "defaultSkillT1Shoot", 4);
            Scribe_Values.Look(ref defaultSkillT1Social, "defaultSkillT1Social");


            Scribe_Values.Look(ref defaultSkillT2Animals, "defaultSkillT2Animals", 6);
            Scribe_Values.Look(ref defaultSkillT2Artistic, "defaultSkillT2Artistic");
            Scribe_Values.Look(ref defaultSkillT2Construction, "defaultSkillT2Construction", 6);
            Scribe_Values.Look(ref defaultSkillT2Cooking, "defaultSkillT2Cooking", 6);
            Scribe_Values.Look(ref defaultSkillT2Crafting, "defaultSkillT2Crafting", 6);
            Scribe_Values.Look(ref defaultSkillT2Intellectual, "defaultSkillT2Intellectual");
            Scribe_Values.Look(ref defaultSkillT2Medical, "defaultSkillT2Medical", 6);
            Scribe_Values.Look(ref defaultSkillT2Melee, "defaultSkillT2Melee", 6);
            Scribe_Values.Look(ref defaultSkillT2Mining, "defaultSkillT2Mining", 6);
            Scribe_Values.Look(ref defaultSkillT2Plants, "defaultSkillT2Plants", 6);
            Scribe_Values.Look(ref defaultSkillT2Shoot, "defaultSkillT2Shoot", 6);
            Scribe_Values.Look(ref defaultSkillT2Social, "defaultSkillT2Social");

            Scribe_Values.Look(ref skillNbpGeneratedOld, "skillNbpGeneratedOld", 5);
            Scribe_Values.Look(ref skillNbpGeneratedBasic, "skillNbpGeneratedOld", 10);
            Scribe_Values.Look(ref skillNbpGeneratedAdvanced, "skillNbpGeneratedOld", 20);

            Scribe_Values.Look(ref hackingNbpGeneratedOld, "hackingNbpGeneratedOld", 5);
            Scribe_Values.Look(ref hackingNbpGeneratedBasic, "hackingNbpGeneratedOld", 10);
            Scribe_Values.Look(ref hackingNbpGeneratedAdvanced, "hackingNbpGeneratedOld", 20);

            Scribe_Values.Look(ref preventM7T5AppearingInCharacterScreen, "preventM7T5AppearingInCharacterScreen", true);

            Scribe_Values.Look(ref minHoursNaniteFramework, "minHoursNaniteFramework", 8);
            Scribe_Values.Look(ref maxHoursNaniteFramework, "maxHoursNaniteFramework", 48);

            Scribe_Values.Look(ref nbSkillPointsPerSkillT1, "nbSkillPointsPerSkillT1", 150);
            Scribe_Values.Look(ref nbSkillPointsPerSkillT2, "nbSkillPointsPerSkillT2", 250);
            Scribe_Values.Look(ref nbSkillPointsPerSkillT3, "nbSkillPointsPerSkillT3", 600);
            Scribe_Values.Look(ref nbSkillPointsPerSkillT4, "nbSkillPointsPerSkillT4", 1000);
            Scribe_Values.Look(ref nbSkillPointsPerSkillT5, "nbSkillPointsPerSkillT5", 1250);

            Scribe_Values.Look(ref removeComfortNeedForT3T4, "removeComfortNeedForT3T4", true);

            Scribe_Values.Look(ref allowHumanDrugsForT3PlusAndroids, "allowHumanDrugsForT3PlusAndroids", true);

            Scribe_Values.Look(ref skyCloudUploadModeForSourceMind, "skyCloudUploadModeForSourceMind", 2);
            Scribe_Values.Look(ref allowHumanDrugsForAndroids, "allowHumanDrugsForAndroids");

            Scribe_Values.Look(ref removeSimpleMindedTraitOnUpload, "removeSimpleMindedTraitOnUpload", true);

            Scribe_Values.Look(ref minDaysAndroidPaintingCanRust, "minDaysAndroidPaintingCanRust", 15);
            Scribe_Values.Look(ref maxDaysAndroidPaintingCanRust, "maxDaysAndroidPaintingCanRust", 45);
            Scribe_Values.Look(ref androidsCanRust, "androidsCanRust", true);

            Scribe_Values.Look(ref chanceGeneratedAndroidCanBePaintedOrRust, "chanceGeneratedAndroidCanBePaintedOrRust", 0.45f);

            Scribe_Values.Look(ref basicAndroidsRandomSKills, "basicAndroidsRandomSKills");

            Scribe_Values.Look(ref androidsAreRare, "androidsAreRare");
            Scribe_Values.Look(ref allowAutoRepaint, "allowAutoRepaint", true);
            Scribe_Values.Look(ref allowAutoRepaintForPrisoners, "allowAutoRepaintForPrisoners", true);

            Scribe_Values.Look(ref percentageChanceMaleAndroidModel, "percentageChanceMaleAndroidModel", 0.5f);


            Scribe_Values.Look(ref allowT5ToWearClothes, "allowT5ToWearClothes", true);
            Scribe_Values.Look(ref maxAndroidByPortableLWPN, "maxAndroidByPortableLWPN", 5);

            Scribe_Values.Look(ref skillSlotsForAdvancedSkillServers, "skillSlotsForAdvancedSkillServers", 200);
            Scribe_Values.Look(ref skillSlotsForBasicSkillServers, "skillSlotsForBasicSkillServers", 100);
            Scribe_Values.Look(ref skillSlotsForOldSkillServers, "skillSlotsForOldSkillServers", 50);

            Scribe_Values.Look(ref nbSkillPointsPassionT1, "nbSkillPointsPassionT1", 4);
            Scribe_Values.Look(ref nbSkillPointsPassionT2, "nbSkillPointsPassionT2", 5);
            Scribe_Values.Look(ref nbSkillPointsPassionT3, "nbSkillPointsPassionT3", 6);
            Scribe_Values.Look(ref nbSkillPointsPassionT4, "nbSkillPointsPassionT4", 7);
            Scribe_Values.Look(ref nbSkillPointsPassionT5, "nbSkillPointsPassionT5", 8);

            Scribe_Values.Look(ref keepPuppetBackstory, "keepPuppetBackstory");
        }
    }
}