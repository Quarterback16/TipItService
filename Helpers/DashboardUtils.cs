using System;
using System.Collections.Generic;
using System.Linq;
using TipItService.Domain;
using TipItService.Models;
using WikiPages;

namespace TipItService.Helpers
{
    public static class DashboardUtils
    {
        public static string Tips(
            TipSet tipset, 
            string leagueCode,
            int roundNo)
        {
            var desiredTipset = FilterTipset(tipset,leagueCode);
            var widget = new Widget
            {
                Name = "Tip Table",
                MdFunc = TipTable
            };

            return widget.MdFunc(
                desiredTipset, 
                leagueCode, 
                roundNo);
        }

        public static string DashboardFile(int season) =>
            $"Footy Tips {season}.md";

        private static TipSet FilterTipset(
            TipSet tipset, 
            string leagueCode) =>
                new TipSet(
                    tipset.Tips
                    .Where(
                        t => t.Match.League.Code == leagueCode)
                    .ToList());

        private static string TipTable(
            TipSet ts,
            string leagueCode,
            int roundNo)
        {
            var p = new WikiPage();
            p.AddHeading($"{leagueCode} Round {roundNo}", 4);
            p.AddBlankLine();
            var t = new WikiTable();

            t.AddCellData<Tip>(
                new List<WikiColumn>
                {
                    new WikiColumn("When"),
                    new WikiColumnCentred("HT"),
                    new WikiColumnRight("HS"),
                    new WikiColumnCentred("AT"),
                    new WikiColumnRight("AS"),
                    new WikiColumnRight("Marg"),
                },
                new List<Func<Tip, string>>
                {
                    (Tip tip) => $@"{
                        tip.Match.MatchDateTime.ToString("D").Substring(0,2)
                        } {
                        tip.Match.MatchDateTime.ToString("MM-dd HH:mm")
                        }",
                    (Tip tip) => IsWinner(tip,tip.Match.HomeTeam.Name),
                    (Tip tip) => tip.Projected.HomeScore.ToString(),
                    (Tip tip) => IsWinner(tip,tip.Match.AwayTeam.Name),
                    (Tip tip) => tip.Projected.AwayScore.ToString(),
                    (Tip tip) => tip.Margin().ToString(),
                },
                ts.Tips,
                t);
            p.AddTable(t);
            return p.PageContents();
        }

        private static string IsWinner(Tip tip, string name) =>
        
            (tip.Winner() == name) ? $"=={name}==" : name;
        
    }
}
