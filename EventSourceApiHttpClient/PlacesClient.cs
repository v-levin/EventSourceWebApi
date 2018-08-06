using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EventSourceApiHttpClient
{
    public class PlacesClient
    {
        private readonly HttpClient client;
        private const string placesRootUrl = "places";
        public PlacesClient(HttpClient _client)
        {
            client = _client;
        }
        public IEnumerable<Place> GetAllPlaces(PlaceSearchRequest request)
        {
            var response = client.GetAsync($"{client.BaseAddress}/{placesRootUrl}?name={request.Name}&city={request.City}&location={request.Location}").Result;
            var places = new List<Place>();

            if (response.IsSuccessStatusCode)
            {
                var responseDate = response.Content.ReadAsStringAsync().Result;
                places = JsonConvert.DeserializeObject<List<Place>>(responseDate);
                return places;
            }
            return places;
        }

        public Place GetPlace(int id)
        {
            Place place = null;
            var response = client.GetAsync($"{client.BaseAddress}/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseDate = response.Content.ReadAsStringAsync().Result;
                place = JsonConvert.DeserializeObject<Place>(responseDate);
                return place;
            }
            return place;
        }
        public void CreatePlace(HttpRequestMessage place)
        {
            HttpResponseMessage response = client.PostAsync($"{client.BaseAddress}places", place.Content).Result;
            response.EnsureSuccessStatusCode();

          
                    
        }

        public void UpdatePlace()
        {

        }

        public void DeletePlace()
        {

        }


    }
}
