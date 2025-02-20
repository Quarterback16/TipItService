using InjectorMicroService;
using LaYumba.Functional.Option;
using Newtonsoft.Json;
using OneOf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using TipItService.Domain;
using TipItService.Helpers;
using TipItService.Implementations;
using TipItService.Models;
using TipItService.TippingStrategies;
using WikiPages;

namespace TipItService
{
    public class TipItService
    {
        public const int K_Season = 2025;
        public TippingComp Comp { get; }

        public TippingContext TippingContext { get;  }
        public string DropBoxFolder { get; }
        public string TippingStateFileName { get; }

        public TippingState CurrentState { get; set; }

        public List<MatchInfo> NewResults { get; set; } = new List<MatchInfo>();

        public TipItService(
            string dropBoxFolder)
        {
            Comp = new TippingComp(
                new TippingSeason(K_Season),
                new List<TippingLeague>
                {
                    new TippingLeague(
                        new LeagueCode(
                            "NRL"),
                        new RoundsInSeason(
                            25),
                        new SourceUrl(
                            $"https://fixturedownload.com/feed/json/nrl-{K_Season}")),
                    new TippingLeague(
                        new LeagueCode(
                            "AFL"),
                        new RoundsInSeason(
                            22),
                        new SourceUrl(
                            $"https://fixturedownload.com/feed/json/afl-{K_Season}")),
                });
            CurrentState = new TippingState();
            DropBoxFolder = dropBoxFolder;
            TippingStateFileName = $"{dropBoxFolder}JSON\\Results.json";
            TippingContext = new TippingContext(
                dropBoxFolder,
                explain: false);
        }

        public int GetResultJson()
        {
            var client = new HttpClient();
            var optionsNewtonsoft = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            var totalResults = 0;
            Comp.Leagues.ForEach(
                league =>
                {
                    league.Results = LoadResults(league, client);
                    totalResults += league.Results.Count;
                    var jsonStringNewtonsoft = JsonConvert.SerializeObject(
                        league.Results, optionsNewtonsoft);
                    var fileName = $@"{DropBoxFolder}JSON\\{league.LeagueCode.Code}-{Comp.Season.Value}.json";
                    File.WriteAllText(
                        fileName,
                        jsonStringNewtonsoft);
                });

            return totalResults;
        }

        private static List<MatchJson> LoadResults(
            TippingLeague league, 
            HttpClient client)
        {
            string url = league.SourceUrl.Value;
            string jsonString = client.GetStringAsync(url).Result;
            var matches = JsonConvert.DeserializeObject<List<MatchJson>>(jsonString);
            var results = matches.Where(m => m.HomeTeamScore != null && m.AwayTeamScore != null)
                .OrderBy(m => m.DateUtc)
                .ToList();
            return results;
        }

        public int LoadTippingState()
        {
            List<MatchEventJson> matches = LoadMatchesFromJson();
            CurrentState.Matches = TippingStateFrom(matches);
            return matches.Count;
        }

        private List<MatchEventJson> LoadMatchesFromJson()
        {
            var jsonString = File.ReadAllText(
                TippingStateFileName);
            var matches = JsonConvert.DeserializeObject<List<MatchEventJson>>(
               jsonString);
            return matches;
        }

        private List<MatchInfo> TippingStateFrom(
            List<MatchEventJson> matches)
        {
            var list = new List<MatchInfo>();
            matches.ForEach(
                match => 
                {
                    // only clean data can get it
                    var matchInfo = MatchInfo.From(
                        match.League,
                        Int32.Parse(match.Round),
                        match.GameDate,
                        match.Location,
                        match.HomeTeam,
                        match.AwayTeam,
                        match.HomeScore,
                        match.AwayScore);
                    list.Add(matchInfo);
                });
            return list;
        }

        public int UpdateTippingState(
            DateTime focusDate)
        {
            var newState = GetNewTippingState(focusDate);
            WriteMatchEventJson(newState, TippingStateFileName);
            return newState.Count;
        }

        public void WriteMatchEventJson(
            List<MatchInfo> newState, 
            string fileName)
        {
            var optionsNewtonsoft = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };
            var matchEventState = ConvertToMatchEvents(newState);
            var jsonStringNewtonsoft = JsonConvert.SerializeObject(
                matchEventState, 
                optionsNewtonsoft);
            File.WriteAllText(
                fileName,
                jsonStringNewtonsoft);
        }

