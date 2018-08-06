using EventSourceWebApi.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EventSourceApiHttpClient
{
    public class EventSourceApiHttpClient
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main()
        {

            var eventSourceApiClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:49999/api"),
                Timeout = new TimeSpan(0, 5, 0),
            };
            eventSourceApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var placeClient = new PlacesClient(eventSourceApiClient);
            placeClient.GetAllPlaces(new PlaceSearchRequest() { Name = "mkc", City = "skopje" });
            placeClient.GetPlace(38);

        }
    }
}
