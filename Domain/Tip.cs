using System;
using TipItService.Models;

namespace TipItService.Domain
{
    public class Tip
    {
        public MatchInfo Match { get; }
        public ProjectedScore Projected { get; }
        public Tip(
            MatchInfo mi,
            ProjectedScore ps)
        {
            Match = mi;
            Projected = ps;
        }

        public static Tip From(
            PredictedResult prediction)
        {
            var mi = MatchInfo.From(
                prediction.Game.League,
                prediction.Game.Round,
                prediction.Game.GameDate,
                "??",
                prediction.Game.HomeTeam,
                prediction.Game.AwayTeam,
                prediction.HomeScore,
                prediction.AwayScore);
            var ps = new ProjectedScore(
                MatchScore.From(
                    new LeagueCode(prediction.Game.League),
                    prediction.HomeScore),
                MatchScore.From(
                    new LeagueCode(prediction.Game.League),
                    prediction.AwayScore));
            return new Tip(mi, ps);
        }

        public string Winner()
        {
            if (Int32.Parse(Match.AwayScore.ToString()) > Int32.Parse(Match.HomeScore.ToString()))
                return Match.AwayTeam.Name;
            return Match.HomeTeam.Name;
        }

        public override string ToString() =>
        
            $@"{Match.MatchDateTime:u} {
                Match.HomeTeam
                } {
                Projected.HomeScore
                } v {Match.AwayTeam} {Projected.AwayScore}";

        public int Margin() =>

            Int32.Parse(Match.AwayScore.ToString()) - Int32.Parse(Match.HomeScore.ToString());
        
    }
}