        private List<MatchEventJson> ConvertToMatchEvents(
            List<MatchInfo> newState)
        {
            var list = new List<MatchEventJson>();
            newState.ForEach(
                mi =>
                {
                    var mej = new MatchEventJson
                    {
                        Round = mi.Round.ToString(),
                        GameDate = mi.MatchDateTime,
                        Location = mi.MatchLocation,
                        HomeTeam = mi.HomeTeam.Code,
                        AwayTeam = mi.AwayTeam.Code,
                        League = mi.League.Code,
                        EventType = "schedule",
                    };
                    if (mi.HomeScore.Score != new None())
                    {
                        mej.HomeScore = mi.HomeScore.Score.Match(
                            Some: i => i,
                            None: () => -1);
                        mej.EventType = "result";
                    }
                    if (mi.AwayScore.Score != new None())
                    {
                        mej.AwayScore = mi.AwayScore.Score.Match(
                            Some: i => i,
                            None: () => -1);
                        mej.EventType = "result";
                    }
                    list.Add(mej);
                });
            return list;
        }


        public List<MatchInfo> GetNewTippingState(
            DateTime focusDate)
        {
            var allResults = LoadResults(Comp);
            List<MatchEventJson> oldMatches = LoadMatchesFromJson();
            var oldState = TippingStateFrom(oldMatches);
            var newState = new List<MatchInfo>();
            oldState.ForEach(
                m =>
                {
                    if (m.Played() 
                        || m.MatchDateTime.Year < focusDate.Year  
                        || m.MatchDateTime > focusDate)
                    {
                        newState.Add(m);
                    }
                    else 
                    {
                        // try to find a result
                        var newMatch = FindResult(
                            m,
                            allResults);
                        if (newMatch.Played())
                        {
                            Console.WriteLine($"New Result {newMatch}");
                            NewResults.Add(newMatch);
                        }
                        newState.Add(newMatch);
                    }
                });
            return newState;
        }

        private List<MatchJson> LoadResults(
            TippingComp comp)
        {
            var allResults = new List<MatchJson>();
            var client = new HttpClient();
            comp.Leagues.ForEach(
                league =>
                {
                    allResults.AddRange(
                        LoadResults(league, client));
                });
            return allResults;
        }

        private MatchInfo FindResult(
            MatchInfo m,
            List<MatchJson> allResults)
        {
            var result = allResults
                .Find(
                    r => r.RoundNumber == m.Round
                    && r.DateUtc.Date == m.MatchDateTime.Date
                    && r.HomeTeam == m.HomeTeam.JsonCode()
                    && r.AwayTeam == m.AwayTeam.JsonCode());
            if (result != null) 
            {
                m.AwayScore = MatchScore.From(
                    m.League,
                    result.AwayTeamScore);
                m.HomeScore = MatchScore.From(
                    m.League,
                    result.HomeTeamScore);
            }
            return m;
        }

        public (string,List<PredictedResult>) Tips()
        {
            var sb = new StringBuilder();
            var context = new TippingContext(
                DropBoxFolder,
                explain: false);
            var tipster = new NibbleTipster(context);
            Console.WriteLine("=== NRL ==================================================");
            sb.AppendLine(
                tipster.ShowTips(
                "NRL",
                context.NextRound("NRL")));
            Console.WriteLine();
            Console.WriteLine(
                tipster.DumpRatings(
                    "NRL"));
            tipster.ClearRatings();
            Console.WriteLine("=== AFL ==================================================");
            sb.AppendLine(
                tipster.ShowTips(
                "AFL",
                context.NextRound("AFL")));
            Console.WriteLine();
            Console.WriteLine(
                tipster.DumpRatings(
                    "AFL"));
            return (sb.ToString(), tipster.Predictions);
        }

        public TipSet Tipset()
        {
            var tipster = new NibbleTipster(TippingContext);
            var predictions = new List<PredictedResult>();
            tipster.ShowTips(
                "NRL",
                TippingContext.NextRound("NRL"));
            predictions.AddRange(tipster.Predictions);
            tipster.ShowTips(
                "AFL",
                TippingContext.NextRound("AFL"));
            predictions.AddRange(tipster.Predictions);
            return PredictionsToTipSet(predictions);
        }

        private TipSet PredictionsToTipSet(
            List<PredictedResult> predictions)
        {
            var tipset = new TipSet();
            foreach (var prediction in predictions) 
            {
                tipset.Tips.Add(Tip.From(prediction));
            }
            return tipset;
        }

