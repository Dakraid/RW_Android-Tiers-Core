using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Designator_AndroidToControl : Designator
    {
        private Map cmap;
        private readonly Pawn controller;
        public bool fromSkyCloud;

        private int kindOfTarget;
        private IntVec3 pos;
        private Thing target;

        public Designator_AndroidToControl(Pawn controller, bool fromSkyCloud = false)
        {
            defaultLabel = "ATPP_AndroidToControlTarget".Translate();
            defaultDesc = "ATPP_AndroidToControlTargetDesc".Translate();
            soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
            soundDragChanged = null;
            soundSucceeded = SoundDefOf.Designate_ZoneDelete;
            useMouseIcon = true;
            icon = Tex.AndroidToControlTarget;
            hotKey = KeyBindingDefOf.Misc4;
            this.controller = controller;
            this.fromSkyCloud = fromSkyCloud;
        }

        public override int DraggableDimensions => 0;

        public override bool DragDrawMeasurements => false;

        public override void DrawMouseAttachments()
        {
            base.DrawMouseAttachments();
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(Map)) return false;


            if (!SXInCell(c))
            {
                if (fromSkyCloud)
                {
                    if (!TurretInCell(c))
                        return "ATPP_DesignatorNeedSelectSX".Translate();
                }
                else
                {
                    return "ATPP_DesignatorNeedSelectSX".Translate();
                }
            }

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

            if (t is Pawn)
            {
                var cp = (Pawn) t;
                var csm = cp.TryGetComp<CompSkyMind>();
                var cas = cp.TryGetComp<CompAndroidState>();

                //Si pas clone ou clone deja utilisé on degage
                if (cas == null || !cas.isSurrogate || cas.surrogateController != null || csm.Infected != -1)
                    return false;

                if (!Utils.GCATPP.isConnectedToSkyMind(cp))
                    //Tentative connection au skymind 
                    if (!Utils.GCATPP.connectUser(cp))
                        return false;

                target = cp;
                kindOfTarget = 1;
                return true;
            }

            if (fromSkyCloud && (t.def.thingClass == typeof(Building_Turret) || t.def.thingClass.IsSubclassOf(typeof(Building_Turret))))
            {
                var build = (Building) t;
                var crt = t.TryGetComp<CompRemotelyControlledTurret>();

                if (crt != null && crt.controller == null && !t.IsBrokenDown() && t.TryGetComp<CompPowerTrader>().PowerOn)
                {
                    if (!Utils.GCATPP.isConnectedToSkyMind(t))
                        //Tentative connection au skymind 
                        if (!Utils.GCATPP.connectUser(t))
                            return false;

                    target = t;
                    kindOfTarget = 2;
                    return true;
                }
            }

            return false;
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


            var cso = controller.TryGetComp<CompSurrogateOwner>();
            if (cso != null)
            {
                if (kindOfTarget == 1)
                {
                    cso.controlMode = true;
                    cso.setControlledSurrogate((Pawn) target);
                }
                else if (kindOfTarget == 2)
                {
                    if (cso.skyCloudHost != null)
                    {
                        var csc = cso.skyCloudHost.TryGetComp<CompSkyCloudCore>();
                        if (csc != null) csc.setRemotelyControlledTurret(controller, (Building) target);
                    }
                }
            }

            if (!controller.VX3ChipPresent())
                Find.DesignatorManager.Deselect();
        }


        [DebuggerHidden]
        private bool SXInCell(IntVec3 c)
        {
            if (!c.Fogged(Map))
            {
                var thingList = c.GetThingList(Map);
                for (var i = 0; i < thingList.Count; i++)
                    if (thingList[i] is Pawn && CanDesignateThing(thingList[i]).Accepted)
                        return true;
            }

            return false;
        }

        private bool TurretInCell(IntVec3 c)
        {
            if (!c.Fogged(Map))
            {
                var thingList = c.GetThingList(Map);
                for (var i = 0; i < thingList.Count; i++)
                    if (thingList[i] != null && (thingList[i].def.thingClass == typeof(Building_Turret) || thingList[i].def.thingClass.IsSubclassOf(typeof(Building_Turret))) &&
                        CanDesignateThing(thingList[i]).Accepted)
                        return true;
            }

            return false;
        }


        protected override void FinalizeDesignationFailed()
        {
            base.FinalizeDesignationFailed();
        }
    }
}