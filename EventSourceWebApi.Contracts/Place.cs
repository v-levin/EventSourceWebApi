using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourceWebApi.Contracts
{
    public class Place
    {
        public int Id { get; set; }

        public string[] Tags { get; set; }

        public string Moto { get; set; }

        public DateTime DateRegistered { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PartnerId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string City { get; set; }
    }
}
