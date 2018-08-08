using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSource.Tests.IntegrationTests
{
    public class PlacesTests
    {
        private static BaseHttpclient _client;
        public PlacesTests(BaseHttpclient client)
        {
            _client = client;
        }

        public void Run()
        {

            //Get all
            var places = _client.PlacesClient.GetPlaces(new PlaceSearchRequest() { City = "Skopje" });

            if (!places.Any())
                return;

            var firstPlace = places.First();

            // Get place by id
            var getPlace = _client.PlacesClient.GetPlace(firstPlace.Id);


            var newPlace = new Place()
            {
                Name = "mkc",
                City = "Radovis"
            };
             
            //Update the place
            var updatePlace = _client.PlacesClient.PutPlace(getPlace.Id, newPlace);


            //Create a new Place
            var place = new Place()
            {
                Name = "place1",
                Description = "Wdefrgb",
                Capacity = 70,
                Location = "Radovis",
                DateRegistered = DateTime.Now,
                City = "Skopje"
            };
            var placeId = _client.PlacesClient.PostPlace(place);

        }

    }
}
