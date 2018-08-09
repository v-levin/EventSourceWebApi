using System;
using System.Threading.Tasks;
using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;

namespace EventSourceApi.Functions
{
    public static class EventsDurableFunction
    {
        private const string baseUrl = "http://localhost:49999/api/";
        private const string mediaType = "application/json";
        private static BaseHttpClient client = new BaseHttpClient(baseUrl, mediaType);

        [FunctionName("EventsDurableFunction")]
        public static async Task<Event> Run([OrchestrationTrigger] DurableOrchestrationContext context)
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

            return await context.CallActivityAsync<Event>("GetEvent", newEventId);
        }

        public static int? CreateEvent([ActivityTrigger] Event @event)
        {
            var request = new PostRequest<Event>() { Payload = @event };

            return client.EventsClient.PostEvent(request);
        }

        public static Event UpdateEvent([ActivityTrigger] int eventId)
        {
            var newEvent = new Event() { City = "Skopje" };
            var request = new PutRequest<Event>() { Id = eventId, Payload = newEvent };

            return client.EventsClient.PutEvent(request);
        }

        public static bool DeleteEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.DeleteEvent(request);
        }

        public static Event GetEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.GetEvent(request);
        }
    }
}
