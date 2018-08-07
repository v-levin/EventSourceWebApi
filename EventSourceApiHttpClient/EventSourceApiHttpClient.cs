using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;

namespace EventSourceApiHttpClient
{
    public class EventSourceApiHttpClient
    {
        public static void Main()
        {
            var baseUrl = "http://localhost:49999/api/";
            var acceptHeader = "application/json";

            var eventClient = new EventsClient(baseUrl, acceptHeader); 

            var placeClient = new PlacesClient(baseUrl, acceptHeader);

            var placeTest = new Test(placeClient);
            placeTest.Run();


            //var getEvents = eventClient.GetEvents(new EventSearchRequest() { City = "skopje" });
            //var getEvent = eventClient.GetEvent(new EventIdRequest() { Id = 16 });
            //var post = eventClient.PostEvent(new Event() { Name = "Http Client", Seats = 400, Description = "Http Client Desc", City = "Skopje", Category = "Art", Location = "Central Park", DateRegistered = DateTime.Now });
            //var put = eventClient.PutEvent(26, new Event() { Id = 26, City = "Kumanovo", Description = "Desc Photo", Category = "Art", Seats = 80 });
            //var delete = eventClient.DeleteEvent(25);
        }
    }
}
