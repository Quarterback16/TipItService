using System.Collections.Generic;

namespace TipItService.Domain
{
    public class TippingState
    {
        public List<MatchInfo> Matches { get; set; }
            = new List<MatchInfo>();
    }
}
