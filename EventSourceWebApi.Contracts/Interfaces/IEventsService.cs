using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsService
    {
        EventResponse GetEvents();
        EventResponse GetEvent(int id);
        EventResponse CreateEvent(Event @event);
        void UpdateEvent(Event @event);
        Event Find(int id);
        void DeleteEvent(Event eventToDelete);
    }
}
