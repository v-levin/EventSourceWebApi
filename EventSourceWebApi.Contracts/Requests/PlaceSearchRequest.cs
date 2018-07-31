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

        public override string ToString()
        {
            return $"Getting all places with name:{Name}, location:{Location}, City: {City}, limit: {Limit}, offset: {Offset}";
        }
    }
}
