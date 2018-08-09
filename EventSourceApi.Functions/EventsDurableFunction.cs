using System;
using System.Threading.Tasks;
using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Serilog;
using Serilog.Core;

namespace EventSourceApi.Functions
{
    public static class EventsDurableFunction
    {
        private const string baseUrl = "http://localhost:49999/api/";
        private const string mediaType = "application/json";
        private static BaseHttpClient client = new BaseHttpClient(baseUrl, mediaType);
        private static Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt").CreateLogger();

        [FunctionName("EventsDurableFunction")]
        public static async Task Run([OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var newEvent = new Event()
            {
                Name = "Durable",
                Description = "Desc",
                City = "Kumanovo",
                Category = "Party",
                Location = "City Square",
                Seats = 150,
                DateRegistered = DateTime.Now
            };

            try
            {
                var newEventId = await context.CallActivityAsync<int>("CreateEvent", newEvent);
                
                var updatedEvent = await context.CallActivityAsync<Event>("UpdateEvent", newEventId);
                
                var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", updatedEvent.Id);
                
                var anEvent = await context.CallActivityAsync<Event>("GetEvent", newEventId);

                log.Information("Done!");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        [FunctionName("CreateEvent")]
        public static int? CreateEvent([ActivityTrigger] Event @event)
        {
            var request = new PostRequest<Event>() { Payload = @event };

            return client.EventsClient.PostEvent(request);
        }

        [FunctionName("UpdateEvent")]
        public static Event UpdateEvent([ActivityTrigger] int eventId)
        {
            var newEvent = new Event() { City = "Skopje" };
            var request = new PutRequest<Event>() { Id = eventId, Payload = newEvent };

            return client.EventsClient.PutEvent(request);
        }

        [FunctionName("DeleteEvent")]
        public static bool DeleteEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.DeleteEvent(request);
        }

        [FunctionName("GetEvent")]
        public static Event GetEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.GetEvent(request);
        }
    }
}
