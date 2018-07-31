using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class PlaceSearchRequest : PageableRequest 
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string City { get; set; }

    }
}
