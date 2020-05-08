using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Recipe_InstallArtificialBrain : Recipe_InstallImplant
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            var cas = pawn.TryGetComp<CompAndroidState>();

            if (cas == null)
                return;


            if (cas.surrogateController != null)
            {
                var cso = cas.surrogateController.TryGetComp<CompSurrogateOwner>();
                if (cso != null) cso.disconnectControlledSurrogate(pawn);
            }


            cas.isSurrogate = false;
            var he = pawn.health.hediffSet.GetFirstHediffOfDef(Utils.hediffNoHost);
            if (he != null)
                pawn.health.RemoveHediff(he);

            cas.isBlankAndroid = true;
            pawn.health.AddHediff(Utils.hediffBlankAndroid);
        }
    }
}