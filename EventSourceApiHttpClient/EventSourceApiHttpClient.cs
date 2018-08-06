using System;

namespace EventSourceApiHttpClient
{
    public class EventSourceApiHttpClient
    {
        public static void Main()
        {
            var client = new EventsClient();
            var @event = client.GetEvent(4);
            Console.WriteLine();
        }
    }
}
