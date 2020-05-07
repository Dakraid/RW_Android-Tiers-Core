using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace MOARANDROIDS
{
    public class Incident_SurrogateHacking : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return !Settings.disableSkyMindSecurityStuff && !Utils.isThereSolarFlare()
                                                         && Utils.GCATPP.getNbSurrogateAndroids() > 0;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (Settings.disableSkyMindSecurityStuff)
                return false;

            List<Pawn> victims;
            var cryptolockedThings = new List<string>();
            var title = "";
            var msg = "";
            var nbConnectedClients = Utils.GCATPP.getNbThingsConnected();
            var nbSurrogates = Utils.GCATPP.getNbSurrogateAndroids();
            var nbUnsecurisedClients = nbConnectedClients - Utils.GCATPP.getNbSlotSecurisedAvailable();

            LetterDef letter;
            //Selection type virus 
            var attackType = 1;
            var fee = 0;

            //Check si sur lensemble des clients connecté il y a quand meme des surrogates
            if (nbSurrogates <= 0)
                return false;

            //Attaque virale faible
            if (nbUnsecurisedClients <= 0)
            {
                if (!Rand.Chance(Settings.riskSecurisedSecuritySystemGetVirus))
                    return false;

                var nb = 0;


                nb = nbSurrogates / 2;
                nb = nb != 0 ? Rand.Range(1, nb + 1) : 1;

                letter = LetterDefOf.ThreatSmall;
                //Obtention des victimes
                victims = Utils.GCATPP.getRandomSurrogateAndroids(nb);
                if (victims.Count == 0) return false;

                foreach (var v in victims)
                {
                    var csm = v.TryGetComp<CompSkyMind>();
                    var cas = v.TryGetComp<CompAndroidState>();
                    if (csm == null || cas == null)
                        continue;

                    csm.Infected = 4;

                    //Deconnection du contorlleur le cas echeant
                    if (cas.surrogateController != null)
                    {
                        var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                        cso?.disconnectControlledSurrogate(null);
                    }

                    var he = v.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
                    if (he != null)
                        v.health.RemoveHediff(he);

                    Utils.ignoredPawnNotifications = v;
                    Utils.VirusedRandomMentalBreak.RandomElement().Worker.TryStart(v, null, false);
                    Utils.ignoredPawnNotifications = null;
                    //v.mindState.mentalStateHandler.TryStartMentalState(  , null, false, false, null, false);
                }


                title = "ATPP_IncidentSurrogateHackingVirus".Translate();
                msg = "ATPP_IncidentSurrogateHackingLiteDesc".Translate(nb);
            }
            else
            {
                letter = LetterDefOf.ThreatBig;

                attackType = Rand.Range(1, 4);

                var nb = 0;
                Lord lord = null;

                if (attackType != 3)
                {
                    //Attaque virale douce
                    //Obtention des victimes (qui peut allez de 1 victime a N/2 victimes
                    nb = nbSurrogates / 2;
                    nb = nb != 0 ? Rand.Range(1, nb + 1) : 1;

                    var lordJob = new LordJob_AssaultColony(Faction.OfAncientsHostile, false, false, false, false, false);

                    lord = LordMaker.MakeNewLord(Faction.OfAncientsHostile, lordJob, Current.Game.CurrentMap);
                }
                else
                {
                    nb = nbSurrogates;
                }

                msg = "ATPP_IncidentSurrogateHackingHardDesc".Translate(nb) + "\n";

                switch (attackType)
                {
                    case 1:
                        title = "ATPP_IncidentSurrogateHackingVirus".Translate();
                        msg += "ATPP_IncidentVirusedDesc".Translate();
                        break;
                    case 2:
                        title = "ATPP_IncidentSurrogateHackingExplosiveVirus".Translate();
                        msg += "ATPP_IncidentVirusedExplosiveDesc".Translate();
                        break;
                    case 3:
                        title = "ATPP_IncidentSurrogateHackingCryptolocker".Translate();
                        msg += "ATPP_IncidentCryptolockerDesc".Translate();
                        break;
                }

                victims = Utils.GCATPP.getRandomSurrogateAndroids(nb);
                if (victims.Count != nb)
                    return false;

                foreach (var v in victims)
                {
                    var csm = v.TryGetComp<CompSkyMind>();

                    v.mindState.canFleeIndividual = false;
                    csm.Infected = attackType;
                    if (v.jobs != null)
                    {
                        v.jobs.StopAll();
                        v.jobs.ClearQueuedJobs();
                    }

                    if (v.mindState != null)
                        v.mindState.Reset(true);


                    switch (attackType)
                    {
                        //Virus
                        case 1:
                            //Devient hostile
                            if (lord != null)
                                lord.AddPawn(v);
                            break;
                        //Virus explosif
                        case 2:
                            //Devient hostile
                            if (lord != null)
                                lord.AddPawn(v);
                            break;
                        //Virus cryptolocker
                        case 3:
                            cryptolockedThings.Add(v.GetUniqueLoadID());

                            switch (v.def.defName)
                            {
                                case Utils.M7:
                                    fee += Settings.ransomCostT5;
                                    break;
                                case Utils.HU:
                                    fee += Settings.ransomCostT4;
                                    break;
                                case Utils.T2:
                                    fee += Settings.ransomCostT2;
                                    break;
                                case Utils.T3:
                                    fee += Settings.ransomCostT3;
                                    break;
                                case Utils.T4:
                                    fee += Settings.ransomCostT4;
                                    break;
                                case Utils.T5:
                                    fee += Settings.ransomCostT5;
                                    break;
                                case Utils.T1:
                                default:
                                    fee += Settings.ransomCostT1;
                                    break;
                            }

                            break;
                    }

                    if (attackType != 1 && attackType != 2) continue;
                    //On va attribuer aleatoirement des poids d'attaque aux surrogate
                    var shooting = v.skills.GetSkill(SkillDefOf.Shooting);
                    if (shooting != null && !shooting.TotallyDisabled) shooting.levelInt = Rand.Range(3, 19);
                    var melee = v.skills.GetSkill(SkillDefOf.Melee);
                    if (melee != null && !melee.TotallyDisabled) melee.levelInt = Rand.Range(3, 19);
                }
            }

            Find.LetterStack.ReceiveLetter(title, msg, letter, victims);


            if (attackType != 3) return true;
            //Déduction faction ennemis au hasard
            var faction = Find.FactionManager.RandomEnemyFaction();

            var ransom = (ChoiceLetter_RansomDemand) LetterMaker.MakeLetter(DefDatabase<LetterDef>.GetNamed("ATPP_CLPayCryptoRansom"));
            ransom.label = "ATPP_CryptolockerNeedPayRansomTitle".Translate();
            ransom.text = "ATPP_CryptolockerNeedPayRansom".Translate(faction.Name, fee);
            ransom.faction = faction;
            ransom.radioMode = true;
            ransom.fee = fee;
            ransom.cryptolockedThings = cryptolockedThings;
            ransom.StartTimeout(60000);
            Find.LetterStack.ReceiveLetter(ransom);

            return true;
        }
    }
}