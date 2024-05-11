using OneOf;
using System;

namespace TipItService.Domain
{
    public class RoundResult
    {
        public int Round { get; set; }
        public string TeamCode { get; set; }
        public OneOf<EasyWin, EasyLoss, EasyUnknown> Result { get; set; }

        private RoundResult(
            int round,
            string teamCode,
            OneOf<EasyWin, EasyLoss, EasyUnknown> result)
        {
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
