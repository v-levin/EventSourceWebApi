using EventSource.Tests.IntegrationTests;
using EventSourceApiHttpClient;
using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Requests;
using System;

namespace EventSource.Tests
{
    class EventSource 
    {
        static void Main(string[] args)
        {
            var baseUrl = "http://localhost:49999/api/";
            var mediaType = "application/json";

            var client = new BaseHttpclient(baseUrl, mediaType);
           
            var placesTest = new PlacesTests(client);
            placesTest.Run();


        }
    }
}
