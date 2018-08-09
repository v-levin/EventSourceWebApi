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

        /// <summary>
        /// Returns the Event collection.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an individual Event.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new Event
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Returns the Id of the newly created Event
        /// </returns>
        public int? PostEvent(PostRequest<Event> request)
        {
            var jsonString = JsonConvert.SerializeObject(request.Payload);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = PostAsync("events", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;
            
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Updates an individual Event
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Returns an Event object
        /// </returns>
        public Event PutEvent(PutRequest<Event> request)
        {
            var jsonString = JsonConvert.SerializeObject(request.Payload);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = PutAsync($"events/{request.Id}", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;
            
            return JsonConvert.DeserializeObject<Event>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Deletes an individual Event
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// Returns true or false
        /// </returns>
        public bool DeleteEvent(EventIdRequest request)
        {
            var response = DeleteAsync($"events/{request.Id}").Result;

            return response.IsSuccessStatusCode;
        }
    }
}
