using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Incident_RansomWare : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return !Settings.disableSkyMindSecurityStuff && !Utils.isThereSolarFlare()
                                                         && Utils.GCATPP.getNbSkyMindUsers() > 0;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (Settings.disableSkyMindSecurityStuff)
                return false;

            Pawn victim;
            string title = "ATPP_LetterFactionRansomware".Translate();
            var msg = "";
            var ransomMsg = "";
            var nbConnectedClients = Utils.GCATPP.getNbThingsConnected();
            var nbUnsecurisedClients = nbConnectedClients - Utils.GCATPP.getNbSlotSecurisedAvailable();

            var faction = Find.FactionManager.RandomEnemyFaction();

            var letter = LetterDefOf.ThreatBig;
            var fee = 0;


            if (nbUnsecurisedClients < 0)
                return false;

            victim = Utils.GCATPP.getRandomSkyMindUser();

            var cso = victim?.TryGetComp<CompSurrogateOwner>();
            if (cso == null)
                return false;

            cso.clearRansomwareVar();


            if (Rand.Chance(0.5f))
            {
                var tr = Utils.RansomAddedBadTraits.ToList();


                foreach (var t in Utils.RansomAddedBadTraits)
                foreach (var t2 in victim.story.traits.allTraits.Where(t2 => t2.def == t || t.conflictingTraits != null && t.conflictingTraits.Contains(t2.def)))
                {
                    tr.Remove(t2.def);
                    break;
                }


                cso.ransomwareTraitAdded = tr.RandomElement();
                victim.story.traits.GainTrait(new Trait(cso.ransomwareTraitAdded, 0, true));

                fee = Rand.Range(Settings.ransomwareMinSilverToPayForBasTrait, Settings.ransomwareMaxSilverToPayForBasTrait);

                var traitLabel = "";

                if (cso.ransomwareTraitAdded.degreeDatas != null && cso.ransomwareTraitAdded.degreeDatas.First() != null)
                    traitLabel = cso.ransomwareTraitAdded.degreeDatas.First().label;


                msg = "ATPP_LetterFactionRansomwareBadTraitDownloadedDesc".Translate(faction.Name, victim.LabelShortCap, traitLabel);
                ransomMsg = "ATPP_RansomNeedPayRansomDownloadedTrait".Translate(faction.Name, traitLabel, victim.LabelShortCap, fee);
            }
            else
            {
                SkillDef find = null;
                SkillRecord sel = null;
                var v = -1;

                foreach (var s in victim.skills.skills.Where(s => s.levelInt >= v))
                {
                    v = s.levelInt;
                    find = s.def;
                    sel = s;
                }


                if (sel != null) sel.levelInt = 0;


                cso.ransomwareSkillStolen = find;
                cso.ransomwareSkillValue = v;

                fee = v * Settings.ransomwareSilverToPayToRestoreSkillPerLevel;

                if (cso.ransomwareSkillStolen != null)
                {
                    msg = "ATPP_LetterFactionRansomwareSkillStolenDesc".Translate(faction.Name, cso.ransomwareSkillStolen.LabelCap, victim.LabelShortCap);
                    ransomMsg = "ATPP_RansomNeedPayRansomCorruptedSKill".Translate(faction.Name, cso.ransomwareSkillStolen.LabelCap, victim.LabelShortCap, fee);
                }
            }


            Find.LetterStack.ReceiveLetter(title, msg, letter, victim);

            var ransom = (ChoiceLetter_RansomwareDemand) LetterMaker.MakeLetter(DefDatabase<LetterDef>.GetNamed("ATPP_CLPayRansomwareRansom"));
            ransom.label = "ATPP_RansomNeedPayRansomTitle".Translate();
            ransom.text = ransomMsg;
            ransom.faction = faction;
            ransom.victim = victim;
            ransom.radioMode = true;
            ransom.StartTimeout(60000);
            ransom.fee = fee;
            Find.LetterStack.ReceiveLetter(ransom);

            return true;
        }
    }
}