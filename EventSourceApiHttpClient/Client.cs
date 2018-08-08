namespace EventSourceApiHttpClient
{
    public class Client 
    {
        private PlacesClient _placesClient { get; set; }

        private EventsClient _eventsClient { get; set; }

        public Client(string baseUrl, string mediaType)
        {
            _placesClient = new PlacesClient(baseUrl, mediaType);
            _eventsClient = new EventsClient(baseUrl, mediaType);
        }

        public PlacesClient PlacesClient { get { return _placesClient; } }

        public EventsClient EventsClient { get { return _eventsClient; } }
    }
}
