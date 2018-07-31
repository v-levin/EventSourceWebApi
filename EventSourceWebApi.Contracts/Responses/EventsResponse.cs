using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
    public class EventsResponse : Response
    {
        public IList<Event> Events { get; set; }
    }
}
