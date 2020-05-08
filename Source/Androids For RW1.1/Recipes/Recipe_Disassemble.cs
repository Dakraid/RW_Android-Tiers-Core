using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Androids
{
	
	public class Recipe_Disassemble : RecipeWorker
	{
		
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			bool flag = pawn.def.HasModExtension<MechanicalPawnProperties>();
			if (flag)
			{
				yield return null;
			}
			yield break;
		}

		
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			Need_Energy need_Energy = pawn.needs.TryGetNeed<Need_Energy>();
			EnergyTrackerComp energyTrackerComp = pawn.TryGetComp<EnergyTrackerComp>();
			bool flag = need_Energy != null;
			if (flag)
			{
				need_Energy.CurLevelPercentage = 0f;
			}
			bool flag2 = energyTrackerComp != null;
			if (flag2)
			{
				energyTrackerComp.energy = 0f;
			}
			ButcherUtility.SpawnDrops(pawn, pawn.Position, pawn.Map);
			pawn.Kill(null, null);
		}
	}
}
