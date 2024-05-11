namespace TipItService.Domain
{
    public class ProjectedScore
    {
        public MatchScore HomeScore { get; }
        public MatchScore AwayScore { get; }
        public ProjectedScore(
            MatchScore homeScore,
            MatchScore awayScore)
        {
            HomeScore = homeScore;
            AwayScore = awayScore;
        }
    }
}
