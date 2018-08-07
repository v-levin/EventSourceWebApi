﻿using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
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

        public EventsClient(string baseUrl, string acceptHeader)
        {
            client = InitHttpClient(baseUrl, acceptHeader);
        }

        public HttpClient InitHttpClient(string baseUrl, string acceptHeader)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl + "events"),
                Timeout = new TimeSpan(0, 5, 0)
            };
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(acceptHeader));

            return client;
        }

        public IEnumerable<Event> GetEvents(EventSearchRequest request)
        {
            var events = new List<Event>();

            var response = client.GetAsync(
                    $"{client.BaseAddress}?" +
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
            var response = client.GetAsync($"{client.BaseAddress}/{request.Id}").Result;

            if (response.IsSuccessStatusCode)
            {
                var result  = response.Content.ReadAsStringAsync().Result;
                @event = JsonConvert.DeserializeObject<Event>(result);
            }

            return @event;
        }

        public int? PostEvent(Event @event)
        {
            var jsonString = JsonConvert.SerializeObject(@event);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, content).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            // returns the Id of the newly created Event
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }

        public Event PutEvent(int id, Event @event)
        {
            var jsonString = JsonConvert.SerializeObject(@event);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PutAsync($"{client.BaseAddress}/{id}", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            // returns an Event object
            return JsonConvert.DeserializeObject<Event>(response.Content.ReadAsStringAsync().Result);
        }

        public bool DeleteEvent(int id)
        {
            var response = client.DeleteAsync($"{client.BaseAddress}/{id}").Result;

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
