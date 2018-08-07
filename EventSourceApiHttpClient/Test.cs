using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventSourceApiHttpClient
{
    public class Test
    {
        private readonly PlacesClient client;
        public Test(PlacesClient _client)
        {
            client = _client;
        }

        public void Run()
        {

            try
            {
                //Get all places
                client.GetAllPlaces(new PlaceSearchRequest() { });

                //Get the place by id
                client.GetPlace(38);

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
                client.CreatePlace(place);

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
                client.UpdatePlace(id, newPlace);

                //Delete the place
                client.DeletePlace(26);
                client.DeletePlace(58);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
