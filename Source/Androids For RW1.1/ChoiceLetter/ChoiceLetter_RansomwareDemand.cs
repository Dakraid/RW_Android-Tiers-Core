using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class ChoiceLetter_RansomwareDemand : ChoiceLetter
    {
        public Faction faction;
        public int fee;
        public Pawn victim;

        public override IEnumerable<DiaOption> Choices
        {
            get
            {
                if (ArchivedOnly)
                {
                    yield return Option_Close;
                }
                else
                {
                    var accept = new DiaOption("RansomDemand_Accept".Translate());
                    accept.action = delegate
                    {
                        Utils.anyPlayerColonnyPaySilver(fee);

                        //Check si la faction tient parole
                        if (Rand.Chance(1.0f - Settings.riskCryptolockerScam))
                        {
                            var cso = victim.TryGetComp<CompSurrogateOwner>();

                            if (cso.ransomwareTraitAdded != null)
                            {
                                if (victim.story.traits.HasTrait(cso.ransomwareTraitAdded))
                                {
                                    Trait ct = null;
                                    foreach (var t in victim.story.traits.allTraits)
                                        if (t.def == cso.ransomwareTraitAdded)
                                        {
                                            ct = t;
                                            break;
                                        }

                                    if (ct != null)
                                        victim.story.traits.allTraits.Remove(ct);
                                }
                            }
                            else
                            {
                                foreach (var s in victim.skills.skills)
                                    if (s.def == cso.ransomwareSkillStolen)
                                    {
                                        s.levelInt = cso.ransomwareSkillValue;
                                        break;
                                    }
                            }

                            cso.clearRansomwareVar();

                            Messages.Message("ATPP_RansomwareClearedByFaction".Translate(faction.Name, victim.LabelShortCap), MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            //ATPP_LetterFactionScamCryptolocker
                            Find.LetterStack.ReceiveLetter("ATPP_LetterFactionScam".Translate(), "ATPP_LetterFactionScamRansomwareDesc".Translate(faction.Name),
                                LetterDefOf.ThreatBig);
                        }

                        Find.LetterStack.RemoveLetter(this);
                    };
                    accept.resolveTree = true;
                    if (!Utils.anyPlayerColonnyHasEnoughtSilver(fee)) accept.Disable("NeedSilverLaunchable".Translate(fee.ToString()));
                    yield return accept;
                    yield return Option_Reject;
                    yield return Option_Postpone;
                }
            }
        }

        public override bool CanShowInLetterStack => base.CanShowInLetterStack;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref faction, "faction");
            Scribe_References.Look(ref victim, "victim");
            Scribe_Values.Look(ref fee, "fee");
        }
    }
}