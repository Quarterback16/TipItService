using System;

namespace TipItService.Models
{
    public class MatchEventJson
    {
        public string Round { get; set; }
        public DateTime GameDate { get; set; }
        public string Location { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string EventType { get; set; }
        public string League { get; set; }
    }
}
