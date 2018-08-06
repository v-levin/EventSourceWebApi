using EventSourceWebApi.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventSourceApiHttpClient
{
    public class PlacesClient
    {
        private readonly string url = "http://localhost:49999/places";
        HttpClient _client= new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:49999/"),
            Timeout = TimeSpan.FromMilliseconds(100)
        };
        public void GetAllPlaces(string url)
        {
   
             

             
        }

        public Place GetPlace(int id)
        {
            Place place = null;
            var response = _client.GetAsync($"{url}{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseDate = response.Content.ReadAsStringAsync().Result;
                place = JsonConvert.DeserializeObject<Place>(responseDate);
                return place;
            }
            return place;
        }
        public void CreatePlace()
        {

        }

        public void UpdatePlace()
        {

        }

        public void DeletePlace()
        {

        }



    }
}
