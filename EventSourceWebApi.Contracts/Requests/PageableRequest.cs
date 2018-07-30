namespace EventSourceWebApi.Contracts.Requests
{
    public class PageableRequest
    {
        public int MaxLimit { get; set; } = 100;

        public int Limit { get; set; } = 5;

        public int Offset { get; set; } = 0;

    }
}
