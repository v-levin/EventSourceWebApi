using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;
using System.Linq;

namespace EventSource.Tests.IntegrationTests
{
    public class PlacesTests
    {
        private static BaseHttpClient _client;
        public PlacesTests(BaseHttpClient client)
        {
            _client = client;
        }

        public void Run()
        {

            try
            {
                //Get all places with City = Skopje
                var places = _client.PlacesClient.GetPlaces(new PlaceSearchRequest() { City = "Skopje" });

                if (!places.Any())
                    return;

                var firstPlace = places.First();

                // Get place by id
                var getPlace = _client.PlacesClient.GetPlace(new PlaceIdRequest() { Id = firstPlace.Id });


                //Update the place
                var newPlace = new Place()
                {
                    Name = "mkc",
                    City = "Radovis"
                };
                var updatePlace = _client.PlacesClient.PutPlace(new PutRequest<Place>() { Id = getPlace.Id, Payload = newPlace });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
