using System;
using System.Collections.Generic;
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

                        //Check si la faction tient parole
                        if (Rand.Chance(1.0f - Settings.riskCryptolockerScam))
                        {
                            //Suppression cryptolocker des surrogates
                            foreach (var map in Find.Maps)
                            foreach (var t in map.listerThings.AllThings)
                                if (cryptolockedThings.Contains(t.GetUniqueLoadID()))
                                    try
                                    {
                                        if (t is Pawn)
                                        {
                                            var p = (Pawn) t;
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

                            if (deviceType)
                                Messages.Message("ATPP_CryptolockerDeviceClearedByFaction".Translate(faction.Name), MessageTypeDefOf.PositiveEvent);
                            else
                                Messages.Message("ATPP_CryptolockerClearedByFaction".Translate(faction.Name), MessageTypeDefOf.PositiveEvent);
                        }
                        else
                        {
                            //ATPP_LetterFactionScamCryptolocker
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

        public override bool CanShowInLetterStack => base.CanShowInLetterStack;

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