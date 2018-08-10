using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;

namespace EventSource.Tests.IntegrationTests
{
    public class CreatePlacesTest
    {
        private static BaseHttpClient _client;
        public CreatePlacesTest(BaseHttpClient client)
        {
            _client = client;
        }

        public void Run()
        {
            try
            {
                var place = new Place()
                {
                    Name = "place1",
                    Description = "Wdefrgb",
                    Capacity = 70,
                    Location = "Radovis",
                    DateRegistered = DateTime.Now,
                    City = "Skopje"
                };
                var placeId = _client.PlacesClient.PostPlace(new PostRequest<Place>() { Payload = place });

                if (placeId == null)
                    return;


                place.City = "Kumanovo";

                //var updatePalce = _client.PlacesClient.PutPlace(new PutRequest<Place>() { Id = placeId, Payload = place });

                //var deletePalce = _client.PlacesClient.DeletePlace(new PlaceIdRequest() { Id = placeId }); 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
