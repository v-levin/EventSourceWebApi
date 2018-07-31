namespace EventSourceWebApi.Contracts.Requests
{
    public class EventSearchRequest : PageableRequest
    {
        public string Name { get; set; }

        public string City { get; set; }

        public string Category { get; set; }

        public string Location { get; set; }
    }
}