        public string Inject(
            string leagueCode, 
            string tag, 
            MarkdownInjector mi)
        {
            var tipster = new NibbleTipster(TippingContext);
            var round = TippingContext.NextRound(leagueCode);
            var tips = tipster.ShowTips(
                leagueCode,
                round);
            Console.WriteLine(tips);
            var ts = PredictionsToTipSet(tipster.Predictions);
            var md = DashboardUtils.Tips(
                ts, 
                leagueCode,
                round);
            mi.InjectMarkdown(
                DashboardUtils.DashboardFile(TippingContext.CurrentSeason),
                tag,
                md);
            return md;
        }

        public string Easiest() =>
        
            EasyDash(
                EasyDashResults(
                    GetEasiestTips()));

        EasyResults EasyDashResults(EasiestTips easyTips)
        {
            var results = new EasyResults(easyTips)
            {
                RoundResults = RoundResults(K_Season)
            };
            return results;
        }

        string EasyDash(
            EasyResults easyResults)
        {
            var results = easyResults.RoundResults;
            var page = new WikiPage();
            page.AddHeading("Easiest Footy Tipping", 2);
            page.AddBlankLine();
            var t = new WikiTable();
            var cols = new List<WikiColumn>
            {
                new WikiColumn("Team"),
                new WikiColumn("Val"),
                new WikiColumn("EW"),
            };
            var nRounds = easyResults.RoundResults
                .GroupBy(er => er.Round).Select(g => g.First()).ToList();
            nRounds.ForEach(n =>
            {
                cols.Add(new WikiColumnRight(n.Round.ToString()));
            });
            cols.Add(new WikiColumnRight("Tot"));

            var colFns = new List<Func<EasySelection, string>>
            {
                (EasySelection es) => es.TeamCode,
                (EasySelection es) => es.PointsPerWin.ToString(),
                (EasySelection es) => es.ExpectedWins.ToString(),
            };
            t.AddCellData<EasySelection>(
                cols,
                colFns,
                easyResults.Selections.EasyTips,
                t);

            results.ForEach(r =>
            {
                var row = IndexOf(r.TeamCode);
                t.AddCell(row, r.Round + 2, r.Result.Match(w => "✅", l => "❌", u => "❔"));
            });
            t.AddRows(1);
            t.AddCell(easyResults.Selections.EasyTips.Count+1, 0, "Totals");
            var grandTotal = 0;
            nRounds.ForEach(n =>
            {
                var roundTotal = RoundTotal(n.Round, easyResults);
                grandTotal += roundTotal;
                t.AddCell(
                    row:easyResults.Selections.EasyTips.Count + 1, 
                    col: n.Round+2,
                    cellValue: roundTotal.ToString());

            });
            t.AddCell(
                row: easyResults.Selections.EasyTips.Count + 1,
                col: nRounds.Count + 3,
                cellValue: grandTotal.ToString());
            easyResults.Selections.EasyTips.ForEach(
                tip =>
                {
                    var row = IndexOf(tip.TeamCode);
                    t.AddCell(
                        row,
                        t.Columns.Count-1,
                        TotalFor(
                            tip.TeamCode, 
                            tip.PointsPerWin,
                            easyResults.RoundResults).ToString());
                });

            page.AddTable(t);
            return page.PageContents();
        }

        private int TotalFor(
            string teamCode, 
            int ptsPerWin,
            List<RoundResult> roundResults)
        {
            var wins = roundResults
                .Where(rr => rr.TeamCode == teamCode && rr.Result.TryPickT0(
                    out EasyWin win,
                    out OneOf<EasyLoss, EasyUnknown> remainder));
            return wins.Count() * ptsPerWin;
        }

        private int RoundTotal(
            int round, 
            EasyResults easyResults)
        {
            var wins = easyResults.RoundResults
                .Where(rr => rr.Round == round && rr.Result.TryPickT0(
                    out EasyWin win,
                    out OneOf<EasyLoss, EasyUnknown> remainder));
            var totalPoints = wins.Aggregate(
                0,
                (tot, w) => tot + PointsForWin(w.TeamCode,easyResults));
            return totalPoints;
        }

        private int PointsForWin(string teamCode, EasyResults easyResults) =>

            easyResults.Selections.EasyTips
                .Find(s => s.TeamCode == teamCode)
                .PointsPerWin;

