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
        public PlacesClient(string baseUrl, string timeout, string mediaType)
        {
            BaseAddress = new Uri(baseUrl);
            Timeout = TimeSpan.Parse(timeout);
            DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaType));
        }

        public PlacesClient(string baseUrl, string mediaType, string timeout, string authenticationScheme, string authenticationToken)
        {
            BaseAddress = new Uri(baseUrl);
            Timeout = TimeSpan.Parse(timeout);
            DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(mediaType));
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationScheme, authenticationToken);

        }

        /// <summary>
        /// Gets all places 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  Gets a place by Id
        /// </summary>
        /// <param name="request">Place Id</param>
        /// <returns>Returns an individual Place</returns>
        /// 
        public Place GetPlace(PlaceIdRequest request)
        {
            Place place = null;
            var response = GetAsync($"places/{request.Id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                return place;
            }

            var responseDate = response.Content.ReadAsStringAsync().Result;
            place = JsonConvert.DeserializeObject<Place>(responseDate);
            return place;
        }

        /// <summary>
        ///  Creates a new Place
        /// </summary>
        /// <param name="request"> The Place object </param>
        /// <returns> Returns the Id of the newly created Place </returns>
        /// 
        public int? PostPlace(PostRequest<Place> request)
        {
            var response = PostAsync($"places",
                                     new StringContent(JsonConvert.SerializeObject(request.Payload), Encoding.UTF8, "application/json")).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Updates an individual Place
        /// </summary>
        /// <param name="request"> Place.Id and Place object </param>
        /// <returns> Returns an updated Place object </returns>
        public Place PutPlace(PutRequest<Place> request)
        {
            var response = PutAsync($"places/{request.Id}",
                                              new StringContent(JsonConvert.SerializeObject(request.Payload), Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var newplace = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Place>(response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        ///  Deletes an individual Place
        /// </summary>
        /// <param name="request"> Place Id </param>
        /// <returns> Returns true or false </returns>
        public bool DeletePlace(PlaceIdRequest request)
        {
            var response = DeleteAsync($"places/{request.Id}").Result;

            return response.IsSuccessStatusCode;
        }
    }
}
