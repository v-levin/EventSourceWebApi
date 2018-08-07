using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            var response = client.GetAsync($"{client.BaseAddress}/{placesRootUrl}?name={request.Name}&city={request.City}&location={request.Location}&limit={request.Limit}&offset={request.Offset}").Result;
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
            var response = client.GetAsync($"{client.BaseAddress}/{placesRootUrl}{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseDate = response.Content.ReadAsStringAsync().Result;
                place = JsonConvert.DeserializeObject<Place>(responseDate);
                return place;
            }
            return place;
        }
        public int? CreatePlace(Place place)
        {
            var response = client.PostAsync($"{client.BaseAddress}/{placesRootUrl}",
                                              new StringContent(JsonConvert.SerializeObject(place), Encoding.UTF8, "application/json")).Result;

            if(response.IsSuccessStatusCode)
            { 
                var id = int.Parse(response.Content.ReadAsStringAsync().Result);
                return id;
            }

            return null;
        }

        public int? UpdatePlace(int id, Place place)
        {
            var response = client.PutAsync($"{client.BaseAddress}/{placesRootUrl}/{id}",
                                              new StringContent(JsonConvert.SerializeObject(place), Encoding.UTF8, "application/json")).Result;
            if (response.IsSuccessStatusCode)
            {
                return id;
            }

            return null;
        }

        public HttpStatusCode DeletePlace(int id)
        {
            var response = client.DeleteAsync($"{client.BaseAddress}/{placesRootUrl}/{id}").Result;
            var status = response.StatusCode;
            return response.StatusCode;
        }
    }
}
