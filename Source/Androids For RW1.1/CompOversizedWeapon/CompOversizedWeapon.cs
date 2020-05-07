using Verse;

namespace MOARANDROIDS
{
    public class CompOversizedWeapon : ThingComp
    {
        private bool isEquipped;

        public CompOversizedWeapon()
        {
            if (!(props is CompProperties_OversizedWeapon))
                props = new CompProperties_OversizedWeapon();
        }

        public CompProperties_OversizedWeapon Props => props as CompProperties_OversizedWeapon;


        public CompEquippable GetEquippable => parent?.GetComp<CompEquippable>();

        public Pawn GetPawn => GetEquippable?.verbTracker?.PrimaryVerb?.CasterPawn;

        public bool IsEquipped
        {
            get
            {
                if (Find.TickManager.TicksGame % 60 != 0) return isEquipped;
                isEquipped = GetPawn != null;
                return isEquipped;
            }
        }

        public bool FirstAttack { get; set; } = false;
    }
}