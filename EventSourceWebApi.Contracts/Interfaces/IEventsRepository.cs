using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        EventResponse GetEvents(EventSearchRequest request);
        EventResponse GetEvent(IdRequest id);
        EventResponse CreateEvent(PostRequest<Event> postRequest);
        EventResponse UpdateEvent(Event @event);
        Response DeleteEvent(int id);
    }
}
