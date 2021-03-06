﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Designator_AndroidToControl : Designator
    {
        private readonly Pawn controller;
        private Map cmap;
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

        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            if (!c.InBounds(Map)) return false;


            if (SXInCell(c)) return true;

            if (fromSkyCloud)
            {
                if (!TurretInCell(c))
                    return "ATPP_DesignatorNeedSelectSX".Translate();
            }
            else
            {
                return "ATPP_DesignatorNeedSelectSX".Translate();
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

            if (t is Pawn cp)
            {
                var csm = cp.TryGetComp<CompSkyMind>();
                var cas = cp.TryGetComp<CompAndroidState>();


                if (cas == null || !cas.isSurrogate || cas.surrogateController != null || csm.Infected != -1)
                    return false;

                if (!Utils.GCATPP.isConnectedToSkyMind(cp))

                    if (!Utils.GCATPP.connectUser(cp))
                        return false;

                target = cp;
                kindOfTarget = 1;
                return true;
            }

            if (!fromSkyCloud || t.def.thingClass != typeof(Building_Turret) && !t.def.thingClass.IsSubclassOf(typeof(Building_Turret))) return false;

            var crt = t.TryGetComp<CompRemotelyControlledTurret>();

            if (crt == null || crt.controller != null || t.IsBrokenDown() || !t.TryGetComp<CompPowerTrader>().PowerOn) return false;

            if (!Utils.GCATPP.isConnectedToSkyMind(t))

                if (!Utils.GCATPP.connectUser(t))
                    return false;

            target = t;
            kindOfTarget = 2;
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


            var cso = controller.TryGetComp<CompSurrogateOwner>();
            if (cso != null)
                switch (kindOfTarget)
                {
                    case 1:
                        cso.controlMode = true;
                        cso.setControlledSurrogate((Pawn) target);
                        break;
                    case 2:
                    {
                        var csc = cso.skyCloudHost?.TryGetComp<CompSkyCloudCore>();
                        csc?.setRemotelyControlledTurret(controller, (Building) target);
                        break;
                    }
                }

            if (!controller.VX3ChipPresent())
                Find.DesignatorManager.Deselect();
        }


        private bool SXInCell(IntVec3 c)
        {
            if (c.Fogged(Map)) return false;

            var thingList = c.GetThingList(Map);
            return Enumerable.Any(thingList, t => t is Pawn && CanDesignateThing(t).Accepted);
        }

        private bool TurretInCell(IntVec3 c)
        {
            if (c.Fogged(Map)) return false;

            var thingList = c.GetThingList(Map);
            return Enumerable.Any(thingList,
                t => t != null && (t.def.thingClass == typeof(Building_Turret) || t.def.thingClass.IsSubclassOf(typeof(Building_Turret))) && CanDesignateThing(t).Accepted);
        }
    }
}