using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TipItService.Events;
using TipItService.Interfaces;

namespace TipItService.Implementations
{
    public class ScheduleEventStore : IEventStore
    {
        public List<ScheduleEvent> Events { get; set; }

        public ScheduleEventStore(
            string jsonFile = @"d:/dropbox/JSON/schedule.json")
        {
            var json = File.ReadAllText(jsonFile);
            Events = JsonConvert.DeserializeObject<List<ScheduleEvent>>(json);
        }

        //  Get all events for a specific aggregate (order by version)
        public IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEvent> Get<T>(string eventType)
        {
            return Events;
        }
    }
}
