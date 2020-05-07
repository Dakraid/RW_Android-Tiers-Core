using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    [StaticConstructorOnStartup]
    internal class AndroidTiersPP : Mod
    {
        public AndroidTiersPP(ModContentPack content) : base(content)
        {
            GetSettings<Settings>();

            var assemblyPsychology = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("psychology"));
            if (assemblyPsychology != null)
            {
                Utils.PSYCHOLOGY_LOADED = true;
                Log.Message("[ATPP] Psychology found");
            }

            var assemblyHellUnit = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("helldrone"));
            if (assemblyHellUnit != null)
            {
                Utils.HELLUNIT_LOADED = true;
                Log.Message("[ATPP] HellUnit found");
            }

            var assemblySmartMedicine = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("smartmedicine"));
            if (assemblySmartMedicine != null)
            {
                Utils.SMARTMEDICINE_LOADED = true;
                Utils.smartMedicineAssembly = assemblySmartMedicine;
                Log.Message("[ATPP] SmartMedicine found");
            }

            var assemblyCE = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("combatextended"));
            if (assemblyCE != null)
            {
                Utils.CELOADED = true;
                Log.Message("[ATPP] CE found");
            }

            var assemblyMedicinePatch = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("mod medicine patch"));
            if (assemblyMedicinePatch != null)
            {
                Utils.medicinePatchAssembly = assemblyMedicinePatch;
                Utils.MEDICINEPATCH_LOADED = true;
                Log.Message("[ATPP] MEDICINE PATCH found");
            }

            var assemblyBurdsAndBees = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("fluffy_birdsandbees"));
            if (assemblyBurdsAndBees != null)
            {
                Utils.BIRDSANDBEES_LOADED = true;
                Log.Message("[ATPP] BIRDS AND BEES found");
            }

            var assemblyPrisonLabor = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("prisonlabor"));
            if (assemblyPrisonLabor != null)
            {
                Utils.PRISONLABOR_LOADED = true;
                Utils.prisonLaborAssembly = assemblyPrisonLabor;
                Log.Message("[ATPP] Prison Labor found");
            }

            var assemblySaveOurShip2 = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("shipshaveinsides"));
            if (assemblySaveOurShip2 != null)
            {
                Utils.SAVEOURSHIP2_LOADED = true;
                Utils.saveOurShip2Assembly = assemblySaveOurShip2;
                Log.Message("[ATPP] SaveOurShip2 found");
            }

            var assemblyWorkTab = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("worktab"));
            if (assemblyWorkTab != null)
            {
                Utils.WORKTAB_LOADED = true;
                Log.Message("[ATPP] WorkTab found");
            }


            var hospitalityAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("hospitality"));
            if (hospitalityAssembly != null)
            {
                Utils.HOSPITALITY_LOADED = true;
                Utils.hospitalityAssembly = hospitalityAssembly;
                Log.Message("[ATPP] Hospitality found");
            }

            var searchAndDestroyAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("searchanddestroy"));
            if (searchAndDestroyAssembly != null)
            {
                Utils.SEARCHANDDESTROY_LOADED = true;
                Utils.searchAndDestroyAssembly = searchAndDestroyAssembly;
                Log.Message("[ATPP] Search and Destroy found");
            }

            var factionDiscoveryAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("faction discovery"));
            if (factionDiscoveryAssembly != null)
            {
                Utils.FACTIONDISCOVERY_LOADED = true;
                Utils.factionDiscoveryAssembly = factionDiscoveryAssembly;
                Log.Message("[ATPP] Faction Discovery found");
            }

            var powerPP = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("power++"));
            if (powerPP != null)
            {
                Utils.POWERPP_LOADED = true;
                Utils.powerppAssembly = powerPP;
                Log.Message("[ATPP] Power++ found");
            }

            if (ModLister.HasActiveModWithName("[1.0] Android tiers - Gynoids"))
            {
                Utils.ANDROIDTIERSGYNOID_LOADED = true;
                Log.Message("[ATPP] Android Tiers Gynoids found");
            }

            var qee = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("questionableethicsenhanced"));
            if (qee != null)
            {
                Utils.QEE_LOADED = true;
                Utils.qeeAssembly = qee;
                Log.Message("[ATPP] Questionable Ethics Enhanced found");
            }

            var rimmsqol = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.ToLower().StartsWith("rimmsqol"));
            if (rimmsqol != null)
            {
                Utils.RIMMSQOL_LOADED = true;
                Log.Message("[ATPP] RIMMSQOL found");
            }
        }

        public void Save()
        {
            LoadedModManager.GetMod<AndroidTiersPP>().GetSettings<Settings>().Write();
        }

        public override string SettingsCategory()
        {
            return "Android Tiers";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }
    }
}