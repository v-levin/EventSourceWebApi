using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class PlaceRequest : PageableRequest
    {
        public int Id { get; set; }

        public Place Place { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string City { get; set; }

    }
}
