using Serilog;
using Serilog.Core;

namespace EventSourceApiHttpClient
{
    public class EventSourceLogger
    {
        public Logger InitializeLogger() 
        {
            return new LoggerConfiguration()
                                            .WriteTo.Console()
                                            .WriteTo.File("log.txt")
                                            .CreateLogger();
        }
    }
}
