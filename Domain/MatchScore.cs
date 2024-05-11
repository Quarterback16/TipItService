using LaYumba.Functional;
using LaYumba.Functional.Option;

namespace TipItService.Domain
{
    public class MatchScore
    {
        public Option<int> Score { get; set; }
        private MatchScore(
            Option<int> score)
        {
            Score = score;
        }

        public static MatchScore From(
            LeagueCode leagueCode,
            int? score)
        {
            if (score == null)
            {
                return new MatchScore(new None());
            }
            switch (leagueCode.Code)
            {
                case "NRL":
                    if (score < 0 || score > 80)
                        return new MatchScore(new None());
                    break;
                case "AFL":
                    if (score < 0 || score > 200)
                        return new MatchScore(new None());
                    break;
                default:
                    return new MatchScore(new None());
            }
            return new MatchScore((Option<int>)score);
        }

        public override string ToString() => 
            $@"{Score.Match( 
                Some: n => n.ToString(), 
                None : () => string.Empty)}";

    }
}
