using System;
using System.Collections.Generic;
using System.Text;
using EventSourceWebApi.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EventSourceWebApi.DataContext
{
    public class EventSourceDbContext : DbContext
    {
        public EventSourceDbContext()
        {
        }

        public EventSourceDbContext(DbContextOptions<EventSourceDbContext> options)
                    : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Place> Places { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
