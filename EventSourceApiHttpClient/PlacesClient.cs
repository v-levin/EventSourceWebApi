using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EventSourceApiHttpClient
{
    public class PlacesClient
    {
        private HttpClient client;

        public PlacesClient(string baseUrl, string mediaType)
        {
            client = InitHttpClient(baseUrl, mediaType);
        }
        public HttpClient InitHttpClient(string baseUrl, string mediaType)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl + "places"),
                Timeout = new TimeSpan(0, 5, 0)
            };
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaType));

            return client;
        }

        public IEnumerable<Place> GetAllPlaces(PlaceSearchRequest request)
        {
            var response = client.GetAsync($"{client.BaseAddress}?name={request.Name}&city={request.City}&location={request.Location}&limit={request.Limit}&offset={request.Offset}").Result;
            var places = new List<Place>();

            if (!response.IsSuccessStatusCode)
            {
                return places;
            }

            var responseDate = response.Content.ReadAsStringAsync().Result;
            places = JsonConvert.DeserializeObject<List<Place>>(responseDate);
            return places;
        }

        public Place GetPlace(int id)
        {
            Place place = null;
            var response = client.GetAsync($"{client.BaseAddress}{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return place;
            }

            var responseDate = response.Content.ReadAsStringAsync().Result;
            place = JsonConvert.DeserializeObject<Place>(responseDate);
            return place;
        }
        public int? CreatePlace(Place place)
        {
            var response = client.PostAsync($"{client.BaseAddress}",
                                              new StringContent(JsonConvert.SerializeObject(place), Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var id = int.Parse(response.Content.ReadAsStringAsync().Result);
            return id;
        }

        public Place UpdatePlace(int id, Place place)
        {
            var response = client.PutAsync($"{client.BaseAddress}/{id}",
                                              new StringContent(JsonConvert.SerializeObject(place), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            place = JsonConvert.DeserializeObject<Place>(response.Content.ReadAsStringAsync().Result);
            return place;
        }

        public HttpStatusCode DeletePlace(int id)
        {
            var response = client.DeleteAsync($"{client.BaseAddress}/{id}").Result;
            var status = response.StatusCode;
            return response.StatusCode;
        }
    }
}
