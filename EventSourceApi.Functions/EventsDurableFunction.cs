using System;
using System.Configuration;
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
        private static BaseHttpClient client = InitializeClient();
        private static Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt").CreateLogger();

        [FunctionName("EventsDurableFunction")]
        public static async Task<bool> Run([OrchestrationTrigger] DurableOrchestrationContext context)
        {
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
                log.Information($"Calling DeleteEvent function for Place with Id: {newEvent.Id}");
                var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", newEvent.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in DeleteEvent function {ex.Message}");
            }

            try
            {
                log.Information($"Calling GetPlace function with palceId : {newEvent.Id}");
                var anEvent = await context.CallActivityAsync<Event>("GetEvent", newEvent.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in GetEvent function {ex.Message}");
            }

            return newEvent != null;
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
