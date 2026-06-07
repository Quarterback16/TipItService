using TipItService.Domain;

namespace TipItService.Models
{
    public class MarcoInfo
    {
        public string Team { get; set; }
        public LeagueTeam LeagueTeam { get; set; }
        public int EasyReward { get; set; }
        public int EasyPointTotal { get; set; }
        public int Wins { get; set; }
        public bool Selected { get; set; }
    }
}
