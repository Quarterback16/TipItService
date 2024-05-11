using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TipItService.Events;
using TipItService.Interfaces;

namespace TipItService.Implementations
{
    public class ResultEventStore : IEventStore
    {
        public List<ResultEvent> Events { get; set; }

        public ResultEventStore(string dropboxFolder)
        {
            var json = File.ReadAllText($"{dropboxFolder}JSON//results.json");
            Events = JsonConvert.DeserializeObject<List<ResultEvent>>(
                json);
        }

        //  Get all events for a specific aggregate (order by version)
        public IEnumerable<IEvent> Get<T>(
            Guid aggregateId,
            int fromVersion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEvent> Get<T>(
            string eventType)
        {
            return Events;
        }
    }
}
