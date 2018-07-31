using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        EventResponse GetEvents(EventSearchRequest request);
        EventResponse GetEvent(int id);
        EventResponse CreateEvent(Event @event);
        EventResponse UpdateEvent(Event @event);
        Response DeleteEvent(int id);
    }
}
