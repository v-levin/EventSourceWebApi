using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsService
    {
        EventResponse GetEvents(EventSearchRequest request);
        EventResponse GetEvent(int id);
        EventResponse CreateEvent(Event @event);
        EventResponse UpdateEvent(int id, Event @event);
        Response DeleteEvent(int id);
    }
}
