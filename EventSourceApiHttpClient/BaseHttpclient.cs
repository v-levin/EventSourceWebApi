using System.Configuration;

namespace EventSourceApiHttpClient
{
    public class BaseHttpClient
    {
        private PlacesClient _placesClient { get; set; }

        private EventsClient _eventsClient { get; set; }

        public BaseHttpClient(string baseUrl, string mediaType, string timeout, string authenticationScheme, string authenticationToken)
        {
            _placesClient = new PlacesClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
            _eventsClient = new EventsClient(baseUrl, mediaType, timeout, authenticationScheme, authenticationToken);
        }

        public PlacesClient PlacesClient { get { return _placesClient; } }

        public EventsClient EventsClient { get { return _eventsClient; } }

       
    }
}