using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TipItService.Events;
using TipItService.Interfaces;

namespace TipItService.Implementations
{
    public class TeamEventStore : IEventStore
    {
        public List<AddTeamEvent> Events { get; set; }

        public TeamEventStore(
            string dropboxFolder)
        {
            var r = new StreamReader($"{dropboxFolder}JSON//teams.json");
            var json = r.ReadToEnd();
            Events = JsonConvert.DeserializeObject<List<AddTeamEvent>>(json);
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
