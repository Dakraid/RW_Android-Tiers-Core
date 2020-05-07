using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    // Token: 0x02000200 RID: 512
    public class ThoughtWorker_UncomfortableClothing : ThoughtWorker
    {
        // Token: 0x06000975 RID: 2421 RVA: 0x00048EA0 File Offset: 0x000472A0
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            string text = null;
            var num = 0;
            var wornApparel = p.apparel.WornApparel;
            foreach (var apparel in wornApparel.Where(apparel => apparel.Stuff == ThingDefOf.SteelWool))
            {
                if (text == null) text = apparel.def.label;
                num++;
            }

            if (num == 0) return ThoughtState.Inactive;

            if (p.IsAndroid() == false) return num >= 5 ? ThoughtState.ActiveAtStage(4, text) : ThoughtState.ActiveAtStage(num - 1, text);

            return ThoughtState.Inactive;
        }
    }
}