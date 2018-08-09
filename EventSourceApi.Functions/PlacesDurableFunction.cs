using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourceApi.Functions
{
    public static class PlacesDurableFunction
    {
        [FunctionName("PlacesDurableFunction")]
        public static void Run(
            [OrchestrationTrigger] DurableOrchestrationContextBase context)
        {
          

        }

    }
}
