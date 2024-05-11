using System;

namespace TipItService.Domain
{
    public class ScheduledMatch
    {
        public string League { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string MatchLocation { get; set; }
    }
}
