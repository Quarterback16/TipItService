using System;
using System.Collections.Generic;
using System.Linq;
using TipItService.Models;
using WikiPages;

namespace TipItService.Helpers
{
    public static class MarkdownHelper
    {
        public static string MarcoReportToMd(
            List<MarcoInfo> teamPoints, 
            string leagueCode, 
            int season)
        {
            var page = new WikiPageWithTable();
            page.AddHeading($"Marco Report {leagueCode} {season}", 2);
            page.AddBlankLine();
            page.Table.AddColumn("Team");
            page.Table.AddColumnRight("Reward");
            page.Table.AddColumnRight("Wins");
            page.Table.AddColumnRight("Points");
            page.Table.AddRows(teamPoints.Count);
            var nRow = 0;
            foreach (var teamPoint in teamPoints
                .OrderByDescending(t=>t.EasyPointTotal))
            {
                page.Table.AddCell(++nRow, "Team", TeamName(teamPoint));
                page.Table.AddCell(nRow, "Reward", teamPoint.EasyReward.ToString());
                page.Table.AddCell(nRow, "Wins", teamPoint.Wins.ToString());
                page.Table.AddCell(nRow, "Points", teamPoint.EasyPointTotal.ToString());
            }
            return page.PageTableContents();
        }

        private static string TeamName(
            MarcoInfo teamPoint)
        {
            var highlight = teamPoint.Selected ? "==" : "";
            return $"{highlight}{teamPoint.LeagueTeam.JsonCode()}{highlight}";
        }
    }
}   
