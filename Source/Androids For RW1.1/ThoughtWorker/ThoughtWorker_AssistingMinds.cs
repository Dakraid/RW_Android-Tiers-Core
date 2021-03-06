﻿using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class ThoughtWorker_AssistedByMinds : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.RaceProps.Humanlike) return false;
            if (!p.IsAndroidTier() && !p.VXChipPresent()) return false;
            if (!Utils.GCATPP.isConnectedToSkyMind(p)) return false;

            var num = Utils.GCATPP.getNbAssistingMinds();
            return num > 0 ? ThoughtState.ActiveAtStage(0) : false;
        }
    }
}