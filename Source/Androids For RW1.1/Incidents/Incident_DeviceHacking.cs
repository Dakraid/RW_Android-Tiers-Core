using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Incident_DeviceHacking : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return !Settings.disableSkyMindSecurityStuff && !Utils.isThereSolarFlare()
                                                         && Utils.GCATPP.getNbDevices() > 0;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (Settings.disableSkyMindSecurityStuff)
                return false;

            List<Thing> victims;
            var title = "";
            var msg = "";
            var nbConnectedClients = Utils.GCATPP.getNbThingsConnected();
            var cryptolockedThings = new List<string>();
            var nbDevices = Utils.GCATPP.getNbDevices();
            var nbUnsecurisedClients = nbConnectedClients - Utils.GCATPP.getNbSlotSecurisedAvailable();

            LetterDef letter;

            var attackType = 1;
            var fee = 0;


            if (nbDevices <= 0)
                return false;


            if (nbUnsecurisedClients <= 0)
            {
                if (!Rand.Chance(Settings.riskSecurisedSecuritySystemGetVirus))
                    return false;

                var nb = 0;


                nb = nbDevices / 2;
                nb = nb != 0 ? Rand.Range(1, nb + 1) : 1;

                letter = LetterDefOf.ThreatSmall;

                victims = Utils.GCATPP.getRandomDevices(nb);
                if (victims.Count == 0) return false;

                foreach (var csm in from v in victims let csm = v.TryGetComp<CompSkyMind>() let cas = v.TryGetComp<CompAndroidState>() where cas != null select csm)
                    csm.Infected = 4;


                title = "ATPP_IncidentDeviceHackingVirus".Translate();
                msg = "ATPP_IncidentDeviceHackingLiteDesc".Translate(nb);

                victims = Utils.GCATPP.getRandomDevices(nb);
                if (victims.Count != nb)
                    return false;

                foreach (var v in victims)
                {
                    var csm = v.TryGetComp<CompSkyMind>();
                    if (csm == null)
                        continue;

                    Utils.GCATPP.disconnectUser(v);
                    csm.Infected = attackType;
                    csm.infectedEndGT = Find.TickManager.TicksGame +
                                        Rand.Range(Settings.nbHourLiteHackingDeviceAttackLastMin, Settings.nbHourLiteHackingDeviceAttackLastMax) * 2500;
                }
            }
            else
            {
                letter = LetterDefOf.ThreatBig;

                attackType = Rand.Range(1, 4);

                var nb = 0;


                nb = nbDevices / 2;
                nb = nb != 0 ? Rand.Range(1, nb + 1) : 1;

                msg = "ATPP_IncidentDeviceHackingHardDesc".Translate(nb) + "\n";

                switch (attackType)
                {
                    case 1:
                        title = "ATPP_IncidentDeviceHackingVirus".Translate();
                        msg += "ATPP_IncidentDeviceVirusedDesc".Translate();
                        break;
                    case 2:
                        title = "ATPP_IncidentDeviceHackingExplosiveVirus".Translate();
                        msg += "ATPP_IncidentDeviceVirusedExplosiveDesc".Translate();
                        break;
                    case 3:
                        title = "ATPP_IncidentDeviceHackingCryptolocker".Translate();
                        msg += "ATPP_IncidentDeviceCryptolockerDesc".Translate();
                        break;
                }

                victims = Utils.GCATPP.getRandomDevices(nb);
                if (victims.Count != nb)
                    return false;

                foreach (var v in victims)
                {
                    var csm = v.TryGetComp<CompSkyMind>();
                    if (csm == null)
                        continue;

                    Utils.GCATPP.disconnectUser(v);
                    csm.Infected = attackType;


                    if (attackType != 3) continue;

                    cryptolockedThings.Add(v.GetUniqueLoadID());
                    fee += (int) (v.def.BaseMarketValue * 0.25f);
                }
            }

            Find.LetterStack.ReceiveLetter(title, msg, letter, victims);


            if (attackType != 3) return true;

            var faction = Find.FactionManager.RandomEnemyFaction();

            var ransom = (ChoiceLetter_RansomDemand) LetterMaker.MakeLetter(DefDatabase<LetterDef>.GetNamed("ATPP_CLPayCryptoRansom"));
            ransom.label = "ATPP_CryptolockerNeedPayRansomTitle".Translate();
            ransom.text = "ATPP_CryptolockerNeedPayRansom".Translate(faction.Name, fee);
            ransom.faction = faction;
            ransom.radioMode = true;
            ransom.fee = fee;
            ransom.cryptolockedThings = cryptolockedThings;
            ransom.deviceType = true;
            ransom.StartTimeout(60000);
            Find.LetterStack.ReceiveLetter(ransom);

            return true;
        }
    }
}