using EventSourceApiHttpClient;

namespace EventSource.Tests
{
    class EventSource 
    {
        static void Main(string[] args)
        {
            var baseUrl = "http://localhost:49999/api/";
            var mediaType = "application/json";

            var client = new BaseHttpClient(baseUrl, mediaType);
           
            //var placesTest = new PlacesTests(client);
            //placesTest.Run();
          

        }
    }
}
