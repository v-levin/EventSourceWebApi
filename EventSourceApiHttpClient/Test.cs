using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;
using System.Linq;

namespace EventSourceApiHttpClient
{
    public class Test
    {
        private readonly PlacesClient _client;
        public Test(PlacesClient client)
        {
            _client = client;
        }

        public void Run()
        {

            try
            {
                //Get all places
                var places = _client.GetAllPlaces(new PlaceSearchRequest() { });

                if (!places.Any())
                    return;

                var firstPlace = places.First();

                //Get the place by id
                _client.GetPlace(firstPlace.Id); //38! no

                //Create a new place
                var place = new Place()
                {
                    Name = "Dion",
                    City = "Skopje",
                    Location = "Skopje",
                    Description = "place for eating and drinking",
                    Capacity = 56,
                    DateRegistered = DateTime.Now
                };
                _client.CreatePlace(place);

                //Update the place
                int id = 57;

                var newPlace = new Place()
                {
                    Name = "Dion",
                    //City = "Skopje",
                    Location = "Skopje",
                    Description = "place for eating and drinking",
                    Capacity = 56,
                    DateRegistered = DateTime.Now
                };

                _client.UpdatePlace(id, newPlace);

                //Delete the place
                _client.DeletePlace(26);
                _client.DeletePlace(58);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
