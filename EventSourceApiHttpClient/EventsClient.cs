using EventSourceWebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventSourceApiHttpClient
{
    public class EventsClient
    {
        private HttpClient client;
        private const string url = "http://localhost:8080/api/events";
        private const string acceptHeader = "application/json";

        public EventsClient()
        {
            client = HttpClient();
        }

        public HttpClient HttpClient()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(url),
                Timeout = new TimeSpan(0, 0, 5)
            };
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(acceptHeader));

            return client;
        }

        public IEnumerable<Event> GetEvents(string name, string city, string category, string location, int offset, int limit)
        {
            return null;
        }

        public Event GetEvent(int id)
        {
            Event @event = null;
            var response = client.GetAsync($"{url}/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                var requestBody  = response.Content.ReadAsStringAsync().Result;
                @event = JsonConvert.DeserializeObject<Event>(requestBody);
            }

            return @event;
        }
    }
}
