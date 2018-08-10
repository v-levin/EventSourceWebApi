using Serilog;
using Serilog.Core;

namespace EventSourceApiHttpClient
{
    public static class EventSourceLogger
    {
        public static Logger InitializeLogger()
        {
            return new LoggerConfiguration()
                                            .WriteTo.Console()
                                            .WriteTo.File("log.txt")
                                            .CreateLogger();
        }
    }
}
