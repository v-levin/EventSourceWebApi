using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
    public class PlacesResponse : Response
    {
        public IList<Place> Places { get; set; }


    }
}
