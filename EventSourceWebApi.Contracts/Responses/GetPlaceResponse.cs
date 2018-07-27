using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
    public class GetPlaceResponse
    {
        public string Message { get; set; }

        public Place Place { get; set; }

    }
}
