using System.Collections.Generic;

namespace TipItService.Domain
{
    public class TippingComp
    {
        public List<TippingLeague> Leagues { get; }
        public TippingSeason Season { get; }

        public TippingComp(
            TippingSeason season,
            List<TippingLeague> leagues)
        {
            Season = season;
            Leagues = leagues;
        }
    }
}
