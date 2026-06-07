using Newtonsoft.Json;

namespace TipItService.Models
{
    public class EasyPointsJson
    {
        [JsonProperty("Season")]
        public string Season { get; set; }
        [JsonProperty("League")]
        public string LeagueCode { get; set; }
        [JsonProperty("TeamCode")]
        public string TeamCode { get; set; }
        [JsonProperty("EasyPoints")]
        public int EasyPoints { get; set; }
    }
}
