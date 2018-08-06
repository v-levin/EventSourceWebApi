using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
