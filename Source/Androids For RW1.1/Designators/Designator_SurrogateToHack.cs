using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace MOARANDROIDS
{
    public class Designator_SurrogateToHack : Designator
    {
        private readonly int hackType;
        private Map cmap;

        private IntVec3 pos;
        private Pawn target;

        public Designator_SurrogateToHack(int hackType)
        {
            this.hackType = hackType;

            switch (hackType)
            {
                case 1:
                    defaultLabel = "ATPP_UploadVirus".Translate();
                    defaultDesc = "ATPP_UploadVirusDesc".Translate();
                    icon = Tex.PlayerVirus;
                    break;
                case 2:
                    defaultLabel = "ATPP_UploadExplosiveVirus".Translate();
                    defaultDesc = "ATPP_UploadExplosiveVirusDesc".Translate();
                    icon = Tex.PlayerExplosiveVirus;
                    break;
                case 3:
                    defaultLabel = "ATPP_HackTemp".Translate();
                    defaultDesc = "ATPP_HackTempDesc".Translate();
                    icon = Tex.PlayerHackingTemp;
                    break;
                case 4:
                    defaultLabel = "ATPP_Hack".Translate();
                    defaultDesc = "ATPP_HackDesc".Translate();
                    icon = Tex.PlayerHacking;
                    break;
            }

            soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
            soundDragChanged = null;
            soundSucceeded = SoundDefOf.Designate_ZoneDelete;
            useMouseIcon = true;
            hotKey = KeyBindingDefOf.Misc4;
        }

        public override int DraggableDimensions => 0;

        public override bool DragDrawMeasurements => false;

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(Map)) return false;
            if (!SXInCell(c)) return "ATPP_DesignatorNeedSelectSXToHack".Translate();

            return true;
        }

        public override void RenderHighlight(List<IntVec3> dragCells)
        {
            base.RenderHighlight(dragCells);

            DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
        }


        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            base.CanDesignateThing(t);

            if (!(t is Pawn))
                return false;

            var cp = (Pawn) t;
            var cas = cp.TryGetComp<CompAndroidState>();


            if (cas == null || !cas.isSurrogate || cp.Faction == Faction.OfPlayer)
                return false;

            target = cp;

            return true;
        }

        public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
        {
            throw new NotImplementedException();
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            pos = c;
            cmap = Current.Game.CurrentMap;
        }

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();

            var csm = target.TryGetComp<CompSkyMind>();
            var cas = target.TryGetComp<CompAndroidState>();
            var surrogateName = target.LabelShortCap;
            CompSurrogateOwner cso = null;

            if (cas.externalController != null)
            {
                surrogateName = cas.externalController.LabelShortCap;
                cso = cas.externalController.TryGetComp<CompSurrogateOwner>();
            }

            var clord = target.GetLord();
            var nbp = Utils.GCATPP.getNbHackingPoints();
            var nbpToConsume = 0;


            switch (hackType)
            {
                case 1:
                    nbpToConsume = Settings.costPlayerVirus;
                    break;
                case 2:
                    nbpToConsume = Settings.costPlayerExplosiveVirus;
                    break;
                case 3:
                    nbpToConsume = Settings.costPlayerHackTemp;
                    break;
                case 4:
                    nbpToConsume = Settings.costPlayerHack;
                    break;
            }

            if (nbpToConsume > nbp)
            {
                Messages.Message("ATPP_CannotHackNotEnoughtHackingPoints".Translate(), MessageTypeDefOf.NegativeEvent);
                return;
            }


            if (target.Faction.RelationKindWith(Faction.OfPlayer) != FactionRelationKind.Hostile) target.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -1 * Rand.Range(5, 36));


            switch (hackType)
            {
                case 1:
                case 2:

                    csm.Hacked = hackType;

                    target.SetFactionDirect(Faction.OfAncients);
                    LordJob_AssistColony lordJob;
                    Lord lord = null;

                    IntVec3 fallbackLocation;
                    RCellFinder.TryFindRandomSpotJustOutsideColony(target.PositionHeld, target.Map, out fallbackLocation);

                    target.mindState.Reset();
                    target.mindState.duty = null;
                    target.jobs.StopAll();
                    target.jobs.ClearQueuedJobs();
                    target.ClearAllReservations();
                    if (target.drafter != null)
                        target.drafter.Drafted = false;

                    lordJob = new LordJob_AssistColony(Faction.OfAncients, fallbackLocation);
                    lord = LordMaker.MakeNewLord(Faction.OfAncients, lordJob, Current.Game.CurrentMap);

                    if (clord != null)
                        if (clord.ownedPawns.Contains(target))
                            clord.Notify_PawnLost(target, PawnLostCondition.IncappedOrKilled, null);

                    lord.AddPawn(target);


                    if (hackType == 2)
                        csm.infectedExplodeGT = Find.TickManager.TicksGame + Settings.nbSecExplosiveVirusTakeToExplode * 60;
                    break;
                case 3:
                case 4:
                    var wasPrisonner = target.IsPrisoner;
                    var prevFaction = target.Faction;
                    target.SetFaction(Faction.OfPlayer);

                    if (target.workSettings == null)
                    {
                        target.workSettings = new Pawn_WorkSettings(target);
                        target.workSettings.EnableAndInitialize();
                    }

                    if (clord != null)
                        if (clord.ownedPawns.Contains(target))
                            clord.Notify_PawnLost(target, PawnLostCondition.ChangedFaction, null);

                    cso?.disconnectControlledSurrogate(null);

                    if (hackType == 4)

                        cas.externalController = null;

                    target.Map.attackTargetsCache.UpdateTarget(target);
                    PawnComponentsUtility.AddAndRemoveDynamicComponents(target);
                    Find.ColonistBar.MarkColonistsDirty();

                    if (hackType == 3)
                    {
                        csm.Hacked = hackType;
                        csm.hackOrigFaction = prevFaction;
                        csm.hackWasPrisoned = wasPrisonner;
                        csm.hackEndGT = Find.TickManager.TicksGame + Settings.nbSecDurationTempHack * 60;
                    }
                    else
                    {
                        if (csm.Infected != -1)
                        {
                            csm.Infected = -1;

                            if (target.skills != null && target.skills.skills != null)
                                foreach (var sr in target.skills.skills)
                                    sr.levelInt = 0;
                        }
                    }


                    break;
            }

            Utils.GCATPP.decHackingPoints(nbpToConsume);

            Utils.soundDefSurrogateHacked.PlayOneShot(null);


            Messages.Message("ATPP_SurrogateHackOK".Translate(surrogateName), target, MessageTypeDefOf.PositiveEvent);


            Utils.soundDefSurrogateConnection.PlayOneShot(null);
            MoteMaker.ThrowDustPuffThick(pos.ToVector3Shifted(), cmap, 4.0f, Color.red);

            Find.DesignatorManager.Deselect();
        }


        private bool SXInCell(IntVec3 c)
        {
            if (c.Fogged(Map)) return false;

            var thingList = c.GetThingList(Map);
            return Enumerable.Any(thingList, t => t is Pawn && CanDesignateThing(t).Accepted);
        }
    }
}