using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using EventSourceWebApi.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourceWebApi.DataContext.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly DbContextOptions<EventSourceDbContext> _dbContext;

        public EventsRepository(DbContextOptions<EventSourceDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public EventResponse GetEvents()
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return new EventResponse
                {
                    Events = db.Events.ToList()
                };
            }
        }

        public EventResponse GetEvent(int id)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return new EventResponse
                {
                    Event = db.Events.FirstOrDefault(e => e.Id == id)
                };
            }
        }

        public EventResponse CreateEvent(Event @event)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                var response = new EventResponse
                {
                    Event = @event
                };

                db.Events.Add(response.Event);
                db.SaveChanges();

                return response;
            }
        }

        public void UpdateEvent(Event @event)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Event Find(int id)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return db.Events.FirstOrDefault(e => e.Id == id);
            }
        }

        public void DeleteEvent(Event eventToDelete)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Remove(eventToDelete);
                db.SaveChanges();
            }
        }
    }
}
