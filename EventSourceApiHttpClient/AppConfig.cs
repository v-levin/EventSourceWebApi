using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceApiHttpClient
{
    public class AppConfig
    {
        private const string baseUrl = "http://localhost:49999/api/";
        public string BaseUrl { get { return baseUrl; } }

    }
}
