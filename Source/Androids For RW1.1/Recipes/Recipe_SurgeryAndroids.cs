using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Recipe_SurgeryAndroids : RecipeWorker
    {
        private const float CatastrophicFailChance = 0.5f;


        private const float RidiculousFailChanceFromCatastrophic = 0.1f;


        private const float InspiredSurgeryFailChanceFactor = 0.1f;


        private static readonly SimpleCurve MedicineMedicalPotencyToSurgeryChanceFactor = new SimpleCurve
        {
            new CurvePoint(0f, 0.7f),
            new CurvePoint(1f, 1f),
            new CurvePoint(2f, 1.3f)
        };

        protected bool CheckSurgeryFailAndroid(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
        {
            var num = 1f;
            if (!patient.RaceProps.IsMechanoid) num *= surgeon.GetStatValue(StatDefOf.AndroidSurgerySuccessChance);
            if (patient.InBed()) num *= patient.CurrentBed().GetStatValue(StatDefOf.AndroidSurgerySuccessChance);
            num *= recipe.surgerySuccessChanceFactor;
            if (surgeon.InspirationDef == InspirationDefOf.Inspired_Surgery && !patient.RaceProps.IsMechanoid)
            {
                if (num < 1f) num = 1f - (1f - num) * 0.1f;
                surgeon.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Surgery);
            }

            if (Rand.Chance(num)) return false;

            if (Rand.Chance(recipe.deathOnFailedSurgeryChance))
            {
                HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
                if (!patient.Dead) patient.Kill(null);
                Messages.Message("MessageMedicalOperationFailureFatalAndroid".Translate(surgeon.LabelShort, patient.LabelShort, recipe.label), patient,
                    MessageTypeDefOf.NegativeHealthEvent);
            }
            else if (Rand.Chance(0.5f))
            {
                if (Rand.Chance(0.1f))
                {
                    Messages.Message("MessageMedicalOperationFailureRidiculousAndroid".Translate(surgeon.LabelShort, patient.LabelShort), patient,
                        MessageTypeDefOf.NegativeHealthEvent);
                    HealthUtility.GiveInjuriesOperationFailureRidiculous(patient);
                }
                else
                {
                    Messages.Message("MessageMedicalOperationFailureCatastrophicAndroid".Translate(surgeon.LabelShort, patient.LabelShort), patient,
                        MessageTypeDefOf.NegativeHealthEvent);
                    HealthUtility.GiveInjuriesOperationFailureCatastrophic(patient, part);
                }
            }
            else
            {
                Messages.Message("MessageMedicalOperationFailureMinorAndroid".Translate(surgeon.LabelShort, patient.LabelShort), patient, MessageTypeDefOf.NegativeHealthEvent);
                HealthUtility.GiveInjuriesOperationFailureMinor(patient, part);
            }

            if (!patient.Dead) TryGainBotchedSurgeryThought(patient, surgeon);
            return true;
        }


        private void TryGainBotchedSurgeryThought(Pawn patient, Pawn surgeon)
        {
            if (!patient.RaceProps.Humanlike) return;

            patient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BotchedMySurgery, surgeon);
        }

        protected void applyFrameworkColor(Pawn pawn)
        {
            var cas = pawn.TryGetComp<CompAndroidState>();


            cas?.clearRusted();
        }
    }
}