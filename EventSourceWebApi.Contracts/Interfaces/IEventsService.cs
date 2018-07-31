using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsService
    {
        EventResponse GetEvents(EventSearchRequest request);
        EventResponse GetEvent(IdRequest id);
        EventResponse CreateEvent(PostRequest<Event> postRequest);
        EventResponse UpdateEvent(int id, Event @event);
        Response DeleteEvent(int id);
    }
}
