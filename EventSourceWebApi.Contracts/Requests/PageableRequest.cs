namespace EventSourceWebApi.Contracts.Requests
{
    public class PageableRequest
    {
        public int Limit { get; set; } = 5;

        public int Offset { get; set; } = 0;

    }
}
