using EventSourceApiHttpClient;
using EventSourcePlaces.Functions;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Serilog;
using Serilog.Core;
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
        private static readonly Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt").CreateLogger();

        [FunctionName(PlacesConstants.FunctionName)]
        public static async Task RunAsync(
            [OrchestrationTrigger] DurableOrchestrationContextBase context)
        {
            var place = context.GetInput<Place>();

            var placeId = await context.CallActivityAsync<int>(PlacesConstants.CreatePlace, place);

            var updatePlace = await context.CallActivityAsync<Place>(PlacesConstants.UpdatePlace, placeId);

            var isDeletedPlace = await context.CallActivityAsync<HttpStatusCode>(PlacesConstants.DeletePlace, updatePlace.Id);

            var getPlace = await context.CallActivityAsync<Place>(PlacesConstants.GetPlace, placeId);

        }

        [FunctionName(PlacesConstants.CreatePlace)]
        public static int? CreatePlace([ActivityTrigger] Place place)
        {
            log.Information("Calling CreatePlace function");
            try
            {
                var request = new PostRequest<Place>() { Payload = place };

                return client.PlacesClient.PostPlace(request);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in CreatePlace function {ex.Message}");
                return null;
            }
        }

        [FunctionName(PlacesConstants.UpdatePlace)]
        public static Place UpdatePlace([ActivityTrigger] int placeId)
        {
            log.Information($"Calling UpdatePlace function for Place with Id: {placeId}");
            try
            {
                var newPlace = new Place()
                {
                    Description = "Update Place...",
                    City = "Radovis"
                };

                var request = new PutRequest<Place>() { Id = placeId, Payload = newPlace };

                return client.PlacesClient.PutPlace(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        [FunctionName(PlacesConstants.DeletePlace)]
        public static bool DeletePlace([ActivityTrigger] int placeId)
        {
            log.Information($"Calling DeletePlace function for Place with Id: {placeId}");
            try
            {
                var request = new PlaceIdRequest() { Id = placeId };

                return client.PlacesClient.DeletePlace(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        [FunctionName(PlacesConstants.GetPlace)]
        public static Place GetPlace([ActivityTrigger] int placeId)
        {
            log.Information($"Calling GetPlace function with palceId : {placeId}");
            try
            {
                var request = new PlaceIdRequest() { Id = placeId };

                return client.PlacesClient.GetPlace(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
