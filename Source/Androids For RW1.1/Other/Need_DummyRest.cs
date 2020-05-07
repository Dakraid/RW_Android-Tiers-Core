using System;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Need_DummyRest : Need_Rest
    {
        public Need_DummyRest(Pawn pawn) : base(pawn)
        {
        }

        public new RestCategory CurCategory => RestCategory.Rested;

        public override void NeedInterval()
        {
            throw new NotImplementedException();
        }

        public override void SetInitialLevel()
        {
            CurLevel = Rand.Range(0.9f, 1f);
        }
    }
}