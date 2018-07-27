using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class Request : PageableRequest
    {
        public string Keyword { get; set; } 
    }
}
