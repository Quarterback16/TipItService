namespace TipItService.Domain
{
    public class PlayedMatch
    {
        public ScheduledMatch ScheduledMatch { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public PlayedMatch(
            ScheduledMatch schedMatch, 
            int awayScore, 
            int homeScore)
        {
            ScheduledMatch = schedMatch;
            AwayScore = awayScore;
            HomeScore = homeScore;
        }
    }
}
