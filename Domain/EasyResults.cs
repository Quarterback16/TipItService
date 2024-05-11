using System.Collections.Generic;

namespace TipItService.Domain
{
    public class EasyResults
    {
        public EasiestTips Selections { get; set; }
        public List<RoundResult> RoundResults { get; set; } = new List<RoundResult>();

        public EasyResults(EasiestTips easyTips)
        {
            Selections = easyTips;
        }
    }
}
