using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace MOARANDROIDS
{
    public class IncidentWorker_DownedT5Android : IncidentWorker
    {
        private static readonly IntRange TimeoutDaysRange = new IntRange(12, 20);

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            var x = new List<SitePartDef>();
            x.Add(SitePartDefOf.DownedT5Android);
            return base.CanFireNowSub(parms) && TryFindTile(out var num) && SiteMakerHelper.TryFindRandomFactionFor(x, out var faction);
        }

        private bool TryFindTile(out int tile)
        {
            return TileFinder.TryFindNewSiteTile(out tile, 7, 15, true, false);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (!TryFindFactions(out var faction, out var faction2)) return false;
            if (!TileFinder.TryFindNewSiteTile(out var tile, 8, 30)) return false;
            var site = SiteMaker.MakeSite(SitePartDefOf.DownedT5Android, tile, faction2);
            site.Tile = tile;
            var randomInRange = TimeoutDaysRange.RandomInRange;
            site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
            Find.WorldObjects.Add(site);

            var labelText = def.letterLabel;
            var letterText = def.letterText;

            Find.LetterStack.ReceiveLetter(labelText, letterText, def.letterDef, site);
            return true;
        }

        private bool TryFindFactions(out Faction alliedFaction, out Faction enemyFaction)
        {
            if ((from x in Find.FactionManager.AllFactions
                where !x.def.hidden && !x.defeated && !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && CommonHumanlikeEnemyFactionExists(Faction.OfPlayer, x) &&
                      !AnyQuestExistsFrom(x)
                select x).TryRandomElement(out alliedFaction))
            {
                enemyFaction = CommonHumanlikeEnemyFaction(Faction.OfPlayer, alliedFaction);
                return true;
            }

            alliedFaction = null;
            enemyFaction = null;
            return false;
        }

        private bool AnyQuestExistsFrom(Faction faction)
        {
            var sites = Find.WorldObjects.Sites;
            for (var i = 0; i < sites.Count; i++)
            {
                var component = sites[i].GetComponent<DefeatAllEnemiesQuestComp>();
                if (component != null && component.Active && component.requestingFaction == faction) return true;
            }

            return false;
        }

        private bool CommonHumanlikeEnemyFactionExists(Faction f1, Faction f2)
        {
            return CommonHumanlikeEnemyFaction(f1, f2) != null;
        }

        private Faction CommonHumanlikeEnemyFaction(Faction f1, Faction f2)
        {
            if ((from x in Find.FactionManager.AllFactions
                where x != f1 && x != f2 && !x.def.hidden && x.def.humanlikeFaction && !x.defeated && x.HostileTo(f1) && x.HostileTo(f2)
                select x).TryRandomElement(out var result))
                return result;
            return null;
        }
    }
}