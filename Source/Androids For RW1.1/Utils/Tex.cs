using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    [StaticConstructorOnStartup]
    internal static class Tex
    {
        public static readonly Texture2D Battery = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_Battery");


        public static readonly Texture2D TexUISkillLogo = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SkillUILogo");
        public static readonly Texture2D NoCare = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_NoCare");
        public static readonly Texture2D NoMed = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_OnlyDocVisit");
        public static readonly Texture2D NanoKitBasic = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_NanoKitBasic/ATPP_NanoKitBasic_a");
        public static readonly Texture2D NanoKitIntermediate = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_NanoKitIntermediate/ATPP_NanoKitIntermediate_a");
        public static readonly Texture2D NanoKitAdvanced = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_NanoKitAdvanced/ATPP_NanoKitAdvanced_a");

        public static readonly Texture2D UploadConsciousness = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_UploadConsciousness");
        public static readonly Texture2D Permute = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_Permute");
        public static readonly Texture2D PermuteDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_PermuteDisabled");
        public static readonly Texture2D Duplicate = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_Duplicate");
        public static readonly Texture2D DuplicateDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_DuplicateDisabled");
        public static readonly Texture2D UploadConsciousnessDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_UploadConsciousnessDisabled");
        public static readonly Texture2D UploadToSkyCloud = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_UploadToSkyCloud");
        public static readonly Texture2D UploadToSkyCloudDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_UploadToSkyCloudDisabled");
        public static readonly Texture2D DownloadFromSkyCloud = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_DownloadFromSkyCloud");
        public static readonly Texture2D DownloadFromSkyCloudDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_DownloadFromSkyCloudDisabled");

        public static readonly Texture2D MindAbsorption = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_MindAbsorption");
        public static readonly Texture2D MindAbsorptionDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_MindAbsorptionDisabled");


        public static readonly Texture2D RepairAndroid = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_RepairAndroids");


        public static readonly Texture2D PassionDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_PassionMinorGrayDisabled");
        public static readonly Texture2D NoPassion = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_PassionMinorGray");
        public static readonly Texture2D MinorPassion = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_PassionMinor");
        public static readonly Texture2D MajorPassion = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_PassionMajor");


        public static readonly Texture2D texAutoDoorClose = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_CloseDoor");
        public static readonly Texture2D texAutoDoorOpen = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_OpenDoor");

        public static readonly Texture2D processInfo = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessInfo");
        public static readonly Texture2D processRemove = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessRemove");
        public static readonly Texture2D processDuplicate = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessDuplicate");
        public static readonly Texture2D processAssist = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessAssist");
        public static readonly Texture2D processMigrate = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessMigrate");
        public static readonly Texture2D processSkillUp = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ProcessSkillUp");


        public static readonly Texture2D SurrogateMode = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SurrogateMode");
        public static readonly Texture2D SkyMindConn = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SkyMind");
        public static readonly Texture2D SkyMindAutoConn = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SkyMindAutoReconnect");
        public static readonly Texture2D SkillUp = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SkillUp");

        public static readonly Texture2D ForceAndroidToExplode = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_OverloadAndroid");
        public static readonly Texture2D ForceAndroidToExplodeDisabled = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_OverloadAndroidDisabled");


        public static readonly Texture2D AndroidToControlTarget = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_AndroidToControlTarget");
        public static readonly Texture2D AndroidToControlTargetRecovery = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_AndroidToControlTargetRecovery");
        public static readonly Texture2D AndroidToControlTargetDisconnect = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_AndroidToControlTargetDisconnect");

        public static readonly Texture2D AndroidSurrogateReconnectToLastController = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_SurrogateReconnectLastUser");


        public static readonly Texture2D PlayerExplosiveVirus = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_ExplosiveVirus");
        public static readonly Texture2D PlayerExplosiveVirusDisabled = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_ExplosiveVirusDisabled");
        public static readonly Texture2D PlayerVirus = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_Virus");
        public static readonly Texture2D PlayerVirusDisabled = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_VirusDisabled");
        public static readonly Texture2D PlayerHacking = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_Hacking");
        public static readonly Texture2D PlayerHackingDisabled = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_HackingDisabled");
        public static readonly Texture2D PlayerHackingTemp = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_HackingTemp");
        public static readonly Texture2D PlayerHackingTempDisabled = ContentFinder<Texture2D>.Get("Things/Misc/Hacking/ATPP_HackingTempDisabled");

        public static readonly Texture2D StopVirused = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_StopVirused");

        public static readonly Texture2D ColorPicker = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_ColorPicker");


        public static readonly Texture2D SettingsHeader = ContentFinder<Texture2D>.Get("Things/Misc/SettingsHeader");

        public static readonly Texture2D LWPNConnected = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_LWPNConnected");
        public static readonly Texture2D LWPNNotConnected = ContentFinder<Texture2D>.Get("Things/Misc/ATPP_LWPNNotConnected");

        public static readonly Material UploadInProgress = MaterialPool.MatFrom("Things/Misc/ATPP_Sync", ShaderDatabase.MetaOverlay);
        public static readonly Material SelectableSX = MaterialPool.MatFrom("Things/Misc/ATPP_SelectableSX", ShaderDatabase.MetaOverlay);

        public static readonly Material SelectableSXToHack = MaterialPool.MatFrom("Things/Misc/ATPP_SelectableSXToHack", ShaderDatabase.MetaOverlay);
        public static readonly Material ConnectedUser = MaterialPool.MatFrom("Things/Misc/ATPP_SkyMind", ShaderDatabase.MetaOverlay);

        public static readonly Material matHotLevel1 = MaterialPool.MatFrom("Things/Building/ATPP_Servers/Temperature/ATPP_Hot1", ShaderDatabase.MetaOverlay);
        public static readonly Material matHotLevel2 = MaterialPool.MatFrom("Things/Building/ATPP_Servers/Temperature/ATPP_Hot2", ShaderDatabase.MetaOverlay);
        public static readonly Material matHotLevel3 = MaterialPool.MatFrom("Things/Building/ATPP_Servers/Temperature/ATPP_Hot3", ShaderDatabase.MetaOverlay);

        public static readonly Material explosiveVirus = MaterialPool.MatFrom("Things/Misc/Virus/ATPP_ExplosiveVirus", ShaderDatabase.MetaOverlay);
        public static readonly Material virus = MaterialPool.MatFrom("Things/Misc/Virus/ATPP_Virus", ShaderDatabase.MetaOverlay);
        public static readonly Material cryptolocker = MaterialPool.MatFrom("Things/Misc/Virus/ATPP_Cryptolocker", ShaderDatabase.MetaOverlay);
        public static readonly Material virusLite = MaterialPool.MatFrom("Things/Misc/Virus/ATPP_VirusLite", ShaderDatabase.MetaOverlay);


        public static readonly Material Virused = MaterialPool.MatFrom("Things/Misc/Hacking/ATPP_Virused", ShaderDatabase.MetaOverlay);
        public static readonly Material ExplosiveVirused = MaterialPool.MatFrom("Things/Misc/Hacking/ATPP_ExplosiveVirused", ShaderDatabase.MetaOverlay);
        public static readonly Material HackedTemp = MaterialPool.MatFrom("Things/Misc/Hacking/ATPP_HackedTemp", ShaderDatabase.MetaOverlay);

        public static readonly Material RemotelyControlledNode = MaterialPool.MatFrom("Things/Misc/ATPP_RemoteControlled", ShaderDatabase.MetaOverlay);


        public static Dictionary<Pair<string, Color>, Graphic> eyeGlowEffectCache = new Dictionary<Pair<string, Color>, Graphic>();

        static Tex()
        {
        }

        public static Graphic getEyeGlowEffect(Color color, string gender, int type, int front)
        {
            var key = new Pair<string, Color>(type + gender + front, color);
            string path;

            Graphic res;
            if (eyeGlowEffectCache.ContainsKey(key))
            {
                res = eyeGlowEffectCache[key];
            }
            else
            {
                if (front == 1)
                {
                    if (gender == "M")
                        path = "Things/Misc/Androids/Effects/Front";
                    else
                        path = "Things/Misc/Androids/Effects/FFront";

                    eyeGlowEffectCache[key] = GraphicDatabase.Get<Graphic_Single>(path + type, ShaderDatabase.MoteGlow, Vector2.one, color);
                }
                else
                {
                    if (gender == "M")
                        path = "Things/Misc/Androids/Effects/Side";
                    else
                        path = "Things/Misc/Androids/Effects/FSide";

                    eyeGlowEffectCache[key] = GraphicDatabase.Get<Graphic_Single>(path + type, ShaderDatabase.MoteGlow, Vector2.one, color);
                }

                res = eyeGlowEffectCache[key];
            }

            return res;
        }
    }
}