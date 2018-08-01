using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
    public class EventsResponse : Response
    {
        public int TotalCount { get; set; }

        public IList<Event> Events { get; set; }
    }
}
