using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace EventSourceApi.Functions
{
    public static class EventsDurableFunction
    {
        [FunctionName("EventsDurableFunction")]
        public static async void Run([OrchestrationTrigger] DurableOrchestrationContext context)
        {
            
        }
    }
}
