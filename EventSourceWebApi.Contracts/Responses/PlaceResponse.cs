﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Responses
{
    public class PlaceResponse : Response
    {
        public string Message { get; set; }

        public Place Place { get; set; }

        public IEnumerable<Place> Places {get; set;}

        public int PlaceId { get; set; }
    }
}