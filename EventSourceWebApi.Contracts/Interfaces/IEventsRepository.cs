using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        EventResponse GetEvents(Request request);
        EventResponse GetEvent(int id);
        EventResponse CreateEvent(Event @event);
        EventResponse UpdateEvent(Event @event);
        Response DeleteEvent(int id);
    }
}
