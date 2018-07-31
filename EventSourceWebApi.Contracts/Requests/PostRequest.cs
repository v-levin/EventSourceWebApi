using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts.Requests
{
    public class PostRequest<T>
    {
        public T Payload { get; set; }
    }
}
