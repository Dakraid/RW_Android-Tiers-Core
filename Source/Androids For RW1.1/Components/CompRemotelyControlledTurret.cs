using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    public class CompRemotelyControlledTurret : ThingComp
    {
        public Pawn controller;

        private CompSkyMind csm;

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_References.Look(ref controller, "ATPP_RemoteTurretController");
        }

        public override void PostDraw()
        {
            Material avatar = null;

            Designator_AndroidToControl desi = null;
            var isConnected = csm != null && csm.connected;

            if (Find.DesignatorManager.SelectedDesignator is Designator_AndroidToControl)
                desi = (Designator_AndroidToControl) Find.DesignatorManager.SelectedDesignator;

            if (desi != null && desi.fromSkyCloud) avatar = Tex.SelectableSX;

            if (isConnected)
                if (controller != null)
                    avatar = Tex.RemotelyControlledNode;

            if (avatar == null) return;
            
            var vector = parent.TrueCenter();
            vector.y = AltitudeLayer.MetaOverlays.AltitudeFor() + 0.28125f;
            vector.z += 1.4f;
            vector.x += parent.def.size.x / 2;

            Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, avatar, 0);
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            disconnectConnectedMind();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            CompSurrogateOwner csc = null;

            if (controller != null) csc = controller.TryGetComp<CompSurrogateOwner>();

            if (csm != null && csm.connected && controller != null && csc != null && csc.skyCloudHost != null && csc.skyCloudHost.Map == parent.Map)
                GenDraw.DrawLineBetween(parent.TrueCenter(), csc.skyCloudHost.TrueCenter(), SimpleColor.Red);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            csm = parent.TryGetComp<CompSkyMind>();
        }

        private void disconnectConnectedMind()
        {
            var cso = controller?.TryGetComp<CompSurrogateOwner>();
            if (cso?.skyCloudHost == null) return;
                
            var csc = cso.skyCloudHost.TryGetComp<CompSkyCloudCore>();
            csc?.stopRemotelyControlledTurret(controller);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (controller != null)
                //Boutton permettant deconnection de la tourelle du pawn controller
                yield return new Command_Action
                {
                    icon = Tex.AndroidToControlTargetDisconnect,
                    defaultLabel = "ATPP_AndroidToControlTargetDisconnect".Translate(),
                    defaultDesc = "ATPP_AndroidToControlTargetDisconnectDesc".Translate(),
                    action = delegate { disconnectConnectedMind(); }
                };
        }

        public override string CompInspectStringExtra()
        {
            var ret = "";

            if (controller != null)
                ret += "ATPP_RemotelyControlledBy".Translate(controller.LabelShortCap) + "\n";

            return ret.TrimEnd('\r', '\n') + base.CompInspectStringExtra();
        }
    }
}