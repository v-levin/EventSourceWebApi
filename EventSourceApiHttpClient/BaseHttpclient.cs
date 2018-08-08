namespace EventSourceApiHttpClient
{
    public class BaseHttpclient
    {
        private PlacesClient _placesClient { get; set; }

        private EventsClient _eventsClient { get; set; }

        public BaseHttpclient(string baseUrl, string mediaType)
        {
            _placesClient = new PlacesClient(baseUrl, mediaType);
            _eventsClient = new EventsClient(baseUrl, mediaType);
        }

        public PlacesClient PlacesClient { get { return _placesClient; } }

        public EventsClient EventsClient { get { return _eventsClient; } }
    }
}
