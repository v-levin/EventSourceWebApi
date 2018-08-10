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

            var newEventId = await context.CallActivityAsync<int>("CreateEvent", newEvent);

            var updatedEvent = await context.CallActivityAsync<Event>("UpdateEvent", newEventId);

            var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", updatedEvent.Id);

            var anEvent = await context.CallActivityAsync<Event>("GetEvent", newEventId);
        }

        [FunctionName("CreateEvent")]
        public static int? CreateEvent([ActivityTrigger] Event @event)
        {
            var request = new PostRequest<Event>() { Payload = @event };

            try
            {
                log.Information("Calling CreateEvent function.");
                return client.EventsClient.PostEvent(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        [FunctionName("UpdateEvent")]
        public static Event UpdateEvent([ActivityTrigger] int eventId)
        {
            var newEvent = new Event() { City = "Skopje" };
            var request = new PutRequest<Event>() { Id = eventId, Payload = newEvent };

            try
            {
                log.Information("Calling UpdateEvent function.");
                return client.EventsClient.PutEvent(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
            
        }

        [FunctionName("DeleteEvent")]
        public static bool DeleteEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            try
            {
                log.Information("Calling DeleteEvent function.");
                return client.EventsClient.DeleteEvent(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        [FunctionName("GetEvent")]
        public static Event GetEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            try
            {
                log.Information("Calling GetEvent function.");
                return client.EventsClient.GetEvent(request);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
