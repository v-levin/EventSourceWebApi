using EventSourceWebApi.Contracts;
using EventSourceWebApi.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public IEnumerable<Event> GetEvents()
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return db.Events;
            }
        }

        public Event GetEvent(int id)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                return db.Events.FirstOrDefaultAsync(e => e.Id == id).Result;
            }
        }

        public void CreateEvent(Event @event)
        {
            using (var db = new EventSourceDbContext(_dbContext))
            {
                db.Events.Add(@event);
                db.SaveChanges();
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
    }
}
