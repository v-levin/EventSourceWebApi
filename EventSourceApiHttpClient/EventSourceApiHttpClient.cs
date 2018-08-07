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

            //var getEvents = client.GetEvents(new EventSearchRequest() { City = "skopje" });
            //var getEvent = client.GetEvent(new EventIdRequest() { Id = 16 });
            //var post = client.PostEvent(new Event() { Name = "Http Client", Seats = 400, Description = "Http Client Desc", City = "Skopje", Category = "Art", Location = "Central Park", DateRegistered = DateTime.Now });
            //var put = client.PutEvent(26, new Event() { Id = 26, City = "Kumanovo", Description = "Desc Photo", Category = "Art", Seats = 80 });
            //var delete = client.DeleteEvent(25);
        }
    }
}
