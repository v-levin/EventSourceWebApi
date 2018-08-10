using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Serilog.Core;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventSourceEvents.Functions
{
    public static class HttpStart
    {
        private static readonly Logger log = EventSourceLogger.InitializeLogger();

        [FunctionName("HttpStart")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = EventsConstants.Route)] HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClientBase starter)
        {
            try
            {
                var eventData = await req.Content.ReadAsAsync<Event>();
                string instanceId = await starter.StartNewAsync(EventsConstants.FunctionName, eventData);

                log.Information($"Started orchestration with ID = '{instanceId}'.");

                var res = starter.CreateCheckStatusResponse(req, instanceId);
                res.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(10));
                return res;
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error occured in HttpStart {ex.Message}");

                return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
}
