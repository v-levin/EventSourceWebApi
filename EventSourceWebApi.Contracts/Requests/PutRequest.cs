namespace EventSourceWebApi.Contracts.Requests
{
    public class PutRequest<T>
    {
        public int Id { get; set; }

        public T Payload { get; set; }
    }
}
