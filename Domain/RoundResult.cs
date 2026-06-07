using OneOf;
using System;

namespace TipItService.Domain
{
    public class RoundResult
    {
        public LeagueCode League { get; set; }
        public int Round { get; set; }
        public string TeamCode { get; set; }
        public OneOf<EasyWin, EasyLoss, EasyUnknown> Result { get; set; }

        private RoundResult(
            LeagueCode league,
            int round,
            string teamCode,
            OneOf<EasyWin, EasyLoss, EasyUnknown> result)
        {
            League = league;
            Round = round;
            TeamCode = teamCode;
            Result = result;
        }

        public override string ToString() => $"{Round} {TeamCode} {Result}";

        public static RoundResult From(
            MatchInfo matchInfo,
            string teamCode)
        {
            if (matchInfo==null)
            {
                throw new ArgumentException(
                    "No Match Info", nameof(matchInfo));
            }
            if (string.IsNullOrEmpty(teamCode))
            {
                throw new ArgumentException(
                    "No team code", nameof(teamCode));
            }
            return new RoundResult(
                matchInfo.League,
                matchInfo.Round,
                teamCode,
                ResultFor(teamCode, matchInfo));            
        }

        private static OneOf<EasyWin, EasyLoss, EasyUnknown> ResultFor(
            string teamCode, 
            MatchInfo matchInfo)
        {
            if (matchInfo.IsWinner(teamCode))
                return new EasyWin();
            else if (matchInfo.IsLoser(teamCode))
                return new EasyLoss();
            return new EasyUnknown();
        }
    }
}
