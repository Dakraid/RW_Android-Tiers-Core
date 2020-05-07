using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Hediff_Fractal : HediffWithComps
    {
        public override string LabelInBrackets
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(base.LabelInBrackets);
                if (comps != null)
                    for (var i = 0; i < comps.Count; i++)
                    {
                        var compLabelInBracketsExtra = comps[i].CompLabelInBracketsExtra;
                        if (!compLabelInBracketsExtra.NullOrEmpty())
                        {
                            if (stringBuilder.Length != 0) stringBuilder.Append(", ");
                            stringBuilder.Append(compLabelInBracketsExtra);
                        }
                    }

                return stringBuilder.ToString();
            }
        }

        public override bool ShouldRemove
        {
            get
            {
                if (comps != null)
                    for (var i = 0; i < comps.Count; i++)
                        if (comps[i].CompShouldRemove)
                            return true;
                return base.ShouldRemove;
            }
        }

        public override bool Visible
        {
            get
            {
                if (comps != null)
                    for (var i = 0; i < comps.Count; i++)
                        if (comps[i].CompDisallowVisible())
                            return false;
                return base.Visible;
            }
        }

        public override string TipStringExtra
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(base.TipStringExtra);
                if (comps != null)
                    for (var i = 0; i < comps.Count; i++)
                    {
                        var compTipStringExtra = comps[i].CompTipStringExtra;
                        if (!compTipStringExtra.NullOrEmpty()) stringBuilder.AppendLine(compTipStringExtra);
                    }

                return stringBuilder.ToString();
            }
        }

        public override TextureAndColor StateIcon
        {
            get
            {
                for (var i = 0; i < comps.Count; i++)
                {
                    var compStateIcon = comps[i].CompStateIcon;
                    if (compStateIcon.HasValue) return compStateIcon;
                }

                return TextureAndColor.None;
            }
        }

        private bool IsSeverelyWounded
        {
            get
            {
                var num = 0f;
                var hediffs = pawn.health.hediffSet.hediffs;
                for (var i = 0; i < hediffs.Count; i++)
                    if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
                        num += hediffs[i].Severity;
                var missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
                for (var j = 0; j < missingPartsCommonAncestors.Count; j++)
                    if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
                        num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(pawn);
                return num > 38f * pawn.RaceProps.baseHealthScale;
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            if (comps != null)
                for (var i = 0; i < comps.Count; i++)
                    comps[i].CompPostPostAdd(dinfo);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (comps != null)
                for (var i = 0; i < comps.Count; i++)
                    comps[i].CompPostPostRemoved();
        }

        public override void PostTick()
        {
            base.PostTick();
            if (comps != null)
            {
                var num = 0f;
                for (var i = 0; i < comps.Count; i++) comps[i].CompPostTick(ref num);
                if (num != 0f) Severity += num;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.LoadingVars) InitializeComps();
            if (comps != null)
                for (var i = 0; i < comps.Count; i++)
                    comps[i].CompExposeData();
        }

        public override void Tended(float quality, int batchPosition = 0)
        {
            for (var i = 0; i < comps.Count; i++) comps[i].CompTended(quality, batchPosition);
        }

        public override bool TryMergeWith(Hediff other)
        {
            if (base.TryMergeWith(other))
            {
                for (var i = 0; i < comps.Count; i++) comps[i].CompPostMerged(other);
                return true;
            }

            return false;
        }

        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            for (var i = 0; i < comps.Count; i++) comps[i].Notify_PawnDied();
        }

        public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
        {
            for (var i = 0; i < comps.Count; i++) comps[i].CompModifyChemicalEffect(chem, ref effect);
        }

        public override void PostMake()
        {
            base.PostMake();
            InitializeComps();
            for (var i = 0; i < comps.Count; i++) comps[i].CompPostMake();
        }

        private void InitializeComps()
        {
            if (def.comps != null)
            {
                comps = new List<HediffComp>();
                for (var i = 0; i < def.comps.Count; i++)
                {
                    var hediffComp = (HediffComp) Activator.CreateInstance(def.comps[i].compClass);
                    hediffComp.props = def.comps[i];
                    hediffComp.parent = this;
                    comps.Add(hediffComp);
                }
            }
        }

        public override string DebugString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.DebugString());
            if (comps != null)
                for (var i = 0; i < comps.Count; i++)
                {
                    string str;
                    if (comps[i].ToString().Contains("_"))
                        str = comps[i].ToString().Split('_')[1];
                    else
                        str = comps[i].ToString();
                    stringBuilder.AppendLine("--" + str);
                    var text = comps[i].CompDebugString();
                    if (!text.NullOrEmpty()) stringBuilder.AppendLine(text.TrimEnd().Indented());
                }

            return stringBuilder.ToString();
        }

        public override void Tick()
        {
            ageTicks++;
            if (Severity >= 1f)
            {
                DoMutation(pawn);
                pawn.Destroy();
            }
        }

        public static void DoMutation(Pawn premutant)
        {
            string text = "Atlas_Mutation".Translate(premutant.Name.ToStringShort);
            text = text.AdjustedFor(premutant);
            string label = "LetterLabelAtlas_Mutation".Translate();
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, premutant);

            var request = new PawnGenerationRequest(PawnKindDefOf.AbominationAtlas, Faction.OfMechanoids, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true,
                false, 1f, false, true, true, false);
            var pawn = PawnGenerator.GeneratePawn(request);
            FilthMaker.TryMakeFilth(premutant.Position, premutant.Map, RimWorld.ThingDefOf.Filth_AmnioticFluid, premutant.LabelIndefinite(), 10);
            FilthMaker.TryMakeFilth(premutant.Position, premutant.Map, RimWorld.ThingDefOf.Filth_Blood, premutant.LabelIndefinite(), 10);

            GenSpawn.Spawn(pawn, premutant.Position, premutant.Map);
            pawn.mindState.mentalStateHandler.TryStartMentalState(RimWorld.MentalStateDefOf.ManhunterPermanent, null, true);
        }
    }
}