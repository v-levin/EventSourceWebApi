using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EventSourceApiHttpClient
{
    public class EventsClient : HttpClient
    {
        public EventsClient(string baseUrl, string mediaType)
        {
            BaseAddress = new Uri(baseUrl);
            Timeout = new TimeSpan(0, 5, 0);
            DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaType));
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
        }


        public IEnumerable<Event> GetEvents(EventSearchRequest request)
        {
            var events = new List<Event>();

            var response = GetAsync(
                    $"events?" +
                    $"name={request.Name}&" +
                    $"city={request.City}&" +
                    $"category={request.Category}&" +
                    $"location={request.Location}&" +
                    $"limit={request.Limit}&" +
                    $"offset={request.Offset}")
                .Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                events = JsonConvert.DeserializeObject<List<Event>>(result);
            }

            return events;
        }

        public Event GetEvent(EventIdRequest request)
        {
            Event @event = null;
            var response = GetAsync($"events/{request.Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                @event = JsonConvert.DeserializeObject<Event>(result);
            }

            return @event;
        }

        public int? PostEvent(Event @event)
        {
            var jsonString = JsonConvert.SerializeObject(@event);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = PostAsync("events", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            // returns the Id of the newly created Event
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }

        public Event PutEvent(int id, Event @event)
        {
            var jsonString = JsonConvert.SerializeObject(@event);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = PutAsync($"events/{id}", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            // returns an Event object
            return JsonConvert.DeserializeObject<Event>(response.Content.ReadAsStringAsync().Result);
        }

        public bool DeleteEvent(int id)
        {
            var response = DeleteAsync($"events/{id}").Result;

            return response.IsSuccessStatusCode;
        }
    }
}
