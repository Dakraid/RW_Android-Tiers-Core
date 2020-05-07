using RimWorld.Planet;
using Verse;

namespace MOARANDROIDS
{
    public class DownedT5AndroidComp : ImportantPawnComp, IThingHolder
    {
        protected override string PawnSaveKey => "transcendant";

        protected override void RemovePawnOnWorldObjectRemoved()
        {
            for (var i = pawn.Count - 1; i >= 0; i--)
                if (!pawn[i].Dead)
                    pawn[i].Kill(null);
            pawn.ClearAndDestroyContents();
        }

        public override string CompInspectStringExtra()
        {
            if (pawn.Any) return "Transcendant".Translate() + ": " + pawn[0].LabelShort;
            
            return null;
        }
    }
}