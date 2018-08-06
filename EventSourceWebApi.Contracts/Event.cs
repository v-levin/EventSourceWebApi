using System;

namespace EventSourceWebApi.Contracts
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateRegistered { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Category { get; set; }

        public string Location { get; set; }

    }
}
