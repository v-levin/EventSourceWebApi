﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventSourceWebApi.Contracts
{
    public class Place
    {
        public int Id { get; set; }

        public DateTime DateRegistered { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string City { get; set; }
    }
}
