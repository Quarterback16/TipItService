using System;
using TipItService.Events;

namespace TipItService.Models
{
    public class Game
    {
        public string League { get; set; }
        public int Match { get; set; }
        public int Round { get; set; }
        public DateTime GameDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        public PredictedResult PredictedResult { get; set; }

        public Game()
        {
        }

        public Game(ScheduleEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
        }

        public Game(ResultEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
            HomeScore = e.HomeScore;
            AwayScore = e.AwayScore;
        }

        public string Result()
        {
            return $"{AwayScore} - {HomeScore}";
        }

        public override string ToString()
        {
            return $@"{League} Season {Season()} Round {Round} : {GameDate.ToString("yyyy-MM-dd HH:mm")} {AwayTeam,-4} {AwayScore,2} @ {HomeTeam,-4} {HomeScore,2}";
        }

        internal bool WinFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode) && HomeScore > AwayScore)
                return true;
            if (AwayTeam.Equals(teamCode) && AwayScore > HomeScore)
                return true;
            return false;
        }

        internal bool LossFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode) && AwayScore > HomeScore)
                return true;
            if (AwayTeam.Equals(teamCode) && HomeScore > AwayScore)
                return true;
            return false;
        }

        internal int WinningMargin()
        {
            return Math.Abs(HomeScore.Value - AwayScore.Value);
        }

        public string GameLine(
            string teamCode)
        {
            var line = $@"{League} Rd {Round,2} {GameDate.ToString("yyyy-MM-dd")} {ResultFor(teamCode)} {ScoreFor(teamCode),2} - {ScoreAgin(teamCode),2}";
            return line;
        }

        public string Season()
        {
            return GameDate.ToString("yyyy");
        }

        private string ScoreAgin(string teamCode)
        {
            if (IsHomeTeam(teamCode))
                return $"{AwayScore}";
            return $"{HomeScore}";
        }

        private string ScoreFor(string teamCode)
        {
            if (IsHomeTeam(teamCode))
                return $"{HomeScore}";
            return $"{AwayScore}";
        }

        public bool IsHomeTeam(string teamCode)
        {
            return HomeTeam.Equals(teamCode);
        }

        private string ResultFor(
            string teamCode)
        {
            if (WinFor(teamCode))
                return "W";
            else if (LossFor(teamCode))
                return "L";
            return "T";
        }

        internal bool Involves(
            string teamCode)
        {
            if (HomeTeam.Equals(teamCode))
                return true;
            if (AwayTeam.Equals(teamCode))
                return true;
            return false;
        }

        internal string GameResultShort(
            string teamCode)
        {
            var shortResult = "T  ";
            if (WinFor(teamCode))
                shortResult = $"+{WinningMargin(),2}";
            else if (LossFor(teamCode))
                shortResult = $"-{WinningMargin(),2}";
            return $"R{Round,2} {shortResult}";
        }

        internal bool HasBeenPlayed()
        {
            return (AwayScore + HomeScore) > 0;
        }

        public string OpponentOf(
            string teamCode)
        {
            if (AwayTeam.Equals(teamCode))
                return HomeTeam;
            return AwayTeam;
        }
    }
}
