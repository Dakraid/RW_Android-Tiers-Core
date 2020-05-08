using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class ChoiceLetter_RansomDemand : ChoiceLetter
    {
        public List<string> cryptolockedThings;
        public bool deviceType;
        public Faction faction;
        public int fee;

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


                        if (Rand.Chance(1.0f - Settings.riskCryptolockerScam))
                        {
                            foreach (var t in Find.Maps.SelectMany(map => map.listerThings.AllThings.Where(t => cryptolockedThings.Contains(t.GetUniqueLoadID()))))
                                try
                                {
                                    if (t is Pawn p)
                                    {
                                        if (p.IsSurrogateAndroid()) p.health.AddHediff(Utils.hediffNoHost);
                                    }
                                    else
                                    {
                                        t.SetFaction(Faction.OfPlayer);
                                        var cf = t.TryGetComp<CompFlickable>();
                                        if (cf != null) cf.SwitchIsOn = true;
                                    }

                                    var csm = t.TryGetComp<CompSkyMind>();
                                    if (csm != null)
                                        csm.Infected = -1;
                                }
                                catch (Exception)
                                {
                                }

                            Messages.Message(
                                deviceType ? "ATPP_CryptolockerDeviceClearedByFaction".Translate(faction.Name) : "ATPP_CryptolockerClearedByFaction".Translate(faction.Name),
                                MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            Find.LetterStack.ReceiveLetter("ATPP_LetterFactionScam".Translate(), "ATPP_LetterFactionScamCryptolockerDesc".Translate(faction.Name),
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref cryptolockedThings, "ATPP_cryptolockedThings", LookMode.Value);
            Scribe_References.Look(ref faction, "ATPP_faction");
            Scribe_Values.Look(ref fee, "ATPP_fee");
            Scribe_Values.Look(ref deviceType, "ATPP_deviceType");
        }
    }
}