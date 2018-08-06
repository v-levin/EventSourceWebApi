using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;

namespace EventSourceApiHttpClient
{
    public class EventSourceApiHttpClient
    {
        public static void Main()
        {
            var client = new EventsClient("http://localhost:49999/api/", "application/json");

            //var events = client.GetEvents(new EventSearchRequest() { City = "skopje" });
            //var @event = client.GetEvent(new EventIdRequest() { Id = 16 });
            //var id = client.PostEvent(new Event() { Name = "Http Client", Seats = 400, Description = "Http Client Desc", City = "Skopje", Category = "Art", Location = "Central Park", DateRegistered = DateTime.Now });
        }
    }
}
