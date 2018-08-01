namespace EventSourceWebApi.Contracts.Requests
{
    public class PageableRequest
    {
        public int Limit { get; set; } = 10;

        public int Offset { get; set; } 

    }
}
