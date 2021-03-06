using EventSourceApiHttpClient;
using EventSourcePlaces.Functions;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Serilog.Core;
using System;
using System.Threading.Tasks;

namespace EventSourceApi.Functions
{
    public static class PlacesDurableFunction
    {
        private static Logger log = EventSourceLogger.InitializeLogger();

        private static BaseHttpClient _client;


        [FunctionName(PlacesConstants.FunctionName)]
        public static async Task<bool> RunAsync(
            [OrchestrationTrigger] DurableOrchestrationContextBase context, ExecutionContext eContext)
        {
            _client = Client.InitializeClient(eContext.FunctionAppDirectory);

            var place = context.GetInput<Place>();

            try
            {
                log.Information("Calling CreatePlace function");
                place.Id = await context.CallActivityAsync<int>(PlacesConstants.CreatePlace, place);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in CreatePlace function {ex.Message} ");
            }

            try
            {
                log.Information($"Calling UpdatePlace function for Place with Id: {place.Id}");
                place = await context.CallActivityAsync<Place>(PlacesConstants.UpdatePlace, place.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in UpdatePlace function {ex.Message}");
            }

            try
            {
                log.Information($"Calling DeletePlace function for Place with Id: {place.Id}");
                var deleted = await context.CallActivityAsync<bool>(PlacesConstants.DeletePlace, place.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in DeletePlace function {ex.Message}");
            }

            try
            {
                log.Information($"Calling GetPlace function with palceId : {place.Id}");
                place = await context.CallActivityAsync<Place>(PlacesConstants.GetPlace, place.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in GetPlace function {ex.Message}");
            }

            return place != null;
        }

        [FunctionName(PlacesConstants.CreatePlace)]
        public static int? CreatePlace([ActivityTrigger] Place place)
        {
            var request = new PostRequest<Place>() { Payload = place };

            return _client.PlacesClient.PostPlace(request);
        }

        [FunctionName(PlacesConstants.UpdatePlace)]
        public static Place UpdatePlace([ActivityTrigger] int placeId)
        {
            var newPlace = new Place()
            {
                Description = "Update Place...",
                City = "Radovis"
            };

            var request = new PutRequest<Place>() { Id = placeId, Payload = newPlace };

            return _client.PlacesClient.PutPlace(request);
        }

        [FunctionName(PlacesConstants.DeletePlace)]
        public static bool DeletePlace([ActivityTrigger] int placeId)
        {
            var request = new PlaceIdRequest() { Id = placeId };

            return _client.PlacesClient.DeletePlace(request);
        }

        [FunctionName(PlacesConstants.GetPlace)]
        public static Place GetPlace([ActivityTrigger] int placeId)
        {
            var request = new PlaceIdRequest() { Id = placeId };

            return _client.PlacesClient.GetPlace(request);
        }

    }
}
