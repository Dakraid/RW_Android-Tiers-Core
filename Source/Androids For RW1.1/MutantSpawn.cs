using System;
using System.Collections.Generic;
using System.Linq;
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
                if (comps == null) return stringBuilder.ToString();
                
                foreach (var compLabelInBracketsExtra in comps.Select(t => t.CompLabelInBracketsExtra).Where(compLabelInBracketsExtra => !compLabelInBracketsExtra.NullOrEmpty()))
                {
                    if (stringBuilder.Length != 0) stringBuilder.Append(", ");
                    stringBuilder.Append(compLabelInBracketsExtra);
                }

                return stringBuilder.ToString();
            }
        }

        public override bool ShouldRemove
        {
            get
            {
                if (comps == null) return base.ShouldRemove;
                
                return Enumerable.Any(comps, t => t.CompShouldRemove) || base.ShouldRemove;
            }
        }

        public override bool Visible
        {
            get
            {
                if (comps == null) return base.Visible;

                return !Enumerable.Any(comps, t => t.CompDisallowVisible()) && base.Visible;
            }
        }

        public override string TipStringExtra
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(base.TipStringExtra);
                if (comps == null) return stringBuilder.ToString();

                foreach (var compTipStringExtra in comps.Select(t => t.CompTipStringExtra).Where(compTipStringExtra => !compTipStringExtra.NullOrEmpty())) stringBuilder.AppendLine(compTipStringExtra);

                return stringBuilder.ToString();
            }
        }

        public override TextureAndColor StateIcon
        {
            get
            {
                foreach (var compStateIcon in comps.Select(t => t.CompStateIcon).Where(compStateIcon => compStateIcon.HasValue)) return compStateIcon;

                return TextureAndColor.None;
            }
        }

        private bool IsSeverelyWounded
        {
            get
            {
                var hediffs = pawn.health.hediffSet.hediffs;
                var num = hediffs.Where(t => t is Hediff_Injury && !t.IsPermanent()).Sum(t => t.Severity);

                var missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
                num += missingPartsCommonAncestors.Where(t => t.IsFreshNonSolidExtremity).Sum(t => t.Part.def.GetMaxHealth(pawn));

                return num > 38f * pawn.RaceProps.baseHealthScale;
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            if (comps == null) return;
            
            foreach (var diff in comps)
                diff.CompPostPostAdd(dinfo);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (comps == null) return;
            
            foreach (var diff in comps)
                diff.CompPostPostRemoved();
        }

        public override void PostTick()
        {
            base.PostTick();
            if (comps == null) return;
            
            var num = 0f;
            foreach (var diff in comps)
                diff.CompPostTick(ref num);

            if (num != 0f) Severity += num;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.LoadingVars) InitializeComps();
            if (comps == null) return;
            
            foreach (var diff in comps)
                diff.CompExposeData();
        }

        public override void Tended(float quality, int batchPosition = 0)
        {
            foreach (var diff in comps)
                diff.CompTended(quality, batchPosition);
        }

        public override bool TryMergeWith(Hediff other)
        {
            if (!base.TryMergeWith(other)) return false;

            foreach (var diff in comps)
                diff.CompPostMerged(other);

            return true;
        }

        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            foreach (var diff in comps)
                diff.Notify_PawnDied();
        }

        public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
        {
            foreach (var diff in comps)
                diff.CompModifyChemicalEffect(chem, ref effect);
        }

        public override void PostMake()
        {
            base.PostMake();
            InitializeComps();
            foreach (var diff in comps)
                diff.CompPostMake();
        }

        private void InitializeComps()
        {
            if (def.comps == null) return;

            comps = new List<HediffComp>();
            foreach (var diff in def.comps)
            {
                var hediffComp = (HediffComp) Activator.CreateInstance(diff.compClass);
                hediffComp.props = diff;
                hediffComp.parent = this;
                comps.Add(hediffComp);
            }
        }

        public override string DebugString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.DebugString());
            if (comps == null) return stringBuilder.ToString();

            foreach (var diff in comps)
            {
                var str = diff.ToString().Contains("_") ? diff.ToString().Split('_')[1] : diff.ToString();
                stringBuilder.AppendLine("--" + str);
                var text = diff.CompDebugString();
                if (!text.NullOrEmpty()) stringBuilder.AppendLine(text.TrimEnd().Indented());
            }

            return stringBuilder.ToString();
        }

        public override void Tick()
        {
            ageTicks++;
            if (!(Severity >= 1f)) return;

            DoMutation(pawn);
            pawn.Destroy();
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