using System.Collections.Generic;
using TipItService.Models;

namespace TipItService.Domain
{
    public class TippingLeague
    {
        public LeagueCode LeagueCode { get;  }
        public RoundsInSeason Rounds { get; }
        public SourceUrl SourceUrl { get; }

        public List<MatchJson> Results { get; set; } = new List<MatchJson>();

        public TippingLeague(
            LeagueCode leagueCode,
            RoundsInSeason rounds,
            SourceUrl sourceUrl)
        {
            LeagueCode = leagueCode;
            Rounds = rounds;
            SourceUrl = sourceUrl;
        }
    }
}
