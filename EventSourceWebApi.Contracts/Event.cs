using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EventSourceWebApi.Contracts
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateRegistered { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string City { get; set; }

        public string Category { get; set; }

        public string Location { get; set; }

        public int PlaceId { get; set; }

        public bool? IsAccepted { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string TimeTo { get; set; }

        public string TimeFrom { get; set; }
    }
}
