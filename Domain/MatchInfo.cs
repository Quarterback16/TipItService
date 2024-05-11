using LaYumba.Functional.Option;
using System;

namespace TipItService.Domain
{
    public class MatchInfo
    {
        public int Round { get; }
        public LeagueCode League { get; set; }
        public LeagueTeam HomeTeam { get; set; }
        public LeagueTeam AwayTeam { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string MatchLocation { get; set; }

        public MatchScore HomeScore { get; set; }
        public MatchScore AwayScore { get; set; }

        private MatchInfo(
            LeagueCode leagueCode,
            int round,
            LeagueTeam homeTeam,
            LeagueTeam awayTeam,
            DateTime matchDateTime,
            string matchLocation,
            MatchScore homeScore,
            MatchScore awayScore)
        {
            League = leagueCode;
            Round = round;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            HomeScore = homeScore;
            AwayScore = awayScore;
            MatchDateTime = matchDateTime;
            MatchLocation = matchLocation;
        }

        public static MatchInfo From(
            string leagueCode,
            int round,
            DateTime matchDateTime,
            string matchLocation,
            string homeTeamCode,
            string awayTeamCode,
            int? homeScore,
            int? awayScore)
        {
            var league = new LeagueCode(leagueCode);
            return new MatchInfo(
                league,
                round,
                LeagueTeam.From(league, homeTeamCode),
                LeagueTeam.From(league, awayTeamCode),
                matchDateTime,
                matchLocation,
                MatchScore.From(league, homeScore),
                MatchScore.From(league, awayScore));
        }

        public bool Played() =>
            HomeScore.Score != new None() 
            && AwayScore.Score != new None();

        public override string ToString() =>

            $"{League.Code} : {MatchDateTime:u} : {HomeTeam.Code} vs {AwayTeam.Code} {HomeScore}-{AwayScore}";

        public bool IsWinner(string teamCode)
        {
            if (HomeTeam.Code == teamCode)
            {
                return 
                    HomeScore.Score.Match(Some: i => i, None: () => 1)
                    > AwayScore.Score.Match(Some: i => i, None: () => -1);
            }
            else if (AwayTeam.Code == teamCode)
            {
                return
                    AwayScore.Score.Match(Some: i => i, None: () => 1)
                    > HomeScore.Score.Match(Some: i => i, None: () => -1);
            }
            return false;
        }

        public bool IsLoser(string teamCode) => !IsWinner(teamCode);
    }
}
