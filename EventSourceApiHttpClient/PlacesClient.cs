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
    public class PlacesClient : HttpClient
    {
        public PlacesClient(string baseUrl, string mediaType) 
        {
            BaseAddress = new Uri(baseUrl);
            Timeout = new TimeSpan(0, 5, 0);
            DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaType));
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
        }

        
        public IEnumerable<Place> GetPlaces(PlaceSearchRequest request)
        {

            var response = GetAsync($"places?name={request.Name}&city={request.City}" +
                                              $"&location={request.Location}" +
                                              $"&limit={request.Limit}" +
                                              $"&offset={request.Offset}").Result;

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
            var response = GetAsync($"/places/{id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return place;
            }

            var responseDate = response.Content.ReadAsStringAsync().Result;
            place = JsonConvert.DeserializeObject<Place>(responseDate);
            return place;
        }

        public int? PostPlace(Place place)
        {
            var response = PostAsync($"places",
                                     new StringContent(JsonConvert.SerializeObject(place), Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var id = int.Parse(response.Content.ReadAsStringAsync().Result);
            return id;
        }

        public Place PutPlace(int id, Place place)
        {
            var response = PutAsync($"places/{id}",
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
            var response = DeleteAsync($"places/{id}").Result;
            var status = response.StatusCode;
            return response.StatusCode;
        }
    }
}
