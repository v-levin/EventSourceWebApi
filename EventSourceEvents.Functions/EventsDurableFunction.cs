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
        private static Logger _logger = EventSourceLogger.InitializeLogger();

        [FunctionName("EventsDurableFunction")]
        public static async Task<bool> Run([OrchestrationTrigger] DurableOrchestrationContext context, ExecutionContext eContext)
        {
            _client = InitClient(eContext.FunctionAppDirectory);

            var newEvent = context.GetInput<Event>();

            try
            {
                _logger.Information("Calling CreateEvent function");
                newEvent.Id = await context.CallActivityAsync<int>("CreateEvent", (newEvent, eContext.FunctionAppDirectory));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured in CreateEvent function {ex.Message} ");
                throw;
            }
            finally
            {
                _client.EventsClient.Dispose();
            }

            try
            {
                _logger.Information($"Calling UpdateEvent function for Event with Id: {newEvent.Id}");
                newEvent = await context.CallActivityAsync<Event>("UpdateEvent", (newEvent.Id, eContext.FunctionAppDirectory));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured in UpdateEvent function {ex.Message}");
            }
            finally
            {
                _client.EventsClient.Dispose();
            }

            try
            {
                _logger.Information($"Calling DeleteEvent function for Event with Id: {newEvent.Id}");
                var isDeleted = await context.CallActivityAsync<bool>("DeleteEvent", (newEvent.Id, eContext.FunctionAppDirectory));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured in DeleteEvent function {ex.Message}");
            }
            finally
            {
                _client.EventsClient.Dispose();
            }

            try
            {
                _logger.Information($"Calling GetEvent function with eventId : {newEvent.Id}");
                newEvent = await context.CallActivityAsync<Event>("GetEvent", (newEvent.Id, eContext.FunctionAppDirectory));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occured in GetPlace function {ex.Message}");
            }
            finally
            {
                _client.EventsClient.Dispose();
            }

            return newEvent != null;
        }

        [FunctionName("CreateEvent")]
        public static int? CreateEvent([ActivityTrigger] DurableActivityContext inputs)
        {
            (Event @Event, string Path) input = inputs.GetInput<(Event, string)>();

            _client = InitClient(input.Path);

            var request = new PostRequest<Event>() { Payload = input.Event };

            return _client.EventsClient.PostEvent(request);
        }

        [FunctionName("UpdateEvent")]
        public static Event UpdateEvent([ActivityTrigger] DurableActivityContext inputs)
        {
            (int EventId, string Path) input = inputs.GetInput<(int, string)>();

            _client = InitClient(input.Path);

            var newEvent = new Event() { City = "Skopje" };
            var request = new PutRequest<Event>() { Id = input.EventId, Payload = newEvent };

            return _client.EventsClient.PutEvent(request);
        }

        [FunctionName("DeleteEvent")]
        public static bool DeleteEvent([ActivityTrigger] DurableActivityContext inputs)
        {
            (int EventId, string Path) input = inputs.GetInput<(int, string)>();

            _client = InitClient(input.Path);

            var request = new EventIdRequest() { Id = input.EventId };

            return _client.EventsClient.DeleteEvent(request);
        }

        [FunctionName("GetEvent")]
        public static Event GetEvent([ActivityTrigger] DurableActivityContext inputs)
        {
            (int EventId, string Path) input = inputs.GetInput<(int, string)>();

            _client = InitClient(input.Path);

            var request = new EventIdRequest() { Id = input.EventId };

            return _client.EventsClient.GetEvent(request);
        }

        private static BaseHttpClient InitClient(string path)
        {
            if (_client == null)
                return Client.InitializeClient(path);

            return _client;
        }
    }
}
