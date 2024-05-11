using System.Collections.Generic;

namespace TipItService.Interfaces
{
    interface IEventStore
    {
        IEnumerable<IEvent> Get<T>(string eventType);
    }
}