        int IndexOf(string teamCode) =>
        
            GetEasiestTips().EasyTips.FindIndex(es => es.TeamCode == teamCode)+1;        

        private EasiestTips GetEasiestTips() =>
            new EasiestTips
            {
                EasyTips = new List<EasySelection>
                {
                    new EasySelection
                    {
                        TeamCode = "CANB",
                        PointsPerWin = 14,
                        ExpectedWins = 15
                    },
                    new EasySelection
                    {
                        TeamCode = "NQLD",
                        PointsPerWin = 11,
                        ExpectedWins = 16
                    },
                    new EasySelection
                    {
                        TeamCode = "DOLP",
                        PointsPerWin = 17,
                        ExpectedWins = 8
                    },
                    new EasySelection
                    {
                        TeamCode = "PARR",
                        PointsPerWin = 13,
                        ExpectedWins = 13
                    },
                    new EasySelection
                    {
                        TeamCode = "NEWC",
                        PointsPerWin = 16,
                        ExpectedWins = 12
                    },
                    new EasySelection
                    {
                        LeagueCode = "AFL",
                        TeamCode = "PORT",
                        PointsPerWin = 12,
                        ExpectedWins = 22
                    },
                    new EasySelection
                    {
                        LeagueCode = "AFL",
                        TeamCode = "RICH",
                        PointsPerWin = 24,
                        ExpectedWins = 9
                    },
                    new EasySelection
                    {
                        LeagueCode = "AFL",
                        TeamCode = "WB",
                        PointsPerWin = 11,
                        ExpectedWins = 19
                    },
                    new EasySelection
                    {
                        LeagueCode = "AFL",
                        TeamCode = "HSTK",
                        PointsPerWin = 16,
                        ExpectedWins = 9
                    },
                    new EasySelection
                    {
                        LeagueCode = "AFL",
                        TeamCode = "MELB",
                        PointsPerWin = 14,
                        ExpectedWins = 13
                    },
                }
            };

        public List<RoundResult> RoundResults(int season)
        {
            LoadTippingState();
            var results = new List<RoundResult>();
            var easyTips = GetEasiestTips();
            var easyTeams = EasyTeams(easyTips);
            CurrentState.Matches.ForEach(
                m =>
                {
                    if (m.MatchDateTime.Year == season && m.Played())
                    {
                        if (MatchInvolvesHomeTeam(
                            m,
                            easyTeams))
                        {
                            results.Add(
                                RoundResult.From(
                                    m,
                                    m.HomeTeam.Code));
                        }
                        if (MatchInvolvesAwayTeam(
                            m,
                            easyTeams))
                        {
                            results.Add(
                                RoundResult.From(
                                    m,
                                    m.AwayTeam.Code));
                        }
                    }
                });
            return results;
        }

        private bool MatchInvolvesHomeTeam(
            MatchInfo m,
            List<LeagueTeam> leagueTeams)
        {
            var involves = leagueTeams
                .Where(t => t.League == m.League.Code
                    && m.HomeTeam.Code == t.Code);
            return involves.Any();
        }

        private bool MatchInvolvesAwayTeam(
            MatchInfo m, 
            List<LeagueTeam> leagueTeams)
        {
            var involves = leagueTeams
                .Where(t => t.League == m.League.Code
                    && m.AwayTeam.Code == t.Code);
            return involves.Any();
        }

        private List<LeagueTeam> EasyTeams(EasiestTips easyTips)
        {
            var teams = new List<LeagueTeam>();
            foreach (var tip in easyTips.EasyTips) 
            {
                teams.Add(
                    LeagueTeam.From(
                        new LeagueCode(tip.LeagueCode),
                        tip.TeamCode));
            }
            return teams;
        }

        public List<MatchInfo> GetRound(
            int round, 
            string league, 
            int season) =>
        
            CurrentState.Matches
                .Where(m => m.League.Code == league 
                    && m.MatchDateTime.Year == season 
                    && m.Round == round)
                .ToList();

        public string InjectRankings(
            string leagueCode,
            string tagName, 
            MarkdownInjector mi)
        {
            var tipster = new NibbleTipster(TippingContext);
            tipster.Rate(leagueCode);

            var md = tipster.DumpRatingsAsMarkdown(leagueCode);

            mi.InjectMarkdown(
                DashboardUtils.DashboardFile(
                    TippingContext.CurrentSeason),
                tagName,
                md);
            return md;
        }
    }
}
