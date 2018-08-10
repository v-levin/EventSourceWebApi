using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Serilog.Core;
using System;
using System.Threading.Tasks;

namespace EventSourceEvents.Functions
{
    public static class EventsDurableFunction
    {
        private static BaseHttpClient _client;
        private static Logger log = EventSourceLogger.InitializeLogger();

        [FunctionName("EventsDurableFunction")]
        public static async Task<bool> Run([OrchestrationTrigger] DurableOrchestrationContext context, ExecutionContext eContext)
        {
            _client = Client.InitializeClient(eContext.FunctionAppDirectory);

            var newEvent = context.GetInput<Event>();

            try
            {
                log.Information("Calling CreateEvent function");
                newEvent.Id = await context.CallActivityAsync<int>("CreateEvent", newEvent);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in CreateEvent function {ex.Message} ");
            }

            try
            {
                log.Information($"Calling UpdateEvent function for Event with Id: {newEvent.Id}");
                newEvent = await context.CallActivityAsync<Event>("UpdateEvent", newEvent.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in UpdateEvent function {ex.Message}");
            }

            try
            {
                log.Information($"Calling DeleteEvent function for Event with Id: {newEvent.Id}");
                var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", newEvent.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in DeleteEvent function {ex.Message}");
            }

            try
            {
                log.Information($"Calling GetEvent function with eventId : {newEvent.Id}");
                newEvent = await context.CallActivityAsync<Event>("GetEvent", newEvent.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in GetPlace function {ex.Message}");
            }

            return newEvent != null;
        }

        [FunctionName("CreateEvent")]
        public static int? CreateEvent([ActivityTrigger] Event @event)
        {
            var request = new PostRequest<Event>() { Payload = @event };

            return _client.EventsClient.PostEvent(request);
        }

        [FunctionName("UpdateEvent")]
        public static Event UpdateEvent([ActivityTrigger] int eventId)
        {
            var newEvent = new Event() { City = "Skopje" };
            var request = new PutRequest<Event>() { Id = eventId, Payload = newEvent };

            return _client.EventsClient.PutEvent(request);
        }

        [FunctionName("DeleteEvent")]
        public static bool DeleteEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return _client.EventsClient.DeleteEvent(request);
        }

        [FunctionName("GetEvent")]
        public static Event GetEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return _client.EventsClient.GetEvent(request);
        }
    }
}
