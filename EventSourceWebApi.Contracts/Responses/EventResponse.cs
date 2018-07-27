using System.Collections.Generic;

namespace EventSourceWebApi.Contracts.Responses
{
    public class EventResponse : Response
    {
        public string Message { get; set; }

        public Event Event { get; set; }

        public IEnumerable<Event> Events { get; set; }
    }
}
