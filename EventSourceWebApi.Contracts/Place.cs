using System;

namespace EventSourceWebApi.Contracts
{
    public class Place
    {
        public int Id { get; set; }

        public DateTime DateRegistered { get; set; }

        public int Capacity { get; set; }
        
        public string Location { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string City { get; set; }
    }
}
