using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using Microsoft.Azure.WebJobs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EventSourceApi.Functions
{
    public static class PlacesDurableFunction
    {
        [FunctionName("PlacesDurableFunction")]
        public static async Task RunAsync(
            [OrchestrationTrigger] DurableOrchestrationContextBase context)
        {
            var place = new Place()
            {
                Name = "place1",
                Description = "Wdefrgb",
                Capacity = 70,
                Location = "Radovis",
                DateRegistered = DateTime.Now,
                City = "Skopje"
            };

            int createPlaceId = await context.CallActivityAsync<int>("CreatePlace", place);

            Place updatePlace = await context.CallActivityAsync<Place>("UpdatePlace", createPlaceId);

            HttpStatusCode deletePlace = await context.CallActivityAsync<HttpStatusCode>("DeletePlace", updatePlace.Id);

            Place getPlace = await context.CallActivityAsync<Place>("GetPlace", deletePlace);
        }

        public static BaseHttpClient IntiClient()
        {
            var baseUrl = "http://localhost:49999/api/";
            var mediaType = "application/json";
            return new BaseHttpClient(baseUrl, mediaType);
        }

        [FunctionName("CreatePlace")]
        public static int? CreatePlace([ActivityTrigger] Place place)
        {
            var client = IntiClient();
            var placeId = client.PlacesClient.PostPlace(place);
            return placeId;
        }

        [FunctionName("UpdatePlace")]
        public static Place UpdatePlace([ActivityTrigger] int placeId)
        {
            var newPlace = new Place()
            {
                Name = "place1",
                Description = "Wdefrgb",
                Capacity = 70,
                Location = "Radovis",
                DateRegistered = DateTime.Now,
                City = "Radovis"
            };
            var client = IntiClient();
            var place = client.PlacesClient.PutPlace(placeId, newPlace);
            return place;
        }

        [FunctionName("DeletePlace")]
        public static HttpStatusCode DeletePlace([ActivityTrigger] int placeId)
        {
            var client = IntiClient();
            var place = client.PlacesClient.DeletePlace(placeId);
            return place;
        }

        [FunctionName("GetPlace")]
        public static Place GetPlace([ActivityTrigger] int placeId)
        {
            var client = IntiClient();
            var place = client.PlacesClient.GetPlace(placeId);
            return place;
        }
    }
}
