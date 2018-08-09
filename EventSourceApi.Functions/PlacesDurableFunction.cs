using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EventSourceApi.Functions
{
    public static class PlacesDurableFunction
    {
        private const string baseUrl = "http://localhost:49999/api/";
        private const string mediaType = "application/json";
        private static BaseHttpClient client = new BaseHttpClient(baseUrl, mediaType);

        [FunctionName("PlacesDurableFunction")]
        public static async Task RunAsync(
            [OrchestrationTrigger] DurableOrchestrationContextBase context)
        {
            var place = new Place()
            {
                Name = "Public room",
                Description = "Place...",
                Capacity = 35,
                Location = "Skopje",
                DateRegistered = DateTime.Now,
                City = "Skopje"
            };

            var placeId = await context.CallActivityAsync<int>("CreatePlace", place);

            var updatePlace = await context.CallActivityAsync<Place>("UpdatePlace", placeId);

            var isDeletePlace = await context.CallActivityAsync<HttpStatusCode>("DeletePlace", updatePlace.Id);

            var getPlace = await context.CallActivityAsync<Place>("GetPlace", placeId);
        }

        [FunctionName("CreatePlace")]
        public static int? CreatePlace([ActivityTrigger] Place place)
        {
            var request = new PostRequest<Place>() { Payload = place };

            return client.PlacesClient.PostPlace(request);
        }

        [FunctionName("UpdatePlace")]
        public static Place UpdatePlace([ActivityTrigger] int placeId)
        {
            var newPlace = new Place()
            {
                Description = "Update Place...",
                City = "Radovis"
            };

            var request = new PutRequest<Place>() { Id = placeId, Payload = newPlace };

            return client.PlacesClient.PutPlace(request);
        }

        [FunctionName("DeletePlace")]
        public static bool DeletePlace([ActivityTrigger] int placeId)
        {
            var request = new PlaceIdRequest() { Id = placeId };

            return client.PlacesClient.DeletePlace(request);
        }

        [FunctionName("GetPlace")]
        public static Place GetPlace([ActivityTrigger] int placeId)
        {
            var request = new PlaceIdRequest() { Id = placeId };

            return client.PlacesClient.GetPlace(request);
        }
    }
}
