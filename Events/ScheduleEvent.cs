using Newtonsoft.Json;
using System;
using TipItService.Interfaces;

namespace TipItService.Events
{
    public class ScheduleEvent : IEvent
    {
        [JsonProperty("EventType")]
        public string EventType { get; set; }  // basically the aggregate

        [JsonProperty("League")]
        public string LeagueCode { get; set; }

        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("match")]
        public int Match { get; set; }

        [JsonProperty("gamedate")]
        public DateTime GameDate { get; set; }

        [JsonProperty("HomeTeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("AwayTeam")]
        public string AwayTeam { get; set; }

        public Guid Id
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public int Version
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public DateTimeOffset TimeStamp
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
