using System.Collections.Generic;

namespace EventSourceWebApi.Contracts.Responses
{
    public class EventResponse : Response
    {
        public Event Event { get; set; }
    }
}
