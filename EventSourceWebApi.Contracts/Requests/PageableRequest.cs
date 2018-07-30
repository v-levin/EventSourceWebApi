using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class PageableRequest
    {
        public const int MaxLimit = 1000;

        public int Limit { get; set; } = 5;

        public int Offset { get; set; } 

    }
}
