using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace EventSourceApi.Functions
{
    public static class EventsDurableFunction
    {
        private const string baseUrl = "http://localhost:49999/api/";
        private const string mediaType = "application/json";
        private static BaseHttpClient client = new BaseHttpClient(baseUrl, mediaType);

        [FunctionName("EventsDurableFunction")]
        public static async Task<bool> Run([OrchestrationTrigger] DurableOrchestrationContext context)
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

            var newlyCretedEvent = await context.CallActivityAsync<Event>("GetEvent", newEventId);

            newEvent.City = "Skopje";
            var updatedEvent = await context.CallActivityAsync<Event>("UpdateEvent", newEvent);

            var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", updatedEvent.Id);

            return isDeleted;
        }

        public static int? CreateEvent([ActivityTrigger] Event @event)
        {
            var request = new PostRequest<Event>() { Payload = @event };

            return client.EventsClient.PostEvent(request);
        }

        public static Event GetEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.GetEvent(request);
        }

        public static Event UpdateEvent([ActivityTrigger] Event @event)
        {
            var request = new PutRequest<Event>() { Id = @event.Id, Payload = @event };

            return client.EventsClient.PutEvent(request);
        }

        public static bool DeleteEvent([ActivityTrigger] int eventId)
        {
            var request = new EventIdRequest() { Id = eventId };

            return client.EventsClient.DeleteEvent(request);
        }
    }
}
