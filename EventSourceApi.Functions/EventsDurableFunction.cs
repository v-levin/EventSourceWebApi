using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace EventSourceApi.Functions
{
    public static class EventsDurableFunction
    {
        [FunctionName("EventsDurableFunction")]
        public static async Task<Event> Run([OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var @event = new Event()
            {
                Name = "Durable",
                Description = "Desc",
                City = "Kumanovo",
                Category = "Party",
                Location = "City Square",
                Seats = 150,
                DateRegistered = DateTime.Now
            };

            var newEvent = await context.CallActivityAsync<Event>("CreateEvent", @event);

            return newEvent;
        }

        //public static Event CreateEvent([ActivityTrigger] Event @event)
        //{
        //    var client = new BaseHttpclient();
        //}
    }
}
