using Newtonsoft.Json;
using System;

namespace TipItService.Models
{
    public class MatchJson
    {
        [JsonProperty("MatchNumber")]
        public int MatchNumber { get; set; }

        [JsonProperty("RoundNumber")]
        public int RoundNumber { get; set; }

        [JsonProperty("DateUtc")]
        public DateTime DateUtc { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        [JsonProperty("HomeTeamScore")]
        public int? HomeTeamScore { get; set; }

        [JsonProperty("AwayTeamScore")]
        public int? AwayTeamScore { get; set; }
    }
}
