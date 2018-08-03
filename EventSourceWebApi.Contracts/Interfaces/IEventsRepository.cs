using EventSourceWebApi.Contracts.Requests;
using EventSourceWebApi.Contracts.Responses;

namespace EventSourceWebApi.Contracts.Interfaces
{
    public interface IEventsRepository
    {
        EventsResponse GetEvents(EventSearchRequest request);
        EventResponse GetEvent(EventIdRequest id);
        EventResponse CreateEvent(PostRequest<Event> postRequest);
        EventResponse UpdateEvent(PutRequest<Event> putRequest);
        Response DeleteEvent(EventIdRequest idRequest);
    }
}
