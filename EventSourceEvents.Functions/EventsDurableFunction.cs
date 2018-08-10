using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.Azure.WebJobs;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourceEvents.Functions
{
    public static class EventsDurableFunction
    {
        private static BaseHttpClient client = InitializeClient();
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

        public static BaseHttpClient InitializeClient()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            var mediaType = ConfigurationManager.AppSettings["MediaType"];
            var authenticationScheme = ConfigurationManager.AppSettings["AuthenticationScheme"];
            var authenticationToken = ConfigurationManager.AppSettings["AuthenticationToken"];
            var timeout = ConfigurationManager.AppSettings["Timeout"];

            if (string.IsNullOrEmpty(baseUrl))
                log.Information("The Url configuration is missing.");

            if (string.IsNullOrEmpty(mediaType))
                log.Information("The MediaType configuration is missing.");

            if (string.IsNullOrEmpty(authenticationScheme))
                log.Information("The AuthenticationScheme configuration is missing.");

            if (string.IsNullOrEmpty(authenticationToken))
                log.Information("The AuthenticationToken configuration is missing.");

            if (string.IsNullOrEmpty(timeout))
                log.Information("The timeout configuration is missing.");

            return new BaseHttpClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }
    }
}
