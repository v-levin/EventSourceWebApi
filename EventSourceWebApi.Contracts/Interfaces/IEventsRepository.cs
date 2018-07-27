using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        IEnumerable<Event> GetEvents();
        Event GetEvent(int id);
        void CreateEvent(Event @event);
        void UpdateEvent(Event @event);
        Event Find(int id);
        void DeleteEvent(Event eventToDelete);
    }
}
